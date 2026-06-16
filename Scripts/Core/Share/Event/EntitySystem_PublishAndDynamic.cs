using System;
using System.Collections.Generic;

namespace ET
{
    //快捷静态扩展 可以直接任意entity.PublishAndDynamicEvent 就可以 先发送事件 再发送动态事件
    //直接发送到对应的纤程中
    public static class PublishAndEntitySystemHelper
    {
        //向任意场景发送
        public static async ETTask PublishAndDynamicEvent<P1>(this Entity self, P1 message) where P1 : struct
        {
            EntityRef<Entity> selfRef = self;
            await EventSystem.Instance.PublishAsync(self.Scene(), message);
            self = selfRef;
            await self.DynamicEvent(0, message);
        }

        //向与传入的实体相同的场景发送
        public static async ETTask PublishAndDynamicEvent<P1>(this Entity self, Entity entity, P1 message) where P1 : struct
        {
            EntityRef<Entity> selfRef = self;
            var sceneType = entity.IScene.SceneType;
            await EventSystem.Instance.PublishAsync(entity.Scene(), message);
            self = selfRef;
            await self.DynamicEvent(sceneType, message);
        }

        //向目标场景发送
        public static async ETTask PublishAndDynamicEvent<P1>(this Entity self, Scene scene, P1 message) where P1 : struct
        {
            EntityRef<Entity> selfRef = self;
            var sceneType = scene.SceneType;
            await EventSystem.Instance.PublishAsync(scene, message);
            self = selfRef;
            await self.DynamicEvent(sceneType, message);
        }
    }
}