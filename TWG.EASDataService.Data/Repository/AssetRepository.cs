using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TWG.EASDataService.Data.Extensions;
using TWG.EASDataService.Data.Infrastructure;
using TWG.EASDataService.Business;

namespace TWG.EASDataService.Data.Repository
{
    public interface IAssetRepository : IChkRepositoryBase<MediaContent>
    {
        IEnumerable<MediaContent> Get(int[] ids);
        List<ModifiedItem> GetMediaContentItemsModifiedSince(DateTime modifiedSince);
    }

    public class AssetRepository : DbRepositoryBase, IAssetRepository
    {
        public MediaContent Get(int id)
        {
            var mediaList = Get(new int[] { id });
            return mediaList.SingleOrDefault();
        }
      
        public IEnumerable<MediaContent> Get(int[] ids)
        {           
            var mediaIdsArray = ids.Select(id => id.ToString()).ToArray();
            var mediaIdsDataTable = Helpers.ElementTableHelper.BuidTable(mediaIdsArray);
            var assetIdsParam = new SqlParameter("@AssetIds", mediaIdsDataTable);
            assetIdsParam.SqlDbType = SqlDbType.Structured;
            var parameterColl = new List<SqlParameter>(){ assetIdsParam };

            Func<IDataRecord,MediaContent> mapperFunc = (record) =>
                (
                        new MediaContent()
                        {
                            Id = record.GetValue<int>("liAssetID"),
                            FileName = record.GetValue<String>("sAssetName"),
                            Extension =  record.GetValue<String>("sFileExt").ToLower(),
                            ContentBinary = record.GetValue<byte[]>("blobAsset"),
                            Type = (MediaContentType)record.GetValue<int>("liAssetTypeID"),                            
                            Description = record.GetValue<String>("sAssetDescription"),
                            Height = record.GetValue<int>("iHeight"),
                            Width = record.GetValue<int>("iWidth"),
                            CreatedDate = record.GetValue<DateTime>("dtEntered"),
                            LastModifiedDate = record.GetValue<DateTime>("dtLastModified")                            
                        }

                );

            var mediaList = new List<MediaContent>();
            mediaList = GetListWithCustomMapping("[easds].GetAsset", parameterColl, mapperFunc);
            return mediaList;
        }

        public List<ModifiedItem> GetMediaContentItemsModifiedSince(DateTime modifiedSince)
        {
            var list = GetListWithAutoMapping<ModifiedItem>("[easds].[GetAssetsModifiedSince]", new { @modifiedDate = modifiedSince });
            return list;
        }


    }
}
