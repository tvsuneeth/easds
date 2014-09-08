﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using TWG.EASDataService.Data.Infrastructure;
using TWG.EASDataService.Business;

namespace  TWG.EASDataService.Data.Repository
{
    public interface IStaticPageRepository : IChkRepositoryBase<StaticPage>
    {
        StaticPage Get(String pageName);
        List<StaticPageSummary> GetAll();
    }

    public class StaticPageRepository : IStaticPageRepository
    {
        public StaticPage Get(String pageName)
        {
            if (String.IsNullOrWhiteSpace(pageName))
            {
                throw new ArgumentNullException("Argument pageName is null or empty");
            }

            pageName = pageName + ".htm";

            StaticPage staticPage = null;
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyChk"].ConnectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "chk.GetStaticPage";
                    command.Parameters.Add(new SqlParameter("@StaticPageName", pageName));

                    var sqlReader = command.ExecuteReader();
                    if (sqlReader.Read())
                    {
                        staticPage = new StaticPage
                        {
                            Id = Convert.ToInt32(sqlReader["liStaticPageID"]),
                            PageName = Convert.ToString(sqlReader["sPageURL"]).Replace(".htm", ""),
                            Title = Convert.ToString(sqlReader["sTitle"]),
                            Body = Convert.ToString(sqlReader["sBody"]),
                            TitleForHtmlPage = Convert.ToString(sqlReader["sPageTitle"]),
                            MetaDescription = Convert.ToString(sqlReader["sMetaDescription"]),
                            MetaKeywords = Convert.ToString(sqlReader["sMetaKeywords"]),
                            LastModified = Convert.ToDateTime(sqlReader["dtLastModified"])
                        };
                    }
                }
            }

            return staticPage;
        }

        public StaticPage Get(int id)
        {
            StaticPage staticPage = null;

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyChk"].ConnectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "chk.GetStaticPage";
                    command.Parameters.Add(new SqlParameter("@StaticPageId", id));

                    var sqlReader = command.ExecuteReader();
                    if (sqlReader.Read())
                    {
                        staticPage = new StaticPage
                        {
                            Id = Convert.ToInt32(sqlReader["liStaticPageID"]),
                            PageName = Convert.ToString(sqlReader["sPageURL"]).Replace(".htm", ""),
                            Title = Convert.ToString(sqlReader["sTitle"]),
                            Body = Convert.ToString(sqlReader["sBody"]),
                            TitleForHtmlPage = Convert.ToString(sqlReader["sPageTitle"]),
                            MetaDescription = Convert.ToString(sqlReader["sMetaDescription"]),
                            MetaKeywords = Convert.ToString(sqlReader["sMetaKeywords"]),
                            LastModified = Convert.ToDateTime(sqlReader["dtLastModified"])
                        };
                    }
                }
            }

            return staticPage;
        }

        public List<StaticPageSummary> GetAll()
        {
            var list = new List<StaticPageSummary>();

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyChk"].ConnectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "chk.GetListOfStaticPages";                    

                    var sqlReader = command.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var staticPage = new StaticPageSummary
                        {
                            Id = Convert.ToInt32(sqlReader["liStaticPageID"]),
                            PageName = Convert.ToString(sqlReader["sPageURL"]).Replace(".htm", ""),                            
                        };

                        list.Add(staticPage);
                    }
                }
            }

            return list;
        }
    }
}