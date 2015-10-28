using Location.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Location.Control;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Location
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewPage : Page
    {
        public Geolocator Geolocator;
        public Geopoint CurrentPoint;
        private BusLine data;
        public ViewPage()
        {
            this.InitializeComponent();
            //
            processBar.Visibility = Visibility.Visible;
            Geolocator = new Geolocator();
            Geolocator.DesiredAccuracyInMeters = 50;
            Geolocator.DesiredAccuracy = PositionAccuracy.High;
            Geolocator.MovementThreshold = 2;
            data = new BusLine();
            listBox.ItemsSource = data.ListPoints;
        }
        // sự kiện khi có sự thay đổi vị trí
        private void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            CurrentPoint = new Geopoint(
                      new BasicGeoposition()
                      {
                          //Geopoint for HaNoi 
                          Latitude = args.Position.Coordinate.Latitude,
                          Longitude = args.Position.Coordinate.Longitude
                      });
            // tải lại map
            reloadMap(CurrentPoint);
            // thêm điểm để vẽ
            data.AddData(CurrentPoint);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            GetBusLineNameDialog dialogName = new GetBusLineNameDialog();
            await dialogName.ShowAsync();
            data.name = dialogName.txt;
            System.Diagnostics.Debug.WriteLine(data.name);
            Geolocator.PositionChanged += geolocator_PositionChanged;
            try
            {
                if (Geolocator.LocationStatus == PositionStatus.Disabled)
                {
                    // Location is turned off
                    var dialog = new MessageDialog("Dịch vụ tìm vị trí bị tắt");
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
                    return;
                }
                Geoposition geoposition = await Geolocator.GetGeopositionAsync(
                maximumAge: TimeSpan.FromMinutes(5),
                timeout: TimeSpan.FromSeconds(10)
                );
                await getCurrentLocation();
                reloadMap(CurrentPoint);

            }
            catch (UnauthorizedAccessException)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("Bật GPS em ơi");
                await messageDialog.ShowAsync();
            }
            base.OnNavigatedTo(e);
            processBar.Visibility = Visibility.Collapsed;
        }

        // Location tracking
        private async Task getCurrentLocation()
        {
            if (Geolocator.LocationStatus == PositionStatus.Disabled)
            {
                // Location is turned off
                var dialog = new MessageDialog("Dịch vụ tìm vị trí bị tắt");
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
                return;
            }
            processBar.Visibility = Visibility.Visible;
            Geoposition geoposition = await Geolocator.GetGeopositionAsync();
            CurrentPoint = new Geopoint(
                      new BasicGeoposition()
                      {
                          //Geopoint for HaNoi 
                          Latitude = geoposition.Coordinate.Point.Position.Latitude,
                          Longitude = geoposition.Coordinate.Point.Position.Longitude
                      });
            processBar.Visibility = Visibility.Collapsed;
        }
        // reload map
        private async void reloadMap(Geopoint geopoint)
        {
            await MyMap.TrySetViewAsync(geopoint, 16D);
        }
        // trở về màn hình đầu
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        // phóng to
        private void zoomIn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MyMap.ZoomLevel < 20)
            {
                MyMap.ZoomLevel++;
            }
        }
        // thu nhỏ
        private void zoomOut_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MyMap.ZoomLevel > 1)
            {
                MyMap.ZoomLevel--;
            }
        }
        // lấy vị trí hiện tại khi bấm nút trên bản đồ
        private async void gps_Tapped(object sender, TappedRoutedEventArgs e)
        {
            processBar.Visibility = Visibility.Visible;
            await getCurrentLocation();
            reloadMap(CurrentPoint);
            processBar.Visibility = Visibility.Collapsed;
        }
        // lấy thông tim điểm hiện tại
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            btnSave.IsEnabled = false;
            processBar.Visibility = Visibility.Visible;
            String currentP = "";
            await getCurrentLocation();
            MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(CurrentPoint);
            if (result.Status == MapLocationFinderStatus.Success)
            {
                MyPoint myPoint = new MyPoint();

                myPoint.Name = result.Locations[0].Address.StreetNumber + " "
                    + result.Locations[0].Address.Street + ", "
                    + result.Locations[0].Address.District + ", "
                    + result.Locations[0].Address.Town;
                myPoint.Long = CurrentPoint.Position.Longitude + "";
                myPoint.Long = CurrentPoint.Position.Latitude + "";

                data.AddPoint(myPoint);
                currentP = myPoint.Name;
            }

            MapIcon mapIcon = new MapIcon();
            mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/image/map_marker.png"));
            mapIcon.Title = currentP;
            mapIcon.Location = CurrentPoint;
            mapIcon.NormalizedAnchorPoint = new Point(0.5, 0.5);
            MyMap.MapElements.Add(mapIcon);

            processBar.Visibility = Visibility.Collapsed;
            btnSave.IsEnabled = true;
            ShowToast(main, "Tap to item for edit/delete");

        }
        // xem ds điểm đang có
        public static void ShowToast(Grid layoutRoot, string message)
        {
            Grid grid = new Grid();
            grid.Width = 360;
            grid.Height = 40;
            grid.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Bottom;
            grid.Margin = new Thickness(0, 0, 0, 30);


            TextBlock text = new TextBlock();
            text.Text = message;
            text.VerticalAlignment = VerticalAlignment.Center;
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.FontSize = 20;
            text.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.White);

            grid.Children.Add(text);

            layoutRoot.Children.Add(grid);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Tick += (sender, args) =>
            {
                layoutRoot.Children.Remove(grid);
                timer.Stop();
            };
            timer.Start();
        }
        // tiếp tục lấy thông tin

        private async void showMsg(String str)
        {
            var messageDialog = new Windows.UI.Popups.MessageDialog(str);
            await messageDialog.ShowAsync();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            data.Reset();
        }

        private void itemPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (SavePage), data);
        }
    }
}
