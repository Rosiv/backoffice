using BackOffice.Jobs.Dto;
using BackOffice.Jobs.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BackOffice.Jobs.Reports
{
    public class SimpleExcelReport : IJob<ProductMessage>
    {
        public SimpleExcelReport(ProductMessage message)
        {
            this.Data = message;
        }

        public string Id { get; set; }

        public string Type
        {
            get { return this.GetType().AssemblyQualifiedName; }
        }

        public string Name
        {
            get
            {
                return "Simple Excel report.";
            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public JobStatus Status { get; set; }

        [JsonProperty("Data")]
        public ProductMessage Data { get; private set; }
    }
}
