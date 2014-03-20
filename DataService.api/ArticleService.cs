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
        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public Article GetById(int id)
        {
            return _articleRepository.Get(id);
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
