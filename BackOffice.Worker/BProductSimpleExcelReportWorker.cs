using BackOffice.Common;
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
        private const string ReportFile = "Reports\\B_Products_Report.xlsx";

        public BProductSimpleExcelReportWorker(SimpleExcelReport report)
        {
            this.report = report;
        }

        public void Start()
        {
            var fi = new FileInfo(ReportFile);

            if (!fi.Exists)
            {
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
            }

            IWorkbook workbook;
            using (FileStream fs = new FileStream(fi.FullName, System.IO.FileMode.OpenOrCreate, FileAccess.Read))
            {
                workbook = WorkbookFactory.Create(fs);
            }
            var worksheet = workbook.GetSheetAt(0);

            for (int lineNumber = 1; true; lineNumber++)
            {
                var row = worksheet.GetRow(lineNumber);

                if (row != null)
                {
                    continue;
                }

                row = worksheet.CreateRow(lineNumber);
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


                //remove this
                Thread.Sleep(10000);
                Logging.Log().Debug("Writing to file {file}", ReportFile);
                using (var fss = new FileStream(ReportFile, FileMode.Truncate, FileAccess.Write))
                    workbook.Write(fss);

                break;
            }
        }
    }
}
