using System;

namespace Firefly.Unity.Asset.Loader
{
    public interface IAssetLoader
    {
        bool FindCache(string asset_path);

        bool IsLoading(string asset_path);

        void LoadAsset<T>(string asset_path, Action<T> load_finish) where T : UnityEngine.Object;

        void LoadScene(string scene_path, Action<bool, float> load_finish);
    }
}
