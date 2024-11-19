using System;

namespace Attributes
{
    public class DataField : Attribute
    {
        public string Name { get; private set; }

        public bool IsAutogenerate { get; private set; }

        public DataField(string name) : this(name, false) { }

        public DataField(bool isAutogenerate) : this(string.Empty, isAutogenerate) { }

        public DataField(string name, bool isAutogenerate)
        {
            Name = name;
            IsAutogenerate = isAutogenerate;
        }
    }
}
