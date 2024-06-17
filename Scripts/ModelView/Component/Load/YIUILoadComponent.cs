using System.Collections.Generic;
using YooAsset;

namespace ET.Client
{
    /// <summary>
    /// UI通用加载组件
    /// 全局唯一 挂在YIUI管理器下
    /// </summary>
    [ComponentOf(typeof(YIUIMgrComponent))]
    public partial class YIUILoadComponent : Entity, IAwake, IAwake<string>, IDestroy
    {
        [StaticField]
        public static EntityRef<YIUILoadComponent> m_InstRef;

        [StaticField]
        public static YIUILoadComponent Inst => m_InstRef;

        public Dictionary<int, AssetHandle> m_AllHandle = new();
        public ResourcePackage              m_Package;
    }
}
