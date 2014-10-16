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
using TWG.EASDataService.Data.Extensions;

namespace TWG.EASDataService.Data.Repository
{
    public interface ISlotRepository :  IRepositoryBase<Slot>
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
            string commandName = @"[easds].[GetSlotPages]";            
            return GetListWithAutoMapping<SlotPageSummary>(commandName, new { slotPageId=-1 });           
        }
       
        public SlotPage GetSlotPageById(int slotPageId)
        {
            SlotPage sp = null;

            using (var connection = CreateConnection())
            {
                using (var cmd = CreateCommand(connection, @"[easds].[GetSlotPageWithSlots]", new { @slotPageId = slotPageId }))
                {                    
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {                        
                        sp = new SlotPage()
                        {
                            Id = reader.GetValue<int>("liSlotPageID"),
                            PageName = reader.GetValue<string>("sPageName")
                        };
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        Slot slot = new Slot()
                        {
                            Id = reader.GetValue<int>("liSlotID"),
                            Headline = reader.GetValue<string>("sHeadline"),
                            SlotName = reader.GetValue<string>("sSlotName"),
                            Content = reader.GetValue<string>("sContent"),
                            URL = reader.GetValue<string>("sURL"),
                            URLTitle = reader.GetValue<string>("sURLTitle"),
                            AccessKey = reader.GetValue<string>("sAccessKey"),
                            LastModifiedDate = reader.GetValue<DateTime>("dtLastModified")
                        };
                        int imageId = reader.GetValue<int>("liImageID");
                        if(imageId!=0)
                        { 
                            Image img = new Image()
                            {
                                Id = imageId,
                                Name = reader.GetValue<string>("sAssetName"),
                                Width = reader.GetValue<int>("iWidth"),
                                Height = reader.GetValue<int>("iHeight"),
                                Extension = reader.GetValue<string>("sFileExt"),
                                CreatedDate = reader.GetValue<DateTime>("imageCreatedDate"),
                                LastModifiedDate = reader.GetValue<DateTime>("imageLastModifiedDate")
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
