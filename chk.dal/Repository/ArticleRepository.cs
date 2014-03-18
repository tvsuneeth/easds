using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using twg.chk.DataService.chkData.Infrastructure;
using twg.chk.DataService.Business;


namespace twg.chk.DataService.chkData.Repository
{
    public interface IArticleRepository : IChkRepositoryBase<Article>
    {
        IEnumerable<Article> GetByTopic(String[] topicNames);
        IEnumerable<Article> GetBySector(String[] sectorNames);
        IEnumerable<Article> GetByArticleSection(String[] articleSectionNames);
        IEnumerable<Article> GetByArticleSectionAndSector(String[] articleSectionNames, String[] sectorNames);
        IEnumerable<Article> GetByArticleSectionSectorAndTopic(String[] articleSectionNames, String[] sectorNames, String[] topicNames);

        IEnumerable<Article> GetByTopic(String[] includeTopicNames, String[] excludeTopicNames);
        IEnumerable<Article> GetBySector(String[] includeSectorNames, String[] excludeSectorNames);
        IEnumerable<Article> GetByArticleSection(String[] includeArticleSectionNames, String[] excludeArticleSectionNames);
        IEnumerable<Article> GetByArticleSectionAndSector(String[] includeArticleSectionNames, String[] includeSectorNames, 
            String[] excludeArticleSectionNames, String[] excludeSectorNames);
        IEnumerable<Article> GetByArticleSectionSectorAndTopic(String[] includeArticleSectionNames, String[] includeSectorNames, 
            String[] includeTopicNames, String[] excludeArticleSectionNames, String[] excludeSectorNames, String[] excludeTopicNames);
    }

    public class ArticleRepository : IArticleRepository
    {
        public Article Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Article> GetByTopic(String[] topicNames)
        {
            return GetByTaxonomy(null, null, topicNames, null, null, null);
        }

        public IEnumerable<Article> GetBySector(String[] sectorNames)
        {
            return GetByTaxonomy(null, sectorNames, null, null, null, null);
        }

        public IEnumerable<Article> GetByArticleSection(String[] articleSectionNames)
        {
            return GetByTaxonomy(articleSectionNames, null, null, null, null, null);
        }

        public IEnumerable<Article> GetByArticleSectionAndSector(String[] articleSectionNames, String[] sectorNames)
        {
            return GetByTaxonomy(articleSectionNames, sectorNames, null, null, null, null);
        }

        public IEnumerable<Article> GetByArticleSectionSectorAndTopic(String[] articleSectionNames, String[] sectorNames, String[] topicNames)
        {
            return GetByTaxonomy(articleSectionNames, sectorNames, topicNames, null, null, null);
        }

        public IEnumerable<Article> GetByTopic(string[] includeTopicNames, string[] excludeTopicNames)
        {
            return GetByTaxonomy(null, null, includeTopicNames, null, null, excludeTopicNames);
        }

        public IEnumerable<Article> GetBySector(string[] includeSectorNames, string[] excludeSectorNames)
        {
            return GetByTaxonomy(null, includeSectorNames, null, null, excludeSectorNames, null);
        }

        public IEnumerable<Article> GetByArticleSection(string[] includeArticleSectionNames, string[] excludeArticleSectionNames)
        {
            return GetByTaxonomy(includeArticleSectionNames, null, null, excludeArticleSectionNames, null, null);
        }

        public IEnumerable<Article> GetByArticleSectionAndSector(string[] includeArticleSectionNames, string[] includeSectorNames, string[] excludeArticleSectionNames, string[] excludeSectorNames)
        {
            return GetByTaxonomy(includeArticleSectionNames, includeSectorNames, null, 
                excludeArticleSectionNames, excludeSectorNames, null);
        }

        public IEnumerable<Article> GetByArticleSectionSectorAndTopic(string[] includeArticleSectionNames, string[] includeSectorNames, string[] includeTopicNames, string[] excludeArticleSectionNames, string[] excludeSectorNames, string[] excludeTopicNames)
        {
            return GetByTaxonomy(includeArticleSectionNames, includeSectorNames, includeTopicNames, 
                excludeArticleSectionNames, excludeSectorNames, excludeTopicNames);
        }

        private IEnumerable<Article> GetByTaxonomy(String[] includeArticleSectionNames, String[] includeSectorNames, String[] includeTopicNames, String[] excludeArticleSectionNames, String[] excludeSectorNames, String[] excludeTopicNames)
        {
            throw new NotImplementedException();
        }
    }
}
