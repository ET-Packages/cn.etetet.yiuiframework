﻿//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

namespace ET.Client
{
    /// <summary>
    /// 全局唯一的UI管理器
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public partial class YIUIMgrComponent : Entity, IAwake, IDestroy
    {
        public EntityRef<YIUIRootComponent> m_RootRef;

        public YIUIRootComponent Root => m_RootRef;
    }
}
