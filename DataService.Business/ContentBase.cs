using System;
using System.Collections.Generic;
using System.Linq;

namespace twg.chk.DataService.Business
{
    public abstract class ContentBase : ITaxonomy, IWebIdentifiable
    {
        public virtual int Id { get; set; }
        public virtual String Title { get; set; }
        public virtual String Body { get; set; }
        public virtual DateTime LastModified { get; set; }
        public virtual String MetaDescription { get; set; }
        public virtual String MetaKeywords { get; set; }
        public virtual Person Author { get; set; }
        //public bool HasAttachedMedia { get { return (AttachedMedia != null); } }
        //public MediaContent AttachedMedia { get; set; }

        public virtual object GetIdentificationElement() { return new { Id }; }

        public virtual String GetIdentificationTitle() { return Title; }

        private List<TaxonomyItem> _taxonomyList;
        public virtual void SetTaxonomyList(List<TaxonomyItem> taxonomyList) { _taxonomyList = taxonomyList; }
      

        public List<TaxonomyItem> _childrenArticleSection;
        public virtual void SetChildrenArticleSection(List<TaxonomyItem> childrenArticleSection) { _childrenArticleSection = childrenArticleSection; }

        public TaxonomyItem GetParentArticleSection()
        {
            var sections = GetArticleSections();
            if (sections == null)
                return null;

            if (sections.Count > 1)
            {
                var a = (
                                        from s in sections
                                        join s2 in sections on s.Id equals s2.ParentId into childParent
                                        from s2 in childParent.DefaultIfEmpty()
                                        select new { Id = s.Id, ChildId = (int?)(s2 == null ? null : (int?)s2.Id) }
                                    );

                var parentSectionId = (
                                        from s in sections
                                        join s2 in sections on s.Id equals s2.ParentId into childParent
                                        from s2 in childParent.DefaultIfEmpty()
                                        select new { Id = s.Id, ChildId = (int?)(s2 == null ? null : (int?)s2.Id) }
                                    )
                                    .Where(c => c.ChildId == null)
                                    .Single()
                                    .Id;

                return sections.Single(s => s.Id == parentSectionId);
            }
            else
            {
                return sections[0];
            }
        }

        public List<TaxonomyItem> GetChildrenArticleSection() { return _childrenArticleSection; }

        public List<TaxonomyItem> GetArticleSections()
        {
            if (_taxonomyList == null || _taxonomyList.Count == 0)
                return null;

            var sections = _taxonomyList.FindAll(t => t.Category == TaxonomyCategories.ArticleSection);
            return sections.Count == 0 ? null : sections;
        }

        public List<TaxonomyItem> GetSectors()
        {
            if (_taxonomyList == null || _taxonomyList.Count == 0)
                return null;

            var sector = _taxonomyList.FindAll(t => t.Category == TaxonomyCategories.Sector);
            return sector.Count == 0 ? null : sector;
        }

        public List<TaxonomyItem> GetTopics()
        {
            if (_taxonomyList == null || _taxonomyList.Count == 0)
                return null;

            var topic = _taxonomyList.FindAll(t => t.Category == TaxonomyCategories.Topic);
            return topic.Count == 0 ? null : topic;
        }
       
    }
}
