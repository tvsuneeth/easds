using System;

using TWG.EASDataService.Data.Repository;
using TWG.EASDataService.Business;

namespace TWG.EASDataService.Services
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
