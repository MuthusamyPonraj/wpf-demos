#region Copyright Syncfusion Inc. 2001-2019.
// Copyright Syncfusion Inc. 2001-2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Microsoft.Win32;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.UI.Xaml.TreeGrid;
using Syncfusion.UI.Xaml.TreeGrid.Converter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PDFExportingDemo
{
    public static class Commands
    {
        static Commands()
        {
            CommandManager.RegisterClassCommandBinding(typeof(SfTreeGrid), new CommandBinding(ExportToPDF, OnExecuteExportToPDF, OnCanExecuteExportToPDF));
        }

        #region ExportToExcel Command

        public static RoutedCommand ExportToPDF = new RoutedCommand("ExportToPDF", typeof(SfTreeGrid));

        private static void OnExecuteExportToPDF(object sender, ExecutedRoutedEventArgs args)
        {
            PDFOptionsConverter pdfOptions = new PDFOptionsConverter();
            var treeGrid = args.Source as SfTreeGrid;
            if (treeGrid == null) return;
            try
            {
                var options = args.Parameter as TreeGridPdfExportingOptions;
                if (pdfOptions.IsCutomized)
                    options.CellsExportingEventHandler = CellsExportingEventHandler;
                if (pdfOptions.IsHeaderFooterEnabled)
                    options.PageHeaderFooterEventHandler = PdfHeaderFooterEventHandler;
                var document = treeGrid.ExportToPdf(options);

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "PDF Files(*.pdf)|*.pdf",
                    FileName = "document1"
                };

                if (sfd.ShowDialog() == true)
                {
                    using (Stream stream = sfd.OpenFile())
                    {
                        document.Save(stream);
                    }

                    //Message box confirmation to view the created Pdf file.
                    if (MessageBox.Show("Do you want to view the Pdf file?", "Pdf file has been created",
                                        MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        //Launching the Pdf file using the default Application.
                        Open(sfd.FileName);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private static void Open(string fileName)
        {
#if !NETCORE
            System.Diagnostics.Process.Start(fileName);
#else
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "cmd",
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = "/c start " + fileName
            };
            System.Diagnostics.Process.Start(psi);
#endif
        }

        #region ExportToPdf Event Handlers
        private static void PdfHeaderFooterEventHandler(object sender, TreeGridPdfHeaderFooterEventArgs e)
        {
            var width = e.PdfPage.GetClientSize().Width;

            PdfPageTemplateElement header = new PdfPageTemplateElement(width, 38);
            header.Graphics.DrawImage(PdfImage.FromFile(@"..\..\Resources\Header.png"), 155, 5, width / 3f, 34);
            e.PdfDocumentTemplate.Top = header;

            PdfPageTemplateElement footer = new PdfPageTemplateElement(width, 30);
            footer.Graphics.DrawImage(PdfImage.FromFile(@"..\..\Resources\Footer.png"), 0, 0);
            e.PdfDocumentTemplate.Bottom = footer;
        }

        static void CellsExportingEventHandler(object sender, TreeGridCellPdfExportingEventArgs e)
        {
            if (e.CellType == TreeGridCellType.RecordCell && e.ColumnName == "Title")
            {
                var cellstyle = new PdfGridCellStyle();
                cellstyle.BackgroundBrush = PdfBrushes.LightGreen;
                e.PdfGridCell.Style = cellstyle;
            }
        }


        #endregion

        private static void OnCanExecuteExportToPDF(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        #endregion

    }
}
