using System.Web;
using System.Web.Mvc;

namespace twg.chk.DataService.BackOffice
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
