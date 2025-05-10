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
}