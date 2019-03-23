using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Translation.V2;

namespace Controllers
{
    class GoogleTranslateController
    {
        public static string Translate(string text, string toLanguaje, string sourceLanguaje = "")
        {
            string result = string.Empty;
            try
            {
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                TranslationClient client = TranslationClient.Create();
                // var response = client.TranslateText("Hello World.", "ru");
                var response = client.TranslateText(text, toLanguaje, sourceLanguaje);
                result = response.TranslatedText;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }
    }
}
