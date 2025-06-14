﻿using System;
using ET;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 组件的单例类基类
    /// 注意：这个单例实现适用于已经存在于场景的物件，但你需要使用Inst来访问。
    /// 它在场景上找不到时不会自动创建，如果有自动创建的需求，请使用MonoSingleton。
    /// 它也会不自动执行DontDestroyOnLoad, 这需要你自己在组件里写。
    /// </summary>
    public abstract class YIUIMonoSceneSingleton<T> : YIUIDisposerMonoSingleton where T : YIUIMonoSceneSingleton<T>
    {
        private EntityRef<Entity> EntityRef;

        protected Entity Entity => EntityRef;

        private static T g_Inst;

        /// <summary>
        /// 是否存在
        /// </summary>
        public static bool Exist => g_Inst != null;

        /// <summary>
        /// 得到单例
        /// </summary>
        public static T Inst
        {
            get
            {
                if (g_Inst == null)
                {
                    if (!UIOperationHelper.IsPlaying())
                    {
                        Debug.LogError($"非运行时 禁止调用");
                        return null;
                    }

                    Debug.LogError($"g_inst == null 这个是MonoSceneSingleton 需要自己创建对象的单例 不会自动创建");
                    return null;
                }

                g_Inst.OnUseSingleton();
                return g_Inst;
            }
        }

        protected virtual void Awake()
        {
            if (YIUISingletonHelper.Disposing)
            {
                Debug.LogError($" {typeof(T).Name} 单例管理器已释放或未初始化 禁止使用");
                return;
            }

            if (g_Inst != null)
            {
                Debug.LogError(typeof(T).Name + "是单例组件，不能在场景中存在多个");
                gameObject.SafeDestroySelf();
                return;
            }

            g_Inst = (T)this;
            g_Inst.EntityRef = YIUISingletonHelper.YIUIMgr;
            gameObject.name = g_Inst.GetCreateName();
            if (g_Inst.GetDontDestroyOnLoad())
            {
                DontDestroyOnLoad(g_Inst);
            }

            if (g_Inst.GetHideAndDontSave())
            {
                gameObject.hideFlags = HideFlags.HideAndDontSave;
            }

            YIUISingletonHelper.Add(g_Inst);
            g_Inst.OnInitSingleton();
        }

        private string GetCreateName()
        {
            return $"[Singleton]{typeof(T).Name}";
        }

        //子类如果使用这个生命周期记得调用base
        //推荐使用 重写 OnDispose
        protected override void OnDestroy()
        {
            base.OnDestroy();
            YIUISingletonHelper.Remove(g_Inst);
            g_Inst = null;
        }

        //释放方法2: 静态释放
        public static bool DisposeInst()
        {
            if (g_Inst == null)
            {
                return true;
            }

            return g_Inst.Dispose();
        }
    }
}