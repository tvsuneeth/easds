using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TWG.EASDataService.Data.Extensions;
using TWG.EASDataService.Data.Infrastructure;
using TWG.EASDataService.Business;

namespace TWG.EASDataService.Data.Repository
{
    public interface IArticleTaxonomyRepository : IChkRepositoryBase<List<TaxonomyCategory>>
    {        
    }

    public class ArticleTaxonomyRepository : DbRepositoryBase, IArticleTaxonomyRepository
    {
        public List<TaxonomyCategory> Get(int id)
        {          
            var categories = new List<TaxonomyCategory>();
            using (var connection = CreateConnection())
            {
                using (var command = CreateCommand(connection,"chk.GetArticleTaxonomies", new { @ArticleId = id } ))
                {
                    var sqlReader = command.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var category = new TaxonomyCategory
                        {
                            CategoryId = sqlReader.GetValue<int>("liCategoryID"),
                            CategoryName =  sqlReader.GetValue<string>("sCategoryName"),
                        };
                        categories.Add(category);
                    }

                    sqlReader.NextResult();

                    while (sqlReader.Read())
                    {
                        int categoryId = sqlReader.GetValue<int>("liCategoryID");
                        var category = categories.Find(i => i.CategoryId == categoryId);
                        var categoryItem = new TaxonomyCategoryItem()
                        {
                            CategoryItemId = sqlReader.GetValue<int>("liCategoryItemID"),
                            CategoryItemName = sqlReader.GetValue<string>("sItemName"),
                            ParentItemId =  sqlReader.GetValue<int>("liParentID")
                        };
                        category.AddItem(categoryItem);
                    }
                }
            }
            return categories;            
        }
      

    }

   
}
