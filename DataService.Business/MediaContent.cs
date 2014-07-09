using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;


namespace twg.chk.DataService.Business
{
    [DataContract]
    public class MediaContent: IWebIdentifiable
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public String FileName { get; set; }

        [DataMember]
        public String Extension { get; set; }        

        [DataMember]
        public MediaContentType Type { get; set;}

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int Width { get; set; }

        [DataMember]
        public int Height { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]        
        public DateTime LastModifiedDate { get; set; }

        public byte[] ContentBinary { get; set; }
        public object GetIdentificationElement() { return new { Id }; }
        public String GetIdentificationTitle() { return FileName; }
    }


    public enum MediaContentType
    {
        Image    = 1,
        PDF      = 2 ,
        WordDoc  = 3,
        Flash    = 4
    }
}
