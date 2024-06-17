using System;

namespace ET
{
    public partial class CodeTypes
    {
        public Type GetType(string typeName, bool error)
        {
            if (this.allTypes.TryGetValue(typeName, out var type))
            {
                return type;
            }

            if (error)
                Log.Error($"没有找到这个类型: + {typeName}");
            return null;
        }
    }
}
