using System;
using System.Collections.Generic;

namespace TWG.EASDataService.Business
{
    public interface IWebIdentifiable
    {
        Object GetIdentificationElement();
        String GetIdentificationTitle();       
    }
}
