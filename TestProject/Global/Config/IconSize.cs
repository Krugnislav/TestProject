using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TestProject.Global.Config
{
    public class IconSize : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
        }

        [ConfigurationProperty("width", IsRequired = false, DefaultValue = "48")]
        public int Width
        {
            get
            {
                return (int)this["width"];
            }
        }

        [ConfigurationProperty("height", IsRequired = false, DefaultValue = "48")]
        public int Height
        {
            get
            {
                return (int)this["height"];
            }
        }
    }
}