using MyPortfolio.Helpers.Interfaces;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MyPortfolio.Helpers.Implementations
{
    public class SlugHelper : ISlugHelper
    {
        public string GenerateSlug(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var normalized = value.ToLowerInvariant().Normalize(NormalizationForm.FormD);

            var stringBuilder = new StringBuilder();

            foreach (var character in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);

                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(character);
            }

            var slug = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-").Trim();
            slug = Regex.Replace(slug, @"-+", "-");

            return slug.Trim('-');
        }
    }
}
