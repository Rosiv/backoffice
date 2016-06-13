using BackOffice.Jobs.Reports;
using System.IO;

namespace BackOffice.Worker
{
    public class AProductSimpleTxtReportWorker
    {
        private readonly AProductSimpleTxtReport report;
        private const string ReportFile = "Reports\\A_Products_Report.txt";

        public AProductSimpleTxtReportWorker(AProductSimpleTxtReport report)
        {
            this.report = report;
        }

        public void Start()
        {
            if (!File.Exists(ReportFile))
            {
                if (!Directory.Exists("Reports"))
                {
                    Directory.CreateDirectory("Reports");
                }
            }

            var txt = string.Format("|{0}|{1}|{2}|{3}|\r\n",
                report.Data.Action.PadRight(15),
                report.Data.Product.Id.ToString().PadRight(4),
                report.Data.Product.Name.PadRight(15),
                report.Data.Product.Description.PadRight(20));

            File.AppendAllLines(ReportFile, new[] { txt });
        }
    }
}
