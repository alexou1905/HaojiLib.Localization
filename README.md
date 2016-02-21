HaojiLib.Localization
============

A lightweight Localization library for ASP.NET 5



### Usage

	@Html.ActionLink(locSvc.Localize("English", "en_US"), "Change", "Locale", new { culture = "en_US" })
	@Html.ActionLink(locSvc.Localize("Chinese", "zh_CN"), "Change", "Locale", new { culture = "zh_CN" })

Index.cshtml

    @Html.Localize("Welcome") or @Html.Localize("Languages",null,2)

Please see the SampleWeb for reference.

Folder structure
--------------- 
At this momenent, all localization contents are put into one json file under wwwroot\Localization\Localization.json file.  

Default language is the first one that defined in the Localization.json file.


###Localization.json samples
---------------  
	{
	  "Cultures": [ "en_US", "zh_CN" ],
	  "Resources": [
		{
		  "Id": "appName",
		  "Values": [
			"Test Web",
			"测试网站"
		  ]
		},
		{
		  "Id": "Welcome",
		  "Values": [
			"Welcome to use HaojiLib.Localization",
			"欢迎使用HaojiLib.Localization"
		  ]
		},
		{
		  "Id": "Change_Language",
		  "Values": [
			"Change Language:",
			"选择语言："
		  ]
		},
		{
		  "Id": "Chinese",
		  "Values": [
			"Chinese",
			"简体中文"
		  ]
		},
		{
		  "Id": "English",
		  "Values": [
			"English",
			"英语"
		  ]
		},
		{
		  "Id": "Languages",
		  "Values": [
			"This website supports {0} languages.",
			"这个网站支持{0}种语言"
		  ]
		}
	  ]
	}

Structure
---------------

### Views
    Views\_ViewImports.cshtml
	   @inject ILocalizationService locSvc

    Razor
         @Html.Localize("appName")         
      
    Code Behind
         Localization.Localize("appName")         
		 
### Controllers

```
public class LocaleController : Controller
    {
        private ILocalizationService locSvc;

        public LocaleController(ILocalizationService locSvc)
        {
            this.locSvc = locSvc;
        }
        // GET: /<controller>/
        public IActionResult Change(String culture)
        {
            locSvc.Culture = culture;
            Response.Redirect(Request.Headers["Referer"]);
            return Content("OK");
        }
    }
```
