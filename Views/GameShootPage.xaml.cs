using CommunityToolkit.Maui.Views;
using System;
using Cell = SuDokuhebi.Utils.Cell;
using SuDokuhebi.Views.Popups;
using Colors = SuDokuhebi.Utils.Colors;
using SuDokuhebi.Services;
using SuDokuhebi.Utils;
using Plugin.Maui.Audio;

namespace SuDokuhebi.Views;

public partial class GameShootPage : ContentPage
{
    private ImageButton[,] buttons;
    private Cell[,] cells;

    private int gridSize;

    private System.Timers.Timer _timer;
    private int _secondsElapsed;
    private int movements;

    private readonly PlayerService _playerService;
    private readonly GameService _gameService;

    private readonly IAudioManager _audioManager;

    public GameShootPage()
    {
        InitializeComponent();
        _audioManager = AudioManager.Current;
        _playerService = new PlayerService();
        _gameService = new GameService();

        // tamaño del grid
        gridSize = 9;

        buttons = new ImageButton[gridSize, gridSize];
        cells = new Cell[gridSize, gridSize];

        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += (s, e) => UpdateTimer();
        _timer.AutoReset = true;
        _timer.Enabled = true;

        StartTimer();
        StartMovements();

        GeneratePurpleZones();
        CreateGameGrid();
        possibleMovement();
    }

