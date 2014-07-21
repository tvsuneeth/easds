using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWG.EASDataService.Business
{
    public class SlotPage
    {
        public SlotPage()
        {
            Slots = new List<Slot>();
        }
        public int Id { get; set; }
        public string PageName { get; set; }
        public IList<Slot> Slots { get; set; }
    }
}
