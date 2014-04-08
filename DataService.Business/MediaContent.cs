using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.Business
{
    public class MediaContent: IWebIdentifiable
    {
        public int Id { get; set; }
        public String FileName { get; set; }
        public String Extension { get; set; }
        public byte[] ContentBinary { get; set; }

        public object GetIdentificationElement() { return new { Id }; }
        public String GetIdentificationTitle() { return FileName; }
    }
}
