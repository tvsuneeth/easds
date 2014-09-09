using System;

using TWG.EASDataService.Data.Repository;
using TWG.EASDataService.Business;
using System.Collections.Generic;

namespace TWG.EASDataService.Services
{
    public interface IMediaService
    {
        MediaContent Get(int id);
        List<ModifiedItem> GetMediaContentItemsModifiedSince(DateTime modifiedSince);
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

        public List<ModifiedItem> GetMediaContentItemsModifiedSince(DateTime modifiedSince)
        {
            return _assetRepository.GetMediaContentItemsModifiedSince(modifiedSince);
        }
    }
}
