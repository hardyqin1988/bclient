using Firefly.Unity.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DoozyUI;
using Firefly.Core.Variant;
using Firefly.Unity.Asset;

namespace Firefly.Unity.UI
{
    public class UIEngine : SingletonEngine<UIEngine>, IEngine
    {
        public bool InstantAction { get; private set; }
        public bool ShouldDisable { get; private set; }

        private Dictionary<string, PanelBase> _PanelDic;

        public override void Awake()
        {
            base.Awake();

            _PanelDic = new Dictionary<string, PanelBase>();
        }

        protected override IEnumerator Shutdown()
        {
            yield return new WaitForFixedUpdate();
        }

        protected override IEnumerator Startup()
        {
            ResourceRequest request = Resources.LoadAsync("global/ui_root");
            while (!request.isDone)
            {
                yield return null;
            }

            GameObject go = GameObject.Instantiate(request.asset, Vector3.zero, Quaternion.identity) as GameObject;
            _UIRoot = go.transform;
            _UIRoot.name = "UI_Root";

            _UIRoot.SetParent(transform, Vector3.zero, Quaternion.identity, Vector3.one);

            _UIContainer = _UIRoot.FindChild("UI Container");

            yield return new WaitForFixedUpdate();
        }

        private Transform _UIRoot { get; set; }

        private Transform _UIContainer { get; set; }

        public PANEL ShowPanel<PANEL>(string panel_path) where PANEL : PanelBase
        {
            PANEL panel = GetPanel<PANEL>();
            if (panel == null)
            {
                panel = CreatePanel<PANEL>();
                BindPanel(panel, panel_path);
            }
            else
            {
                panel.Show();
            }

            return panel;
        }

        public bool IsShow<PANEL>() where PANEL : PanelBase
        {
            PANEL panel = GetPanel<PANEL>();
            if (panel == null)
            {
                return false;
            }
            else
            {
                return panel.IsShow;
            }
        }

        public bool HidePanel<PANEL>() where PANEL : PanelBase
        {
            PANEL panel = GetPanel<PANEL>();
            if (panel == null)
            {
                return false;
            }

            panel.Hide();

            return true;
        }

        private void BindPanel(PanelBase panel, string panel_path)
        {
            AssetEngine.Instance.CreateObject("ui/" + panel_path, (go) =>
            {
                go.SetRectParent(_UIContainer.gameObject);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                go.name = "ui/" + panel_path;

                UIElement uiElement = go.GetComponent<UIElement>();
                if (uiElement)
                {
                    panel.UIElement = uiElement;
                    //uiElement.ResetStartPosition(false);
                    // uiElement.isVisible = false;
                }

                panel.PanelObject = go;
                panel.Create();

                panel.Show();
            });
            
        }

        private PANEL CreatePanel<PANEL>() where PANEL : PanelBase
        {
            Type t = typeof(PANEL);
            string name = t.Name;

            string ui_path = name;

            PanelBase panel = null;
            if (_PanelDic.TryGetValue(name, out panel))
            {
                return panel as PANEL;
            }

            panel = Activator.CreateInstance<PANEL>();
            panel.Name = name;

            _PanelDic.Add(name, panel);

            return panel as PANEL;
        }

        public PANEL GetPanel<PANEL>() where PANEL : PanelBase
        {
            Type t = typeof(PANEL);
            string name = t.Name;

            PanelBase panel = null;
            if (!_PanelDic.TryGetValue(name, out panel))
            {
                return null;
            }

            return panel as PANEL;
        }

        public void OnRecvGameEvent(string gameEvent, VariantList param)
        {
            foreach (PanelBase panel in _PanelDic.Values)
            {
                if (!panel.IsShow)
                {
                    continue;
                }

                panel.OnEvent(gameEvent, param);
            }
        }
    }
}
