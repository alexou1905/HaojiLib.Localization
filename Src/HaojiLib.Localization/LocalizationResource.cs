using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaojiLib.Localization
{
    public class LocalizationResource
    {
        public String[] Cultures { get; set; }

        public LocalizationResourceItem[] Resources { get; set; }
    }

    public class LocalizationResourceItem
    {
        public String Id { get; set; }
        public String[] Values { get; set; }
    }
}
