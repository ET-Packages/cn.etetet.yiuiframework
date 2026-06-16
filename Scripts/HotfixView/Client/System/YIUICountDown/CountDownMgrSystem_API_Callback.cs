using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class CountDownMgrSystem
    {
        public static bool Add(this CountDownMgr self, ref long guid, YIUIInvokeEntity_CountDownAdd args)
        {
            if (args.TimerCallback == null)
            {
                Log.Error($"<color=red> 必须有callback </color>");
                guid = 0;
                return false;
            }

            if (self.m_CallbackGuidDic.ContainsKey(args.TimerCallback))
            {
                Debug.LogError($"当前回调已存在 不能重复添加 如果想重复添加请使用另外一个API 使用GUID");
                guid = 0;
                return false;
            }

            CountDownMgr.CountDownData countDownData = RefPool.Get<CountDownMgr.CountDownData>();
            countDownData.Reset(args.TotalTime, args.Interval, args.TimerCallback, args.Forever);
            bool result = self.Add(ref guid, countDownData, args.StartCallback);
            if (!result)
            {
                RefPool.Put(countDownData);
                return false;
            }

            self.m_CallbackGuidDic[args.TimerCallback] = guid;
            return true;
        }

        public static bool Add(this CountDownMgr self, YIUIInvokeEntity_CountDownAdd args)
        {
            long guid = 0;
            return self.Add(ref guid, args);
        }

        public static bool Remove(this CountDownMgr self, YIUIInvokeEntity_CountDownRemove args)
        {
            if (self == null || args.TimerCallback == null)
            {
                return false;
            }

            if (!self.m_CallbackGuidDic.TryGetValue(args.TimerCallback, out long guid))
            {
                return false;
            }

            bool result = self.Remove(guid);
            if (result)
            {
                self.m_CallbackGuidDic.Remove(args.TimerCallback);
            }

            return result;
        }
    }
}