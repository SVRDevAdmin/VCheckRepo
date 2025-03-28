using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using SkiaSharp;
using VCheck.Lib.Data;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.Function;
using Document = QuestPDF.Fluent.Document;

namespace VCheckViewer.Lib.DocumentTemplate
{
    public class TestResultTemplate : IDocument
    {
        //private TestResultListingExtendedObj sTestResultRow { get; set; }
        //private List<TestResultDetailsModel> sTestResultDetail { get; set; }
        //private List<TestResultDetailsModel> sPreviousTestResultDetail { get; set; }
        //private string sPreviousTestDateTime { get; set; }
        private List<DownloadPrintResultModel> sDownloadPrintResultModels { get; set; }
        private bool sIsPrint { get; set; }

        public ConfigurationDBContext configDBContext = new ConfigurationDBContext(ConfigSettings.GetConfigurationSettings());

        //public TestResultTemplate(TestResultListingExtendedObj sTestResultRow, List<TestResultDetailsModel> sTestResultDetail, List<TestResultDetailsModel> sPreviousTestResultDetail, string sPreviousTestDateTime)
        //{
        //    this.sTestResultRow = sTestResultRow;
        //    this.sTestResultDetail = sTestResultDetail;
        //    this.sPreviousTestResultDetail = sPreviousTestResultDetail;
        //    this.sPreviousTestDateTime = sPreviousTestDateTime;
        //}

        public TestResultTemplate(List<DownloadPrintResultModel> sDownloadPrintResultModels, bool sIsPrint)
        {
            this.sDownloadPrintResultModels = sDownloadPrintResultModels;
            this.sIsPrint = sIsPrint;
        }

        private DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        private DocumentSettings GetSettings() => DocumentSettings.Default;

        //public void Compose(IDocumentContainer container)
        //{
        //    var sReportImagePath = configDBContext.GetConfigurationData("ReportImagePath").FirstOrDefault();
        //    var sClinicName = configDBContext.GetConfigurationData("ClinicName").FirstOrDefault();
        //    var sReportTitle = configDBContext.GetConfigurationData("ReportTitle").FirstOrDefault();
        //    var sClinicAddress = configDBContext.GetConfigurationData("ClinicAddress").FirstOrDefault();
        //    var sBuilder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
        //    String sIconConfigPath = sBuilder.Configuration.GetSection("PrintTemplate:IconPath").Value;
        //    String sFooterLogo = System.Environment.CurrentDirectory + sIconConfigPath;
        //    String sIconPath = sReportImagePath != null && File.Exists(sReportImagePath.ConfigurationValue) ? sReportImagePath.ConfigurationValue : sFooterLogo;
        //    String sDownloadPath = sBuilder.Configuration.GetSection("Configuration:DownloadFolderPath").Value;

        //    try
        //    {
        //        var sDocument = Document.Create(container =>
        //        {
        //            container.Page(page =>
        //            {
        //                page.Size(PageSizes.A3);
        //                page.MarginHorizontal(2, Unit.Centimetre);
        //                page.MarginBottom(1, Unit.Centimetre);
        //                page.PageColor(Colors.White);
        //                page.DefaultTextStyle(x => x.FontSize(10));
        //                page.DefaultTextStyle(x => x.FontFamily("Noto Sans"));

        //                //page.Header()
        //                //    .Height(5, Unit.Centimetre)
        //                //    .Image(sIconPath);

        //                page.Header().BorderBottom(1).Column(c =>
        //                {
        //                    c.Item().Row(row =>
        //                    {
        //                        row.ConstantItem(7, Unit.Centimetre).PaddingLeft(20).PaddingRight(20).AlignCenter().AlignMiddle().Image(sIconPath);

        //                        row.RelativeItem().AlignMiddle()
        //                        .Column(middleColumn =>
        //                        {

        //                            middleColumn.Item().Column(col =>
        //                            {
        //                                col.Item()
        //                                    .PaddingRight(5)
        //                                    .Text(text =>
        //                                    {
        //                                        text.Span((sReportTitle != null) ? sReportTitle.ConfigurationValue : "").Bold();
        //                                    });
        //                            });

        //                            middleColumn.Item().Column(col =>
        //                            {
        //                                col.Item()
        //                                    .PaddingRight(5)
        //                                    .PaddingTop(10)
        //                                    .Text(text =>
        //                                    {
        //                                        text.Span((sClinicName != null) ? sClinicName.ConfigurationValue : "").Bold();
        //                                    });
        //                            });

        //                            middleColumn.Item().Column(col =>
        //                            {
        //                                col.Item()
        //                                    .PaddingRight(5)
        //                                    .PaddingTop(10)
        //                                    .AlignLeft()
        //                                    .Text(text =>
        //                                    {
        //                                        text.Span((sClinicAddress != null) ? sClinicAddress.ConfigurationValue : "").Bold();
        //                                    });
        //                            });
        //                        });

        //                        row.ConstantItem(7, Unit.Centimetre).AlignCenter().AlignMiddle()
        //                        .Column(rightColumn =>
        //                        {
        //                            rightColumn.Item().Column(col =>
        //                            {
        //                                col.Item()
        //                                    .PaddingLeft(5)
        //                                    .PaddingTop(10)
        //                                    .Height(20)
        //                                    .Text(text =>
        //                                    {
        //                                        text.Span(Properties.Resources.Results_Label_PatientID + " : ").Bold();
        //                                        text.Span(sTestResultRow.PatientID);
        //                                    });
        //                            });

