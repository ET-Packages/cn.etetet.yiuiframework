#if TextMeshPro
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YIUIFramework
{
    /// <summary>
    /// 文本自适应宽高：宽度从0增长到最大宽度，超过后自动换行增高
    /// 挂载到带 TMP_Text 的 GameObject 上即可，无需其他组件
    /// </summary>
    [AddComponentMenu("YIUIFramework/Widget/文本最大宽度自适应 【TextMaxWidthSizeFitter】")]
    public class TextMaxWidthSizeFitter : UIBehaviour, ILayoutElement, ILayoutSelfController
    {
        [SerializeField]
        [LabelText("最大宽度")]
        protected float m_MaxWidth = 500f;

        [SerializeField]
        [LabelText("布局优先级")]
        protected int m_LayoutPriority = 2;

        [System.NonSerialized]
        private TMP_Text m_Text;

        private TMP_Text textComponent
        {
            get
            {
                if (m_Text == null)
                    m_Text = GetComponent<TMP_Text>();
                return m_Text;
            }
        }

        [System.NonSerialized]
        private RectTransform m_Rect;

        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        float ILayoutElement.minWidth
        {
            get { return ((ILayoutElement)this).preferredWidth; }
        }

        float ILayoutElement.preferredWidth
        {
            get
            {
                if (textComponent == null) return -1;
                return Mathf.Min(textComponent.preferredWidth, m_MaxWidth);
            }
        }

        float ILayoutElement.flexibleWidth
        {
            get { return -1; }
        }

        float ILayoutElement.minHeight
        {
            get { return ((ILayoutElement)this).preferredHeight; }
        }

        float ILayoutElement.preferredHeight
        {
            get
            {
                if (textComponent == null) return -1;

                var unwrappedWidth = textComponent.preferredWidth;
                if (unwrappedWidth <= m_MaxWidth)
                    return textComponent.preferredHeight;

                // 文本超出最大宽度，按最大宽度换行计算高度
                var values = textComponent.GetPreferredValues(textComponent.text, m_MaxWidth, float.MaxValue);
                return values.y;
            }
        }

        float ILayoutElement.flexibleHeight
        {
            get { return -1; }
        }

        int ILayoutElement.layoutPriority
        {
            get { return m_LayoutPriority; }
        }

        void ILayoutElement.CalculateLayoutInputHorizontal() { }

        void ILayoutElement.CalculateLayoutInputVertical() { }

        void ILayoutController.SetLayoutHorizontal()
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ((ILayoutElement)this).preferredWidth);
        }

        void ILayoutController.SetLayoutVertical()
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ((ILayoutElement)this).preferredHeight);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ForceRebuild();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ForceRebuild();
        }

        public void ForceRebuild()
        {
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

        #if UNITY_EDITOR
        protected override void OnValidate()
        {
            ForceRebuild();
        }
        #endif
    }
}
#endif