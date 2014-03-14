using System;

namespace twg.chk.DataService.Business
{
    public class StaticPage : ContentBase
    {
        public String PageName { get; set; }
        public String TitleForHtmlPage { get; set; }
        public String MetaDescription { get; set; }
        public String MetaKeywords { get; set; }
    }
}
