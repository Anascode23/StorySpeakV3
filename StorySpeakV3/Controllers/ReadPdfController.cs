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
        [HttpPost]
        public IActionResult ExtractText(IFormFile pdfFile)
        {
            if (pdfFile != null && pdfFile.Length > 0)
            {
                using (PdfReader reader = new PdfReader(pdfFile.OpenReadStream()))
                {
                    using (StringWriter textContent = new StringWriter())
                    {
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            string pageText = PdfTextExtractor.GetTextFromPage(reader, i);
                            pageText = CleanText(pageText);
                            textContent.WriteLine(pageText);
                        }
                        string text = textContent.ToString();
                        ViewBag.TextContent = text;
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

    }
}
