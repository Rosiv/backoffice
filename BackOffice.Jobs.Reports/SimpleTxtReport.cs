using BackOffice.Jobs.Dto;
using BackOffice.Jobs.Interfaces;

namespace BackOffice.Jobs.Reports
{
    public class AProductSimpleTxtReport : IJob<ProductMessage>
    {
        public AProductSimpleTxtReport(ProductMessage message)
        {
            this.Data = message;
        }

        public string Type
        {
            get { return this.GetType().FullName; }
        }

        public string Name
        {
            get
            {
                return "Simple txt file report for all 'A products'";
            }
        }

        public ProductMessage Data { get; private set; }
    }
}
