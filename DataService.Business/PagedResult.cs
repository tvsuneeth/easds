using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace twg.chk.DataService.Business
{
    public class PagedResult<T> : List<T>, IPagination<T>, ITaxonomy where T : IWebIdentifiable, IMediaAttachment
    {
        private TaxonomyCategories _taxonomySearchItem;
        public PagedResult(TaxonomyCategories taxonomySearchItem, int currentPage, int totalResult, int numberOfResultPerPage)
        {
            _taxonomySearchItem = taxonomySearchItem;
            
            // Setting-up pagination properties
            CurrentPage = currentPage;
            _totalResults = totalResult;
            _numberOfResultPerPage = numberOfResultPerPage;

            var totalPageNumber = totalResult / _numberOfResultPerPage;
            totalPageNumber = totalResult % _numberOfResultPerPage > 0 ? totalPageNumber + 1 : totalPageNumber;

            LastPage = totalPageNumber;
            FirstPage = 1;

            HasMultiplePage = LastPage > 1;
            HasNextPage = CurrentPage < LastPage;
            NextPage = HasNextPage ? CurrentPage + 1 : 0;
            HasPreviousPage = CurrentPage > FirstPage;
            PreviousPage = HasPreviousPage ? CurrentPage - 1 : 0;
        }

        private int _numberOfResultPerPage;
        private int _totalResults;
        private IEnumerable<TaxonomyItem> _taxonomyList;

        public virtual void SetTaxonomyList(IEnumerable<TaxonomyItem> taxonomyList) { _taxonomyList = taxonomyList; }

        public IEnumerable<TaxonomyItem> _childrenArticleSection;
        public virtual void SetChildrenArticleSection(IEnumerable<TaxonomyItem> childrenArticleSection) { _childrenArticleSection = childrenArticleSection; }

        public TaxonomyItem GetParentArticleSection()
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
                        .Take(1)
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

        public List<TaxonomyItem> GetChildrenArticleSection()
        {
            if (_childrenArticleSection == null)
                return null;

            var items = _childrenArticleSection.ToList();

            return items.Count == 0 ? null : items;
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

        public bool HasMultiplePage { get; private set; }
        public int CurrentPage { get; private set; }
        public bool HasNextPage { get; private set; }
        public int NextPage { get; private set; }
        public bool HasPreviousPage { get; private set; }
        public int PreviousPage { get; private set; }
        public int FirstPage { get; private set; }
        public int LastPage { get; private set; }
    }
}
