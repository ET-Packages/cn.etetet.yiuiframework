using System;
using UnityObject = UnityEngine.Object;

namespace YIUIFramework
{
    // 回收YIUI资源
    public struct YIUIInvokeRelease
    {
        public UnityObject obj;
    }

    // 加载任意
    public struct YIUIInvokeLoad
    {
        public Type   LoadType;
        public string PkgName;
        public string ResName;
    }

    // 加载图片
    public struct YIUIInvokeLoadSprite
    {
        public string ResName;
    }

    //屏蔽所有YIUI操作
    public struct YIUIInvokeBanLayerOptionForever
    {
    }

    //恢复指定屏蔽的操作
    public struct YIUIInvokeRecoverLayerOptionForever
    {
        public long ForeverCode;
    }

    //添加倒计时
    public struct YIUIInvokeCountDownAdd
    {
        public CountDownTimerCallback TimerCallback;
        public double                 TotalTime;
        public double                 Interval;
        public bool                   StartCallback;
        public bool                   Forever;
    }

    //移除倒计时
    public struct YIUIInvokeCountDownRemove
    {
        public CountDownTimerCallback TimerCallback;
    }

    //等一帧 (1毫秒)
    public struct YIUIInvokeWaitFrameAsync
    {
    }

    //等指定毫秒
    public struct YIUIInvokeWaitAsync
    {
        public long Time;
    }
}
