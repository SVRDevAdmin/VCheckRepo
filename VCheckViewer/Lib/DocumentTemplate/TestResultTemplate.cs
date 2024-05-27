using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using VCheck.Lib.Data.Models;
using Document = QuestPDF.Fluent.Document;

namespace VCheckViewer.Lib.DocumentTemplate
{
    public class TestResultTemplate : IDocument
    {
        private TestResultListingObj sTestResultRow { get; set; }

        public TestResultTemplate(TestResultListingObj sRow)
        {
            this.sTestResultRow = sRow;
        }

        private DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        private DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            var sBuilder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
            String sIconPath = sBuilder.Configuration.GetSection("PrintTemplate:IconPath").Value;
            String sDownloadPath = sBuilder.Configuration.GetSection("Configuration:DownloadFolderPath").Value;

            try
            {
                var sDocument = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A3);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(10));
                        page.DefaultTextStyle(x => x.FontFamily("Noto Sans"));

                        page.Header()
                            .Height(1, Unit.Centimetre)
                            .Image(sIconPath);

                        // -- Content -- //
                        page.Content()
                            .Column(c =>
                            {
                                c.Item().Height(20);
                                c.Item().Text("Test Results")
                                        .FontSize(11)
                                        .Bold();
                                c.Item().Height(10);

                                c.Item().Background("#f2f2f2")
                                        .PaddingLeft(5)
                                        .PaddingTop(10)
                                        .Height(20)
                                        .Text(text =>
                                        {
                                            text.Span("Patient ID : ").SemiBold();
                                            text.Span(sTestResultRow.PatientID).SemiBold();
                                        });

                                c.Item().Background("#f2f2f2")
                                        .PaddingLeft(5)
                                        .PaddingBottom(5)
                                        .Height(20)
                                        .Text(text =>
                                        {
                                            text.Span("Date & Time : ").SemiBold();
                                            text.Span((sTestResultRow.TestResultDateTime != null) ?
                                                       sTestResultRow.TestResultDateTime.Value.ToString("dd/MM/yyyy HH:mm") : "")
                                                .SemiBold();
                                        });

                                c.Item().Height(25).Background(Colors.White);

                                c.Item().Row(row =>
                                {
                                    row.RelativeItem().Column(c1 =>
                                    {

                                        c1.Item().Background("#f2f2f2")
                                                .BorderLeft(0)
                                                .BorderRight(25)
                                                .BorderTop(10)
                                                .BorderColor(Colors.White)
                                                .Height(250)
                                                .Width(150)
                                                .PaddingTop(35)
                                                .PaddingLeft(50)
                                                .AlignTop()
                                                .AlignLeft()
                                                .Text(text =>
                                                {
                                                    text.Span("Observation Type").Bold();
                                                    text.EmptyLine();
                                                    text.Span(sTestResultRow.TestResultType);
                                                    text.EmptyLine();
                                                    text.EmptyLine();
                                                    text.Span("Observation Result").Bold();
                                                    text.EmptyLine();
                                                    text.Span(sTestResultRow.TestResultStatus);
                                                    text.EmptyLine();
                                                    text.EmptyLine();
                                                    text.Span("Observation Value").Bold();
                                                    text.EmptyLine();
                                                    text.Span((sTestResultRow.TestResultValue != null) ?
                                                               sTestResultRow.TestResultValue.Value.ToString("F") : "");
                                                    text.EmptyLine();
                                                    text.EmptyLine();
                                                });
                                    });

                                    row.RelativeItem().Column(c2 =>
                                    {
                                        c2.Item().Background("#f2f2f2")
                                                .BorderLeft(25)
                                                .BorderRight(25)
                                                .BorderTop(10)
                                                .BorderColor(Colors.White)
                                                .Height(250)
                                                .Width(170)
                                                .PaddingTop(35)
                                                .PaddingLeft(60)
                                                .AlignTop()
                                                .AlignLeft()
                                                .Text(text =>
                                                {
                                                    text.Span("Observation DateTime").Bold();
                                                    text.EmptyLine();
                                                    text.Span((sTestResultRow.TestResultDateTime != null) ?
                                                               sTestResultRow.TestResultDateTime.Value.ToString("dd/MM/yyyy HH:mm") : "");
                                                    text.EmptyLine();
                                                    text.EmptyLine();
                                                    text.Span("Operator ID").Bold();
                                                    text.EmptyLine();
                                                    text.Span(sTestResultRow.OperatorID);
                                                    text.EmptyLine();
                                                    text.EmptyLine();
                                                });
                                    });

                                    row.RelativeItem().Column(c3 =>
                                    {
                                        c3.Item().Background("#f2f2f2")
                                                .BorderLeft(25)
                                                .BorderRight(0)
                                                .BorderTop(10)
                                                .BorderColor(Colors.White)
                                                .Height(250)
                                                .Width(150)
                                                .PaddingTop(35)
                                                .PaddingLeft(60)
                                                .PaddingRight(1)
                                                .AlignTop()
                                                .AlignLeft()
                                                .Text(text =>
                                                {
                                                    text.Span("Printed On").Bold();
                                                    text.EmptyLine();
                                                    text.Span((sTestResultRow.printedOn != null) ?
                                                               sTestResultRow.printedOn.Value.ToString("dd/MM/yyyy HH:mm") : "");
                                                    text.EmptyLine();
                                                    text.EmptyLine();
                                                    text.Span("Printed By").Bold();
                                                    text.EmptyLine();
                                                    text.Span(sTestResultRow.printedBy);
                                                    text.EmptyLine();
                                                    text.EmptyLine();
                                                });
                                    });
                                });
                            })
                            ;

                        page.Footer();
                    });
                });

                String sFileName = "TestResult_" + sTestResultRow.TestResultType.Replace(" ", "") +
                                   "_PatientID_" + sTestResultRow.PatientID.Replace(" ", "") + "_" +
                                   DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                String sOutputPath = System.IO.Path.Combine(sDownloadPath, sFileName);

                sDocument.GeneratePdf(sOutputPath);
                if (sTestResultRow.isPrint)
                {
                    ProcessStartInfo infoPrint = new ProcessStartInfo();
                    infoPrint.FileName = System.IO.Path.Combine(sDownloadPath, sFileName);
                    infoPrint.Verb = "PrintTo";
                    infoPrint.CreateNoWindow = false;
                    infoPrint.WindowStyle = ProcessWindowStyle.Normal;
                    infoPrint.UseShellExecute = true;

                    Process printProcess = new Process();
                    printProcess = Process.Start(infoPrint);
                }

                sDocument = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
