using BackOffice.Jobs.Dto;
using BackOffice.Jobs.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BackOffice.Jobs.Reports
{
    public class SimpleTxtReport : IJob<ProductMessage>
    {
        public SimpleTxtReport(ProductMessage message)
        {
            this.Data = message;
        }

        public string Type
        {
            get { return this.GetType().AssemblyQualifiedName; }
        }

        public string Name
        {
            get
            {
                return "Simple txt file report.";
            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public JobStatus Status { get; set; }

        [JsonProperty("Data")]
        public ProductMessage Data { get; private set; }
    }
}
