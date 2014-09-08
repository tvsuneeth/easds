﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using TWG.EASDataService.Data.Infrastructure;
using TWG.EASDataService.Business;

namespace TWG.EASDataService.Data.Repository
{
    public interface ITaxonomyRepository : IChkRepositoryBase<TaxonomyItem>
    {
        IEnumerable<TaxonomyItem> GetArticleSectionSectorAndTopicParents(String articleSection, String sector, String topic);
        IEnumerable<TaxonomyItem> GetArticleSectionAndSectorParents(String articleSection, String sector);
        IEnumerable<TaxonomyItem> GetArticleSectionParents(String articleSection);
        IEnumerable<TaxonomyItem> GetSectorParents(String sector);
        IEnumerable<TaxonomyItem> GetTopicParents(String topic);
        IEnumerable<TaxonomyItem> GetChildrenArticleSection(String articleSectionName);
        List<TaxonomyCategory> GetAllTaxonomyCategoriesAndItems();
    }

    public class TaxonomyRepository : ITaxonomyRepository
    {
        public TaxonomyItem Get(int id)
        {
            var taxonomy = GetTaxonomy(id, null, null, null);
            return taxonomy.SingleOrDefault();
        }

        #region Taxonomy with parents

        public IEnumerable<TaxonomyItem> GetArticleSectionSectorAndTopicParents(String articleSection, String sector, String topic)
        {
            return GetTaxonomy(null, articleSection, sector, topic);
        }

        public IEnumerable<TaxonomyItem> GetArticleSectionAndSectorParents(String articleSection, String sector)
        {
            return GetTaxonomy(null, articleSection, sector, null);
        }

        public IEnumerable<TaxonomyItem> GetArticleSectionParents(String articleSection)
        {
            return GetTaxonomy(null, articleSection, null, null);
        }

        public IEnumerable<TaxonomyItem> GetSectorParents(String sector)
        {
            return GetTaxonomy(null, null, sector, null);
        }

        public IEnumerable<TaxonomyItem> GetTopicParents(String topic)
        {
            return GetTaxonomy(null, null, null, topic);
        }

        private IEnumerable<TaxonomyItem> GetTaxonomy(int? taxonomyItemId, String articleSectionName, String sectorName, String topicName)
        {
            var taxonomyItems = new List<TaxonomyItem>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyChk"].ConnectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "chk.GetTaxonomy";
                    command.Parameters.Add(new SqlParameter("@CategoryItemId", taxonomyItemId ?? 0));
                    command.Parameters.Add(new SqlParameter("@ArticleSectionName", articleSectionName));
                    command.Parameters.Add(new SqlParameter("@SectorName", sectorName));
                    command.Parameters.Add(new SqlParameter("@TopicName", topicName));

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

        #endregion

        public List<TaxonomyCategory> GetAllTaxonomyCategoriesAndItems()
        {
            var categories = new List<TaxonomyCategory>();
            //var list = new List<TaxonomyCategory>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyChk"].ConnectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "chk.GetAllTaxonomyCategoriesandItems";                    

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

                    while (sqlReader.Read())
                    {
                        int categoryId = Convert.ToInt32(sqlReader["liCategoryID"]);
                        var category = categories.Find(i => i.CategoryId == categoryId);
                        var categoryItem = new TaxonomyCategoryItem()
                        {
                            CategoryItemId = Convert.ToInt32(sqlReader["liCategoryItemID"]),
                            CategoryItemName = Convert.ToString(sqlReader["sItemName"]),
                            ParentItemId = DBNull.Value.Equals(sqlReader["liParentID"]) ? null : (int?)Convert.ToInt32(sqlReader["liParentID"])
                        };
                        category.AddItem(categoryItem);
                    }
                }
            }

            return categories;
        }


        #region Taxonomy with direct children

        public IEnumerable<TaxonomyItem> GetChildrenArticleSection(String articleSectionName)
        {
            return GetChildrenTaxonomy(null, articleSectionName, null, null);
        }

        private IEnumerable<TaxonomyItem> GetChildrenTaxonomy(int? taxonomyItemId, String articleSectionName, String sectorName, String topicName)
        {
            var taxonomyItems = new List<TaxonomyItem>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyChk"].ConnectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "chk.GetChildrenTaxonomy";
                    command.Parameters.Add(new SqlParameter("@CategoryItemId", taxonomyItemId ?? 0));
                    command.Parameters.Add(new SqlParameter("@ArticleSectionName", articleSectionName));
                    command.Parameters.Add(new SqlParameter("@SectorName", sectorName));
                    command.Parameters.Add(new SqlParameter("@TopicName", topicName));

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

        #endregion
    }
}