using Firefly.Unity.Asset.Loader;
using Firefly.Unity.Global;
using Firefly.Unity.Utility;
using System;
using System.Collections;
using System.IO;
using System.Text;
using UniRx;
using UnityEngine;

namespace Firefly.Unity.Asset
{
    public class AssetEngine : SingletonEngine<AssetEngine>, IEngine
    {
        public const string PAK_CONFIG = "config";
        public const string PAK_LUA    = "lua";
        public const string PAK_BUNDLE = "bundle";

        private int _CurLoadedPaks = 0;
        private int _MaxLoadedPaks = 3;

        public IAssetLoader Loader { get; private set; }

        public override void Awake()
        {
            base.Awake();

            string folder = StringToUTF8(AssetPath.PersistentDataPath) + "Build";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public static string StringToUTF8(string strcode)
        {
            return Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(strcode));
        }

        private void InstallPak(string pak)
        {
            string config_path = "";

            if (Application.platform == RuntimePlatform.Android)
            {
                config_path = AssetPath.StreamingPath + pak;
            }
            else
            {
                config_path = "file://" + AssetPath.StreamingPath + pak;
            }

            ObservableWWW.GetAndGetBytes(config_path).Subscribe(
                bytes =>
                {
                    string path = StringToUTF8(AssetPath.PersistentDataPath + "Build/" + pak);
                    int res = lzip.decompress_File(AssetPath.StreamingPath + pak + "", path, new int[1]);
                    if (res == 1)
                    {
                        _CurLoadedPaks++;
                    }
                });
        }

        protected override IEnumerator Startup()
        {
            InstallPak(PAK_CONFIG);
            InstallPak(PAK_LUA);
            InstallPak(PAK_BUNDLE);

            while (_CurLoadedPaks != _MaxLoadedPaks)
            {
                yield return null;
            }

            GameObject loader = new GameObject("Loader");
            loader.transform.parent = this.transform;
            Loader = loader.AddComponent<ResourceLoader>();
            yield return new WaitForFixedUpdate();

            GameObject pool = new GameObject("Pool");
            pool.transform.parent = this.transform;
            pool.AddComponent<FastPoolManager>();
            yield return new WaitForFixedUpdate();
        }

        protected override IEnumerator Shutdown()
        {
            yield return new WaitForFixedUpdate();
        }

        public void LoadAsset<T>(string asset_path, Action<T> load_finish) where T : UnityEngine.Object
        {
            Loader.LoadAsset<T>(asset_path, (asset) => {
                load_finish(asset);
            });
        }

        public void LoadScene(string scene_path, Action<bool, float> load_finish)
        {
            Loader.LoadScene(scene_path, (is_done, progress) => {
                load_finish(is_done, progress);
            });
        }

        public void CreateObject(string asset_path, Action<GameObject> load_finish)
        {
            Loader.LoadAsset<UnityEngine.Object>(asset_path, (asset) => {
                FastPool pool = FastPoolManager.CreatePool(asset as GameObject);
                GameObject child = pool.FastInstantiate();

                load_finish(child);
            });
        }

        public void CreateObject(string asset_path, Transform parent, Action<GameObject> load_finish)
        {
            Loader.LoadAsset<UnityEngine.Object>(asset_path, (asset) => {
                GameObject prefab = GameObject.Instantiate(asset) as GameObject;
                FastPool pool = FastPoolManager.CreatePool(prefab);
                GameObject child = pool.FastInstantiate(parent);

                load_finish(child);
            });
        }

        public void CreateObject(string asset_path, Vector3 position, Quaternion rotation,
            Action<GameObject> load_finish)
        {
            Loader.LoadAsset<UnityEngine.Object>(asset_path, (asset) => {
                GameObject prefab = GameObject.Instantiate(asset) as GameObject;
                FastPool pool = FastPoolManager.CreatePool(prefab);
                GameObject child = pool.FastInstantiate(position, rotation);

                load_finish(child);
            });
        }

        public void CreateObject(string asset_path, Transform parent, Vector3 position, Quaternion rotation,
            Action<GameObject> load_finish)
        {
            Loader.LoadAsset<UnityEngine.Object>(asset_path, (asset) => {
                GameObject prefab = GameObject.Instantiate(asset) as GameObject;
                FastPool pool = FastPoolManager.CreatePool(prefab);
                GameObject child = pool.FastInstantiate(position, rotation, parent);

                load_finish(child);
            });
        }
    }
}

