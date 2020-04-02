using System;
using webBeta.NSerializer.Base.Types;

namespace webBeta.NSerializer.Formatter
{
    public class FieldFormatter : IFieldFormatter
    {
        
        private FieldFormatterType _formatterType;

        public FieldFormatter(FieldFormatterType type) {
            _formatterType = type;
        }

        public FieldFormatter(string type) {
            Enum.TryParse(type, true, out _formatterType);
        }
        
        public string Format(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}