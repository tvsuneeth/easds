using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.Business
{
    public class ArticleSummary: IIdentification
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public String Introduction { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime PublishedDate { get; set; }
        public MediaContent ThumbnailImage { get; set; }
        public Person Author { get; set; }

        public dynamic GetIdentityItem()
        {
            return Id;
        }
    }
}
