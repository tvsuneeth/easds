using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.Business
{
    public class ArticleSummary : IWebIdentifiable, IMediaAttachment
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public String Introduction { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool HasAttachedMedia { get { return (AttachedMedia != null); } }
        public MediaContent AttachedMedia { get; set; }
        public Person Author { get; set; }

        public object GetIdentificationElement() { return new { Id }; }
        public String GetIdentificationTitle() { return Title; }

        public ArticleTaxonomy Taxonomy { get; set; }
        public int NavigationId { get; set; }
    }
}
