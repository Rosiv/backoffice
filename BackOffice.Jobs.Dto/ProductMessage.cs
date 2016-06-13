using BackOffice.Jobs.Interfaces;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace BackOffice.Jobs.Dto
{
    [XmlRoot(ElementName = "Message")]
    public class ProductMessage : IJobData
    {
        [XmlAttribute(AttributeName = "Action")]
        [JsonProperty("Action")]
        public string Action { get; set; }

        [JsonProperty("Product")]
        [XmlElement(ElementName = "Product")]
        public Product Product { get; set; }
    }
}
