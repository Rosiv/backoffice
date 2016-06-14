using BackOffice.Jobs.Interfaces;
using BackOffice.Jobs.Reports;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using System.IO;
using System.Threading;

namespace BackOffice.Worker
{
    public class BProductSimpleExcelReportWorker : IJobWorker
    {
        private readonly SimpleExcelReport report;
        private const string ReportFile = "Reports\\B_Products_Report.xls";

        public BProductSimpleExcelReportWorker(SimpleExcelReport report)
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
            using (FileStream fs = new FileStream("Reports\\B_Products_Report.xlsx", System.IO.FileMode.Open))
            {
                var workbook = WorkbookFactory.Create(fs);
                var worksheet = workbook.GetSheetAt(0);


                for (int lineNumber = 1; true; lineNumber++)
                {
                    var row = worksheet.GetRow(lineNumber);

                    if (row != null )
                    {
                        continue;
                    }

                    row = worksheet.CreateRow(0);
                    row.CreateCell(0);
                    row.CreateCell(1);
                    row.CreateCell(2);
                    row.CreateCell(3);

                    var whatHappenedCell = row.GetCell(0);
                    var productIdCell = row.GetCell(1);
                    var productNameCell = row.GetCell(2);
                    var productDescriptionCell = row.GetCell(3);

                    whatHappenedCell.SetCellValue(this.report.Data.Action);
                    productIdCell.SetCellValue(this.report.Data.Product.Id);
                    productNameCell.SetCellValue(this.report.Data.Product.Name);
                    productDescriptionCell.SetCellValue(this.report.Data.Product.Description);

                    workbook.Write(fs);
                    workbook.Close();
                }
            }
            var txt = string.Format("|{0}|{1}|{2}|{3}|\r\n",
                report.Data.Action.PadRight(15),
                report.Data.Product.Id.ToString().PadRight(4),
                report.Data.Product.Name.PadRight(15),
                report.Data.Product.Description.PadRight(20));

            //remove this
            Thread.Sleep(10000);

            File.AppendAllLines(ReportFile, new[] { txt });
        }
    }
}
