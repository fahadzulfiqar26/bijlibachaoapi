using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PdfSharp.Drawing.Layout;
using NPOI.SS.Formula.Functions;


namespace WebApplication6.Models

{
    public class Genrating_PDF
    {
        public void Genrating_PDF_Function()
        {
string filePath =  "Bijli Bachao.pdf";

            PdfDocument document = new PdfDocument();
            double maxHeightPerPage = PageSizeConverter.ToSize(PdfSharp.PageSize.A4).Height;

            PdfPage page = document.AddPage();
            page.Size = PageSize.A4;
            XGraphics gfx = XGraphics.FromPdfPage(page);
            DrawHeader(gfx);
            #region Dynamically Table Creation  

            string[,] tableContent = new string[2, 4]; // Declare a 2D array with 2 rows and 4 columns

            // Add header row
            tableContent[0, 0] = "Sr";
            tableContent[0, 1] = "Date";
            tableContent[0, 2] = "Running Hours";
            tableContent[0, 3] = "Times";

            // Add data row
            tableContent[1, 0] = "1";
            tableContent[1, 1] = "06-Dec-22";
            tableContent[1, 2] = "02 hours 06 mins";
            tableContent[1, 3] = "10:17AM - 1128:AM , 10:17AM - 1128:AM, 10:17AM - 1128:AM, 10:17AM - 1128:AM";

            double[] columnWidths = { 30, 100, 150, 150 }; // Example column widths

            // Call the function
            AddTables(gfx, tableContent, 80, 250, columnWidths, maxHeightPerPage, document);
            #endregion//}
            DrawFooter(gfx, "Simplifying Energy Savings for People of Pakistan", "Powered By: www.bijli-bachao.pk", maxHeightPerPage);
           document.Save(@"D:/css/sddd.pdf");
          document.Save(filePath);
            
         }
        private void DrawHeader(XGraphics gfx)
        {
            #region Image and Texts (Header)
            string imagePath = @"logo.png";
            XImage image = XImage.FromFile(imagePath);
            gfx.DrawImage(image, 30, 40, 550, 80); //

            AddHeading(gfx, "Generator Running Hours", 50, 130, 300, 20, XColors.Black, 18);
            AddHeading(gfx, "From : Start Date To End Date", 50, 160, 300, 20, XColors.Black, 16);

            AddHeading(gfx, "Device# ID", 400, 150, 300, 20, XColors.Black, 18);
            AddHeading(gfx, "Total : 09 hours 14 mins", 200, 220, 300, 20, XColors.Black, 18);
            #endregion
        }
        static void AddTables(XGraphics gfx, string[,] tableContent, double x, double y, double[] columnWidths, double maxHeightPerPage, PdfDocument document)
        {
            // Determine the number of rows and columns based on the size of the table content array
            int rows = tableContent.GetLength(0);
            int columns = tableContent.GetLength(1);

            // Calculate row heights based on content
            double[] rowHeights = new double[rows];
            for (int i = 0; i < rows; i++)
            {
                double maxHeight = 0;
                for (int j = 0; j < columns; j++)
                {
                    string[] lines = tableContent[i, j].Split(new[] { "," }, StringSplitOptions.None);
                    double totalHeight = 0;
                    foreach (string line in lines)
                    {
                        XSize size = MeasureString(gfx, line, new XFont("Arial", 10));
                        totalHeight += size.Height;
                    }
                    maxHeight = Math.Max(maxHeight, totalHeight);
                }
                rowHeights[i] = maxHeight + 20; // Add some padding
            }

            // Draw table borders and content
            double currentY = y;
            for (int row = 0; row < rows; row++)
            {
                double currentX = x;
                double totalRowHeight = rowHeights[row];

                // Check if the current row height exceeds the available space on the page
                if (currentY + totalRowHeight > maxHeightPerPage - 120)

                {
                    PdfPage newPage = document.AddPage();
                    newPage.Size = PageSize.A4; // Set page size to A4
                    gfx = XGraphics.FromPdfPage(newPage);
                    currentY = 50; // Reset Y position for the new page

                    DrawFooter(gfx, "Simplifying Energy Savings for People of Pakistan", "Powered By: www.bijli-bachao.pk", maxHeightPerPage);
                }

                for (int col = 0; col < columns; col++)
                {
                    string[] lines = tableContent[row, col].Split(new[] { "," }, StringSplitOptions.None);

                    //double leftMargin = (columnWidths[col] - MeasureString(gfx, lines[0], new XFont("Arial", 10)).Width) / 2;
                    double topMargin = 5;   // Example top margin

                    double totalTextHeight = 0;
                    foreach (string line in lines)
                    {
                        XSize size = MeasureString(gfx, line, new XFont("Arial", 10));
                        totalTextHeight += size.Height;
                    }

                    XRect cellRect = new XRect(currentX, currentY, columnWidths[col], rowHeights[row]);
                    gfx.DrawRectangle(XPens.Black, cellRect); // Draw cell border

                    double lineHeight = totalTextHeight / lines.Length; // Calculate the height for each line

                    for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
                    {
                        string text = lines[lineIndex];
                        double textWidth = MeasureString(gfx, text, new XFont("Arial-Bold", 14)).Width; // Measure the width of each line
                        double textHeight = MeasureString(gfx, text, new XFont("Arial", 12)).Height; // Measure the height of each line

                        // Calculate the position for the current line
                        double lineX = currentX + (columnWidths[col] - textWidth) / 2; // Center horizontally
                        double lineY;
                        if (col < columns - 1)
                        {
                            lineY = currentY + (rowHeights[row] - textHeight) / 2; // Center vertically
                        }
                        else
                        {
                            lineY = currentY + topMargin + lineHeight * lineIndex + (lineHeight - textHeight) / 2; // Center vertically

                        }
                        XRect rect = new XRect(
                            lineX,
                            lineY,
                            textWidth,
                            textHeight
                        );

                        XTextFormatter formatter = new XTextFormatter(gfx);

                        if (row == 0)
                        {
                            formatter.DrawString(text, new XFont("Arial-Bold", 14), XBrushes.Black, rect);
                        }
                        else
                        {
                            formatter.DrawString(text, new XFont("Arial", 12), XBrushes.Black, rect);
                        }
                    }

                    currentX += columnWidths[col];
                }
                currentY += totalRowHeight;
            }

        }