        //                            rightColumn.Item().Column(col =>
        //                            {
        //                                col.Item()
        //                                    .PaddingLeft(5)
        //                                    .PaddingTop(10)
        //                                    .Height(20)
        //                                    .Text(text =>
        //                                    {
        //                                        text.Span(Properties.Resources.Results_Label_PatientName + " : ").Bold();
        //                                        text.Span(sTestResultRow.PatientName);
        //                                    });
        //                            });

        //                            rightColumn.Item().Column(col =>
        //                            {
        //                                col.Item()
        //                                    .PaddingLeft(5)
        //                                    .PaddingTop(10)
        //                                    .Height(20)
        //                                    .Text(text =>
        //                                    {
        //                                        text.Span(Properties.Resources.Report_Label_PrintedOn + " : ").Bold();
        //                                        text.Span((sTestResultRow.printedOn != null) ?
        //                                                   sTestResultRow.printedOn.Value.ToString("dd/MM/yyyy HH:mm") : "");
        //                                    });
        //                            });

        //                            rightColumn.Item().Column(col =>
        //                            {
        //                                col.Item()
        //                                    .PaddingLeft(5)
        //                                    .PaddingTop(10)
        //                                    .Height(20)
        //                                    .Text(text =>
        //                                    {
        //                                        text.Span(Properties.Resources.Report_Label_PrintedBy + " : ").Bold();
        //                                        text.Span(sTestResultRow.printedBy);
        //                                    });
        //                            });
        //                        });
        //                    });
        //                });

        //                // -- Content -- //
        //                page.Content()
        //                    .Column(c =>
        //                    {
        //                        //c.Item().Height(20);
        //                        //c.Item().Text("Test Results")
        //                        //        .FontSize(12)
        //                        //        .Bold();
        //                        //c.Item().Height(10);

        //                        //c.Item().Row(row =>
        //                        //{
        //                        //    row.ConstantItem(7, Unit.Centimetre).PaddingLeft(20).PaddingRight(20).AlignCenter().AlignMiddle().Image(sIconPath);

        //                        //    row.RelativeItem().AlignMiddle()
        //                        //    .Column(middleColumn =>
        //                        //    {

        //                        //        middleColumn.Item().Column(col =>
        //                        //        {
        //                        //            col.Item()
        //                        //                .PaddingRight(5)
        //                        //                .Text(text =>
        //                        //                {
        //                        //                    text.Span((sReportTitle != null) ? sReportTitle.ConfigurationValue : "").Bold();
        //                        //                });
        //                        //        });

        //                        //        middleColumn.Item().Column(col =>
        //                        //        {
        //                        //            col.Item()
        //                        //                .PaddingRight(5)
        //                        //                .PaddingTop(10)
        //                        //                .Text(text =>
        //                        //                {
        //                        //                    text.Span((sClinicName != null) ? sClinicName.ConfigurationValue : "").Bold();
        //                        //                });
        //                        //        });

        //                        //        middleColumn.Item().Column(col =>
        //                        //        {
        //                        //            col.Item()
        //                        //                .PaddingRight(5)
        //                        //                .PaddingTop(10)
        //                        //                .AlignLeft()
        //                        //                .Text(text =>
        //                        //                {
        //                        //                    text.Span((sClinicAddress != null) ? sClinicAddress.ConfigurationValue : "").Bold();
        //                        //                });
        //                        //        });
        //                        //    });

        //                        //    row.ConstantItem(7, Unit.Centimetre)
        //                        //    .Column(rightColumn =>
        //                        //    {
        //                        //        rightColumn.Item().Column(col =>
        //                        //        {
        //                        //            col.Item()
        //                        //                .PaddingLeft(5)
        //                        //                .PaddingTop(10)
        //                        //                .Height(20)
        //                        //                .Text(text =>
        //                        //                {
        //                        //                    text.Span(Properties.Resources.Results_Label_PatientID + " : ").Bold();
        //                        //                    text.Span(sTestResultRow.PatientID);
        //                        //                });
        //                        //        });

        //                        //        rightColumn.Item().Column(col =>
        //                        //        {
        //                        //            col.Item()
        //                        //                .PaddingLeft(5)
        //                        //                .PaddingTop(10)
        //                        //                .Height(20)
        //                        //                .Text(text =>
        //                        //                {
        //                        //                    text.Span(Properties.Resources.Results_Label_PatientName + " : ").Bold();
        //                        //                    text.Span(sTestResultRow.PatientName);
        //                        //                });
        //                        //        });

        //                        //        rightColumn.Item().Column(col =>
        //                        //        {
        //                        //            col.Item()
        //                        //                .PaddingLeft(5)
        //                        //                .PaddingTop(10)
        //                        //                .Height(20)
        //                        //                .Text(text =>
        //                        //                {
        //                        //                    text.Span(Properties.Resources.Report_Label_DateTime + " : ").Bold();
        //                        //                    text.Span((sTestResultRow.TestResultDateTime != null) ?
        //                        //                               sTestResultRow.TestResultDateTime.Value.ToString("dd/MM/yyyy HH:mm") : "");
        //                        //                });
        //                        //        });

        //                        //        rightColumn.Item().Column(col =>
        //                        //        {
        //                        //            col.Item()
        //                        //                .PaddingLeft(5)
        //                        //                .PaddingTop(10)
        //                        //                .Height(20)
        //                        //                .Text(text =>
        //                        //                {
        //                        //                    text.Span(Properties.Resources.Report_Label_PrintedOn + " : ").Bold();
        //                        //                    text.Span((sTestResultRow.printedOn != null) ?
        //                        //                               sTestResultRow.printedOn.Value.ToString("dd/MM/yyyy HH:mm") : "");
        //                        //                });
        //                        //        });

