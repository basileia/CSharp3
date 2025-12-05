namespace ToDoList.Domain.Models;

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Length(1, 50)]
    public required string Name { get; set; }
    [StringLength(50)]
    public string NormalizedName { get; set; } = string.Empty;

    public ICollection<ToDoItem>? ToDoItems { get; set; }

    public static string Normalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        string normalizedString = input.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (char c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        string result = stringBuilder.ToString().ToLowerInvariant();
        if (string.IsNullOrEmpty(result))
        {
            return string.Empty;
        }

        return char.ToUpper(result[0], CultureInfo.InvariantCulture) + result[1..];
    }
}
