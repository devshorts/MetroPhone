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
    public partial class StationList : UserControl
    {
        public StationList()
        {
            InitializeComponent();

            MetroManager.Instance.UpdateStations(null);
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

        }

        private void StationsUpdated(object sender, MetroEventArgs args)
        {
            MetroManager.Instance.Stations.Sort(new StationInfo(null));
            Stations.DataContext = MetroManager.Instance.Stations;
        }

        private void LinesUpdated(object sender, MetroEventArgs args)
        {
           
        }

        private void RefreshStationList(object sender, RoutedEventArgs e)
        {
            
        }

        private void ShowArrivals(object sender, SelectionChangedEventArgs e)
        {
            var selected = Stations.SelectedItem as StationInfo;
            if (selected != null)
            {
                MetroManager.Instance.UpdateArrivalTimes(new List<StationInfo>() {selected});
            }
        }

        private void MouseOut(object sender, MouseEventArgs e)
        {
            
        }

    }
}
