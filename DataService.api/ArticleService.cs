using System;
using System.Collections.Generic;
using System.Linq;
using twg.chk.DataService.chkData.Repository;
using twg.chk.DataService.Business;
using twg.chk.DataService.chkData;

namespace twg.chk.DataService.api
{
    public interface IArticleService
    {
        Article GetById(int id);
        PagedResult<ArticleSummary> GetAll(int page, int pageSize);
        PagedResult<ArticleSummary> GetByTopic(String topicName, int page, int pageSize);
        PagedResult<ArticleSummary> GetBySector(String sectorName, int page, int pageSize);
        PagedResult<ArticleSummary> GetByArticleSection(String articleSectionName, int page, int pageSize);
        PagedResult<ArticleSummary> GetByArticleSectionAndSector(String articleSectionName, String sectorName, int page, int pageSize);
        List<ModifiedArticle> GetModifiedArticles(DateTime updatedSince);        
        ArticleTaxonomy GetArticleTaxonomy(int articleId);
        List<TaxonomyCategory> GetAllTaxonomyCategories();
    }

    public class ArticleService : IArticleService
    {
        private IArticleRepository _articleRepository;
        private IArticleTaxonomyRepository _articleTaxonomyRepository;
        private ITaxonomyRepository _taxonomyRepository;
        public ArticleService(IArticleRepository articleRepository, IArticleTaxonomyRepository articleTaxonomyRepository, ITaxonomyRepository taxonomyRepository)
        {
            _articleRepository = articleRepository;
            _articleTaxonomyRepository = articleTaxonomyRepository;
            _taxonomyRepository = taxonomyRepository;
        }

        public List<TaxonomyCategory> GetAllTaxonomyCategories()
        {
            var rowtaxonomies = _taxonomyRepository.GetAllTaxonomyCategories();
            return GetTaxonomyfromRowTaxonomyTable(rowtaxonomies);
        }

        public List<ModifiedArticle> GetModifiedArticles(DateTime modifiedSince)
        {
            return _articleRepository.GetModifiedArticles(null, null, null, modifiedSince);
        }

        public Article GetById(int id)
        {
            var article = _articleRepository.Get(id);
            if (article != null)
            {
                //ravi
                article.SetTaxonomyList(_articleTaxonomyRepository.Get(id));
                
                //suneeth                
                article.Taxonomy = GetArticleTaxonomy(id);
            }
            return article;
        }

        public List<TaxonomyCategory> GetTaxonomyfromRowTaxonomyTable(List<RowTaxonomyItem> taxonomy)
        {
            List<TaxonomyCategory> categories = taxonomy.GroupBy(i => i.CategoryId)
                                                        .Select(g => g.First())
                                                        .ToList()
                                                        .Select(x => new TaxonomyCategory() { CategoryId = x.CategoryId, CategoryName = x.CategoryName })
                                                        .ToList();


            foreach (var item in categories)
            {
                item.CategoryItems = taxonomy.Where(i => i.CategoryId == item.CategoryId && i.CategoryItemId != 0)
                                             .Select(s => new TaxonomyCategoryItem() { CategoryItemId = s.CategoryItemId, CategoryItemName = s.CategoryItemName, ParentId = s.ParentId })
                                             .ToList();
            }

            return categories;
        }


        public ArticleTaxonomy GetArticleTaxonomy(int articleId)
        {
            var taxonomy = _articleTaxonomyRepository.GetTaxonomies(articleId);
            if (taxonomy == null)
            {
                return null;
            }
            var categories = GetTaxonomyfromRowTaxonomyTable(taxonomy);

            if (categories == null)
            { return null; }

            var articleTaxonomy = new ArticleTaxonomy();
            articleTaxonomy.CategoryAssignments = categories;
            articleTaxonomy.ParentSection = taxonomy.Where(i => i.CategoryId == 1 && i.ParentId != null)
                                                    .Select(t => new ArticleSection() { SectionId = t.CategoryItemId, SectionName = t.CategoryItemName })
                                                    .FirstOrDefault();

            return articleTaxonomy;
        }

               
        
        public PagedResult<ArticleSummary> GetAll(int page, int pageSize)
        {
            var articleSummaries = _articleRepository.GetAll(null, null, null, page, pageSize);
            
            foreach (var item in articleSummaries)
            {
                item.Taxonomy = GetArticleTaxonomy(item.Id);
            }
            return articleSummaries;
        }

        public PagedResult<ArticleSummary> GetByTopic(String topicName, int page, int pageSize)
        {
            var articleSummaries = _articleRepository.GetByTopic(new String[] {topicName}, page, pageSize);

            //Note: we don't fetch parent taxonomy for topic because topics elements are one level taxonomy elements

            return articleSummaries;
        }

        public PagedResult<ArticleSummary> GetBySector(String sectorName, int page, int pageSize)
        {
            var articleSummaries = _articleRepository.GetBySector(new String[] { sectorName }, page, pageSize);

            //fetch taxonomy for the given sector
            var sectors = _taxonomyRepository.GetSectorParents(sectorName);
            articleSummaries.SetTaxonomyList(sectors);

            return articleSummaries;
        }

        public PagedResult<ArticleSummary> GetByArticleSection(String articleSectionName, int page, int pageSize)
        {
            var articleSummaries = _articleRepository.GetByArticleSection(new String[] { articleSectionName }, page, pageSize);

            //fetch taxonomy for the given article section
            var articleSections = _taxonomyRepository.GetArticleSectionParents(articleSectionName);
            articleSummaries.SetTaxonomyList(articleSections);

            //fetch children article sections for the given article section
            var childrenArticleSection = _taxonomyRepository.GetChildrenArticleSection(articleSectionName);
            articleSummaries.SetChildrenArticleSection(childrenArticleSection);

            return articleSummaries;
        }

        public PagedResult<ArticleSummary> GetByArticleSectionAndSector(String articleSectionName, String sectorName, int page, int pageSize)
        {
            var articleSummaries = _articleRepository.GetByArticleSectionAndSector(new String[] { articleSectionName }, new String[] { sectorName }, page, pageSize);

            //fetch taxonomy for the given article section
            var articleSectionsAndSectors = _taxonomyRepository.GetArticleSectionAndSectorParents(articleSectionName, sectorName);
            articleSummaries.SetTaxonomyList(articleSectionsAndSectors);

            //fetch children article sections for the given article section
            var childrenArticleSection = _taxonomyRepository.GetChildrenArticleSection(articleSectionName);
            articleSummaries.SetChildrenArticleSection(childrenArticleSection);

            return articleSummaries;
        }
    }
}
