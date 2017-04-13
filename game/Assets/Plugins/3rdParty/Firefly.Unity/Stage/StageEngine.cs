using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firefly.Unity.Stage
{
    public class StageEngine : SingletonEngine<StageEngine>, IEngine
    {
        private Dictionary<string, StageBase> _StageDic = null;

        enum StageStatus : byte
        {
            None    = 1,
            Load    = 2,
            Unload  = 3,
        }

        public override void Awake()
        {
            base.Awake();

            _StageDic = new Dictionary<string, StageBase>();
        }

        private StageStatus _StageStatus = StageStatus.None;

        private StageBase _CurStage;

        private bool Switching
        {
            get
            {
                return _StageStatus != StageStatus.None;
            }
        }

        protected override IEnumerator Shutdown()
        {
            yield return new WaitForFixedUpdate();
        }

        protected override IEnumerator Startup()
        {
            yield return new WaitForFixedUpdate();
        }

        public void Open<STAGE>() where STAGE : StageBase
        {
            if (Switching)
            {
                Logger.Warn("is Switching!");
                return;
            }

            STAGE stage = GetStage<STAGE>();
            if (stage == null)
            {
                stage = CreateStage<STAGE>();

                _StageDic.Add(stage.Name, stage);
            }

            StartCoroutine(LoadStage(stage));
        }

        private IEnumerator LoadStage(StageBase stage)
        {
            if (_CurStage != null)
            {
                _StageStatus = StageStatus.Unload;
                yield return _CurStage.OnUnload();
            }

            _StageStatus = StageStatus.Load;
            yield return stage.OnLoad();

            _StageStatus = StageStatus.None;
        }

        private STAGE GetStage<STAGE>() where STAGE : StageBase
        {
            Type t = typeof(STAGE);
            string name = t.Name;

            StageBase panel = null;
            if (!_StageDic.TryGetValue(name, out panel))
            {
                return null;
            }

            return panel as STAGE;
        }

        private STAGE CreateStage<STAGE>() where STAGE : StageBase
        {
            Type t = typeof(STAGE);
            string name = t.Name;

            StageBase stage = null;
            if (_StageDic.TryGetValue(name, out stage))
            {
                return stage as STAGE;
            }

            stage = Activator.CreateInstance<STAGE>();
            stage.Name = name;

            return stage as STAGE;
        }
    }
}

