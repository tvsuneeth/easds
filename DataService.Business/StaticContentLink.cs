using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.Business
{
    public enum StaticLinkType { Article = 1, StaticPage = 2,  ArticleSection = 3, Sector = 4, Topic = 5 };
    public class StaticContentLink
    {
        public int Id { get; set; }
        public StaticLinkType LinkType { get; set; }
        public String IdentificationValue { get; set; }
        public String Title { get; set; }
    }
}
