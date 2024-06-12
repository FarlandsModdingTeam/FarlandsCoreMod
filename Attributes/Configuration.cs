using System;

namespace FarlandsCoreMod.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class Configuration : Attribute
    {
        public string section;
        public string key;
        public string description;
        public Configuration(string section, string key, string description)
        {
            this.section = section;
            this.key = key;
            this.description = description;
        }

        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
        public class Bool : Configuration
        {
            public bool defaultValue;
            public Bool(string section, string key, string description, bool defaultValue) : base(section, key, description)
            {
                this.defaultValue = defaultValue;
            }
        }
    }
}