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
    public partial class LineColors : UserControl
    {
        public LineColors()
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

        }

        private void StationsUpdated(object sender, MetroEventArgs args)
        {
            
        }

        private void LinesUpdated(object sender, MetroEventArgs args)
        {
            Lines.DataContext = MetroManager.Instance.Lines;
        }

        private void RefreshStationList(object sender, RoutedEventArgs e)
        {
            Lines.DataContext = new object();
            MetroManager.Instance.UpdateLines();
        }

        private void ShowStations(object sender, RoutedEventArgs e)
        {
            var selected = Lines.SelectedItem as LineInfo;
            if (selected != null)
            {
                MetroManager.Instance.UpdateStations(selected.LineCode);
            }
        }
    }


}
