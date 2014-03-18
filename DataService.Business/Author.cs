using System;

namespace twg.chk.DataService.Business
{
    public class Author
    {
        public virtual String FirstName { get; set; }
        public virtual String LastName { get; set; }

        public String Names { get { return String.Format("{0} {1}", FirstName, LastName); } }
    }
}
