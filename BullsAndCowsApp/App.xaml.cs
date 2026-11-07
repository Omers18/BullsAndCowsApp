using BullsAndCowsApp.Views;

namespace BullsAndCowsApp
{
    // The App class starts the app.
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        // Creates the app's window.
        // Our app has only one page, so the window shows MainPage directly (no Shell).
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage());
        }
    }
}