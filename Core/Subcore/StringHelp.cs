using System;
using System.Linq;
using System.Text;
using static NexusShadow.Core.Subcore.Logging;

namespace NexusShadow.Core.Subcore
{
    public static class StringHelper
    {
        public static string Reverse(string input)
        {
            try
            {
                char[] charArray = input.ToCharArray();
                Array.Reverse(charArray);
                return new string(charArray);
            }
            catch (Exception e)
            {
                LogError($"String reverse error: {e.Message}");
                return input;
            }
        }

        public static string Replace(string input, string oldValue, string newValue)
        {
            try
            {
                return input.Replace(oldValue, newValue);
            }
            catch (Exception e)
            {
                LogError($"String replace error: {e.Message}");
                return input;
            }
        }

        public static string RemoveWhitespace(string input)
        {
            try
            {
                return new string(input.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());
            }
            catch (Exception e)
            {
                LogError($"String remove whitespace error: {e.Message}");
                return input;
            }
        }
    }
}