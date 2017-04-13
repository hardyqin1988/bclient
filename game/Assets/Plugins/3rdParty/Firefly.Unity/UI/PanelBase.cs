using DoozyUI;
using Firefly.Core.Variant;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Firefly.Unity.UI
{
    public abstract class PanelBase
    {
        /// <summary>
        /// panel被创建
        /// </summary>
        protected abstract void OnCreate();

        /// <summary>
        /// panel被销毁
        /// </summary>
        protected abstract void OnDestroy();

        /// <summary>
        /// panel被显示
        /// </summary>
        protected abstract void OnShow();

        /// <summary>
        /// panel被隐藏
        /// </summary>
        protected abstract void OnHide();

        public virtual void OnEvent(string event_id, VariantList param) { }

        public string Name { get; set; }

        public GameObject PanelObject { get; set; }

        public UIElement UIElement { get; set; }

        public bool IsShow { get; private set; }

        private void RegisterButtonClickMusic()
        {
            Button[] btnList = PanelObject.GetComponentsInChildren<Button>();

            foreach (Button btn in btnList)
            {
                if (btn != null)
                {
                    btn.onClick.AddListener(() =>
                    {
                    });
                }
            }

            Toggle[] togList = PanelObject.GetComponentsInChildren<Toggle>();
            foreach (Toggle btn in togList)
            {
                if (btn != null)
                {
                    btn.onValueChanged.AddListener((bool isOn) =>
                    {
                    });
                }
            }
        }

        internal void Create()
        {
            RegisterButtonClickMusic();

            OnCreate();
        }

        internal void Show()
        {
            IsShow = true;

            if (UIElement != null)
            {
                UIElement.Show(UIEngine.Instance.InstantAction);
            }
            else if (PanelObject != null)
            {
                PanelObject.SetActive(true);
            }

            PanelObject.transform.SetAsLastSibling();
            OnShow();
        }

        internal void Hide()
        {
            IsShow = false;

            if (UIElement != null)
            {
                UIElement.Hide(UIEngine.Instance.InstantAction, UIEngine.Instance.ShouldDisable);
            }
            else if (PanelObject != null)
            {
                PanelObject.SetActive(false);
            }

            OnHide();
        }
    }
}

