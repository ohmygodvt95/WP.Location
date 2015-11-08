using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Location.DataModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Location
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            // Set the background color of the status bar, and DON'T FORGET to set the opacity!

            var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();

            statusBar.BackgroundColor = Color.FromArgb(255, 0, 200, 248);

            statusBar.BackgroundOpacity = 1;
           


        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataHelper dbHelper = new DataHelper();
            await dbHelper.OnCreate();
        }

        private void btnGetLocation_Click(object sender, RoutedEventArgs e)
        {
           Frame.Navigate(typeof(ViewPage));
        }

        private void btnReview_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (History));
        }
    }
}
