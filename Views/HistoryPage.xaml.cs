using SuDokuhebi.Utils;
using SuDokuhebi.ViewModels;

namespace SuDokuhebi.Views;

public partial class HistoryPage : ContentPage
{
    private readonly HistoryViewModel _viewModel = new();

    public HistoryPage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadHistoryAsync();
    }

    private void OnFilterByDifficulty(object sender, EventArgs e)
    {
        _viewModel.ApplyFilter("difficulty");
    }

    private void OnFilterByResult(object sender, EventArgs e)
    {
        _viewModel.ApplyFilter("result");
    }

    private void OnFilterByScore(object sender, EventArgs e)
    {
        _viewModel.ApplyFilter("score");
    }

    private void OnFilterByTime(object sender, EventArgs e)
    {
        _viewModel.ApplyFilter("time");
    }

    private void OnFilterByMovements(object sender, EventArgs e)
    {
        _viewModel.ApplyFilter("movements");
    }

    private void OnFilterChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        string? selected = picker.SelectedItem as string;

        string? field = selected switch
        {
            "Dificultad" => "difficulty",
            "Resultado" => "result",
            "Puntuación" => "score",
            "Tiempo" => "time",
            "Movimientos" => "movements",
            _ => null
        };

        if (field != null) _viewModel.ApplyFilter(field);
        
    }


}