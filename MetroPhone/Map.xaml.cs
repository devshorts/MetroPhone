using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MetroPhone
{
    public partial class Map : UserControl
    {
        private GeoCoordinateWatcher watcher;
        public Map()
        {
            InitializeComponent();
        }


    }
}
