using SnakeUWP.ViewModel;

namespace SnakeUWP.View
{
    /// <summary>
    /// The page which contains user points and map of the game.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainPageViewModel();
        }
    }
}
