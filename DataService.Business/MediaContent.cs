using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.Business
{
    public class MediaContent
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public String FileName { get; set; }
        public String Extension { get; set; }
        public byte[] ContentBinary { get; set; }
    }
}
