using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Aspose.Pdf.Facades;
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
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public TestResultListingObj sTestResultRow { get; set; }

        public TestResultTemplate(TestResultListingObj sRow)
        {
            this.sTestResultRow = sRow;
        }

        public void Compose(IDocumentContainer container)
        {
            var sBuilder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
            String sIconPath = sBuilder.Configuration.GetSection("PrintTemplate:IconPath").Value;
            String sDownloadPath = sBuilder.Configuration.GetSection("PrintTemplate:DownloadPath").Value;

            try
            {
                var sDocument = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        // -- Page -- //
                        page.Size(PageSizes.A3);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(10));
                        page.DefaultTextStyle(x => x.FontFamily("Noto Sans"));


                        // -- Header -- //
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

                                // -- Result Content -- //
                                c.Item().Row(row =>
                                {
                                    // -- Column Left -- //
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

                                    // -- Column Center --- //
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

                                    // -- Column Right --- //
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

                    });
                });

                String sFileName = "TestResult_" + sTestResultRow.TestResultType.Replace(" ", "") +
                   "_PatientID_" + sTestResultRow.PatientID.Replace(" ", "") + "_" +
                   DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";

                sDocument.GeneratePdf(System.IO.Path.Combine(sDownloadPath, sFileName));

                if (sTestResultRow.isPrint)
                {
                    Aspose.Pdf.Facades.PdfViewer v = new Aspose.Pdf.Facades.PdfViewer();
                    v.BindPdf(System.IO.Path.Combine(sDownloadPath, sFileName));

                    v.AutoResize = true;
                    v.AutoRotate = true;
                    v.PrintPageDialog = true;

                    //PrintDialog pdialog = new PrintDialog();
                    //if (pdialog.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    //v.PrintDocumentWithSetup();
                }
                else
                {
                    //String sFileName = "TestResult_" + sTestResultRow.TestResultType.Replace(" ", "") +
                    //                   "_PatientID_" + sTestResultRow.PatientID.Replace(" ", "") + "_" + 
                     //                  DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";

                    //sDocument.GeneratePdf(System.IO.Path.Combine(sDownloadPath, sFileName));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //public void Showsss()
        //{
        //    Document.Create(container =>
        //    {
        //        container.Page(page =>
        //        {
        //            page.Size(PageSizes.A4);
        //            page.Margin(2, Unit.Centimetre);
        //            page.PageColor(Colors.White);
        //            page.DefaultTextStyle(x => x.FontSize(20));

        //            page.Header()
        //                .Text("Hello PDF!")
        //                .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

        //            page.Content()
        //                .PaddingVertical(1, Unit.Centimetre)
        //                .Column(x =>
        //                {
        //                    x.Spacing(20);

        //                    x.Item().Text(Placeholders.LoremIpsum());
        //                    x.Item().Image(Placeholders.Image(200, 100));
        //                });

        //            page.Footer()
        //                .AlignCenter()
        //                .Text(x =>
        //                {
        //                    x.Span("Page ");
        //                    x.CurrentPageNumber();
        //                });
        //        });
        //    })
        //    .GeneratePdfAndShow();

        //    //.GeneratePdf("hello.pdf");
        //}
    }
}
