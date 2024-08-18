//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIFactory
    {
        internal static void Destroy(GameObject obj)
        {
            EventSystem.Instance?.YIUIInvokeSync(new YIUIInvokeReleaseInstantiate
                                                 {
                                                     obj = obj
                                                 });
        }
    }
}
