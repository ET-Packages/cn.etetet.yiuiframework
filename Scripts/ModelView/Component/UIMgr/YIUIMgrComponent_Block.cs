﻿using System.Collections.Generic;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    //UI最高层级之上的 屏蔽所有操作的模块
    //屏蔽层显示时所有UI操作会被屏蔽 默认隐藏
    //可通过API 快捷屏蔽操作
    //适用于 统一动画播放时 某些操作需要等待时 都可以调节
    public partial class YIUIMgrComponent
    {
        //当前层级屏蔽操作状态 true = 显示 = 无法操作 不要与可操作搞混
        public bool LayerBlockActiveSelf => m_LayerBlock.activeSelf;

        //当前UI是否可以操作 true = 可以操作
        public bool CanLayerBlockOption => !LayerBlockActiveSelf;

        //如果当前被屏蔽了操作 下一次可以操作的时间是什么时候基于 Time.unscaledTime;
        //=0 不表示可以操作  也有可能被永久屏蔽了
        //可单独判断是否永久屏蔽
        //也可以使用上面的方法 CanLayerBlockOption  也可以得到是否被屏蔽
        public float LastRecoverOptionTime => m_LastRecoverOptionTime;

        public GameObject m_LayerBlock; //内部屏蔽对象 显示时之下的所有UI将不可操作

        public long m_LastCountDownGuid; //倒计时的唯一ID

        public float m_LastRecoverOptionTime; //下一次恢复操作时间

        //当前是否正在被永久屏蔽
        public bool IsForeverBlock => m_AllForeverBlockCode.Count >= 1;

        //永久屏蔽引用计数 一定要成对使用且保证
        //否则将会出现永久屏蔽的情况只能通过RecoverLayerOptionCountDown 强制恢复
        public HashSet<long> m_AllForeverBlockCode = new();
    }
}
