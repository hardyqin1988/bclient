using UnityEngine;

namespace Firefly.Unity.Asset.Loader
{
    public class BundleCache : AssetCache
    {
        public AssetBundle Bundle { get; private set; }

        public BundleCache(AssetBundle bundle) : base()
        {
            Bundle = bundle;
        }

        public BundleCache(AssetBundle bundle, Object asset) : base(asset)
        {
            Bundle = bundle;
        }
    }
    public class AssetCache
    {
        public Object Asset { get; private set; }

        public int RefCount { get; private set; }

        public AssetCache()
        {
            RefCount = 1;
        }

        public AssetCache(UnityEngine.Object asset)
        {
            Asset = asset;
            RefCount = 1;
        }

        public void IncRef(int refCount = 1)
        {
            RefCount += refCount;
        }

        public void DecRef(int refCount = 1)
        {
            if (RefCount <= 0) return;
            RefCount -= refCount;
        }
    }
}
