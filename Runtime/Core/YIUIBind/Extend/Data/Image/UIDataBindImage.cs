﻿using System;
using ET;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace YIUIFramework
{
    [RequireComponent(typeof(Image))]
    [LabelText("Image 图片")]
    [AddComponentMenu("YIUIBind/Data/图片 【Image】 UIDataBindImage")]
    public sealed class UIDataBindImage : UIDataBindSelectBase
    {
        [SerializeField]
        [ReadOnly]
        [Required("必须有此组件")]
        [LabelText("图片")]
        private Image m_Image;

        [SerializeField]
        [LabelText("自动调整图像大小")]
        private bool m_SetNativeSize = false;

        [SerializeField]
        [LabelText("可修改Enabled")]
        private bool m_ChangeEnabled = true;

        [NonSerialized]
        private string m_LastSpriteName;

        protected override int Mask()
        {
            return 1 << (int)EUIBindDataType.String;
        }

        protected override int SelectMax()
        {
            return 1;
        }

        protected override void OnRefreshData()
        {
            base.OnRefreshData();
            m_Image = GetComponent<Image>();
            if (!m_ChangeEnabled && !m_Image.enabled)
            {
                Logger.LogError($"{name} 当前禁止修改Enabled 且当前处于隐藏状态 可能会出现问题 请检查");
            }
        }

        private void SetEnabled(bool set)
        {
            if (!m_ChangeEnabled) return;

            if (m_Image == null) return;

            m_Image.enabled = set;
        }

        protected override void OnValueChanged()
        {
            if (!UIOperationHelper.IsPlaying())
            {
                return;
            }

            if (m_Image == null || gameObject == null) return;

            var dataValue = GetFirstValue<string>();

            if (string.IsNullOrEmpty(dataValue))
            {
                SetEnabled(false);
                return;
            }

            ChangeSprite(dataValue).NoContext();
        }

        private async ETTask ChangeSprite(string resName)
        {
            using var coroutineLock = await EventSystem.Instance?.YIUIInvokeAsync<YIUIInvokeCoroutineLock, ETTask<Entity>>(new YIUIInvokeCoroutineLock { Lock = this.GetHashCode() });

            if (m_LastSpriteName == resName)
            {
                if (m_Image != null && m_Image.sprite != null)
                {
                    SetEnabled(true);
                }
                else
                {
                    SetEnabled(false);
                }

                return;
            }

            var sprite = await EventSystem.Instance?.YIUIInvokeAsync<YIUIInvokeLoadSprite, ETTask<Sprite>>(new YIUIInvokeLoadSprite { ResName = resName });

            if (sprite == null)
            {
                m_LastSpriteName = "";
                SetEnabled(false);
                return;
            }

            ReleaseLastSprite();

            if (this == null || gameObject == null)
            {
                EventSystem.Instance?.YIUIInvokeSync(new YIUIInvokeRelease { obj = sprite });
                return;
            }

            if (m_Image == null)
            {
                EventSystem.Instance?.YIUIInvokeSync(new YIUIInvokeRelease { obj = sprite });
                Logger.LogError($"{resName} 加载过程中 对象被摧毁了 || m_Image == null");
                return;
            }

            m_LastSprite   = sprite;
            m_Image.sprite = sprite;
            if (m_SetNativeSize)
                m_Image.SetNativeSize();

            SetEnabled(true);
            m_LastSpriteName = resName;
        }

        protected override void UnBindData()
        {
            base.UnBindData();
            if (!UIOperationHelper.IsPlaying())
            {
                return;
            }

            ReleaseLastSprite();
        }

        private Sprite m_LastSprite;

        private void ReleaseLastSprite()
        {
            if (m_LastSprite != null)
            {
                EventSystem.Instance?.YIUIInvokeSync(new YIUIInvokeRelease { obj = m_LastSprite });
                m_LastSprite = null;
            }
        }
    }
}