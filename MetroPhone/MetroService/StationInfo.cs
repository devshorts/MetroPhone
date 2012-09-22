using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MetroTest;

namespace MetroPhone.MetroService
{
    public class StationInfo : XmlDecoder, IComparer<StationInfo>
    {
        public StationInfo(XElement elem, XNamespace df) : base(elem, df)
        {
        }

        public StationInfo(string code)
        {
            Code = code;
        }

        [MetroElement]
        public String Code { get; set; }

        [MetroElement]
        public Double Lat { get; set; }

        [MetroElement]
        public String LineCode1 { get; set; }

        [MetroElement]
        public String LineCode2 { get; set; }

        [MetroElement]
        public String LineCode3 { get; set; }

        [MetroElement]
        public String LineCode4 { get; set; }

        [MetroElement]
        public Double Lon { get; set; }

        [MetroElement]
        public String Name { get; set; }

        [MetroElement]
        public String StationTogether1 { get; set; }

        [MetroElement]
        public String StationTogether2 { get; set; }

        
        public int Compare(StationInfo x, StationInfo y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
