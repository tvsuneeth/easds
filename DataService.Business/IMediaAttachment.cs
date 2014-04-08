using System;

namespace twg.chk.DataService.Business
{
    public interface IMediaAttachment
    {
        bool HasAttachedMedia { get; }
        MediaContent AttachedMedia { get; }
    }
}
