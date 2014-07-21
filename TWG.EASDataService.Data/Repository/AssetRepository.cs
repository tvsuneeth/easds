using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using TWG.EASDataService.Data.Infrastructure;
using TWG.EASDataService.Business;

namespace TWG.EASDataService.Data.Repository
{
    public interface IAssetRepository : IChkRepositoryBase<MediaContent>
    {
        IEnumerable<MediaContent> Get(int[] ids);        
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
            var mediaList = new List<MediaContent>();

            var mediaIdsArray = ids.Select(id => id.ToString()).ToArray();
            var mediaIdsDataTable = Helpers.ElementTableHelper.BuidTable(mediaIdsArray);

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyChk"].ConnectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "chk.GetAsset";

                    var assetIdsParam = command.Parameters.AddWithValue("@AssetIds", mediaIdsDataTable);
                    assetIdsParam.SqlDbType = SqlDbType.Structured;

                    var sqlReader = command.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var mediaContent = new MediaContent
                        {
                            Id = Convert.ToInt32(sqlReader["liAssetID"]),
                            FileName = Convert.ToString(sqlReader["sAssetName"]),
                            Extension = Convert.ToString(sqlReader["sFileExt"]).ToLower(),
                            ContentBinary = (byte[])sqlReader["blobAsset"],
                            Type = (MediaContentType)GetValue<int>(sqlReader["liAssetTypeID"]),                            
                            Description = GetValue<string>(sqlReader["sAssetDescription"]),
                            Height = GetValue<int>(sqlReader["iHeight"]),
                            Width = GetValue<int>(sqlReader["iWidth"]),
                            CreatedDate = GetValue<DateTime>(sqlReader["dtEntered"]),
                            LastModifiedDate = GetValue<DateTime>(sqlReader["dtLastModified"]),                            
                        };

                        mediaList.Add(mediaContent);
                    }
                }
            }

            return mediaList;
        }
    }
}
