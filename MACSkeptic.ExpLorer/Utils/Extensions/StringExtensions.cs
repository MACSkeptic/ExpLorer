using System.Reflection;

namespace MACSkeptic.ExpLorer.Utils.Extensions
{
    internal static class StringExtensions
    {
        internal static string ApplyArguments(this string @string, object arguments)
        {
            if (@string.IsEmpty())
            {
                return @string;
            }

            var resultAfterFields = @string;

            arguments.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).
                ExecuteForEach(
                field =>
                    {
                        var placeholder = "#{" + field.Name + "}";
                        var value = field.GetValue(arguments);
                        var replacement = value != null
                                              ? value.ToString()
                                              : string.Empty;
                        resultAfterFields = resultAfterFields.Replace(placeholder, replacement);
                    });

            var resultAfterProperties = resultAfterFields;

            arguments.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).
                ExecuteForEach(
                property =>
                    {
                        var placeholder = "#{" + property.Name + "}";
                        var value = property.GetValue(arguments, null);
                        var replacement = value != null
                                              ? value.ToString()
                                              : string.Empty;
                        resultAfterProperties = resultAfterProperties.Replace(placeholder, replacement);
                    });

            return resultAfterProperties;
        }

        internal static bool IsEmpty(this string @string)
        {
            return string.IsNullOrEmpty(@string);
        }
    }
}