using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    [Serializable]
    [LabelText("UI大小-高")]
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("YIUIBind/Data/UI大小-高 【RectSizeHeight】 UIDataBindRectSizeHeight")]
    public class UIDataBindRectSizeHeight : UIDataBindSelectBase
    {
        protected override int Mask()
        {
            return 1 << (int)EUIBindDataType.Float;
        }

        protected override int SelectMax()
        {
            return 1;
        }

        [SerializeField]
        [ReadOnly]
        [Required("必须有此组件")]
        [LabelText("UI变换组件")]
        private RectTransform m_RectTransform;

        private Vector2 m_SizeData;

        protected override void OnRefreshData()
        {
            base.OnRefreshData();
            m_RectTransform = GetComponent<RectTransform>();
        }

        protected override void OnValueChanged()
        {
            if (m_RectTransform == null) return;

            m_SizeData.y              = GetFirstValue<float>();
            m_SizeData.x              = m_RectTransform.sizeDelta.x;
            m_RectTransform.sizeDelta = m_SizeData;
        }
    }
}
