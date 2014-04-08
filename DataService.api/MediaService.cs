using System;

using twg.chk.DataService.chkData.Repository;
using twg.chk.DataService.Business;

namespace twg.chk.DataService.api
{
    public interface IMediaService
    {
        MediaContent Get(int id);
    }

    public class MediaService : IMediaService
    {
        private IAssetRepository _assetRepository;
        public MediaService(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public MediaContent Get(int id)
        {
            return _assetRepository.Get(id);
        }
    }
}
