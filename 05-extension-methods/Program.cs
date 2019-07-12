using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExtensionMethodsExternal
{
    internal static class ExternalMethodsBucket
    {
        public static int CountDigitsExternal(this string s)
        {
            return string.IsNullOrEmpty(s) ? 0 : Regex.Replace(s, "\\D+", "").Length;
        }
    }

}

namespace ExtensionMethods
{
    using  ExtensionMethodsExternal;

    internal static class MethodsBucket
    {
        public static int CountDigits( this string s )
        {
            return string.IsNullOrEmpty(s) ? 0 : Regex.Replace(s, "\\D+", "").Length;
        }

        public static bool OnlyOne<T>(this IEnumerable<T> elements, Func<T, bool> predicate)
        {
            return elements != null && predicate != null && elements.Where(predicate).Count() == 1;
        }

        public static void Show(this string s)
        {
            Console.WriteLine($"STRING => {s}");
        }

        public static void Show(this object s)
        {
            Console.WriteLine($"OBJECT => {s}");
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ALA12 MA34".CountDigits());
            Console.WriteLine("ALA12 MA34".CountDigitsExternal());

            var names = new List<string>() { "ala", "igor", "roman" };
            Console.WriteLine(names.OnlyOne(name => name.Length == 3));

            "ALA".Show();
        }
    }
}
