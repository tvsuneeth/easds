using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWG.EASDataService.Business;
using TWG.EASDataService.Data.Repository;

namespace TWG.EASDataService.Services
{
    public interface ISlotService
    {
        SlotPage GetSlotPageById(int slotPageId);
        List<SlotPageSummary> GetListOfSlotPages();
    }

    public class SlotService : ISlotService
    {
        private ISlotRepository _slotrepository;
        public SlotService(ISlotRepository  slotRepository)
        {
            _slotrepository = slotRepository;
        }
        public SlotPage GetSlotPageById(int slotPageId)
        {
            var sp = _slotrepository.GetSlotPageById(slotPageId);
            return sp;
        }
        public List<SlotPageSummary> GetListOfSlotPages()
        {
            return  _slotrepository.GetListOfSlotPages();
        }
    }
}
