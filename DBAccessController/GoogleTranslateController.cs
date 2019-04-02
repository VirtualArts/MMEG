using System;
using Google.Cloud.Translation.V2;
using Google.Apis.Auth.OAuth2;
using System.IO;

namespace Controllers
{
    public static class GoogleTranslateController
    {
        public static string Translate(string text, string toLanguaje, string sourceLanguaje = "")
        {
            string result = string.Empty;
            try
            {
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                GoogleCredential credential = GoogleCredential.FromFile(Directory.GetCurrentDirectory() + @"\GoogleTranslateCredentials.json");
                TranslationClient client = TranslationClient.Create(credential);
                var response = client.TranslateText(text, toLanguaje, sourceLanguaje);
                result = response.TranslatedText;
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, "GoogleTranslateController.Translate(string text, string toLanguaje, string sourceLanguaje)");
            }
            return result;
        }
    }
}
