using System.Xml.Linq;
using MetroTest;

namespace MetroPhone.MetroService
{
    public class LineInfo : XmlDecoder
    {
        public LineInfo(XElement elem, XNamespace df) : base(elem, df)
        {
        }

        [MetroElement]
        public string DisplayName { get; set; }

        [MetroElement]
        public string EndStationCode { get; set; }

        [MetroElement]
        public string InternalDestination1 { get; set; }

        [MetroElement]
        public string InternalDestination2 { get; set; }

        [MetroElement]
        public string LineCode { get; set; }

        [MetroElement]
        public string StartStationCode { get; set; }
    }
}
