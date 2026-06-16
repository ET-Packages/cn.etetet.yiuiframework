#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    public sealed partial class UIData
    {
        public event Action<UIData> OnDataChangAction;

        public void OnDataChange(string name)
        {
            m_Name = name;
            try
            {
                OnDataChangAction?.Invoke(this);
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                throw;
            }
        }

        //移除注册
        [HideInInspector]
        public Action<UIData> OnDataRemoveAction;

        [GUIColor(0, 1, 1)]
        [Button("移除")]
        [PropertyOrder(100)]
        [ShowIf("@UIOperationHelper.CommonShowIf()")]
        private void OnRemoveDataClick()
        {
            try
            {
                OnDataRemoveAction?.Invoke(this);
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                throw;
            }
        }

        //确定移除
        public void OnDataRemoveCallBack()
        {
            EditorRemoveDestroyedBinds();

            if (m_Binds == null || m_Binds.Count <= 0)
            {
                return;
            }

            for (var i = m_Binds.Count - 1; i >= 0; i--)
            {
                var bind = m_Binds[i];
                if (bind == null)
                {
                    m_Binds.RemoveAt(i);
                    continue;
                }

                bind.RemoveBindData(this);
            }
        }

        private bool ShowIfBindsTips => m_Binds.Count <= 0;
        private bool ShowIfBinds     => m_Binds.Count >= 1;

        [SerializeField]
        [HideReferenceObjectPicker]
        [LabelText("所有绑定关联")]
        [ReadOnly]
        [PropertyOrder(101)]
        [ShowIf("ShowIfBinds")]
        private List<UIDataBind> m_Binds = new List<UIDataBind>();

        public int GetBindCount()
        {
            EditorRemoveDestroyedBinds();
            return m_Binds?.Count ?? 0;
        }

        internal void AddBind(UIDataBind bind)
        {
            if (bind == null)
            {
                return;
            }

            EditorRemoveDestroyedBinds();
            if (m_Binds?.IndexOf(bind) == -1)
            {
                m_Binds?.Add(bind);
            }
        }

        internal void RemoveBind(UIDataBind bind)
        {
            if (m_Binds == null)
            {
                return;
            }

            for (var i = m_Binds.Count - 1; i >= 0; i--)
            {
                var current = m_Binds[i];
                if (current == null || current == bind)
                {
                    m_Binds.RemoveAt(i);
                }
            }
        }

        internal void ClearBinds()
        {
            m_Binds?.Clear();
        }

        private void EditorRemoveDestroyedBinds()
        {
            if (m_Binds == null)
            {
                return;
            }

            for (var i = m_Binds.Count - 1; i >= 0; i--)
            {
                if (m_Binds[i] == null)
                {
                    m_Binds.RemoveAt(i);
                }
            }
        }
    }
}
#endif
