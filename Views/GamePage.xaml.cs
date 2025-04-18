using CommunityToolkit.Maui.Views;
using System;
using Cell = SuDokuhebi.Utils.Cell;
using SuDokuhebi.Views.Popups;
using Colors = SuDokuhebi.Utils.Colors;

namespace SuDokuhebi.Views;

public partial class GamePage : ContentPage
{
    private ImageButton[,] buttons = new ImageButton[9, 9]; // mapa de botones que ve e interactua el usuario
    private Cell[,] cells; // mapa de objetos cell para almacenar datos de cada celda

    private System.Timers.Timer _timer;
    private int _secondsElapsed;

    private int movements;

    public GamePage()
    {
        InitializeComponent();

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
        // Inicializo el mapa de cells con su row, col y asigno aleatoriamente la PurpleZone
        cells = new Cell[9, 9];
        Random random = new Random();

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                bool purpleZone;

                if (j % 2 == 0 && i % 2 == 0)
                {
                    purpleZone = false;
                }
                else if (i == 7 && j == 1)
                {
                    purpleZone = false;
                }
                else
                {
                    purpleZone = random.NextDouble() < 0.4;
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

        // creo el grid dinamicamente (9x9)
        for (int i = 0; i < 9; i++)
        {
            GameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        // recorro el mapa creando los botones
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                // indico la celda del personaje
                if (row == 8 && col == 0) cells[row, col].PlayerIn = true;
                // indico la celda de la serpiente
                else if (row == 0 && col == 8)
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
                    Source = cells[row, col].PlayerIn ? "personaje.png" :
                             cells[row, col].SnakeHead ? "serpiente.png" : null,
                    Aspect = Aspect.AspectFit
                };


                // copia local de row y col para evitar problemas con la captura de variables en lambdas
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

        if (cells[row, col].PurpleZone)
        {
            button.BackgroundColor = Colors.red;
            await Task.Delay(700);
            button.BackgroundColor = Colors.purple;
        }
        else if (button.BackgroundColor == Colors.LightGreen)
        {
            // busca la anterior posision del personaje y la limpia
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (cells[i, j].PlayerIn)
                    {
                        cells[i, j].PlayerIn = false;
                        buttons[i, j].Source = null;
                    }
                }
            }

            // cambia la posicion del personaje y actualiza los posibles movimientos
            button.Source = "personaje.png";
            cells[row, col].PlayerIn = true;
            possibleMovement();

            movements++;
            UpdateMovementsLabel();

            // comprobacion de victoria
            if (checkWinLost())
            {

                _timer.Stop(); // Detiene el tiempo al ganar


                // Mostrar popup y esperar a que se cierre
                var popup = new VictoryPopup(movements, _secondsElapsed);
                var result = await this.ShowPopupAsync(popup);

                // Solo después de que el popup se cierre, navega al menú
                if (result is bool ok && ok)
                {
                    await Task.Delay(200);
                    Application.Current.Windows[0].Page = new MenuTabbedPage();
                }


            }
            else
            {
                // movimiento de la serpiente
                snakeMovement();
                // si pierde
                if (checkWinLost())
                {
                    _timer.Stop(); // Detiene el tiempo al ganar

                    // Mostrar popup y esperar a que se cierre
                    var popup = new DefeatPopup(movements, _secondsElapsed);
                    var result = await this.ShowPopupAsync(popup);

                    // Solo después de que el popup se cierre, navega al menú
                    if (result is bool ok && ok)
                    {
                        Application.Current.Windows[0].Page = new MenuTabbedPage();
                    }

                }

            }
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
        foreach (ImageButton button in buttons) if (button.BackgroundColor == Colors.LightGreen) button.BackgroundColor = Colors.white;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (cells[i, j].PlayerIn)
                {
                    updateCellColor(i - 1, j - 1);
                    updateCellColor(i - 1, j);
                    updateCellColor(i - 1, j + 1);
                    updateCellColor(i, j + 1);
                    updateCellColor(i + 1, j + 1);
                    updateCellColor(i + 1, j);
                    updateCellColor(i + 1, j - 1);
                    updateCellColor(i, j - 1);
                    break;
                }
            }
        }

        return false;
    }

    private void updateCellColor(int i, int j)
    {
        if (i >= 0 && i < 9 && j >= 0 && j < 9)
        {
            if (buttons[i, j].BackgroundColor == Colors.white) buttons[i, j].BackgroundColor = Colors.LightGreen;
        }
    }

    private void snakeMovement()
    {
        int rows = cells.GetLength(0);
        int cols = cells.GetLength(1);
        int snakeRow, snakeCol;
        // Encuentra la posicion de la serpiente
        snakePosition(rows, cols, out snakeRow, out snakeCol);

        // Direcciones posibles (8 alrededor)
        List<(int dRow, int dCol)> directions = DireccionesPosibles();

        // Mezclar direcciones para elegir una al azar
        var rnd = new Random();
        directions = directions.OrderBy(x => rnd.Next()).ToList();

        foreach (var (dRow, dCol) in directions)
        {
            int newRow = snakeRow + dRow;
            int newCol = snakeCol + dCol;

            // Chequeamos que esté dentro del grid y que no haya ya una serpiente ahí
            if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols && !cells[newRow, newCol].SnakeIn)
            {
                // Limpiamos la celda anterior
                cells[snakeRow, snakeCol].SnakeHead = false; // deja de ser la cabeza
                cells[snakeRow, snakeCol].SnakeIn = true; // es parte del cuerpo
                buttons[snakeRow, snakeCol].Source = null; // quito la foto de la cabeza
                buttons[snakeRow, snakeCol].BackgroundColor = Colors.darkSnakeGreen; // pongo el fondo verde simulando el cuerpo

                // Movemos la serpiente a la nueva celda
                cells[newRow, newCol].SnakeHead = true; // la nueva celda pasa a ser la cabeza
                buttons[newRow, newCol].Source = "serpiente.png"; // le pongo la foto de la cabeza
                break;
            }
        }

        checkGameOver();

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


    private bool checkGameOver()
    {



        return false;
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
}