        //                        //        rightColumn.Item().Column(col =>
        //                        //        {
        //                        //            col.Item()
        //                        //                .PaddingLeft(5)
        //                        //                .PaddingTop(10)
        //                        //                .Height(20)
        //                        //                .Text(text =>
        //                        //                {
        //                        //                    text.Span(Properties.Resources.Report_Label_PrintedBy + " : ").Bold();
        //                        //                    text.Span(sTestResultRow.printedBy);
        //                        //                });
        //                        //        });
        //                        //    });
        //                        //});

        //                        c.Item().Height(25);

        //                        c.Item().Height(20).Text(text =>
        //                        {
        //                            text.Span("C10 ").ExtraBold().FontSize(15);
        //                            text.Span(" (" + sTestResultRow.TestResultDateTime.Value.ToString("dd/MM/yyyy HH:mm") + ")").FontSize(8);
        //                        });

        //                        c.Item().Border(1).Row(tableRow =>
        //                        {
        //                            tableRow.RelativeItem().Column(column =>
        //                            {
        //                                column.Item().Background("#f2f2f2").Row(row =>
        //                                {
        //                                    row.RelativeItem()
        //                                    .Column(leftColumn =>
        //                                    {
        //                                        leftColumn.Item().Column(col =>
        //                                        {
        //                                            col.Item().Background("#f2f2f2")
        //                                                .PaddingLeft(5)
        //                                                .PaddingTop(10)
        //                                                .Height(20)
        //                                                .AlignCenter()
        //                                                .Text(Properties.Resources.Report_Label_Parameter)
        //                                                .Bold();
        //                                        });
        //                                    });

        //                                    row.RelativeItem()
        //                                    .Column(leftColumn =>
        //                                    {
        //                                        leftColumn.Item().Column(col =>
        //                                        {
        //                                            col.Item().Background("#f2f2f2")
        //                                                .PaddingLeft(5)
        //                                                .PaddingTop(10)
        //                                                .Height(20)
        //                                                .AlignCenter()
        //                                                .Text(Properties.Resources.Results_Label_Result)
        //                                                .Bold();
        //                                        });
        //                                    });

        //                                    row.RelativeItem()
        //                                    .Column(leftColumn =>
        //                                    {
        //                                        leftColumn.Item().Column(col =>
        //                                        {
        //                                            col.Item().Background("#f2f2f2")
        //                                                .PaddingLeft(5)
        //                                                .PaddingTop(10)
        //                                                .Height(20)
        //                                                .AlignCenter()
        //                                                .Text(Properties.Resources.Report_Label_Reference)
        //                                                .Bold();
        //                                        });
        //                                    });

        //                                    row.RelativeItem()
        //                                    .Column(leftColumn =>
        //                                    {
        //                                        leftColumn.Item().Column(col =>
        //                                        {
        //                                            col.Item().Background("#f2f2f2")
        //                                                .PaddingLeft(5)
        //                                                .PaddingTop(10)
        //                                                .Height(20)
        //                                                .AlignCenter()
        //                                                .Text(Properties.Resources.Results_Label_Units)
        //                                                .Bold();
        //                                        });
        //                                    });

        //                                    row.RelativeItem()
        //                                    .Column(leftColumn =>
        //                                    {
        //                                        leftColumn.Item().Column(col =>
        //                                        {
        //                                            col.Item().Background("#f2f2f2")
        //                                                .PaddingLeft(5)
        //                                                .PaddingTop(10)
        //                                                .Height(20)
        //                                                .AlignCenter()
        //                                                .Text(" ")
        //                                                .Bold();
        //                                        });
        //                                    });

        //                                    row.RelativeItem()
        //                                    .Column(leftColumn =>
        //                                    {
        //                                        leftColumn.Item().Column(col =>
        //                                        {
        //                                            col.Item().Background("#f2f2f2")
        //                                                .PaddingLeft(5)
        //                                                .PaddingTop(10)
        //                                                .Height(20)
        //                                                .AlignCenter()
        //                                                .Text("Previous Records")
        //                                                .Bold();
        //                                        });

        //                                        leftColumn.Item().Column(col =>
        //                                        {
        //                                            col.Item().Background("#f2f2f2")
        //                                                .PaddingLeft(5)
        //                                                .AlignTop()
        //                                                .AlignCenter()
        //                                                .Text("(" + sPreviousTestDateTime + ")")
        //                                                .FontSize(8);
        //                                        });
        //                                    });
        //                                });

        //                                column.Item().Row(row =>
        //                                {
        //                                    row.RelativeItem()
        //                                    .Column(leftColumn =>
        //                                    {
        //                                        leftColumn.Item().Column(col =>
        //                                        {
        //                                            col.Item()
        //                                                .PaddingLeft(5)
        //                                                .PaddingTop(1)
        //                                                .Height(15)
        //                                                .AlignCenter()
        //                                                .Text(" ")
        //                                                .Bold();
        //                                        });
        //                                    });

        //                                    row.RelativeItem()
        //                                    .Column(leftColumn =>
        //                                    {
        //                                        leftColumn.Item().Column(col =>
        //                                        {
        //                                            col.Item()
        //                                                .PaddingLeft(5)
        //                                                .PaddingTop(1)
        //                                                .Height(15)
        //                                                .AlignCenter()
        //                                                .Text(" ")
        //                                                .Bold();
        //                                        });
        //                                    });

        //                                    row.RelativeItem()
        //                                    .Column(leftColumn =>
        //                                    {
        //                                        leftColumn.Item().Column(col =>
        //                                        {
        //                                            col.Item()
        //                                                .PaddingLeft(5)
        //                                                .PaddingTop(1)
        //                                                .Height(15)
        //                                                .AlignCenter()
        //                                                .Text(" ")
        //                                                .Bold();
        //                                        });
        //                                    });

