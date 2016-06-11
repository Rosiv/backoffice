using BackOffice.Jobs.Interfaces;
using System.Xml.Serialization;

namespace BackOffice.Jobs.Dto
{
    [XmlRoot(ElementName = "Message")]
    public class Product : IJobDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
