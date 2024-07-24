using ET;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 简单的窗口动画
    /// 只是一个案例 不需要可以删除
    /// 这里用到dotween动画 ettask 简单的扩展
    /// 目前已知BUG 同时有多个人调用动画 其他异步还没有完成时 被其他的删除了就会错误
    /// 导致其他异步全部中断
    /// </summary>
    public static class WindowFadeAnimStatic
    {
        [StaticField]
        public static Vector3 m_AnimScale = new Vector3(0.8f, 0.8f, 0.8f);
    }
}