    private void GeneratePurpleZones()
    {
        Random random = new Random();

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                bool purpleZone;

                if (j % 2 == 0 && i % 2 == 0)
                {
                    purpleZone = false;
                }
                else if (i + j == gridSize - 1)
                {
                    purpleZone = false;
                }
                else if (i == gridSize - 2 && j == 1)
                {
                    purpleZone = false;
                }
                else
                {
                    purpleZone = random.NextDouble() < 0.45;

                }

                cells[i, j] = new Cell(purpleZone, i, j);
            }
        }
    }

    private void CreateGameGrid()
    {
        GameGrid.Children.Clear();
        GameGrid.RowDefinitions.Clear();
        GameGrid.ColumnDefinitions.Clear();

        for (int i = 0; i < gridSize; i++)
        {
            GameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                if (row == gridSize - 1 && col == 0) cells[row, col].PlayerIn = true;
                else if (row == 0 && col == gridSize - 1)
                {
                    cells[row, col].SnakeHead = true;
                    cells[row, col].SnakeIn = true;
                }

                var button = new ImageButton
                {
                    BackgroundColor = cells[row, col].PurpleZone ? Colors.purple : cells[row, col].SnakeHead ? Colors.darkSnakeGreen : Colors.white,
                    BorderWidth = 1,
                    CornerRadius = 5,
                    Padding = 5,
                    Source = cells[row, col].PlayerIn ? "personajehard.png" :
                             cells[row, col].SnakeHead ? "serpiente.png" : null,
                    Aspect = Aspect.AspectFit
                };

                int currentRow = row;
                int currentCol = col;

                buttons[row, col] = button;
                button.Clicked += (s, e) => OnCellClicked(s, currentRow, currentCol);

                GameGrid.Children.Add(button);
                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);
            }
        }
    }

    private async void OnCellClicked(object sender, int row, int col)
    {
        var button = sender as ImageButton;

        if (button.BackgroundColor == Colors.purple)
        {
            button.BackgroundColor = Colors.lightPurple;
            // sonido de acido
            if (_soundEnabled)
            {
                var audioStream = await FileSystem.OpenAppPackageFileAsync("acid.mp3");
                var player = _audioManager.CreatePlayer(audioStream);
                player.Loop = false;
                player.Play();
            }
            
            await Task.Delay(500);
            button.BackgroundColor = Colors.purple;
        }
        else if (button.BackgroundColor == Colors.LightGreen)
        {
            // busca la anterior posision del personaje y la limpia
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (cells[i, j].PlayerIn)
                    {
                        cells[i, j].PlayerIn = false;
                        buttons[i, j].Source = null;
                    }
                }
            }

            // cambia la posicion del personaje y actualiza los posibles movimientos
            button.Source = "personajehard.png";
            cells[row, col].PlayerIn = true;

            // posibilidad de gruñido
            if (_soundEnabled)
            {
                var random = new Random();
                double posibility = random.NextDouble();
                if (posibility <= 0.2) // 30% de probabilidad
                {
                    var audioStream = await FileSystem.OpenAppPackageFileAsync("barbaro.mp3");
                    var player = _audioManager.CreatePlayer(audioStream);
                    player.Loop = false;
                    player.Play();
                }
                else if (posibility > 0.2 && posibility <= 0.4)
                {
                    var audioStream = await FileSystem.OpenAppPackageFileAsync("cansado.mp3");
                    var player = _audioManager.CreatePlayer(audioStream);
                    player.Loop = false;
                    player.Play();
                }
            }
            

            possibleMovement();

            movements++;
            UpdateMovementsLabel();

            // por si el personaje se suicida
            if (checkWinLost())
            {
                SessionManager.CurrentResult = "Derrota";
                await saveGame();

                // Mostrar popup y esperar a que se cierre
                var popup = new DefeatPopup(movements, _secondsElapsed, _soundEnabled);
                var result = await this.ShowPopupAsync(popup);

                // Solo después de que el popup se cierre, navega al menú
                if (result == "menu")
                {
                    SessionManager.ClearGame();
                    Application.Current.Windows[0].Page = new MenuTabbedPage();
                }
                else if (result == "again")
                {
                    // Reiniciar el juego u otra acción
                    Application.Current.Windows[0].Page = new NavigationPage(new GameShootPage());
                }

            }
            else
            {
                // movimiento de la serpiente
                snakeMovement();
                // posibilidad de silbido
                if (_soundEnabled)
                {
                    var randomSnake = new Random();
                    double posibilitySnake = randomSnake.NextDouble();
                    if (posibilitySnake <= 0.2) // 30% de probabilidad
                    {
                        var audioStream = await FileSystem.OpenAppPackageFileAsync("snakehissing.mp3");
                        var player = _audioManager.CreatePlayer(audioStream);
                        player.Loop = false;
                        player.Play();
                    }
                    else if (posibilitySnake > 0.2 && posibilitySnake <= 0.4)
                    {
                        var audioStream = await FileSystem.OpenAppPackageFileAsync("cascabeleo.mp3");
                        var player = _audioManager.CreatePlayer(audioStream);
                        player.Loop = false;
                        player.Play();
                    }
                }
                
                // si pierde
                if (checkWinLost())
                {
                    SessionManager.CurrentResult = "Derrota";
                    await saveGame();

                    // Mostrar popup y esperar a que se cierre
                    var popup = new DefeatPopup(movements, _secondsElapsed, _soundEnabled);
                    var result = await this.ShowPopupAsync(popup);

                    // Solo después de que el popup se cierre, navega al menú
                    if (result == "menu")
                    {
                        SessionManager.ClearGame();
                        Application.Current.Windows[0].Page = new MenuTabbedPage();
                    }
                    else if (result == "again")
                    {
                        // Reiniciar el juego u otra acción
                        Application.Current.Windows[0].Page = new NavigationPage(new GameShootPage());
                    }

                }

                shootSnakeCellColor();

            }




        }
        else if (button.BackgroundColor == Colors.red && cells[row, col].SnakeHead)
        {

            SessionManager.CurrentResult = "Victoria";
            await saveGame();

            // Mostrar popup y esperar a que se cierre
            var popup = new VictoryPopup(movements, _secondsElapsed, _soundEnabled);
            var result = await this.ShowPopupAsync(popup);

            // Solo después de que el popup se cierre, navega al menú
            if (result == "menu")
            {
                SessionManager.ClearGame();
                Application.Current.Windows[0].Page = new MenuTabbedPage();
            }
            else if (result == "again")
            {
                // Reiniciar el juego u otra acción
                Application.Current.Windows[0].Page = new NavigationPage(new GameShootPage());
            }
        }

    }

    private async Task saveGame()
    {
        _timer.Stop(); // Detiene el tiempo al ganar

        // Calcular score
        int score = 1000 - (int)(_secondsElapsed * 10) - (movements * 10) + 300; // + bonus de dificultad

        if (SessionManager.CurrentResult == "Derrota") score = 0;

        // Validar que CurrentDifficulty no sea nulo antes de llamar a ToString()
        string difficulty = SessionManager.CurrentDifficulty?.ToString() ?? "Error";

        // Guardar partida en la base de datos
        await _gameService.SaveGame(SessionManager.CurrentUserId, difficulty, SessionManager.CurrentResult, TimeSpan.FromSeconds(_secondsElapsed), movements, score);

        // actualizar highest score
        var player = await _playerService.GetPlayerByIdAsync(SessionManager.CurrentUserId);  // obtener jugador actual

        if (player != null && score > player.highestScore) // si el score es mayor al anterior
        {
            player.highestScore = score;
            await _playerService.UpdatePlayerAsync(player);
        }
    }

    private bool checkWinLost()
    {
        int rows = cells.GetLength(0);
        int cols = cells.GetLength(1);

        // Encontrar la posición actual del jugador
        int playerRow, playerCol;
        playerPosition(rows, cols, out playerRow, out playerCol);

        // Encuentra la posicion de la serpiente
        int snakeRow, snakeCol;
        snakePosition(rows, cols, out snakeRow, out snakeCol);

        // Direcciones posibles (8 alrededor)
        List<(int dRow, int dCol)> directions = DireccionesPosibles();

        // probamos cada celda adyacente a la cabeza de la serpiente
        foreach (var (dRow, dCol) in directions)
        {
            // sumando la posicion de la serpiente a cada movimiento posible
            int adjacentRow = snakeRow + dRow;
            int adjacentCol = snakeCol + dCol;

            // Verificamos si la celda adyacente está dentro del grid
            if (adjacentRow >= 0 && adjacentRow < rows && adjacentCol >= 0 && adjacentCol < cols)
            {
                // Si el jugador esta en alguna de las celdas adyacentes
                if (adjacentRow == playerRow && adjacentCol == playerCol)
                {
                    return true; // El jugador está al lado de la cabeza de la serpiente
                }
                else if (snakeRow == playerRow && snakeCol == playerCol)
                {
                    return true; // la serpiente se ha puesto encima
                }
            }
        }
        // No está al lado
        return false;
    }

    private static List<(int dRow, int dCol)> DireccionesPosibles()
    {
        var directions = new List<(int dRow, int dCol)>
        {
            (-1, 0),  // arriba
            (1, 0),   // abajo
            (0, -1),  // izquierda
            (0, 1),   // derecha
            (-1, -1), // arriba-izquierda
            (-1, 1),  // arriba-derecha
            (1, -1),  // abajo-izquierda
            (1, 1)    // abajo-derecha
        };
        return directions;
    }

    private void playerPosition(int rows, int cols, out int playerRow, out int playerCol)
    {
        playerRow = -1;
        playerCol = -1;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (cells[row, col].PlayerIn)
                {
                    playerRow = row;
                    playerCol = col;
                }
            }
        }
    }

    private bool possibleMovement()
    {
        foreach (ImageButton button in buttons) if (button.BackgroundColor == Colors.LightGreen || button.BackgroundColor == Colors.yellowSnakeEye) button.BackgroundColor = Colors.white;

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (cells[i, j].PlayerIn)
                {
                    movementCellColor(i - 1, j - 1);
                    movementCellColor(i - 1, j);
                    movementCellColor(i - 1, j + 1);
                    movementCellColor(i, j + 1);
                    movementCellColor(i + 1, j + 1);
                    movementCellColor(i + 1, j);
                    movementCellColor(i + 1, j - 1);
                    movementCellColor(i, j - 1);

                    shootCellColor(i - 1, j - 2); // disparos izquierda
                    shootCellColor(i + 1, j - 2);
                    shootCellColor(i - 2, j - 1); // disparos arriba
                    shootCellColor(i - 2, j + 1);
                    shootCellColor(i - 1, j + 2); // disparos derecha
                    shootCellColor(i + 1, j + 2);
                    shootCellColor(i + 2, j - 1); // disparos abajo 
                    shootCellColor(i + 2, j + 1);
                    break;
                }
            }
        }

        return false;
    }

    private void movementCellColor(int i, int j)
    {
        if (i >= 0 && i < gridSize && j >= 0 && j < gridSize)
        {
            if (buttons[i, j].BackgroundColor == Colors.white) buttons[i, j].BackgroundColor = Colors.LightGreen;
        }
    }

    private void shootCellColor(int i, int j)
    {
        if (i >= 0 && i < gridSize && j >= 0 && j < gridSize)
        {
            if (buttons[i, j].BackgroundColor == Colors.white) buttons[i, j].BackgroundColor = Colors.yellowSnakeEye;
        }
    }

    private void shootSnakeCellColor()
    {
        for (int row = 0; row < cells.GetLength(0); row++)
        {
            for (int col = 0; col < cells.GetLength(1); col++)
            {
                if (cells[row, col].SnakeHead && buttons[row, col].BackgroundColor == Colors.yellowSnakeEye)
                {
                    buttons[row, col].BackgroundColor = Colors.red;
                }
            }
        }
    }

    private void snakeMovement()
    {
        int rows = cells.GetLength(0);
        int cols = cells.GetLength(1);

        int snakeRow, snakeCol;
        snakePosition(rows, cols, out snakeRow, out snakeCol);

        int playerRow, playerCol;
        playerPosition(rows, cols, out playerRow, out playerCol);

        // Direcciones posibles (8 alrededor)
        List<(int dRow, int dCol)> directions = DireccionesPosibles();

        Random rnd = new Random();

        bool useSmartMove = rnd.NextDouble() < 0.63; // 63% de las veces elige la mejor dirección

        if (useSmartMove)
        {
            // Ordena por distancia al jugador (inteligente)
            directions = directions
                .OrderBy(dir =>
                {
                    int newRow = snakeRow + dir.dRow;
                    int newCol = snakeCol + dir.dCol;

                    if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols || cells[newRow, newCol].SnakeIn)
                        return int.MaxValue;

                    int distance = Math.Abs(playerRow - newRow) + Math.Abs(playerCol - newCol);
                    return distance;
                }).ToList();
        }
        else
        {
            // Aleatorio total (30% de las veces)
            directions = directions.OrderBy(x => rnd.Next()).ToList();
        }

        // Intentar moverse
        foreach (var (dRow, dCol) in directions)
        {
            int newRow = snakeRow + dRow;
            int newCol = snakeCol + dCol;

            if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols && !cells[newRow, newCol].SnakeIn)
            {
                // Limpiar celda anterior
                cells[snakeRow, snakeCol].SnakeHead = false;
                cells[snakeRow, snakeCol].SnakeIn = true;
                buttons[snakeRow, snakeCol].Source = null;
                buttons[snakeRow, snakeCol].BackgroundColor = Colors.darkSnakeGreen;

                // Nueva cabeza
                cells[newRow, newCol].SnakeHead = true;
                buttons[newRow, newCol].Source = "serpiente.png";
                break;
            }
        }
    }



    private void snakePosition(int rows, int cols, out int snakeRow, out int snakeCol)
    {
        // Encontrar la posición actual de la serpiente
        snakeRow = -1;
        snakeCol = -1;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (cells[i, j].SnakeHead)
                {
                    snakeRow = i;
                    snakeCol = j;
                    break;
                }
            }
            if (snakeRow != -1) break;
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        _timer.Stop();


        bool answer = await DisplayAlert("¿Quieres salir del juego?", "La partida no se guardará", "Sí", "No");

        if (answer)
        {
            // El usuario pulsó "Sí"
            SessionManager.ClearGame();
            Application.Current.Windows[0].Page = new MenuTabbedPage();
        }
        else
        {
            // El usuario pulsó "No"
            _timer.Start();
        }

    }

    private void OnRestartClicked(object sender, EventArgs e)
    {
        GeneratePurpleZones();
        CreateGameGrid();
        possibleMovement();

        StartTimer();
        StartMovements();
    }

    private void StartTimer()
    {
        _secondsElapsed = 0;
        UpdateTimerLabel();
        _timer.Start();
    }

    private void UpdateTimer()
    {
        _secondsElapsed++;
        MainThread.BeginInvokeOnMainThread(() =>
        {
            UpdateTimerLabel();
        });
    }

    private void UpdateTimerLabel()
    {
        TimeSpan time = TimeSpan.FromSeconds(_secondsElapsed);
        TimerLabel.Text = $"Tiempo: {time:mm\\:ss}";
    }

    private void StartMovements()
    {
        movements = 0;
        Movements.Text = $"Movimientos: {movements}";
    }

    private void UpdateMovementsLabel()
    {
        Movements.Text = $"Movimientos: {movements}";
    }

    private bool _soundEnabled = true;

    private void OnSoundIconTapped(object sender, EventArgs e)
    {
        _soundEnabled = !_soundEnabled;
        SoundIcon.Source = _soundEnabled ? "soundon.png" : "soundoff.png";

        // Opcional: Pequeña animación al tocar
        SoundIcon.ScaleTo(1.2, 100);
        SoundIcon.ScaleTo(1.0, 100);
    }
}