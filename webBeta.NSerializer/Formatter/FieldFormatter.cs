using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using webBeta.NSerializer.Base.Types;

namespace webBeta.NSerializer.Formatter
{
    public class FieldFormatter : IFieldFormatter
    {
        private readonly FieldFormatterType _formatterType;
        private static readonly TextInfo TextInfo = new CultureInfo("en-US", false).TextInfo;
        
        public FieldFormatter(FieldFormatterType type)
        {
            _formatterType = type;
        }

        public FieldFormatter(string type)
        {
            Enum.TryParse(type, true, out _formatterType);
        }

        public string Format(string name)
        {
            if (name.Contains(" "))
                throw new ArgumentException("A field cannot have empty spaces.");

            return _formatterType switch
            {
                FieldFormatterType.LOWER_HYPHEN => ToLowerHyphen(name),
                FieldFormatterType.LOWER_UNDERSCORE => ToLowerUnderscore(name),
                FieldFormatterType.LOWER_CAMEL => ToLowerCamel(name),
                FieldFormatterType.UPPER_CAMEL => ToUpperCamel(name),
                FieldFormatterType.UPPER_UNDERSCORE => ToUpperUnderscore(name),
                FieldFormatterType.INHERITED => name,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static IEnumerable<string> Explode(string name)
        {
            var truncated = Regex.Replace(name, @"([A-Z])", "-$1");
            var split = Regex.Split(truncated, @"[^a-zA-Z]");
            return split.Where(part => !string.IsNullOrEmpty(part)).ToArray();
        }

        private static string ToLowerHyphen(string name)
        {
            var explosion = Explode(name);
            return string.Join("-", explosion.Select(part => part.ToLower()));
        }

        private static string ToLowerUnderscore(string name)
        {
            var explosion = Explode(name);
            return string.Join("_", explosion.Select(part => part.ToLower()));
        }

        private static string ToUpperCamel(string name)
        {
            var explosion = Explode(name);
            return string.Join(string.Empty, explosion.Select(part => TextInfo.ToTitleCase(part)));
        }

        private static string ToLowerCamel(string name)
        {
            var explosion = Explode(name);
            var word = string.Join(string.Empty, explosion.Select(part => TextInfo.ToTitleCase(part)));
            return word.Substring(0, 1).ToLower() + word.Substring(1);
        }

        private static string ToUpperUnderscore(string name)
        {
            var explosion = Explode(name);
            return string.Join("_", explosion.Select(part => part.ToUpper()));
        }
    }
}