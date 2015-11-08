using Location.Common;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Location.DataModels;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Location
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class History : Page
    {
        private ObservableCollection<DataBusLine> data;
        //private DataBusLineHelper db;
        private DataHelper Db;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public History()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            data = new ObservableCollection<DataBusLine>();
            
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            //db = new DataBusLineHelper();
            //if (await db.CheckFileExists(db.DbName) == false)
            //{
            //    await db.OnCreate(db.DbPath);
            //}


            //data = db.ReadAllBusLines();
            Db = new DataHelper();
            data = await Db.ReadAllBusLines();
            listBox.ItemsSource = data;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void textBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock a = (TextBlock) sender;
            String Id = a.Tag.ToString();
            Frame.Navigate(typeof (ReviewPage), Id);
        }

        private void appBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
