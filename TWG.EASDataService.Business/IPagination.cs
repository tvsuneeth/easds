using System;
using System.Collections.Generic;

namespace TWG.EASDataService.Business
{
    public interface IPagination<T> : IList<T>, IEnumerable<T>
    {
        bool HasMultiplePage { get; }
        int CurrentPage { get; }
        bool HasNextPage { get; }
        int NextPage { get; }
        bool HasPreviousPage { get; }
        int PreviousPage { get; }
        int FirstPage { get; }
        int LastPage { get; }
    }
}
