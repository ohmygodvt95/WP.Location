using System;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Location.Control;
using Location.DataModels;
using Location.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Location
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SavePage : Page
    {
        private BusLine data;
        public SavePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            data = new BusLine();
            data = e.Parameter as BusLine;
            listBox.ItemsSource = data.ListPoints;
            textBlock.Text = data.Name;
        }

        private async void edit_Click(object sender, RoutedEventArgs e)
        {
            EditPointDialog dialog = new EditPointDialog(data.ListPoints.ElementAt(listBox.SelectedIndex).Name);
            await dialog.ShowAsync();
            if (dialog.check == 1)
            {
                data.ListPoints.ElementAt(listBox.SelectedIndex).Name = dialog.txt;
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            data.ListPoints.RemoveAt(listBox.SelectedIndex);
        }

        private void itemPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            processBar.Visibility  = Visibility.Visible;
            DataHelper dbHelper = new DataHelper();
            DataBusLine name = new DataBusLine(data.Name, data.Data);
            dbHelper.InsertNewBusLine(name);
            //System.Diagnostics.Debug.WriteLine("Them tuyen");
            DataBusLine newLine = await dbHelper.GetNewLine();
            //System.Diagnostics.Debug.WriteLine("Lay gia tri vua them");
            int i = 0;
            foreach (var item in data.ListPoints)
            {
                i++;
                DataPoint dataPoint = new DataPoint(item.Name, newLine.Id, item.Long, item.Lat);
                dbHelper.InsertNewPoint(dataPoint);
                //System.Diagnostics.Debug.WriteLine("Them diem");
            }
            processBar.Visibility = Visibility.Collapsed;
            var dialog = new MessageDialog("Lưu dữ liệu thành công! total: " + i);
            await dialog.ShowAsync();
            System.Diagnostics.Debug.WriteLine(data.Name);
            System.Diagnostics.Debug.WriteLine(data.Data);
            Frame.Navigate(typeof (MainPage));
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (MainPage));
        }
    }
}
