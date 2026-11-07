using BullsAndCowsApp.ViewModels;

namespace BullsAndCowsApp.Views
{
    // Code-behind of MainPage.
    // In MVVM this file stays almost empty: no game logic here.
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Connect the View to the ViewModel.
            // Every {Binding ...} in MainPage.xaml reads from this object.
            BindingContext = new GameViewModel();
        }
    }
}
