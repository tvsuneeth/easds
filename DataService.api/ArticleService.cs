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
        IEnumerable<Article> GetByTopic(String topicName);
        IEnumerable<Article> GetBySector(String sectorName);
        IEnumerable<Article> GetByArticleSection(String articleSectionName);
        IEnumerable<Article> GetByArticleSectionAndSector(String articleSectionName, String sectorName);
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


        public IEnumerable<Article> GetByTopic(String topicName)
        {
            return _articleRepository.GetByTopic(new String[] {topicName});
        }

        public IEnumerable<Article> GetBySector(String sectorName)
        {
            return _articleRepository.GetBySector(new String[] { sectorName });
        }

        public IEnumerable<Article> GetByArticleSection(String articleSectionName)
        {
            return _articleRepository.GetByArticleSection(new String[] { articleSectionName });
        }

        public IEnumerable<Article> GetByArticleSectionAndSector(String articleSectionName, String sectorName)
        {
            return _articleRepository.GetByArticleSectionAndSector(new String[] { articleSectionName }, new String[] { sectorName });
        }
    }
}
