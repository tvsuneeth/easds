﻿using System;

namespace twg.chk.DataService.Business
{
    public interface IPagination
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
