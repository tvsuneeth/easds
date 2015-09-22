using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWG.EASDataService.Business
{
    public class Author
    {
        public int ID { get; set; }
        public virtual String Title { get; set; }
        public virtual String FirstName { get; set; }
        public virtual String LastName { get; set; }
        public virtual String Email { get; set; }                
        public int ThumbnailImageID { get; set; }
        public string TelephoneNumber { get; set; }
        public String Names { get { return String.Format("{0} {1}", FirstName, LastName); } }
        public string Description { get; set; }
    }
}
