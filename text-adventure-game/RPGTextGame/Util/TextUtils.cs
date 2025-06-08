using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Util
{
    class TextUtils
    {
        public static string RemoverAcentos(string text)
        {
            if (text == null)
                return null;

            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
        public static string GenerateHash(string json)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public static string CenterText(string text, int width)
        {
            if (text.Length >= width)
                return text.Substring(0, width);

            int spaces = width - text.Length;
            int left = spaces / 2;
            int right = spaces - left;
            return new string(' ', left) + text + new string(' ', right);
        }

        public static string CenterLeftText(string text, int width)
        {
            if (text.Length >= width)
                return text.Substring(0, width);

            int spaces = width - text.Length;
            int left = 0;
            int right = spaces - left;
            return new string(' ', left) + text + new string(' ', right);
        }
    }
}
