﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using twg.chk.DataService.chkData.Infrastructure;
using twg.chk.DataService.Business;

namespace twg.chk.DataService.chkData.Repository
{
    public interface IAssetRepository : IChkRepositoryBase<MediaContent>
    {
        IEnumerable<MediaContent> Get(int[] ids);
    }

    public class AssetRepository : IAssetRepository
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
                            ContentBinary = (byte[])sqlReader["blobAsset"] 
                        };

                        mediaList.Add(mediaContent);
                    }
                }
            }

            return mediaList;
        }
    }
}