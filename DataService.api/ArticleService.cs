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
        List<TaxonomyCategory> GetAllTaxonomyCategoriesAndItems();
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

        public List<TaxonomyCategory> GetAllTaxonomyCategoriesAndItems()
        {
            var taxonomies = _taxonomyRepository.GetAllTaxonomyCategoriesAndItems();
            return taxonomies;
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
                //ravi's taxonomy call
                //commentign this as we dont require this anymore
               //article.SetTaxonomyList(_articleTaxonomyRepository.Get(id));
                
                //suneeth                
                article.Taxonomy = GetArticleTaxonomy(id);
            }
            return article;
        }
       

        public ArticleTaxonomy GetArticleTaxonomy(int articleId)
        {
          
            var categories = _articleTaxonomyRepository.GetArticleTaxonomies(articleId);
            if (categories == null || categories.Count==0)
            { return null; }


            var section = categories.Find(i => i.CategoryId == 1);
            ArticleSection articleSection = null;
            if (section != null)
            {
                articleSection = section.CategoryItems
                                        .Select(t => new ArticleSection() { SectionId = t.CategoryItemId, SectionName = t.CategoryItemName })
                                        .FirstOrDefault();           
            }
            
            var articleTaxonomy = new ArticleTaxonomy()
            {
                CategoryAssignments = categories,
                ParentSection = articleSection
            };                                   
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