        //                                    row.RelativeItem()
        //                                    .Column(leftColumn =>
        //                                    {
        //                                        leftColumn.Item().Column(col =>
        //                                        {
        //                                            col.Item()
        //                                                .PaddingLeft(5)
        //                                                .PaddingTop(1)
        //                                                .Height(15)
        //                                                .AlignCenter()
        //                                                .Text(" ")
        //                                                .Bold();
        //                                        });
        //                                    });

        //                                    //row.RelativeItem().PaddingRight(5).PaddingTop(1).AlignCenter().AlignMiddle().Height(15).Width(140).Layers(layer =>
        //                                    //{
        //                                    //    // Background bar
        //                                    //    layer.PrimaryLayer().PaddingRight((float)93.333).Text(Properties.Resources.Report_Label_Low).AlignCenter().FontSize(10);

        //                                    //    layer.Layer().PaddingLeft((float)46.666).PaddingRight((float)46.666).Text(Properties.Resources.Schedule_Label_Normal).AlignCenter().FontSize(10);


        //                                    //    layer.Layer().PaddingLeft((float)93.333).Text(Properties.Resources.Report_Label_High).AlignCenter().FontSize(10);
        //                                    //});



        //                                    row.RelativeItem().PaddingRight(5).PaddingTop(1).AlignCenter().AlignMiddle().Height(15).Width(110).Layers(layer =>
        //                                    {
        //                                        // Background bar
        //                                        layer.PrimaryLayer().PaddingRight((float)73.3333).Text(Properties.Resources.Report_Label_Low).AlignCenter().FontSize(10);

        //                                        layer.Layer().PaddingLeft((float)36.6666).PaddingRight((float)36.6666).Text(Properties.Resources.Schedule_Label_Normal).AlignCenter().FontSize(10);


        //                                        layer.Layer().PaddingLeft((float)73.3333).Text(Properties.Resources.Report_Label_High).AlignCenter().FontSize(10);
        //                                    });

        //                                    row.RelativeItem()
        //                                    .Column(leftColumn =>
        //                                    {
        //                                        leftColumn.Item().Column(col =>
        //                                        {
        //                                            col.Item()
        //                                                .PaddingLeft(5)
        //                                                .PaddingTop(1)
        //                                                .Height(15)
        //                                                .AlignCenter()
        //                                                .Text(" ")
        //                                                .Bold();
        //                                        });
        //                                    });
        //                                });

        //                                foreach (var d in sTestResultDetail)
        //                                {
        //                                    column.Item().Row(row =>
        //                                    {
        //                                        row.RelativeItem()
        //                                        .Column(leftColumn =>
        //                                        {
        //                                            leftColumn.Item().Column(col =>
        //                                            {
        //                                                col.Item()
        //                                                    .PaddingLeft(5)
        //                                                    .PaddingTop(10)
        //                                                    .Height(20)
        //                                                    .AlignCenter()
        //                                                    .Text(d.TestParameter);
        //                                            });
        //                                        });

        //                                        row.RelativeItem()
        //                                        .Column(leftColumn =>
        //                                        {
        //                                            leftColumn.Item().Column(col =>
        //                                            {
        //                                                col.Item()
        //                                                    .PaddingLeft(5)
        //                                                    .PaddingTop(10)
        //                                                    .Height(20)
        //                                                    .AlignCenter()
        //                                                    .Text(d.TestResultValue);
        //                                            });
        //                                        });

        //                                        var reference = "-";
        //                                        var paddingLeft = 0;
        //                                        var paddingRight = 0;
        //                                        var haveIndicator = false;

        //                                        if (!string.IsNullOrEmpty(d.ReferenceRange))
        //                                        {
        //                                            var range = d.ReferenceRange.Replace("[", "").Replace("]", "").Contains(";") ? d.ReferenceRange.Split(";") : d.ReferenceRange.Split("-");
        //                                            reference = range[0] + " - " + range[1];
        //                                            paddingLeft = (CalculateRange(float.Parse(range[0].Replace("[", "")), float.Parse(range[1].Replace("]", "")), float.Parse(d.TestResultValue))) - 2;
        //                                            //paddingRight = 140 - paddingLeft - 2;
        //                                            paddingRight = 110 - paddingLeft - 2;
        //                                            haveIndicator = true;
        //                                        }

        //                                        row.RelativeItem()
        //                                        .Column(leftColumn =>
        //                                        {
        //                                            leftColumn.Item().Column(col =>
        //                                            {
        //                                                col.Item()
        //                                                    .PaddingLeft(5)
        //                                                    .PaddingTop(10)
        //                                                    .Height(20)
        //                                                    .AlignCenter()
        //                                                    .Text(reference);
        //                                            });
        //                                        });

        //                                        row.RelativeItem()
        //                                        .Column(leftColumn =>
        //                                        {
        //                                            leftColumn.Item().Column(col =>
        //                                            {
        //                                                col.Item()
        //                                                    .PaddingLeft(5)
        //                                                    .PaddingTop(10)
        //                                                    .Height(20)
        //                                                    .AlignCenter()
        //                                                    .Text(string.IsNullOrEmpty(d.TestResultUnit) ? "-" : d.TestResultUnit);
        //                                            });
        //                                        });

        //                                        //row.RelativeItem().PaddingRight(5).PaddingTop(1).AlignCenter().AlignMiddle().Height(15).Width(140).Layers(layer =>
        //                                        //{
        //                                        //    if (haveIndicator)
        //                                        //    {
        //                                        //        // Background bar
        //                                        //        layer.PrimaryLayer().Border(1);

