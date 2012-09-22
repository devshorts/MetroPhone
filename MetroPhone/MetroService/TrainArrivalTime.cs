using System;
using System.Xml.Linq;
using MetroTest;

namespace MetroPhone.MetroService
{
    public class TrainArrivalTime :XmlDecoder
    {
        public TrainArrivalTime(XElement elem, XNamespace df) : base(elem, df)
        {
        }

        [MetroElement]
        public String Car { get; set; }

        [MetroElement]
        public String Destination { get; set; }

        [MetroElement]
        public String DestinationCode { get; set; }

        [MetroElement]
        public String DestinationName { get; set; }

        [MetroElement]
        public String Group { get; set; }

        [MetroElement]
        public String Line { get; set; }

        [MetroElement]
        public String LocationCode { get; set; }

        [MetroElement]
        public String LocationName { get; set; }

        [MetroElement]
        public String Min { get; set; }
    }
}
