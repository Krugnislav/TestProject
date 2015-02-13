using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TestProject.Global.Config
{
    public class IconSizesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("iconSizes")]
        public IconSizesCollection IconSizes
        {
            get
            {
                return this["iconSizes"] as IconSizesCollection;
            }
        }
    }

    public class IconSizesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new IconSize();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IconSize)element).Name;
        }
    }
}