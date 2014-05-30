using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using twg.chk.DataService.chkData.Infrastructure;
using twg.chk.DataService.Business;

namespace twg.chk.DataService.chkData.Repository
{
    public interface IArticleTaxonomyRepository : IChkRepositoryBase<List<TaxonomyItem>>
    {
        List<TaxonomyCategory> GetArticleTaxonomies(int articleId);
    }

    public class ArticleTaxonomyRepository : IArticleTaxonomyRepository
    {
        public List<TaxonomyItem> Get(int id)
        {
            var taxonomyItems = new List<TaxonomyItem>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyChk"].ConnectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "chk.GetArticleTaxonomy";
                    command.Parameters.Add(new SqlParameter("@ArticleId", id));

                    var sqlReader = command.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var taxonmy = new TaxonomyItem
                        {
                            Id = Convert.ToInt32(sqlReader["liCategoryItemID"]),
                            Name = Convert.ToString(sqlReader["sItemName"]),
                            ParentId = DBNull.Value.Equals(sqlReader["liParentID"]) ? null : (int?)Convert.ToInt32(sqlReader["liParentID"]),
                            Category = (TaxonomyCategories)Enum.Parse(typeof(TaxonomyCategories), Convert.ToString(sqlReader["liCategoryID"]))
                        };
                        taxonomyItems.Add(taxonmy);
                    }
                }
            }

            return taxonomyItems;
        }


        public List<TaxonomyCategory> GetArticleTaxonomies(int articleId)
        {
                                   
            var categories = new List<TaxonomyCategory>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyChk"].ConnectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "chk.GetArticleTaxonomies";
                    command.Parameters.Add(new SqlParameter("@ArticleId", articleId));

                    var sqlReader = command.ExecuteReader();                    
                    while (sqlReader.Read())
                    {
                        var category = new TaxonomyCategory
                        {
                            CategoryId = Convert.ToInt32(sqlReader["liCategoryID"]),
                            CategoryName = Convert.ToString(sqlReader["sCategoryName"]),                          
                        };
                        categories.Add(category);
                    }

                    sqlReader.NextResult();

                    while(sqlReader.Read())
                    {
                        int categoryId = Convert.ToInt32(sqlReader["liCategoryID"]);
                        var category  = categories.Find(i=>i.CategoryId==categoryId);
                        var categoryItem = new TaxonomyCategoryItem()
                        {
                             CategoryItemId  = Convert.ToInt32(sqlReader["liCategoryItemID"]),
                             CategoryItemName = Convert.ToString(sqlReader["sItemName"]),
                             ParentItemId = DBNull.Value.Equals(sqlReader["liParentID"]) ? null : (int?)Convert.ToInt32(sqlReader["liParentID"])
                        };
                        category.AddItem(categoryItem);
                    }
                }
            }
            return categories;            
        }

    }

   
}
