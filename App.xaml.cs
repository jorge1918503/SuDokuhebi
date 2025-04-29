using static Microsoft.Maui.Devices.DeviceDisplay;
using SuDokuhebi.Views;

namespace SuDokuhebi
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new WelcomePage());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            var screenWidth = MainDisplayInfo.Width / MainDisplayInfo.Density;
            var screenHeight = MainDisplayInfo.Height / MainDisplayInfo.Density;

            const int newWidth = 900;
            const int newHeight = 800;

            // Establecer el tamaño de la ventana
            window.Width = newWidth;
            window.Height = newHeight;

            // Establecer el tamaño máximo y mínimo para deshabilitar el redimensionamiento
            window.MaximumHeight = newHeight;
            window.MaximumWidth = newWidth;
            window.MinimumHeight = newHeight;
            window.MinimumWidth = newWidth;

            // Posicionar la ventana en el centro de la pantalla
            window.X = (screenWidth - newWidth) / 2;
            window.Y = (screenHeight - newHeight) / 2;

            // Establecer el título de la ventana
            window.Title = "SuDokuhebi";

            return window;
        }
    }
}