using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.DbContext.Intrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
