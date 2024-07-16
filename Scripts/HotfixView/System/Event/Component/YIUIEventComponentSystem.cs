//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

namespace ET.Client
{
    [FriendOf(typeof(YIUIEventComponent))]
    [EntitySystemOf(typeof(YIUIEventComponent))]
    public static partial class YIUIEventComponentSystem
    {
        [EntitySystem]
        private static void Awake(this YIUIEventComponent self)
        {
            YIUIEventComponent.Inst = self;
            self.Init();
        }

        [EntitySystem]
        private static void Destroy(this YIUIEventComponent self)
        {
            YIUIEventComponent.Inst = null;
        }

        private static void Init(this YIUIEventComponent self)
        {
            self._AllEventInfo = new();

            var types = CodeTypes.Instance.GetTypes(typeof(YIUIEventAttribut));
            foreach (var type in types)
            {
                var eventAttribut = type.GetCustomAttribute<YIUIEventAttribut>(false);
                var obj           = (IYIUICommonEvent)Activator.CreateInstance(type);
                var eventType     = eventAttribut.EventType;
                var componentName = eventAttribut.ComponentType.Name;
                var info          = new YIUIEventInfo(eventType, componentName, obj);

                if (!self._AllEventInfo.ContainsKey(eventAttribut.EventType))
                {
                    self._AllEventInfo.Add(eventAttribut.EventType, new Dictionary<string, List<YIUIEventInfo>>());
                }

                if (!self._AllEventInfo[eventAttribut.EventType].ContainsKey(componentName))
                {
                    self._AllEventInfo[eventAttribut.EventType].Add(componentName, new List<YIUIEventInfo>());
                }

                var infoList = self._AllEventInfo[eventAttribut.EventType][componentName];

                infoList.Add(info);
            }
        }

        public static async ETTask Run<T>(this YIUIEventComponent self, string componentName, T data)
        {
            var eventType = typeof(T);
            if (!self._AllEventInfo.TryGetValue(eventType, out var componentDic))
            {
                return;
            }

            if (!componentDic.TryGetValue(componentName, out var eventInfos))
            {
                return;
            }

            foreach (var info in eventInfos)
            {
                await info.UIEvent.Run(self.Root(), data);
            }
        }
    }
}