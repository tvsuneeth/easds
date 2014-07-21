using System;

namespace TWG.EASDataService.Business
{
    public class Person
    {
        public virtual String Title { get; set; }
        public virtual String FirstName { get; set; }
        public virtual String LastName { get; set; }
        public virtual String Email { get; set; }

        public String Names { get { return String.Format("{0} {1}", FirstName, LastName); } }
    }
}