        static XSize MeasureString(XGraphics gfx, string text, XFont font)
        {
            XSize size = gfx.MeasureString(text, font);
            return size;
        }
        static void AddHeading(XGraphics gfx, string headingText, double x, double y, double cellWidth, double cellHeight, XColor color, double fontSize, bool useBoldFont = false)
        {
            // Define font name based on whether to use bold font
            string fontName = useBoldFont ? "Arial-Bold" : "Arial";

            // Define font and brush for the heading
            XFont font = new XFont(fontName, fontSize);
            XBrush brush = new XSolidBrush(color);

            // Calculate the position for the heading below the table
            XTextFormatter tf = new XTextFormatter(gfx);

            // Draw the heading in a rectangle with the specified font and brush
            tf.DrawString(headingText, font, brush, new XRect(x, y, cellWidth, cellHeight));
        }


        static void DrawFooter(XGraphics gfx, string footerLine1, string footerLine2, double maxHeightPerPage)
        {
            XFont footerFont = new XFont("Arial", 14);
            double footerX = 150; // Adjust according to your requirement
            double footerY = maxHeightPerPage - 100; // Adjust according to your requirement

            // Draw horizontal line
            gfx.DrawLine(XPens.Black, 50, footerY - 10, 550, footerY - 10);

            // Draw footer lines
            gfx.DrawString(footerLine1, footerFont, XBrushes.Black, new XRect(footerX, footerY + 10, 400, 20), XStringFormats.TopLeft);
            gfx.DrawString(footerLine2, footerFont, XBrushes.Black, new XRect(footerX + 50, footerY + 30, 400, 20), XStringFormats.TopLeft);
        }
        List<DailyLiveDoT2> dataList;
        internal string start(List<DailyLiveDoT2> result)
        {
            dataList=result;
            Genrating_PDF_Function();
            return "";
        }
    }

}
