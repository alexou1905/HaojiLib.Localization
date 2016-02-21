using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaojiLib.Localization
{
    public interface ILocalizationService
    {
        string Localize(string id, string culture = null, params object[] values);

        string Culture { get; set; }

        IEnumerable<string> Validate();
    }
}
