using System;

namespace TWG.EASDataService.Api.Models
{
    public interface IPaginatedFeed
    {
        LinkItem PreviousLink { get; }
        LinkItem NextLink { get;}
        LinkItem FirstLink { get; }
        LinkItem LastLink { get; }
    }
}
