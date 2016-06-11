using BackOffice.Jobs.Interfaces;
using System.Xml.Serialization;

namespace BackOffice.Jobs.Dto
{
    [XmlRoot(ElementName = "Message")]
    public class ProductMessage : IJobData
    {
        [XmlAttribute(AttributeName = "Action")]
        public string Action { get; set; }

        [XmlElement(ElementName = "Product")]
        public Product Product { get; set; }
    }
}
