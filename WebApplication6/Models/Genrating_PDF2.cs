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
using Google.Protobuf.WellKnownTypes;
using System.Collections;


namespace WebApplication6.Models

{
    public class Genrating_PDF2
    {

        public static string GetDayName(string dateString)
        {
            DateTime date;
            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out date))
            {
                return date.ToString("dddd ")+ date.ToShortDateString();
            }
            else
            {
                return "Invalid date format";
            }
        }

        public static string GetDayName2(DateTime date)
        {
          
           
                return date.ToString("dddd ") + date.ToShortDateString();
            
        }
        public string Genrating_PDF_Function(List<List<TodayLiveGenerator>> result, MsnByDates2 value)
        {
            try
            {
                if (result == null || result.Count == 0)
                {
                    return null;
                }
                PdfDocument document = new PdfDocument();
                double maxHeightPerPage = PageSizeConverter.ToSize(PdfSharp.PageSize.A4).Height;

                PdfPage page = document.AddPage();
                page.Size = PageSize.A4;
                XGraphics gfx = XGraphics.FromPdfPage(page);
                DrawHeader(gfx, result, value);
                #region Dynamically Table Creation  

                string[,] tableContent = new string[result.Count + 1, 4]; // Declare a 2D array with 2 rows and 4 columns

                // Add header row
                tableContent[0, 0] = "Sr";
                tableContent[0, 1] = "Date";
                tableContent[0, 2] = "Running Hours";
                tableContent[0, 3] = "Time";
                for (int i = 0; i < result.Count; i++)
                {
                    int index = i + 1;
                    double totalMinutes = result[i].Sum(obj => obj.Minutes);

                    tableContent[index, 0] = index + "";
                    tableContent[index, 1] = result[i][0].start_time.ToString("dd-MMM-yy");
                    tableContent[index, 2] = getHoursMinutes(totalMinutes);
                    string final_result = "";
                    foreach (var item in result[i])
                    {
                        string startTimeString =item.start_time.ToString("hh:mm:tt");
                        string endTimeString = item.end_time.ToString("hh:mm:tt");

                        // Combine into desired format
                        string outputString = startTimeString + " - " + endTimeString;
                        final_result += outputString + ",";
                    }
                    tableContent[index, 3] = final_result;

                }
                // Add data row

                double[] columnWidths = { 30, 80, 160, 160 }; // Example column widths

                // Call the function
                AddTables(gfx, tableContent, 80, 210, columnWidths, maxHeightPerPage, document);
                #endregion//}
                DrawFooter(gfx, "Simplifying Energy Savings for People of Pakistan", "Powered By: www.bijlibachao.pk", maxHeightPerPage);

                string fullpath = getpath();
                //  document.Save(@"D:/css/sddd.pdf");
                document.Save(fullpath);

                // You can save the association between employeeId, description, id, and filePath in your database or any other storage
                var path = Path.GetFileName(fullpath);

                uploadFolderPathUrl = uploadFolderPathUrl + path;

                return uploadFolderPathUrl;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        private string uploadFolderPath = "httpdocs/Uploads"; // Folder where images will be saved
        private string uploadFolderPathUrl = "https://www.bijli-bachao-dashboard.pk/uploads/"; // Folder where images will be saved

        private string getpath()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            // Move to the parent directory (equivalent to cd..)
            currentDirectory = Path.Combine(currentDirectory, "..");


            // Get the updated current working directory

            uploadFolderPath = Path.Combine(currentDirectory, uploadFolderPath);

            // Ensure the uploads folder exists
            EnsureUploadFolderExists();

            // Generate a unique filename to avoid overwriting existing files
            string uniqueFileName = $"_{Guid.NewGuid().ToString()}_{DateTime.Now.ToString("MM-dd-yyyy_H-mm")}"+".pdf";

            // Combine the folder path and the unique filename
            string filePath = Path.Combine(uploadFolderPath, uniqueFileName);

            // Compress and save the image
          
      
            // Return the file path or other relevant information
         
            return filePath;
        }

        private void EnsureUploadFolderExists()
        {
            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }
        }

        private void DrawHeader(XGraphics gfx, List<List<TodayLiveGenerator>> result, MsnByDates2 value)
        {
            #region Image and Texts (Header)
            string imagePath = @"logo.png";
            XImage image = XImage.FromFile(imagePath);
            gfx.DrawImage(image, 130, 10, 270, 70); //

            AddHeading(gfx, "From: " + value.start_date.ToLongDateString(), 50, 110, 300, 20, XColors.Black, 14);
            AddHeading(gfx, "To: " + value.end_date.ToLongDateString(), 50, 140, 300, 20, XColors.Black, 14);

            AddHeading(gfx, "Device ID: " + value.msn, 370, 110, 300, 20, XColors.Black, 14);
            AddHeading(gfx, "Location: " + value.Location, 370, 140, 300, 20, XColors.Black, 14);
           
            var min = result.SelectMany(list => list).Sum(obj => obj.Minutes);
            string time = getHoursMinutes(min);
           
            AddHeading(gfx, "Total: " + time , 190, 180, 400, 20, XColors.Black, 16);

            //Total units: 898 (WAPDA: 670, Generator: 80)
            #endregion
        }

        private string getHoursMinutes(double line)
        {
            TimeSpan spWorkMin = TimeSpan.FromMinutes(Math.Round( line));
            string workHours = spWorkMin.ToString(@"hh\:mm");
            return  workHours.Split(":")[0] + " Hours " + workHours.Split(":")[1] + " Minutes ";
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
                rowHeights[i] = maxHeight + 15; // Add some padding
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

                    DrawFooter(gfx, "Simplifying Energy Savings for People of Pakistan", "Powered By: www.bijlibachao.pk", maxHeightPerPage);
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
                            if (row > 0)
                            {
                                lineY += 10;
                            }
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
            gfx.DrawLine(XPens.Black, 50, footerY , 550, footerY );

            // Draw footer lines
            gfx.DrawString(footerLine1, footerFont, XBrushes.Black, new XRect(footerX, footerY + 10, 400, 20), XStringFormats.TopLeft);
            gfx.DrawString(footerLine2, footerFont, XBrushes.Black, new XRect(footerX + 50, footerY + 30, 400, 20), XStringFormats.TopLeft);
        }
       
      
    }

}
