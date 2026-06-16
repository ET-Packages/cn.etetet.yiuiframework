//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using UnityObject = UnityEngine.Object;

namespace ET.Client
{
    /// <summary>
    /// 注入加载方法
    /// </summary>
    public static partial class YIUILoadDI
    {
        //异步加载方法
        [StaticField]
        public static Func<string, string, Type, ETTask<(UnityObject, int)>> LoadAssetAsyncFunc { get; set; }

        //验证是否有效
        [StaticField]
        public static Func<string, string, bool> VerifyAssetValidityFunc { get; set; }

        //释放方法
        [StaticField]
        public static Action<int> ReleaseAction { get; set; }

        //释放所有方法
        [StaticField]
        public static Action ReleaseAllAction { get; set; }
    }
}