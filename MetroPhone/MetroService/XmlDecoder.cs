using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using MetroTest;

namespace MetroPhone.MetroService
{
    public class XmlDecoder : IMetroData
    {
        public XmlDecoder()
        {
        }

        public XmlDecoder(XElement elem, XNamespace df)
        {
            Decode(elem, df);
        }
        public void Decode(XElement elem, XNamespace df)
        {
            // get all the public properties
            var publicProperties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance |
                                                           BindingFlags.OptionalParamBinding |
                                                           BindingFlags.CreateInstance)
                .Where(prop => prop.GetCustomAttributes(typeof (MetroElement), false) != null);

            var methods = publicProperties.Select(p => p.Name);

            foreach(var method in methods)
            {
                var value = elem.Element(df + method).Value;
                PropertyInfo property = publicProperties.Where(p => p.Name == method).FirstOrDefault();
                property.SetValue(this, Convert.ChangeType(value, property.PropertyType, null), null);
            }
        }
    }
}
