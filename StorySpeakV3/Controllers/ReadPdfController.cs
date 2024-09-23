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

                            textContent.WriteLine(pageText);
                        }
                        string text = textContent.ToString();


                        string[] tokens = TokenizeText(text);


                        ViewBag.TextContent = string.Join(" ", tokens);


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


        private string CleanText(string text)
        {

            text = WebUtility.HtmlDecode(text);


            text = XmlConvert.DecodeName(text);


            text = text.Replace("\n", " ");


            text = Regex.Replace(text, @"[^\w\s]", "");
            text = Regex.Replace(text, @"\s+", " ");

            text = Regex.Replace(text, @"\b[A-Z\s]{3,}\b", "");

            return text;
        }


        private string[] TokenizeText(string text)
        {

            char[] delimiters = [];

            string[] tokens = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            return tokens;
        }







    }
}
