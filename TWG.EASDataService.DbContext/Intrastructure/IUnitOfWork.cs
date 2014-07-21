using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWG.EASDataService.DbContext.Intrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
