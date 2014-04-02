using System;
using System.Collections.Generic;
using System.Linq;

namespace twg.chk.DataService.Business
{
    public class PaginatedArticleSummaries : ITaxonomy
    {
        private TaxonomyCategories _taxonomySearchItem;
        public PaginatedArticleSummaries(TaxonomyCategories taxonomySearchItem)
        {
            _taxonomySearchItem = taxonomySearchItem;
        }

        public List<ArticleSummary> Summaries { get; set; }
        public int TotalResult { get; set; }
        public int PageNumber { get; set; }
        private IEnumerable<TaxonomyItem> _taxonomyList;
        public virtual void SetTaxonomyList(IEnumerable<TaxonomyItem> taxonomyList) { _taxonomyList = taxonomyList; }

        public TaxonomyItem GetParent()
        {
            TaxonomyItem parentItem = null;

            switch (_taxonomySearchItem)
            {
                // Parent is an article section
                case TaxonomyCategories.ArticleSection :
                    var sections = GetArticleSections();
                    if (sections != null)
                    {
                        var parentSectionId =
                        (
                            from s in sections
                            join s2 in sections on s.Id equals s2.ParentId into childParent
                            from s2 in childParent.DefaultIfEmpty()
                            select new { Id = s.Id, ChildId = (int?)(s2 == null ? null : (int?)s2.Id) }
                        )
                        .Where(c => c.ChildId == null)
                        .Single()
                        .Id;

                        parentItem = sections.Single(s => s.Id == parentSectionId);
                    }
                    break;
                
                // Parent is a sector
                case TaxonomyCategories.Sector :
                    var sectors = GetSectors();
                    if (sectors != null)
                    {
                        var parentSectionId =
                        (
                            from s in sectors
                            join s2 in sectors on s.Id equals s2.ParentId into childParent
                            from s2 in childParent.DefaultIfEmpty()
                            select new { Id = s.Id, ChildId = (int?)(s2 == null ? null : (int?)s2.Id) }
                        )
                        .Where(c => c.ChildId == null)
                        .Single()
                        .Id;

                        parentItem = sectors.Single(s => s.Id == parentSectionId);
                    }
                    break;

                // Parent is a topic (topic are single level taxonomy elements)
                case TaxonomyCategories.Topic :
                    parentItem = null;
                    break;
            }

            return parentItem;
        }

        public List<TaxonomyItem> GetArticleSections()
        {
            if (_taxonomyList == null)
                return null;

            var sections = _taxonomyList.Where(t => t.Category == TaxonomyCategories.ArticleSection).ToList();

            return sections.Count == 0 ? null : sections;
        }

        public List<TaxonomyItem> GetSectors()
        {
            if (_taxonomyList == null)
                return null;

            var sectors = _taxonomyList.Where(t => t.Category == TaxonomyCategories.Sector).ToList();

            return sectors.Count == 0 ? null : sectors;
        }

        public List<TaxonomyItem> GetTopics()
        {
            if (_taxonomyList == null)
                return null;

            var topics = _taxonomyList.Where(t => t.Category == TaxonomyCategories.Topic).ToList();

            return topics.Count == 0 ? null : topics;
        }
    }
}
