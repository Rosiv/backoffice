using BackOffice.Jobs.Interfaces;

namespace BackOffice.Jobs.Dto
{
    public class Product : IJobDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
