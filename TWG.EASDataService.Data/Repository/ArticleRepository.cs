using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using TWG.EASDataService.Data.Extensions;
using TWG.EASDataService.Data.Infrastructure;
using TWG.EASDataService.Business;
using Microsoft.SqlServer.Server;

namespace TWG.EASDataService.Data.Repository
{
    public interface IArticleRepository :  IChkRepositoryBase<Article>
    {        
        PagedResult<ArticleSummary> GetAll(String[] excludeArticleSectionNames, String[] excludeSectorNames, String[] excludeTopicNames, int page, int pageSize);        
        List<ArticleModificationSummary> GetChangedArticles(DateTime changedSince);        
    }

    public class ArticleRepository : DbRepositoryBase, IArticleRepository
    {
        public Article Get(int id)
        {
            return GetArticle(id);
        } 
            
        private Article MapDataRrowToArticle(IDataRecord record)
        {
            var article = new Article
            {
                Id = record.GetValue<int>("liArticleID"),
                Title = record.GetValue<string>("sHeadline"),
                Introduction = record.GetValue<string>("sIntro"),
                Body = record.GetValue<string>("sBody"),
                PublishedDate = record.GetValue<DateTime>("dtPublicationDate"),
                LastModified = record.GetValue<DateTime>("dtLastModified"),
                ExpiryDate = record.GetValue<DateTime>("dtExpiryDate"),
                MetaDescription = record.GetValue<string>("metaDescription"),
                MetaKeywords = record.GetValue<string>("metaKeywords"),
                NavigationId = record.GetValue<int>("liNavigationItemID"),
                AbbreviatedHeadline = record.GetValue<string>("sAbbreviatedHeadline"),
                SubHeadline = record.GetValue<string>("sArticleSubHeadline")                

            };
            var author = new Person
            {
                Title = record.GetValue<string>("sTitle") == null ? string.Empty: record.GetValue<string>("sTitle").Trim(),
                FirstName = record.GetValue<string>("sFirstName") == null ? string.Empty: record.GetValue<string>("sFirstName").Trim(),
                LastName = record.GetValue<string>("sLastName") == null ? string.Empty: record.GetValue<string>("sLastName").Trim(),
                Email = record.GetValue<string>("sEmailAddress") == null ? string.Empty: record.GetValue<string>("sEmailAddress").Trim(),
                
            };

            if (!String.IsNullOrWhiteSpace(author.Names))
            {
                article.Author = author;
            }

            if (record.GetValue<int>("liAssetID")!=0)
            {
                var image = new Image
                {
                    Id = record.GetValue<int>("liAssetID"),
                    Name = record.GetValue<string>("sAssetName"),
                    Extension = record.GetValue<string>("sFileExt"),
                    CreatedDate = record.GetValue<DateTime>("imageCreatedDate"),
                    LastModifiedDate = record.GetValue<DateTime>("imageLastModifiedDate")
                };
                
                article.ThumbnailImage = image;
            }
            return article;
        }

        public Article GetArticle(int id)
        {            
            string commandText = "chk.GetArticle";            
            var obj = GetObjectWithCustomMapping<Article>(commandText, new { @ArticleId = id }, MapDataRrowToArticle);
            return obj;           
        }

        public PagedResult<ArticleSummary> GetAll(String[] excludeArticleSectionNames, String[] excludeSectorNames, String[] excludeTopicNames, int page, int pageSize)
        {
            page--; //pagination in database with 0 as first page

            var articleList = new List<ArticleSummary>();
            Nullable<int> totalNumberOfResult = null;

            var excludeArticleSectionDataTable = Helpers.ElementTableHelper.BuidTable(excludeArticleSectionNames);
            var excludeSectorDataTable = Helpers.ElementTableHelper.BuidTable(excludeSectorNames);
            var excludeTopicDataTable = Helpers.ElementTableHelper.BuidTable(excludeTopicNames);            

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyChk"].ConnectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "chk.GetArticles";

                    command.Parameters.Add(new SqlParameter("@PageNumber", page));
                    command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                    
                    var excludeArticleSectionParam = command.Parameters.AddWithValue("@ExcludeArticleSectionNames", excludeArticleSectionDataTable);
                    excludeArticleSectionParam.SqlDbType = SqlDbType.Structured;
                    var excludeSectorParam = command.Parameters.AddWithValue("@ExcludeSectorNames", excludeSectorDataTable);
                    excludeSectorParam.SqlDbType = SqlDbType.Structured;
                    var excludeTopicParam = command.Parameters.AddWithValue("@ExcludeTopicNames", excludeTopicDataTable);
                    excludeTopicParam.SqlDbType = SqlDbType.Structured;

                    var sqlReader = command.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        totalNumberOfResult = totalNumberOfResult ?? Convert.ToInt32(sqlReader["TotalNumberOfRow"]);
                        var articleSummary = new ArticleSummary
                        {
                            Id = Convert.ToInt32(sqlReader["liArticleID"]),
                            Title = Convert.ToString(sqlReader["sHeadline"]),
                            Introduction = Convert.ToString(sqlReader["sIntro"]),
                            PublishedDate = Convert.ToDateTime(sqlReader["dtPublicationDate"]),
                            LastModified = Convert.ToDateTime(sqlReader["dtLastModified"]),
                            NavigationId = Convert.ToInt32(sqlReader["liNavigationItemID"])
                        };

                        var author = new Person
                        {
                            Title = DBNull.Value.Equals(sqlReader["sTitle"]) ? String.Empty : Convert.ToString(sqlReader["sTitle"]).Trim(),
                            FirstName = DBNull.Value.Equals(sqlReader["sFirstName"]) ? String.Empty : Convert.ToString(sqlReader["sFirstName"]).Trim(),
                            LastName = DBNull.Value.Equals(sqlReader["sLastName"]) ? String.Empty : Convert.ToString(sqlReader["sLastName"]).Trim(),
                            Email = DBNull.Value.Equals(sqlReader["sEmailAddress"]) ? String.Empty : Convert.ToString(sqlReader["sEmailAddress"]).Trim(),
                        };
                        if (!String.IsNullOrWhiteSpace(author.Names))
                        {
                            articleSummary.Author = author;
                        }


                        if (!DBNull.Value.Equals(sqlReader["liAssetID"]))
                        {
                            var image = new MediaContent
                            {
                                Id = Convert.ToInt32(sqlReader["liAssetID"]),
                                FileName = Convert.ToString(sqlReader["sAssetName"]),
                                Extension = Convert.ToString(sqlReader["sFileExt"])
                            };

                            articleSummary.AttachedMedia = image;
                        }

                        articleList.Add(articleSummary);
                    }
                }
            }

            PagedResult<ArticleSummary> paginatedArticleSummaries = new PagedResult<ArticleSummary>((page + 1), totalNumberOfResult ?? 0, pageSize);

            paginatedArticleSummaries.AddRange(articleList);

            return paginatedArticleSummaries;
        }


        public List<ArticleModificationSummary> GetChangedArticles(DateTime changedSince)
        {                        
            var list = GetListWithAutoMapping<ArticleModificationSummary>(@"[chk].[GetArticlesChangedSince]", new { @changedDate = changedSince });
            return list;
        }
             
    }
   

}
