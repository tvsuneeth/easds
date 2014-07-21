using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWG.EASDataService.Business;
using TWG.EASDataService.Data.Infrastructure;

namespace TWG.EASDataService.Data.Repository
{
    public interface ISlotRepository :  IChkRepositoryBase<Slot>
    {
        SlotPage GetSlotPageById(int slotPageId);
        List<SlotPageSummary> GetListOfSlotPages();
    }

    public class SlotRepository : DbRepositoryBase, ISlotRepository
    {
        public Slot Get(int id)
        {
            return null;
        }
        
        public List<SlotPageSummary> GetListOfSlotPages()
        {            
            string commandName = @"[chk].[GetSlotPages]";            
            return FillListWithAutoMapping<SlotPageSummary>(commandName, new { slotPageId=-1 });           
        }
       
        public SlotPage GetSlotPageById(int slotPageId)
        {
            SlotPage sp = null;

            using (var connection = CreateConnection())
            {                
                using (var cmd = CreateCommand(connection, @"[chk].[GetSlotPageWithSlots]"))
                {
                    AddCommandParameter(cmd, "@slotPageId", slotPageId);                                        
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {                        
                        sp = new SlotPage()
                        {
                            Id = GetValue<int>(reader["liSlotPageID"]),
                            PageName = GetValue<string>(reader["sPageName"])
                        };
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        Slot slot = new Slot()
                        {
                            Id = GetValue<int>(reader["liSlotID"]),
                            Headline = GetValue<string>(reader["sHeadline"]),
                            SlotName = GetValue<string>(reader["sSlotName"]),
                            Content = GetValue<string>(reader["sContent"]),
                            URL = GetValue<string>(reader["sURL"]),
                            URLTitle = GetValue<string>(reader["sURLTitle"]),
                            AccessKey = GetValue<string>(reader["sAccessKey"]),
                            LastModifiedDate = GetValue<DateTime>(reader["dtLastModified"]),
                        };
                        int imageId = GetValue<int>(reader["liImageID"]);
                        if(imageId!=0)
                        { 
                            Image img = new Image()
                            {
                                Id = imageId,
                                Name = GetValue<string>(reader["sAssetName"]),
                                Width = GetValue<int>(reader["iWidth"]),
                                Height = GetValue<int>(reader["iHeight"]),
                                Extension = GetValue<string>(reader["sFileExt"]),
                                CreatedDate = GetValue<DateTime>(reader["imageCreatedDate"]),
                                LastModifiedDate = GetValue<DateTime>(reader["imageLastModifiedDate"])
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
