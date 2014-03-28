using System;
using System.Collections.Generic;

using twg.chk.DataService.chkData.Repository;
using twg.chk.DataService.Business;
using twg.chk.DataService.chkData;

namespace twg.chk.DataService.api
{
    public interface IArticleService
    {
        Article GetById(int id);
        IEnumerable<Article> GetByTopic(String topicName, int page, int pageSize, out int totalElements);
        IEnumerable<Article> GetBySector(String sectorName, int page, int pageSize, out int totalElements);
        IEnumerable<Article> GetByArticleSection(String articleSectionName, int page, int pageSize, out int totalElements);
        IEnumerable<Article> GetByArticleSectionAndSector(String articleSectionName, String sectorName, int page, int pageSize, out int totalElements);
    }

    public class ArticleService : IArticleService
    {
        private IArticleRepository _articleRepository;
        private IArticleTaxonomyRepository _articleTaxonomyRepository;
        public ArticleService(IArticleRepository articleRepository, IArticleTaxonomyRepository articleTaxonomyRepository)
        {
            _articleRepository = articleRepository;
            _articleTaxonomyRepository = articleTaxonomyRepository;
        }

        public Article GetById(int id)
        {
            var article = _articleRepository.Get(id);
            if (article != null)
            {
                article.SetTaxonomyList(_articleTaxonomyRepository.Get(id));
            }
            return article;
        }


        public IEnumerable<Article> GetByTopic(String topicName, int page, int pageSize, out int totalElements)
        {
            page--; //page number are 0 based
            return _articleRepository.GetByTopic(new String[] {topicName}, page, pageSize, out totalElements);
        }

        public IEnumerable<Article> GetBySector(String sectorName, int page, int pageSize, out int totalElements)
        {
            page--; //page number are 0 based
            return _articleRepository.GetBySector(new String[] { sectorName }, page, pageSize, out totalElements);
        }

        public IEnumerable<Article> GetByArticleSection(String articleSectionName, int page, int pageSize, out int totalElements)
        {
            page--; //page number are 0 based
            return _articleRepository.GetByArticleSection(new String[] { articleSectionName }, page, pageSize, out totalElements);
        }

        public IEnumerable<Article> GetByArticleSectionAndSector(String articleSectionName, String sectorName, int page, int pageSize, out int totalElements)
        {
            page--; //page number are 0 based
            return _articleRepository.GetByArticleSectionAndSector(new String[] { articleSectionName }, new String[] { sectorName }, page, pageSize, out totalElements);
        }
    }
}
