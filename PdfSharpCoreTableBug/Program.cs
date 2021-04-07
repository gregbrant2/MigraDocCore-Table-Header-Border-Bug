using System;
using System.Diagnostics;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.Rendering;
using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Utils;

namespace PdfSharpCoreTableBug
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            GlobalFontSettings.FontResolver = new FontResolver();

            var document = new Document();
            var section = document.AddSection();
            var table = section.AddTable();
            
            // Add columns with right border on all but last
            for (int i = 0; i < 3; i++)
            {
                var column = table.AddColumn("5cm");

                if (i < 2)
                {
                    column.Borders.Right = new Border
                    {
                        Color = Colors.Red,
                        Style = BorderStyle.Single,
                        Width = 0.5
                    };
                }
            }
            
            // Add Header Row
            var header = table.AddRow();
            header.HeadingFormat = true;
            header.Format.Font.Bold = true;

            header.Borders.Bottom = new Border
            {
                Color = Colors.Green,
                Style = BorderStyle.Single,
                Width = 0.5
            };

            for (int i = 0; i < 3; i++)
            {
                var cell = header.Cells[i].AddParagraph("Header " + i);
            }

            // Add data row
            var data = table.AddRow();
            
            for (int i = 0; i < 3; i++)
            {
                var cell = data.Cells[i].AddParagraph("Data " + i);
            }


            var renderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };
            
            renderer.PrepareRenderPages();
            renderer.RenderDocument();
            
            renderer.Save("test.pdf");
        }
    }
}
