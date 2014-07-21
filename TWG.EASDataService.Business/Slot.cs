using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWG.EASDataService.Business
{
    public class Slot
    {
        public int Id { get; set; }
        public string SlotName { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public string URL { get; set; }
        public string URLTitle { get; set; }
        public string AccessKey { get; set; }        
        public DateTime LastModifiedDate { get; set; }
        public Image Image { get; set; }
    }
}
