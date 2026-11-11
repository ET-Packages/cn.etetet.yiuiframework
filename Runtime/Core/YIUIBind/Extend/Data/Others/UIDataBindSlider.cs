using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace YIUIFramework
{
    [LabelText("滑动动画")]
    internal enum ETweenType
    {
        [LabelText("只增加")]
        IncreaseOnly,

        [LabelText("只减少")]
        DecreaseOnly,

        [LabelText("可双向增减")]
        DoubleWay,
    }

    [Serializable]
    [LabelText("滑块")]
    [RequireComponent(typeof(Slider))]
    [AddComponentMenu("YIUIBind/Data/滑块 【Slider】 UIDataBindSlider")]
    public partial class UIDataBindSlider : UIDataBindSelectBase
    {
        [ReadOnly]
        [Required("必须有此组件")]
        [SerializeField]
        [LabelText("滑块")]
        private Slider m_Slider;

        [InfoBox(">0时 则设置值时会以动画的形式慢慢变化 需要运行时")]
        [SerializeField]
        [LabelText("滑动动画速度")]
        #if UNITY_EDITOR
        [OnValueChanged("OnTweenSpeedValueChanged")]
        #endif
        private float m_TweenSpeed = 0.0f;

        [SerializeField]
        [ShowIf("ShowIfTweenType")]
        private ETweenType m_TweenType = ETweenType.DoubleWay;

        private bool ShowIfTweenType()
        {
            return m_TweenSpeed > 0.0f;
        }

        [SerializeField]
        [ShowIf("ShowIfOneWayTween")]
        [LabelText("仅所选方向播放动画")]
        [Tooltip("勾选：只有所选方向（只增加=变大，只减少=变小）播放动画，反方向直接赋值；取消勾选：恢复原有的回绕动画")]
        private bool m_OneWayTween = true;

        private bool ShowIfOneWayTween()
        {
            return m_TweenType != ETweenType.DoubleWay;
        }

        private float m_TargetValue;
        private bool  m_PlayingTween = false;

        //启用后还没发生过真实数值变化 期间的赋值视为重新绑定 直接到位不播补间
        private bool m_SnapOnFirstUpdate = false;

        protected override int Mask()
        {
            return 1 << (int)EUIBindDataType.Float;
        }

        protected override int SelectMax()
        {
            return 1;
        }

        //重新显示时先对齐当前数据：隐藏期间的残留显示值、对象池复用带过来的旧数值都不该参与补间
        private void OnEnable()
        {
            if (!Application.isPlaying) return;

            SnapToDataValue();
            m_SnapOnFirstUpdate = true;
        }

        protected override void OnRefreshData()
        {
            base.OnRefreshData();
            SnapToDataValue();
        }

        private void SnapToDataValue()
        {
            m_Slider ??= GetComponent<Slider>();
            if (m_Slider == null) return;

            var dataValue = GetFirstValue<float>();
            m_TargetValue  = dataValue;
            m_Slider.value = dataValue;
            m_PlayingTween = false;
        }

        protected override void OnValueChanged()
        {
            if (m_Slider == null) return;

            //启用后的第一次赋值属于重新绑定 直接到位 不算数值变化
            if (m_SnapOnFirstUpdate)
            {
                SnapToDataValue();
                return;
            }

            var dataValue = GetFirstValue<float>();
            if (m_TweenSpeed > 0.0f && Application.isPlaying)
            {
                //勾选后只有所选方向播放动画，反方向直接赋值
                if (m_OneWayTween && IsReverseDirection(dataValue))
                {
                    m_TargetValue  = dataValue;
                    m_Slider.value = dataValue;
                    m_PlayingTween = false;
                    return;
                }

                m_TargetValue  = dataValue;
                m_PlayingTween = true;
            }
            else
            {
                m_TargetValue  = dataValue;
                m_Slider.value = dataValue;
                m_PlayingTween = false;
            }
        }

        /// 只增加：值变小是反方向；只减少：值变大是反方向；可双向：没有反方向
        private bool IsReverseDirection(float dataValue)
        {
            switch (m_TweenType)
            {
                case ETweenType.IncreaseOnly:
                    return dataValue < m_TargetValue;
                case ETweenType.DecreaseOnly:
                    return dataValue > m_TargetValue;
                default:
                    return false;
            }
        }

        #if UNITY_EDITOR
        private void OnTweenSpeedValueChanged()
        {
            //如果突然吧动画速度改为0
            //且当前正在动画中 那么会错误 虽然只有editor才会出现
            //这里还是处理
            if (m_TweenSpeed <= 0)
            {
                if (!Mathf.Approximately(m_Slider.value, m_TargetValue))
                {
                    m_Slider.value = m_TargetValue;
                }
            }
        }
        #endif

        private void Update()
        {
            //启用当帧再收口一次：外部通常先显示再赋值 两者先后顺序不固定
            //这里清掉标记 之后的真实数值变化才走补间
            if (m_SnapOnFirstUpdate)
            {
                m_SnapOnFirstUpdate = false;
                SnapToDataValue();
            }

            if (m_PlayingTween &&
                m_TweenSpeed > 0.0f &&
                !Mathf.Approximately(m_Slider.value, m_TargetValue))
            {
                // 防止浮点精度误差导致tween无法结束
                if (Mathf.Abs(m_Slider.value - m_TargetValue) < 0.01f)
                {
                    m_Slider.value = m_TargetValue;
                    m_PlayingTween = false;
                    return;
                }

                switch (m_TweenType)
                {
                    case ETweenType.IncreaseOnly:
                        UpdateIncreaseOnly();
                        break;
                    case ETweenType.DecreaseOnly:
                        UpdateDecreaseOnly();
                        break;
                    case ETweenType.DoubleWay:
                        UpdateDoubleWay();
                        break;
                }
            }
        }

        private void UpdateIncreaseOnly()
        {
            if (m_TargetValue > m_Slider.value)
            {
                UpdateDoubleWay();
                return;
            }

            var movement = m_TweenSpeed * Time.deltaTime;
            var newValue = m_Slider.value + movement;
            if (newValue >= m_Slider.maxValue)
            {
                m_Slider.value = m_Slider.minValue;
            }
            else
            {
                m_Slider.value = newValue;
            }
        }

        private void UpdateDecreaseOnly()
        {
            if (m_TargetValue < m_Slider.value)
            {
                UpdateDoubleWay();
                return;
            }

            var movement = m_TweenSpeed * Time.deltaTime;
            var newValue = m_Slider.value - movement;
            if (newValue <= m_Slider.minValue)
            {
                m_Slider.value = m_Slider.maxValue;
            }
            else
            {
                m_Slider.value = newValue;
            }
        }

        private void UpdateDoubleWay()
        {
            var offset   = m_TargetValue - m_Slider.value;
            var movement = m_TweenSpeed * Time.deltaTime;
            if (movement >= Mathf.Abs(offset))
            {
                m_Slider.value = m_TargetValue;
                m_PlayingTween = false;
            }
            else
            {
                if (offset > 0.0f)
                {
                    m_Slider.value += movement;
                }
                else
                {
                    m_Slider.value -= movement;
                }
            }
        }
    }
}
