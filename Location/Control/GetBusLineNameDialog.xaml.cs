using System;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Location
{
    public sealed partial class GetBusLineNameDialog : ContentDialog
    {
        public String txt = "";
        public GetBusLineNameDialog()
        {
            this.InitializeComponent();
        }
        private String GetName()
        {
            return name.Text;
        }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            txt = name.Text;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
           
        }
    }
}
