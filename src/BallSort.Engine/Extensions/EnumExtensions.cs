using System.Text;

namespace BallSort.Engine.Extensions;

public static class EnumExtensions
{
    public static string ToHumanReadable<T>(this T enumeration) where T : Enum
    {
        var text = enumeration.ToString();

        var result = new StringBuilder();

        for (var i = 0; i < text.Length; i++)
        {
            if (i > 0 && char.IsUpper(text[i]))
            {
                result.Append(' ');
            }

            result.Append([text[i]]);
        }

        return result.ToString();
    }
}