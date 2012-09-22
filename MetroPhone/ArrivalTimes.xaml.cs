using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MetroPhone.MetroService;
using MetroTest;

namespace MetroPhone
{
    public partial class ArrivalTimes : UserControl
    {
        public ArrivalTimes()
        {
            InitializeComponent(); 
            MetroManager.Instance.LinesUpdated += LinesUpdated;
            MetroManager.Instance.StationsUpdated += StationsUpdated;
            MetroManager.Instance.ArrivalsUpdated += ArrivalsUpdated;
            MetroManager.Instance.EntrancesUpdated += EntrancesUpdated;
        }

        private void EntrancesUpdated(object sender, MetroEventArgs args)
        {

        }

        private void ArrivalsUpdated(object sender, MetroEventArgs args)
        {
            Arrivals.DataContext = MetroManager.Instance.ArrivalTimes;
        }

        private void StationsUpdated(object sender, MetroEventArgs args)
        {
            
        }

        private void LinesUpdated(object sender, MetroEventArgs args)
        {
            
        }

        private void GetArrivals(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void RefreshArrivals(object sender, RoutedEventArgs e)
        {
            Arrivals.DataContext = new object();
            MetroManager.Instance.UpdateArrivalTimes(null);
        }
    }
}
