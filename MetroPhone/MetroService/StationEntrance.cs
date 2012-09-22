using System;
using System.Xml.Linq;
using MetroTest;

namespace MetroPhone.MetroService
{
    public class StationEntrance : XmlDecoder
    {
        public StationEntrance(XElement elem, XNamespace df) : base(elem, df)
        {
        }

        [MetroElement]
        public string Description { get; set; }

        [MetroElement]
        public string ID { get; set; }

        [MetroElement]
        public Double Lat { get; set; }

        [MetroElement]
        public Double Lon { get; set; }

        [MetroElement]
        public string Name { get; set; }

        [MetroElement]
        public string StationCode1 { get; set; }

        [MetroElement]
        public string StationCode2 { get; set; }
    }
}
