using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWG.EASDataService.Data.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveExtension(this string value )
        {
            value = value.Trim();
            return value.Remove(value.LastIndexOf("."));
        }
    }
}
