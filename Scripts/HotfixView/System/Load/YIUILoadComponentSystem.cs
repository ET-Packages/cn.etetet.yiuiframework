//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using YIUIFramework;
using YooAsset;
using System;

namespace ET.Client
{
    /// <summary>
    /// UI面板组件
    /// </summary>
    [FriendOf(typeof(YIUILoadComponent))]
    [EntitySystemOf(typeof(YIUILoadComponent))]
    public static partial class YIUILoadComponentSystem
    {
        [EntitySystem]
        private static void Awake(this YIUILoadComponent self)
        {
            self.LoadAwake();
            YIUILoadComponent.m_InstRef = self;
        }

        [EntitySystem]
        private static void Awake(this YIUILoadComponent self, string packageName)
        {
            self.LoadAwake(packageName);
        }

        [EntitySystem]
        private static void Destroy(this YIUILoadComponent self)
        {
            YIUILoadComponent.m_InstRef = null;
            self.ReleaseAllAction();
        }

        private static void LoadAwake(this YIUILoadComponent self, string packageName = "DefaultPackage")
        {
            self.m_Package = YooAssets.GetPackage(packageName);

            //关联UI工具中自动生成绑定代码 Tools >> YIUI自动化工具 >> 发布 >> UI自动生成绑定替代反射代码
            //在ET中这个自动生成的代码在ModelView中所以在此框架中无法初始化赋值
            //将由HotfixView AddComponent<YIUIMgrComponent> 之前调用一次
            //会在 InitAllBind 方法中被调用
            //YIUIBindHelper.InternalGameGetUIBindVoFunc = YIUICodeGenerated.YIUIBindProvider.Get;

            //YIUI会用到的各种加载 需要自行实现 Demo中使用的是YooAsset 根据自己项目的资源管理器实现下面的方法
            YIUILoadDI.LoadAssetFunc           = self.LoadAssetFunc;           //同步加载
            YIUILoadDI.LoadAssetAsyncFunc      = self.LoadAssetAsyncFunc;      //异步加载
            YIUILoadDI.ReleaseAction           = self.ReleaseAction;           //释放
            YIUILoadDI.VerifyAssetValidityFunc = self.VerifyAssetValidityFunc; //检查
            YIUILoadDI.ReleaseAllAction        = self.ReleaseAllAction;        //释放所有
        }

        /// <summary>
        /// 释放方法
        /// </summary>
        /// <param name="hashCode">加载时所给到的唯一ID</param>
        private static void ReleaseAction(this YIUILoadComponent self, int hashCode)
        {
            if (!self.m_AllHandle.TryGetValue(hashCode, out var value))
            {
                return;
            }

            value.Release();
            self.m_AllHandle.Remove(hashCode);
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="arg1">包名</param>
        /// <param name="arg2">资源名</param>
        /// <param name="arg3">类型</param>
        /// <returns>返回值(obj资源对象,唯一ID)</returns>
        private static async ETTask<(UnityEngine.Object, int)> LoadAssetAsyncFunc(this YIUILoadComponent self, string arg1, string arg2, Type arg3)
        {
            var handle = self.m_Package.LoadAssetAsync(arg2, arg3);
            await handle.Task;
            return self.LoadAssetHandle(handle);
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="arg1">包名</param>
        /// <param name="arg2">资源名</param>
        /// <param name="arg3">类型</param>
        /// <returns>返回值(obj资源对象,唯一ID)</returns>
        private static (UnityEngine.Object, int) LoadAssetFunc(this YIUILoadComponent self, string arg1, string arg2, Type arg3)
        {
            var handle = self.m_Package.LoadAssetSync(arg2, arg3);
            return self.LoadAssetHandle(handle);
        }

        //Demo中对YooAsset加载后的一个简单返回封装
        //只有成功加载才返回 否则直接释放
        private static (UnityEngine.Object, int) LoadAssetHandle(this YIUILoadComponent self, AssetHandle handle)
        {
            if (handle.AssetObject != null)
            {
                var hashCode = handle.GetHashCode();
                self.m_AllHandle.Add(hashCode, handle);
                return (handle.AssetObject, hashCode);
            }
            else
            {
                handle.Release();
                return (null, 0);
            }
        }

        //释放所有
        private static void ReleaseAllAction(this YIUILoadComponent self)
        {
            foreach (var handle in self.m_AllHandle.Values)
            {
                handle.Release();
            }

            self.m_AllHandle.Clear();
        }

        //检查合法
        private static bool VerifyAssetValidityFunc(this YIUILoadComponent self, string arg1, string arg2)
        {
            return self.m_Package.CheckLocationValid(arg2);
        }
    }
}
