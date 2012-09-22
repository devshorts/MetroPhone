using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using MetroPhone.Common;

namespace MetroPhone.MetroService
{
    public class MetroManager
    {
        private const string API_KEY = "u6ctd5b38t8nun52dbe7m9pc";
        private const string BaseUrl = "http://api.wmata.com/";

        public event MetroEvent LinesUpdated;
        public event MetroEvent StationsUpdated;
        public event MetroEvent ArrivalsUpdated;
        public event MetroEvent EntrancesUpdated;

        public List<StationInfo> Stations { get; set; }
        public List<LineInfo> Lines { get; set; }
        public List<TrainArrivalTime> ArrivalTimes { get; set; }
        public List<StationEntrance> Entrances { get; set; }

        private List<StationInfo> _arrivalStations;
        public static MetroManager Instance = new MetroManager();

        private  MetroManager()
        {
            
        }

        public void UpdateLines()
        {
            string url = GetApiKeySingle(BaseUrl + "Rail.svc/Lines");
            GetXmlFromUrl(url, data =>
                                                    {
                                                        Lines = MetroBuilder<LineInfo>.Build(data, "Lines");
                                                        LinesUpdated(null, null);
                                                    });
        }

        public void UpdateStations(String lineCode)
        {
            string url;
            
            // get stations for a specific line
            if(lineCode != null)
            {
                string lineCodeString = "?LineCode=" + lineCode;
                url = GetApiKeyMultiple(BaseUrl + "Rail.svc/Stations" + lineCodeString);
            }

            // list all stations
            else
            {
                url = GetApiKeySingle(BaseUrl + "Rail.svc/Stations");
            }

            GetXmlFromUrl(url, data =>
                                   {
                                       Stations = MetroBuilder<StationInfo>.Build(data, "Stations");
                                       StationsUpdated(null, null);
                                   });
        }

        public void UpdateArrivalTimes(List<StationInfo> stations)
        {
            if (stations != null)
            {
                // cache the stations whos arrival times we want to know, if we get a null use the cached value to refresh
                _arrivalStations = stations;
            }
            string url = GetApiKeySingle(BaseUrl + "StationPrediction.svc/GetPrediction/" + _arrivalStations.FoldToCommaDelimitedList(s => s.Code));
            GetXmlFromUrl(url, data =>
                                   {
                                       ArrivalTimes = MetroBuilder<TrainArrivalTime>.Build(data, "Trains");
                                       ArrivalsUpdated(null, null);
                                   });
        }

        public void UpdateEntrances(double lat, double lon, int radiusInMeters = 500)
        {
            string url = GetApiKeyMultiple(BaseUrl + String.Format("Rail.svc/StationEntrances?lat={0}&lon={1}&radius={2}", lat, lon, radiusInMeters));
            GetXmlFromUrl(url, data =>
                                   {
                                       Entrances = MetroBuilder<StationEntrance>.Build(data, "Entrances");
                                       EntrancesUpdated(null, null);
                                   });
        }

        private static void GetXmlFromUrl(string url, Action<XDocument> callback)
        {
            var client = new WebClient();

            client.OpenReadCompleted += (sender, e) =>
                                            {
                                                XDocument xdoc;
                                                using (var str = e.Result)
                                                {
                                                    xdoc = XDocument.Load(str);
                                                }
                                                callback(xdoc);
                                            };

            client.OpenReadAsync(new Uri(url, UriKind.Absolute));
        }

        /// <summary>
        /// Use when you have no url paramters in the url string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string GetApiKeySingle(string s)
        {
            
            return s + "?" + ApiKey;
        }

        /// <summary>
        /// Use when you have multiple url paramters in the url string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string GetApiKeyMultiple(string s)
        {
            return s + "&" + ApiKey;
        }

        private string ApiKey
        {
            get { return "api_key=" + API_KEY; }
        }
    }

    public delegate void MetroEvent(object sender, MetroEventArgs args);

    public class MetroEventArgs
    {
    }

    internal static class MetroBuilder<T> where T : class, IMetroData
    {
        public static List<T> Build(XDocument root, string childName)
        {
            if (root != null && root.Root != null)
            {
                XNamespace df = root.Root.Name.Namespace;

                var val = (from items in root.Descendants(df + childName)
                                    from item in items.Elements()
                                    select Activator.CreateInstance(typeof (T), item, df) as T).ToList();

                return val;
            }
            return null;
        }
    }
}


