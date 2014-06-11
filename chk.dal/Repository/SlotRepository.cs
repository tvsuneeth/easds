using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using twg.chk.DataService.Business;
using twg.chk.DataService.chkData.Infrastructure;

namespace twg.chk.DataService.chkData.Repository
{
    public interface ISlotRepository : IChkRepositoryBase<Slot>
    {
        SlotPage GetSlotPageById(int slotPageId);
        List<SlotPageSummary> GetListOfSlotPages();
    }

    public class SlotRepository : ISlotRepository
    {
        public Slot Get(int id)
        {
            return null;
        }

        public List<SlotPageSummary> GetListOfSlotPages()
        {
            var list = new List<SlotPageSummary>();
           
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyChk"].ConnectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = @"[chk].[GetSlotPages]";

                    cmd.Parameters.Add(new SqlParameter("@slotPageId", -1));

                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var sp = new SlotPageSummary()
                        {
                            Id = Convert.ToInt32(reader["liSlotPageID"]),
                            PageName = Convert.ToString(reader["sPageName"])
                        };
                        list.Add(sp);
                    }                                       
                }
            }

            return list;        
        }

        public SlotPage GetSlotPageById(int slotPageId)
        {
            SlotPage sp = null;
           
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyChk"].ConnectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = @"[chk].[GetSlotPageWithSlots]";

                    cmd.Parameters.Add(new SqlParameter("@slotPageId", slotPageId));

                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        sp = new SlotPage()
                        {
                            Id = Convert.ToInt32(reader["liSlotPageID"]),
                            PageName = Convert.ToString(reader["sPageName"])
                        };
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {

                        Slot slot = new Slot();

                        slot.Id = Convert.ToInt32(reader["liSlotID"]);
                        slot.Headline = Convert.ToString(reader["sHeadline"]);
                        slot.SlotName = Convert.ToString(reader["sSlotName"]);
                        slot.Content = Convert.ToString(reader["sContent"]);
                        slot.URL = Convert.ToString(reader["sURL"]);
                        slot.URLTitle = Convert.ToString(reader["sURLTitle"]);
                        slot.AccessKey = Convert.ToString(reader["sAccessKey"]);                        
                        slot.dtLastModified = Convert.ToDateTime(reader["dtLastModified"]);

                        int imageId = Convert.ToInt32((reader["liImageID"] != DBNull.Value) ? reader["liImageID"] : -1);
                        if(imageId!=-1)
                        { 
                            Image img = new Image()
                            {
                                Id = imageId,
                                Name = Convert.ToString(reader["sAssetName"]),
                                Width = Convert.ToInt32((reader["iWidth"] != DBNull.Value) ? reader["iWidth"] : 0),
                                Height = Convert.ToInt32((reader["iHeight"] != DBNull.Value) ? reader["iHeight"] : 0),
                                Extension = Convert.ToString(reader["sFileExt"]),
                                CreatedDate = Convert.ToDateTime(reader["imageCreatedDate"]),
                                LastModifiedDate = Convert.ToDateTime(reader["imageLastModifiedDate"])
                            };
                            slot.Image = img;
                        }
                        sp.Slots.Add(slot);
                    }
                }
            }

            return sp;
        }
    }
}