        //                                        //        layer.Layer().PaddingRight((float)93.333).BorderRight(1);


        //                                        //        layer.Layer().PaddingLeft((float)93.333).BorderLeft(1);


        //                                        //        layer.Layer().PaddingLeft(paddingLeft).PaddingRight(paddingRight).Background(Colors.Black);
        //                                        //    }
        //                                        //    else
        //                                        //    {
        //                                        //        layer.PrimaryLayer().Text("-").AlignCenter();
        //                                        //    }
        //                                        //});

        //                                        row.RelativeItem().PaddingRight(5).PaddingTop(1).AlignCenter().AlignMiddle().Height(15).Width(110).Layers(layer =>
        //                                        {
        //                                            if (haveIndicator)
        //                                            {
        //                                                // Background bar
        //                                                layer.PrimaryLayer().Border(1);

        //                                                layer.Layer().PaddingRight((float)73.3333).BorderRight(1);


        //                                                layer.Layer().PaddingLeft((float)73.3333).BorderLeft(1);


        //                                                layer.Layer().PaddingLeft(paddingLeft).PaddingRight(paddingRight).Background(Colors.Black);
        //                                            }
        //                                            else
        //                                            {
        //                                                layer.PrimaryLayer().Text("-").AlignCenter();
        //                                            }
        //                                        });

        //                                        var previousParameter = sPreviousTestResultDetail.FirstOrDefault(x => x.TestParameter == d.TestParameter);

        //                                        row.RelativeItem()
        //                                        .Column(leftColumn =>
        //                                        {
        //                                            leftColumn.Item().Column(col =>
        //                                            {
        //                                                col.Item()
        //                                                    .PaddingLeft(5)
        //                                                    .PaddingTop(10)
        //                                                    .Height(20)
        //                                                    .AlignCenter()
        //                                                    .Text((previousParameter != null && !string.IsNullOrEmpty(previousParameter.TestResultValue)) ? previousParameter.TestResultValue : "-");
        //                                            });
        //                                        });

        //                                    });
        //                                }
        //                            });
        //                        });    
        //                    });

        //                page.Footer().AlignRight()
        //                .Row(row =>
        //                {
        //                    row.ConstantItem(3, Unit.Centimetre).Text(Properties.Resources.Report_Label_PoweredBy).FontColor("#aeb3b5").Bold();
        //                    row.ConstantItem(4, Unit.Centimetre).Image(sFooterLogo);
        //                });

        //            });
        //        });

        //        if (sTestResultRow.isPrint)
        //        {
        //            //App.FilePath = System.IO.Path.Combine(sDownloadPath, "Report-temp.pdf");
        //            App.FilePath = "Report-temp.pdf";
        //            sDocument.GeneratePdf(App.FilePath);

        //            Process process = new Process();
        //            process.StartInfo = new ProcessStartInfo()
        //            {
        //                FileName = App.FilePath,
        //                UseShellExecute = true
        //            };
        //            process.Start();
        //        }
        //        else
        //        {
        //            var testResultType = string.IsNullOrEmpty(sTestResultRow.TestResultType) ? "General" : sTestResultRow.TestResultType.Replace(" ", "");
        //            var patientID = string.IsNullOrEmpty(sTestResultRow.PatientID) ? "General" : sTestResultRow.PatientID.Replace(" ", "");
        //            String sFileName = "TestResult_" + testResultType +
        //                               "_PatientID_" + patientID + "_" +
        //                               DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
        //            String sOutputPath = System.IO.Path.Combine(sDownloadPath, sFileName);
        //            sDocument.GeneratePdf(sOutputPath);
        //        }

        //        sDocument = null;
        //        App.ErrorOccur = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex;
        //        App.ErrorOccur = true;
        //    }

        //}

