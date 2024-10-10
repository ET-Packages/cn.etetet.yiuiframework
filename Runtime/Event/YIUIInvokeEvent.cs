using System;
using ET;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace YIUIFramework
{
    public struct YIUIInvokeGetAssetInfo
    {
        public string Location;
        public Type   AssetType;
    }

    public struct YIUIInvokeGetAssetInfoByGUID
    {
        public string AssetGUID;
        public Type   AssetType;
    }

    // 加载任意实例化 to Vo
    public struct YIUIInvokeLoadInstantiateByVo
    {
        public YIUIBindVo BindVo;
        public Entity     ParentEntity;
        public Transform  ParentTransform;
    }

    //实例化一个GameObject
    public struct YIUIInvokeInstantiateGameObject
    {
        public string PkgName;
        public string ResName;
    }

    //回收YIUI实例化资源
    public struct YIUIInvokeReleaseInstantiate
    {
        public GameObject obj;
    }

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

    public struct YIUIInvokeLoadTexture2D
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

    //协程锁
    public struct YIUIInvokeCoroutineLock
    {
        public long LockType;
        public long Lock;
    }
}