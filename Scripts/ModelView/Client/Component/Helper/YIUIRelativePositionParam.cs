using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 打开窗口相对方向
    /// </summary>
    public enum EYIUIRelativeOpenDirection
    {
        Right = 0, //右边
        Left = 1, //左边
        Up = 2, //上面
        Down = 3, //下面
    }

    /// <summary>
    /// 相对位置 rect 偏移
    /// </summary>
    public struct YIUIRelativePositionRectOffset
    {
        public float Left { get; set; }

        public float Right { get; set; }

        public float Top { get; set; }

        public float Bottom { get; set; }
    }

    /// <summary>
    /// 窗口相对位置打开参数
    /// </summary>
    public struct YIUIRelativePositionParam
    {
        /// <summary>
        /// 起点位置
        /// </summary>
        public RectTransform BlockRectTransform { get; set; }

        /// <summary>
        /// 目标窗口
        /// </summary>
        public RectTransform WindowRectTransform { get; set; }

        /// <summary>
        /// 打开方向  可不传，默认右边
        /// </summary>
        public EYIUIRelativeOpenDirection PreferredDirection { get; set; }

        /// <summary>
        /// 偏移 可不传，默认不偏移
        /// </summary>
        public YIUIRelativePositionRectOffset BlockRectOffset { get; set; }
    }
}