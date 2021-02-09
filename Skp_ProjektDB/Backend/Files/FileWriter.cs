using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SkpDbLib.Files
{
    /// <summary>
    /// Class is NOT needed in prod
    /// Its Only for creating a secure file for the connection string
    /// </summary>
    internal class FileWriter
    {
        public void WriteStringToFile(string filePath, string data)
        {
            // writes a txt file
            File.WriteAllText(filePath, data);
        }

        public void WritePdf(string filePath, List<string> data)
        {
            // needs work for proper set up, so data wont spill over edges

            PdfDocument pdf = new PdfDocument();
            PdfPage page = pdf.AddPage();
            XGraphics graphics = XGraphics.FromPdfPage(page);
            XFont fontHeadline = new XFont("Calibri", 20);
            XFont fontText = new XFont("Calibri", 12);
            XRect rect = new XRect(0, 50, page.Width, page.Height);

            foreach (string line in data)
            {
                if (rect.Y >= page.Height - 100)
                {
                    page = pdf.AddPage();
                    graphics = XGraphics.FromPdfPage(page);
                    rect.Y = 50;
                }
                string text;
                if (line.Contains(@"\h"))
                {
                    text = line.Replace(@"\h", "");
                    graphics.DrawString(text, fontHeadline, XBrushes.Black, rect, XStringFormats.TopCenter);
                    rect.Y += 50;
                }
                if (line.Contains(@"\t"))
                {
                    text = line.Replace(@"\t", "");
                    graphics.DrawString(text, fontText, XBrushes.Black, rect, XStringFormats.TopCenter);
                    rect.Y += 20;
                }
            }


            //for (int i = 0; i < data.Length; i++)
            //{
            //    if (i <= 1)
            //    {
            //        graphics.DrawString(data[i], fontHeadline, XBrushes.Black, rect, XStringFormats.TopCenter);
            //        rect.Y += 50;
            //    }
            //    else
            //    {
            //        graphics.DrawString(data[i], fontText, XBrushes.Black, rect, XStringFormats.TopCenter);
            //        rect.Y += 20;
            //    }
            //}

            pdf.Save(filePath);
        }
    }
}
