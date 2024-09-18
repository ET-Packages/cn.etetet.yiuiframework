using System;

namespace ET
{
    public static class Entity_Extend
    {
        //自定义扩展 获取不到就报错 不管你log是true 还是false
        //不想报错就别调用这个方法
        public static K GetComponent<K>(this Entity self, bool log) where K : Entity
        {
            var component = self.GetComponent<K>();
            if (component == null)
            {
                Log.Error($"{self.GetType().Name} 目标没有这个组件 {typeof(K).Name}");
            }

            return component;
        }

        //虽然使用 AddChild也可以实现
        //但是为了保持一致性 跟第一直觉 还是写一个方法
        //且相同时不报错
        public static void SetParent(this Entity self, Entity parent)
        {
            if (parent == null) return;
            if (self.Parent == parent) return;
            parent.AddChild(self);
        }
    }
}