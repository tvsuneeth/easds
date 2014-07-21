using System;

namespace TWG.EASDataService.Business
{
    public interface IMediaAttachment
    {
        bool HasAttachedMedia { get; }
        MediaContent AttachedMedia { get; }
    }
}
