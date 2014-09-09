using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWG.EASDataService.Business
{
    public class ModifiedItem
    {
        public int Id { get; set; }
        public DateTime ModifiedDate { get; set; }
        public ItemStatus CurrentStatus { get; set; }
    }

    public enum ItemStatus
    {
        Live = 1,
        Deleted = 2,        
   }
}
