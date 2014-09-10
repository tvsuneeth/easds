using System;
using System.Collections.Generic;
using System.Linq;
using TWG.EASDataService.Data.Repository;
using TWG.EASDataService.Business;
using TWG.EASDataService.Data;

namespace TWG.EASDataService.Services
{
    public interface IArticleService
    {
        Article GetById(int id);
        PagedResult<ArticleSummary> GetAll(int page, int pageSize);        
        List<ArticleModificationSummary> GetChangedArticles(DateTime changedSince);        
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

        public List<ArticleModificationSummary> GetChangedArticles(DateTime changedSince)
        {
            return _articleRepository.GetChangedArticles(changedSince);
        }
      

        public Article GetById(int id)
        {
            var article = _articleRepository.Get(id);
            if (article != null)
            {               
                article.Taxonomy = GetArticleTaxonomy(id);
            }
            return article;
        }
       

        public ArticleTaxonomy GetArticleTaxonomy(int articleId)
        {
          
            var categories = _articleTaxonomyRepository.Get(articleId);
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

    }
}
