using System;
using System.Collections.Generic;

namespace twg.chk.DataService.Business
{
    public interface IWebIdentifiable
    {
        Object GetIdentificationElement();
        String GetIdentificationTitle();
    }
}
