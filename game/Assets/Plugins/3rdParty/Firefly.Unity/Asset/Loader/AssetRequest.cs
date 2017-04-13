using System;

namespace Firefly.Unity.Asset.Loader
{
    class SceneLoadRequest : IAssetLoadRequest
    {
        public Action<bool, float> OnFinished { get; private set; }

        public string AssetPath { get; private set; }

        public SceneLoadRequest(string asset_path, Action<bool, float> load_finish)
        {
            AssetPath = asset_path;
            OnFinished = load_finish;
        }
    }

    class AssetLoadRequest<T> : IAssetLoadRequest where T : UnityEngine.Object
    {
        public Action<T> OnFinished { get; private set; }
        public string AssetPath { get; private set; }

        public AssetLoadRequest(string asset_path, Action<T> load_finish)
        {
            AssetPath = asset_path;
            OnFinished = load_finish;
        }
    }

    public interface IAssetLoadRequest
    {
        string AssetPath { get; }
    }
}

