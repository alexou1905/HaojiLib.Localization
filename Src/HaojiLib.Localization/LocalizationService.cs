using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Primitives;

namespace HaojiLib.Localization
{
    public class LocalizationService : ILocalizationService
    {
        protected LocalizationResource _LocResource;

        protected IHttpContextAccessor httpContext;
        protected IHostingEnvironment hostingEnvironment;
        protected void LoadResources(String webRoot)
        {
            var localizationFolder = Path.Combine(webRoot, Folder); ;
            var localizationFile = Path.Combine(localizationFolder, FileName);

            if (!File.Exists(localizationFile))
                throw new FileNotFoundException("Localization File was not found.", localizationFile);

            var jsonContent = System.IO.File.ReadAllText(localizationFile);
            if (String.IsNullOrEmpty(jsonContent))
                throw new Exception("Localization file is empty");

            _LocResource = JsonConvert.DeserializeObject(jsonContent, typeof(LocalizationResource)) as LocalizationResource;

            if (_LocResource == null)
                throw new Exception("Invalid Localization file");
        }

        public LocalizationService(IHostingEnvironment environment, IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
            this.hostingEnvironment = environment;
            LoadResources(environment.WebRootPath);
        }

        private const String cookieKey = "HaojiLib.Localization.CacheLang";
        private void SetCultureToCookie(String culture)
        {
            httpContext.HttpContext.Response.Cookies.Delete(cookieKey);
            httpContext.HttpContext.Response.Cookies.Append(cookieKey, culture, new CookieOptions() { Expires = DateTime.Now.AddYears(10) });
        }
        private String ReadCultureFromCookie()
        {
            var httpCookie = httpContext.HttpContext.Request.Cookies[cookieKey];
            if (StringValues.IsNullOrEmpty(httpCookie))
                return null;
            return httpCookie;
        }

        public string Culture
        {
            get
            {
                return ReadCultureFromCookie();
            }
            set
            {
                SetCultureToCookie(value);
            }
        }

        public String Folder { get; set; } = "Localization";

        public String FileName
        {
            get; set;
        } = "Localization.json";

        private String GetLocalizedValue(String id, Int32 index)
        {
            var item = _LocResource.Resources.FirstOrDefault(c => c.Id == id);
            if (item == null)
                return id + "_Name_Not_Found";

            if (item.Values == null || item.Values.Length < index)
                return id + "_Culture_Not_Found";

            var value = item.Values[index];
            return value;
        }

        public String Localize(String id, string culture = null, params object[] values)
        {
            Int32 cultureIndex = 0;

            if (culture == null)
                culture = ReadCultureFromCookie();

            if (!String.IsNullOrWhiteSpace(culture))
                cultureIndex = Array.FindIndex<String>(_LocResource.Cultures, c => c == culture);

            if (cultureIndex < 0)
                throw new Exception($"Culture: {culture} was not defined.");

            var value = GetLocalizedValue(id, cultureIndex);

            if (values != null && values.Length > 0)
                value = String.Format(value, values);

            return value;
        }

        public IEnumerable<string> Validate()
        {
            List<String> violations = new List<string>();
            if (_LocResource == null || _LocResource.Cultures == null || _LocResource.Resources == null)
            {
                violations.Add("Localization file was not loaded properly");
                return violations;
            }

            var culturesCount = _LocResource.Cultures.Count();

            if (culturesCount == 0)
            {
                violations.Add("At least one Culture should be defined.");
                return violations;
            }

            foreach (var item in _LocResource.Resources)
            {
                if (item.Values.Count() != culturesCount)
                    violations.Add($"Incorrect number of cultures for item: {item}");
            }

            return violations;
        }
    }
}
