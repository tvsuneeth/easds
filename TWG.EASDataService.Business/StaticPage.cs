using System;

namespace TWG.EASDataService.Business
{
    public class StaticPage : ContentBase
    {
        public String PageName { get; set; }
        public String TitleForHtmlPage { get; set; }

        public override object GetIdentificationElement() { return new { name = PageName }; }
    }
}
