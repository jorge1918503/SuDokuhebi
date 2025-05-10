using SuDokuhebi.ViewModels;

namespace SuDokuhebi.Views;

public partial class RankingPage : ContentPage
{
    private RankingViewModel viewModel;

    public RankingPage()
    {
        InitializeComponent();
        viewModel = new RankingViewModel();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.LoadRankingAsync();
    }
}