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
        List<RowTaxonomyItem> GetTaxonomies(int id);
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


        public List<RowTaxonomyItem> GetTaxonomies(int id)
        {
            
            var taxonomyItems = new List<RowTaxonomyItem>();
            //var list = new List<TaxonomyCategory>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyChk"].ConnectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "chk.GetArticleTaxonomies";
                    command.Parameters.Add(new SqlParameter("@ArticleId", id));

                    var sqlReader = command.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var taxonmy = new RowTaxonomyItem
                        {
                            CategoryItemId = Convert.ToInt32(sqlReader["liCategoryItemID"]),
                            CategoryItemName = Convert.ToString(sqlReader["sItemName"]),
                            CategoryId = Convert.ToInt32(sqlReader["liCategoryID"]),
                            CategoryName = Convert.ToString(sqlReader["scategoryName"]),
                            ParentId = DBNull.Value.Equals(sqlReader["liParentID"]) ? null : (int?)Convert.ToInt32(sqlReader["liParentID"])
                        };
                        taxonomyItems.Add(taxonmy);
                    }
                }
            }

            return taxonomyItems;
        }

    }

   
}