        public void Compose(IDocumentContainer container)
        {
            var sReportImagePath = configDBContext.GetConfigurationData("ReportImagePath").FirstOrDefault();
            var sClinicName = configDBContext.GetConfigurationData("ClinicName").FirstOrDefault();
            var sReportTitle = configDBContext.GetConfigurationData("ReportTitle").FirstOrDefault();
            var sClinicAddress = configDBContext.GetConfigurationData("ClinicAddress").FirstOrDefault();
            var sBuilder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
            String sIconConfigPath = sBuilder.Configuration.GetSection("PrintTemplate:IconPath").Value;
            String sFooterLogo = System.Environment.CurrentDirectory + sIconConfigPath;
            String sIconPath = sReportImagePath != null && File.Exists(sReportImagePath.ConfigurationValue) ? sReportImagePath.ConfigurationValue : sFooterLogo;
            String sDownloadPath = sBuilder.Configuration.GetSection("Configuration:DownloadFolderPath").Value;

            try
            {
                var sDocument = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A3);
                        page.MarginHorizontal(2, Unit.Centimetre);
                        page.MarginBottom(1, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(10));
                        page.DefaultTextStyle(x => x.FontFamily("Noto Sans"));

                        //page.Header()
                        //    .Height(5, Unit.Centimetre)
                        //    .Image(sIconPath);

                        page.Header().BorderBottom(1).Column(c =>
                        {
                            c.Item().Row(row =>
                            {
                                row.ConstantItem(7, Unit.Centimetre).PaddingLeft(20).PaddingRight(20).AlignCenter().AlignMiddle().Image(sIconPath);

                                row.RelativeItem().AlignMiddle()
                                .Column(middleColumn =>
                                {

                                    middleColumn.Item().Column(col =>
                                    {
                                        col.Item()
                                            .PaddingRight(5)
                                            .Text(text =>
                                            {
                                                text.Span((sReportTitle != null) ? sReportTitle.ConfigurationValue : "").Bold();
                                            });
                                    });

                                    middleColumn.Item().Column(col =>
                                    {
                                        col.Item()
                                            .PaddingRight(5)
                                            .PaddingTop(10)
                                            .Text(text =>
                                            {
                                                text.Span((sClinicName != null) ? sClinicName.ConfigurationValue : "").Bold();
                                            });
                                    });

                                    middleColumn.Item().Column(col =>
                                    {
                                        col.Item()
                                            .PaddingRight(5)
                                            .PaddingTop(10)
                                            .AlignLeft()
                                            .Text(text =>
                                            {
                                                text.Span((sClinicAddress != null) ? sClinicAddress.ConfigurationValue : "").Bold();
                                            });
                                    });
                                });

                                row.ConstantItem(7, Unit.Centimetre).AlignCenter().AlignMiddle()
                                .Column(rightColumn =>
                                {
                                    rightColumn.Item().Column(col =>
                                    {
                                        col.Item()
                                            .PaddingLeft(5)
                                            .PaddingTop(10)
                                            .Height(20)
                                            .Text(text =>
                                            {
                                                text.Span(Properties.Resources.Results_Label_PatientID + " : ").Bold();
                                                text.Span(sDownloadPrintResultModels[0].TestResult.PatientID);
                                            });
                                    });

                                    rightColumn.Item().Column(col =>
                                    {
                                        col.Item()
                                            .PaddingLeft(5)
                                            .PaddingTop(10)
                                            .Height(20)
                                            .Text(text =>
                                            {
                                                text.Span(Properties.Resources.Results_Label_PatientName + " : ").Bold();
                                                text.Span(sDownloadPrintResultModels[0].TestResult.PatientName);
                                            });
                                    });

                                    rightColumn.Item().Column(col =>
                                    {
                                        col.Item()
                                            .PaddingLeft(5)
                                            .PaddingTop(10)
                                            .Height(20)
                                            .Text(text =>
                                            {
                                                text.Span(Properties.Resources.Report_Label_PrintedOn + " : ").Bold();
                                                text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                                            });
                                    });

                                    rightColumn.Item().Column(col =>
                                    {
                                        col.Item()
                                            .PaddingLeft(5)
                                            .PaddingTop(10)
                                            .Height(20)
                                            .Text(text =>
                                            {
                                                text.Span(Properties.Resources.Report_Label_PrintedBy + " : ").Bold();
                                                text.Span(App.MainViewModel.CurrentUsers.FullName);
                                            });
                                    });
                                });
                            });
                        });

                        // -- Content -- //
                        page.Content()
                            .Column(c =>
                            {                                
                                foreach(var test in sDownloadPrintResultModels)
                                {
                                    c.Item().ShowEntire().Column(c =>
                                    {
                                        c.Item().Height(25);

                                        c.Item().Height(20).Text(text =>
                                        {
                                            text.Span(DeviceRepository.GetDeviceNameBySerialNo(ConfigSettings.GetConfigurationSettings(), test.TestResult.DeviceSerialNo) + " ").ExtraBold().FontSize(15);
                                            text.Span(" (" + test.TestResult.TestResultDateTime.Value.ToString("dd/MM/yyyy HH:mm") + ")").FontSize(8);
                                        });

                                        c.Item().Border(1).Row(tableRow =>
                                        {
                                            tableRow.RelativeItem().Column(column =>
                                            {
                                                column.Item().Background("#f2f2f2").Row(row =>
                                                {
                                                    row.RelativeItem()
                                                    .Column(leftColumn =>
                                                    {
                                                        leftColumn.Item().Column(col =>
                                                        {
                                                            col.Item().Background("#f2f2f2")
                                                                .PaddingLeft(5)
                                                                .PaddingTop(10)
                                                                .Height(20)
                                                                .AlignCenter()
                                                                .Text(Properties.Resources.Report_Label_Parameter)
                                                                .Bold();
                                                        });
                                                    });

                                                    row.RelativeItem()
                                                    .Column(leftColumn =>
                                                    {
                                                        leftColumn.Item().Column(col =>
                                                        {
                                                            col.Item().Background("#f2f2f2")
                                                                .PaddingLeft(5)
                                                                .PaddingTop(10)
                                                                .Height(20)
                                                                .AlignCenter()
                                                                .Text(Properties.Resources.Results_Label_Result)
                                                                .Bold();
                                                        });
                                                    });

                                                    row.RelativeItem()
                                                    .Column(leftColumn =>
                                                    {
                                                        leftColumn.Item().Column(col =>
                                                        {
                                                            col.Item().Background("#f2f2f2")
                                                                .PaddingLeft(5)
                                                                .PaddingTop(10)
                                                                .Height(20)
                                                                .AlignCenter()
                                                                .Text(Properties.Resources.Report_Label_Reference)
                                                                .Bold();
                                                        });
                                                    });

                                                    row.RelativeItem()
                                                    .Column(leftColumn =>
                                                    {
                                                        leftColumn.Item().Column(col =>
                                                        {
                                                            col.Item().Background("#f2f2f2")
                                                                .PaddingLeft(5)
                                                                .PaddingTop(10)
                                                                .Height(20)
                                                                .AlignCenter()
                                                                .Text(Properties.Resources.Results_Label_Units)
                                                                .Bold();
                                                        });
                                                    });

                                                    row.RelativeItem()
                                                    .Column(leftColumn =>
                                                    {
                                                        leftColumn.Item().Column(col =>
                                                        {
                                                            col.Item().Background("#f2f2f2")
                                                                .PaddingLeft(5)
                                                                .PaddingTop(10)
                                                                .Height(20)
                                                                .AlignCenter()
                                                                .Text(" ")
                                                                .Bold();
                                                        });
                                                    });

                                                    row.RelativeItem()
                                                    .Column(leftColumn =>
                                                    {
                                                        leftColumn.Item().Column(col =>
                                                        {
                                                            col.Item().Background("#f2f2f2")
                                                                .PaddingLeft(5)
                                                                .PaddingTop(10)
                                                                .Height(20)
                                                                .AlignCenter()
                                                                .Text("Previous Records")
                                                                .Bold();
                                                        });

                                                        var previousTestDateTime = "-";

                                                        if (test.PreviousTestResult != null)
                                                        {
                                                            previousTestDateTime = test.PreviousTestResult.TestResultDateTime.Value.ToString("dd/MM/yyyy hh:mm");
                                                        }

                                                        leftColumn.Item().Column(col =>
                                                        {
                                                            col.Item().Background("#f2f2f2")
                                                                .PaddingLeft(5)
                                                                .AlignTop()
                                                                .AlignCenter()
                                                                .Text("(" + previousTestDateTime + ")")
                                                                .FontSize(8);
                                                        });
                                                    });
                                                });

                                                column.Item().Row(row =>
                                                {
                                                    row.RelativeItem()
                                                    .Column(leftColumn =>
                                                    {
                                                        leftColumn.Item().Column(col =>
                                                        {
                                                            col.Item()
                                                                .PaddingLeft(5)
                                                                .PaddingTop(1)
                                                                .Height(15)
                                                                .AlignCenter()
                                                                .Text(" ")
                                                                .Bold();
                                                        });
                                                    });

                                                    row.RelativeItem()
                                                    .Column(leftColumn =>
                                                    {
                                                        leftColumn.Item().Column(col =>
                                                        {
                                                            col.Item()
                                                                .PaddingLeft(5)
                                                                .PaddingTop(1)
                                                                .Height(15)
                                                                .AlignCenter()
                                                                .Text(" ")
                                                                .Bold();
                                                        });
                                                    });

                                                    row.RelativeItem()
                                                    .Column(leftColumn =>
                                                    {
                                                        leftColumn.Item().Column(col =>
                                                        {
                                                            col.Item()
                                                                .PaddingLeft(5)
                                                                .PaddingTop(1)
                                                                .Height(15)
                                                                .AlignCenter()
                                                                .Text(" ")
                                                                .Bold();
                                                        });
                                                    });

                                                    row.RelativeItem()
                                                    .Column(leftColumn =>
                                                    {
                                                        leftColumn.Item().Column(col =>
                                                        {
                                                            col.Item()
                                                                .PaddingLeft(5)
                                                                .PaddingTop(1)
                                                                .Height(15)
                                                                .AlignCenter()
                                                                .Text(" ")
                                                                .Bold();
                                                        });
                                                    });

                                                    row.RelativeItem().PaddingRight(5).PaddingTop(1).AlignCenter().AlignMiddle().Height(15).Width(110).Layers(layer =>
                                                    {
                                                        // Background bar
                                                        layer.PrimaryLayer().PaddingRight((float)73.3333).Text(Properties.Resources.Report_Label_Low).AlignCenter().FontSize(10);

                                                        layer.Layer().PaddingLeft((float)36.6666).PaddingRight((float)36.6666).Text(Properties.Resources.Schedule_Label_Normal).AlignCenter().FontSize(10);


                                                        layer.Layer().PaddingLeft((float)73.3333).Text(Properties.Resources.Report_Label_High).AlignCenter().FontSize(10);
                                                    });

                                                    row.RelativeItem()
                                                    .Column(leftColumn =>
                                                    {
                                                        leftColumn.Item().Column(col =>
                                                        {
                                                            col.Item()
                                                                .PaddingLeft(5)
                                                                .PaddingTop(1)
                                                                .Height(15)
                                                                .AlignCenter()
                                                                .Text(" ")
                                                                .Bold();
                                                        });
                                                    });
                                                });

                                                foreach (var d in test.TestResultDetails)
                                                {
                                                    column.Item().Row(row =>
                                                    {
                                                        row.RelativeItem()
                                                        .Column(leftColumn =>
                                                        {
                                                            leftColumn.Item().Column(col =>
                                                            {
                                                                col.Item()
                                                                    .PaddingLeft(5)
                                                                    .PaddingTop(10)
                                                                    .Height(20)
                                                                    .AlignCenter()
                                                                    .Text(d.TestParameter);
                                                            });
                                                        });

                                                        row.RelativeItem()
                                                        .Column(leftColumn =>
                                                        {
                                                            leftColumn.Item().Column(col =>
                                                            {
                                                                col.Item()
                                                                    .PaddingLeft(5)
                                                                    .PaddingTop(10)
                                                                    .Height(20)
                                                                    .AlignCenter()
                                                                    .Text(d.TestResultValue);
                                                            });
                                                        });

                                                        var reference = "-";
                                                        var paddingLeft = 0;
                                                        var paddingRight = 0;
                                                        var haveIndicator = false;

                                                        if (!string.IsNullOrEmpty(d.ReferenceRange))
                                                        {
                                                            var range = d.ReferenceRange.Replace("[", "").Replace("]", "").Contains(";") ? d.ReferenceRange.Split(";") : d.ReferenceRange.Split("-");
                                                            reference = range[0] + " - " + range[1];
                                                            paddingLeft = (CalculateRange(float.Parse(range[0].Replace("[", "")), float.Parse(range[1].Replace("]", "")), float.Parse(d.TestResultValue.Replace("< ", "").Replace("> ", "")))) - 2;
                                                            //paddingRight = 140 - paddingLeft - 2;
                                                            paddingRight = 110 - paddingLeft - 2;
                                                            haveIndicator = true;
                                                        }

                                                        row.RelativeItem()
                                                        .Column(leftColumn =>
                                                        {
                                                            leftColumn.Item().Column(col =>
                                                            {
                                                                col.Item()
                                                                    .PaddingLeft(5)
                                                                    .PaddingTop(10)
                                                                    .Height(20)
                                                                    .AlignCenter()
                                                                    .Text(reference);
                                                            });
                                                        });

                                                        row.RelativeItem()
                                                        .Column(leftColumn =>
                                                        {
                                                            leftColumn.Item().Column(col =>
                                                            {
                                                                col.Item()
                                                                    .PaddingLeft(5)
                                                                    .PaddingTop(10)
                                                                    .Height(20)
                                                                    .AlignCenter()
                                                                    .Text(string.IsNullOrEmpty(d.TestResultUnit) ? "-" : d.TestResultUnit);
                                                            });
                                                        });

                                                        row.RelativeItem().PaddingRight(5).PaddingTop(1).AlignCenter().AlignMiddle().Height(15).Width(110).Layers(layer =>
                                                        {
                                                            if (haveIndicator)
                                                            {
                                                                // Background bar
                                                                layer.PrimaryLayer().Border(1);

                                                                layer.Layer().PaddingRight((float)73.3333).BorderRight(1);


                                                                layer.Layer().PaddingLeft((float)73.3333).BorderLeft(1);


                                                                layer.Layer().PaddingLeft(paddingLeft).PaddingRight(paddingRight).Background(Colors.Black);
                                                            }
                                                            else
                                                            {
                                                                layer.PrimaryLayer().Text("-").AlignCenter();
                                                            }
                                                        });

                                                        var previousValue = "-";

                                                        if (test.PreviousTestResultDetails != null)
                                                        {
                                                            var previousParameter = test.PreviousTestResultDetails != null ? test.PreviousTestResultDetails.FirstOrDefault(x => x.TestParameter == d.TestParameter) : null;
                                                            previousValue = (previousParameter != null && !string.IsNullOrEmpty(previousParameter.TestResultValue)) ? previousParameter.TestResultValue : "-";
                                                        }

                                                        row.RelativeItem()
                                                        .Column(leftColumn =>
                                                        {
                                                            leftColumn.Item().Column(col =>
                                                            {
                                                                col.Item()
                                                                    .PaddingLeft(5)
                                                                    .PaddingTop(10)
                                                                    .Height(20)
                                                                    .AlignCenter()
                                                                    .Text(previousValue);
                                                            });
                                                        });

                                                    });
                                                }
                                            });
                                        });
                                    });                                    
                                }                                
                            });

                        page.Footer().AlignRight()
                        .Row(row =>
                        {
                            row.ConstantItem(3, Unit.Centimetre).Text(Properties.Resources.Report_Label_PoweredBy).FontColor("#aeb3b5").Bold();
                            row.ConstantItem(4, Unit.Centimetre).Image(sFooterLogo);
                        });

                    });
                });

                if (sIsPrint)
                {
                    //App.FilePath = System.IO.Path.Combine(sDownloadPath, "Report-temp.pdf");
                    App.FilePath = "Report-temp.pdf";
                    sDocument.GeneratePdf(App.FilePath);

                    Process process = new Process();
                    process.StartInfo = new ProcessStartInfo()
                    {
                        FileName = App.FilePath,
                        UseShellExecute = true
                    };
                    process.Start();
                }
                else
                {
                    var testResultType = "MultipleTest";
                    if(sDownloadPrintResultModels.Count == 1)
                    {
                        testResultType = string.IsNullOrEmpty(sDownloadPrintResultModels[0].TestResult.TestResultType) ? "General" : sDownloadPrintResultModels[0].TestResult.TestResultType.Replace(" ", "");
                    }

                    var patientID = string.IsNullOrEmpty(sDownloadPrintResultModels[0].TestResult.PatientID) ? "General" : sDownloadPrintResultModels[0].TestResult.PatientID.Replace(" ", "");
                    String sFileName = "TestResult_" + testResultType +
                                       "_PatientID_" + patientID + "_" +
                                       DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                    String sOutputPath = System.IO.Path.Combine(sDownloadPath, sFileName);
                    sDocument.GeneratePdf(sOutputPath);
                }

                sDocument = null;
                App.ErrorOccur = false;
            }
            catch (Exception ex)
            {
                //throw ex;
                App.ErrorOccur = true;
            }

        }

        public int CalculateRange(float start, float end, float value)
        {
            //float onceWidth = (float)46.6666;
            //float twiceWidth = (float)93.3333;
            //float fullwidth = 140;
            float onceWidth = (float)36.6666;
            float twiceWidth = (float)73.3333;
            float fullwidth = 110;

            int position = 0;
            float range = end - start;
            float actualStart = start - range;
            float actualEnd = end + range;
            float percentage = 0;

            if (value > start && end > value)
            {
                float balance = value - start;
                percentage = (balance / range) * onceWidth + onceWidth;
            }
            else if (value < start && actualStart < value)
            {
                float balance = value - actualStart;
                percentage = (balance / range) * onceWidth;
            }
            else if (value > end && actualEnd > value)
            {
                float balance = value - end;
                percentage = (balance / range) * onceWidth + twiceWidth;
            }
            else if(value > actualEnd)
            {
                percentage = fullwidth;
            }

            position = Convert.ToInt32(percentage);

            return position;
        }
    }
}
