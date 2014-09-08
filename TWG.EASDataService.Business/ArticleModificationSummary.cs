using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWG.EASDataService.Business
{
    public class ArticleModificationSummary
    {
        public int Id { get; set; }
        public DateTime LastModified { get; set; }
        public ArticleStatus CurrentStatus { get; set; }

    }

    public enum ArticleStatus
    {
        Editing=1, 
        UnderReview=2,
        Live=3,
        Deleted=4,
        Expired=5,
        NotReleased=6
    }
}
