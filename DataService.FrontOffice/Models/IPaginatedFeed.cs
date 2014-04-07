using System;

namespace twg.chk.DataService.FrontOffice.Models
{
    public interface IPaginatedFeed
    {
        LinkItem PreviousLink { get; }
        LinkItem NextLink { get;}
        LinkItem FirstLink { get; }
        LinkItem LastLink { get; }
    }
}
