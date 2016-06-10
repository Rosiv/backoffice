using BackOffice.Jobs.Dto;
using BackOffice.Jobs.Interfaces;

namespace BackOffice.Jobs.Reports
{
    internal class AProductSimpleTxtReport : IJob<Product>
    {
        public AProductSimpleTxtReport(Product product)
        {
            this.Dto = product;
        }

        public string Type
        {
            get { return this.GetType().FullName; }
        }

        public string Name
        {
            get
            {
                return "Txt report for all 'A products'";
            }
        }

        public Product Dto { get; private set; }
    }
}
