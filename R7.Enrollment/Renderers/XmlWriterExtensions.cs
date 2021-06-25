using System.Xml;

namespace R7.Enrollment.Renderers
{
    static class XmlWriterExtensions
    {
        public static void WriteElementWithAttributeString (this XmlWriter writer, string localName, string value, string attrLocalName, string attrValue)
        {
            writer.WriteStartElement (localName);
            writer.WriteAttributeString (attrLocalName, attrValue);
            writer.WriteString (value);
            writer.WriteEndElement ();
        }

        public static void WriteStartElementWithAttributeString (this XmlWriter writer, string localName, string attrLocalName, string attrValue)
        {
            writer.WriteStartElement (localName);
            writer.WriteAttributeString (attrLocalName, attrValue);
        }
    }
}
