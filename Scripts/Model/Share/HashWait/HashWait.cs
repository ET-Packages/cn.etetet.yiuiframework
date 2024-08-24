using System;
using System.Collections.Generic;

namespace ET
{
    [ComponentOf]
    public class HashWait: Entity, IAwake, IDestroy
    {
        public Dictionary<long, ETTask<HashWaitError>> m_HashWaitTasks = new();
    }
}