using Firefly.Core.Interface;
using Firefly.Unity.Global;
using System.Collections;
using UnityEngine;

namespace Firefly.Unity
{
    public interface IEngine
    {
        bool LoadFinsh { get; }

        bool Running { get; set; }
    }

    public abstract class SingletonEngine<ENGINE> : MonoBehaviour where ENGINE : SingletonEngine<ENGINE>, IEngine
    {
        protected ILog Logger;
        private static ENGINE _instance;
        public static ENGINE Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    System.Type type = typeof(ENGINE);
                    obj.name = "[Engine][" + type.Name + "]";
                    _instance = obj.AddComponent<ENGINE>();

                    _instance.Logger = LogAssert.GetLog(obj.name);
                }
                return _instance;
            }
        }

        public virtual void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            if (_instance == null)
            {
                _instance = this as ENGINE;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (!Running) { return; }

            Loop();
        }

        public bool LoadFinsh { get; private set; }

        public bool Running { get; set; }

        protected abstract IEnumerator Shutdown();

        protected abstract IEnumerator Startup();

        protected virtual void Loop() { }

        // Use this for initialization
        IEnumerator Start()
        {
            yield return Startup();

            LoadFinsh = true;
        }
    }
}

