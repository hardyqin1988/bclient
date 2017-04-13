using Firefly.Unity.Global;
using Firefly.Unity.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityObject = UnityEngine.Object;

namespace Firefly.Unity.Asset.Loader
{
    public class BundleLoader : MonoBehaviour, IAssetLoader
    {
        private AssetBundleManifest                         _AssetBundleManifest    = null;

        private Dictionary<string, BundleCache>             _RefCaches              = null;

        private Dictionary<string, List<IAssetLoadRequest>> _LoadRequests           = null;

        private HashSet<string>                             _LoadingCaches          = null;

        enum ManifestStatus : byte
        {
            None    = 0,
            Loading = 1,
            Finish  = 2,
        }

        private ManifestStatus Manifest = ManifestStatus.None;

        private void Awake()
        {
            _RefCaches     = new Dictionary<string, BundleCache>();
            _LoadRequests  = new Dictionary<string, List<IAssetLoadRequest>>();
            _LoadingCaches = new HashSet<string>();
        }

        private IEnumerator CheckManifest()
        {
            if (Manifest == ManifestStatus.Loading)
            {
                yield return null;
            }

            if (Manifest == ManifestStatus.Finish)
            {
                yield break;
            }

            if (Manifest == ManifestStatus.None)
            {
                Manifest = ManifestStatus.Loading;

                string manifest = AssetPath.BundlePath + "manifest";

                AssetBundleCreateRequest create_request = AssetBundle.LoadFromFileAsync(manifest);
                yield return create_request;

                AssetBundleRequest load_request = create_request.assetBundle.LoadAssetAsync<AssetBundleManifest>(BundleUtil.ASSET_BUNDLE_MANIFEST);
                yield return load_request;
                _AssetBundleManifest = (AssetBundleManifest)load_request.asset;

                Manifest = ManifestStatus.Finish;
            }
        }

        void AddRequest(IAssetLoadRequest request)
        {
            List<IAssetLoadRequest> requests = null;
            if (!_LoadRequests.TryGetValue(request.AssetPath, out requests))
            {
                requests = new List<IAssetLoadRequest>();
                _LoadRequests.Add(request.AssetPath, requests);
            }
            requests.Add(request);
        }

        public void LoadAsset<T>(string asset_path, Action<T> load_finish) where T : UnityObject
        {
            AssetLoadRequest<T> request = new AssetLoadRequest<T>(asset_path, load_finish);
            AddRequest(request);

            if (!FindCache(asset_path) && !IsLoading(asset_path))
            {
                StartCoroutine(OnLoadAsset<T>(asset_path));
            }
        }

        public void LoadScene(string scene_path, Action<bool, float> load_finish)
        {
            SceneLoadRequest request = new SceneLoadRequest(scene_path, load_finish);
            AddRequest(request);

            if (IsLoading(scene_path))
            {
                return;
            }

            StartCoroutine(OnLoadScene(scene_path));
        }

        IEnumerator OnLoadScene(string scene_path)
        {
            yield return CheckManifest();

            yield return OnLoadDepAsset(scene_path);

            AssetBundleCreateRequest create_bundle_request = AssetBundle.LoadFromFileAsync(AssetPath.BundlePath + scene_path);
            yield return create_bundle_request;

            string scene_name = BundleUtil.GetAssetName(scene_path);
            AsyncOperation scene = SceneManager.LoadSceneAsync(scene_name);

            List<IAssetLoadRequest> requests = null;
            if (!_LoadRequests.TryGetValue(scene_path, out requests))
            {
                yield break;
            }

            while (!scene.isDone)
            {
                for (int i = 0; i < requests.Count; i++)
                {
                    SceneLoadRequest request = (SceneLoadRequest)requests[i];
                    if (request != null)
                    {
                        request.OnFinished(false, scene.progress);
                    }
                }

                yield return null;
            }

            for (int i = 0; i < requests.Count; i++)
            {
                SceneLoadRequest request = (SceneLoadRequest)requests[i];
                if (request != null)
                {
                    request.OnFinished(true, 1.0f);
                }
            }

            requests.Clear();
            _LoadRequests.Remove(scene_path);
        }

        IEnumerator OnLoadDepAsset(string asset_path)
        {
            string[] dependencies = _AssetBundleManifest.GetAllDependencies(asset_path);
            if (dependencies.Length > 0)
            {
                for (int i = 0; i < dependencies.Length; i++)
                {
                    string dep_path = dependencies[i];
                    BundleCache bundle_cache = null;
                    if (_RefCaches.TryGetValue(dep_path, out bundle_cache))
                    {
                        bundle_cache.IncRef();
                    }
                    else if (!IsLoading(dep_path))
                    {
                        yield return OnLoadAsset<UnityObject>(dep_path);
                    }
                }
            }
        }

        IEnumerator OnLoadAsset<T>(string asset_path) where T : UnityObject
        {
            yield return CheckManifest();

            _LoadingCaches.Add(asset_path);

            yield return OnLoadDepAsset(asset_path);

            AssetBundleCreateRequest create_bundle_request = AssetBundle.LoadFromFileAsync(AssetPath.BundlePath + asset_path);
            yield return create_bundle_request;

            string asset_name = BundleUtil.GetAssetName(asset_path);
            AssetBundleRequest load_asset_request = create_bundle_request.assetBundle.LoadAssetAsync(asset_name);
            yield return load_asset_request;

            List<IAssetLoadRequest> requests = null;
            if (!_LoadRequests.TryGetValue(asset_path, out requests))
            {
                yield break;
            }

            for (int i = 0; i < requests.Count; i++)
            {
                AssetLoadRequest<T> request = (AssetLoadRequest<T>)requests[i];
                if (request != null)
                {
                    request.OnFinished((T)load_asset_request.asset);
                }
            }

            requests.Clear();
            _LoadRequests.Remove(asset_path);

            BundleCache cache = new BundleCache(create_bundle_request.assetBundle, load_asset_request.asset);
            _RefCaches.Add(asset_path, cache);
            _LoadingCaches.Remove(asset_path);
        }

        public bool FindCache(string asset_path)
        {
            return _RefCaches.ContainsKey(asset_path);
        }

        public bool IsLoading(string asset_path)
        {
            return _LoadingCaches.Contains(asset_path);
        }

        /// <summary>
        /// Unloads assetbundle and its dependencies.
        /// </summary>
        private void UnloadAsset(string asset_path)
        {
            UnloadAssetInternal(asset_path);
            UnloadDependencies(asset_path);
        }

        private void UnloadDependencies(string asset_path)
        {
            string[] dependencies = _AssetBundleManifest.GetAllDependencies(asset_path);
            for (int i = 0; i < dependencies.Length; i++)
            {
                string dep_path = dependencies[i];
                UnloadAssetInternal(dep_path);
            }
        }

        private void UnloadAssetInternal(string asset_path)
        {
            BundleCache cache = null;
            if (!_RefCaches.TryGetValue(asset_path, out cache))
            {
                return;
            }

            cache.DecRef();

            if (cache.RefCount == 0)
            {
                cache.Bundle.Unload(false);
                _RefCaches.Remove(asset_path);

                LogAssert.Util.Debug("asset_bundle:{0} has been unloaded successfully.", asset_path);
            }
        }
    }
}
