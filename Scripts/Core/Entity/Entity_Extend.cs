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
    }
}