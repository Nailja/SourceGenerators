using System;

namespace Attributes
{
    public class DataTableAttribute : Attribute
    {
        public string Name { get; private set; }

        public string PrimaryKey { get; private set; }

        public DataTableAttribute(string name, string primaryKey = "ID")
        {
            Name = name;
            PrimaryKey = primaryKey;
        }
    }
}
