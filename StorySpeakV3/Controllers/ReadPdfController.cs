using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Net;
using System.Xml;

namespace StorySpeak.Controllers
{
    public class ReadPdfController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("upload")]
        public IActionResult ExtractText(IFormFile pdfFile)
        {
            if (pdfFile != null && pdfFile.Length > 0)
            {
                using (PdfReader reader = new PdfReader(pdfFile.OpenReadStream()))
                {
                    reader.SelectPages("5-33");
                    using (StringWriter textContent = new StringWriter())
                    {
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            string pageText = PdfTextExtractor.GetTextFromPage(reader, i);
                            //pageText = CleanText(pageText);
                            textContent.WriteLine(pageText);
                        }
                        string text = textContent.ToString();

                        // Tokenize the extracted and cleaned text
                        string[] tokens = TokenizeText(text);

                        // Combine tokens into a single string for displaying
                        ViewBag.TextContent = string.Join(" ", tokens);

                        // Return a view that displays the content
                        return View();
                    }
                }
            }
            else
            {
                ViewBag.ErrorMessage = "No file uploaded.";
                return View("Error");
            }
        }




        [HttpPost]
        public IActionResult TextToSpeech(string text)
        {
            text = CleanText(text);


            return Json(new { cleanedText = text });
        }

        // Method to clean the text (optional)
        private string CleanText(string text)
        {
            // Decode HTML entities
            text = WebUtility.HtmlDecode(text);

            // Decode XML entities
            text = XmlConvert.DecodeName(text);

            // Replace newline characters with a space
            text = text.Replace("\n", " ");

            // Remove non-alphanumeric characters and normalize whitespace
            text = Regex.Replace(text, @"[^\w\s]", ""); // Remove non-alphanumeric characters
            text = Regex.Replace(text, @"\s+", " ");   // Replace multiple whitespaces with a single space

            return text;
        }

        // ** Remove TokenizeText method because it does nothing to solve the problem of make a coherent sentences **
        private string[] TokenizeText(string text)
        {
            // Define delimiters (e.g., space, period, comma)
            char[] delimiters = [' ', '.', ','];

            // Tokenize the text
            string[] tokens = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            return tokens;
        }







    }
}
