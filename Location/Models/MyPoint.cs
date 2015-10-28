using System;
using System.ComponentModel;

namespace Location.Models
{
    class MyPoint : INotifyPropertyChanged
    {
        private String _name;
        private String _lat;
        private String _long;
        public MyPoint()
        {
            this._name = "";
            this._lat = "";
            this._long = "";
        }
        public String Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyChanged("Name");
                }
            }
        }

        public String Long
        {
            get { return _long; }
            set
            {
                if (_long != value)
                {
                    _long = value;
                    NotifyChanged("Long");
                }
            }
        }

        public String Lat
        {
            get { return _lat; }
            set
            {
                if (_lat != value)
                {
                    _lat = value;
                    NotifyChanged("Lat");
                }
            }
        }
        public void NotifyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
