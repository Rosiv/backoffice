using BackOffice.Jobs.Interfaces;
using System.Xml.Serialization;

namespace BackOffice.Jobs.Dto
{
    public class Product : IJobData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
