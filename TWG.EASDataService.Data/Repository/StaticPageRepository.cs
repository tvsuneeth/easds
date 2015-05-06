using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TWG.EASDataService.Data.Extensions;
using TWG.EASDataService.Data.Infrastructure;
using TWG.EASDataService.Business;

namespace  TWG.EASDataService.Data.Repository
{
    public interface IStaticPageRepository :  IRepositoryBase<StaticPage>
    {       
        List<StaticPageSummary> GetAll();
    }

    public class StaticPageRepository : DbRepositoryBase, IStaticPageRepository
    {            
        public List<StaticPageSummary> GetAll()
        {
            Func<IDataRecord, StaticPageSummary> mapperFunc = (record) =>
               (
                    new StaticPageSummary()
                    {
                        Id =record.GetValue<int>("liStaticPageID"),
                        PageName = record.GetValue<string>("sPageURL").RemoveExtension()  
                    }
               );

            var list = GetListWithCustomMapping("[easds].GetListOfStaticPages", null, mapperFunc);
            return list;           
        }

        public StaticPage Get(int id)
        {
            var obj = GetObjectWithCustomMapping("[easds].GetStaticPage", new { @StaticPageId = id }, MapDataRrowToStaticPage);
            return obj;
        }

        private StaticPage MapDataRrowToStaticPage(IDataRecord record)
        {
            var staticPage = new StaticPage
            {
                Id = record.GetValue<int>("liStaticPageID"),
                PageName = record.GetValue<string>("sPageURL").RemoveExtension(),
                Title = record.GetValue<string>("sTitle"),
                Body = record.GetValue<string>("sBody"),
                TitleForHtmlPage = record.GetValue<string>("sPageTitle"),
                MetaDescription = record.GetValue<string>("sMetaDescription"),
                MetaKeywords = record.GetValue<string>("sMetaKeywords"),
                LastModified = record.GetValue<DateTime>("dtLastModified")
            };
            return staticPage;
        }
        

    }
}
