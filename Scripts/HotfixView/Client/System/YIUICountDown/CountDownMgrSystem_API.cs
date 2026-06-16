//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using UnityEngine;

namespace ET.Client
{
    //ref回调索引  这个回调就可以一对多
    public static partial class CountDownMgrSystem
    {
        /// <summary>
        /// ref传递 成功移除 则会吧这个值改为 0
        /// 省去自己修改
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool Remove(this CountDownMgr self, ref long guid)
        {
            var result = self.Remove(guid);
            if (result) guid = 0;
            return result;
        }

        public static bool Add(this CountDownMgr self, ref long guid, CountDownMgr.CountDownData countDownData, bool startCallback = false)
        {
            if (countDownData == null)
            {
                guid = 0;
                return false;
            }

            if (!self.TryAdd())
            {
                Debug.LogError("已经达到可同时倒计时上限 请确认");
                guid = 0;
                return false;
            }

            guid = self.AddCountDownTimer(countDownData);

            if (startCallback)
                self.Callback(countDownData);

            return true;
        }

        public static bool Add(this CountDownMgr self, CountDownMgr.CountDownData countDownData, bool startCallback = false)
        {
            long guid = 0;
            return self.Add(ref guid, countDownData, startCallback);
        }

        /// <summary>
        /// 重新设置这个倒计时的已过去时间
        /// </summary>
        public static bool SetElapseTime(this CountDownMgr self, long guid, double elapseTime)
        {
            var exist = self.m_AllCountDown.TryGetValue(guid, out CountDownMgr.CountDownData data);
            if (!exist) return false;

            data.ElapseTime = elapseTime;
            data.LastCallBackTime = self.GetTime;
            return true;
        }

        /// <summary>
        /// 获取一个倒计时剩余的倒计时时间
        /// </summary>
        public static double GetRemainTime(this CountDownMgr self, long guid)
        {
            var exist = self.m_AllCountDown.TryGetValue(guid, out CountDownMgr.CountDownData data);
            if (!exist) return 0;

            return data.TotalTime - data.ElapseTime;
        }

        /// <summary>
        /// 强制执行 一个倒计时到最后时间
        /// </summary>
        public static bool ForceToEndTime(this CountDownMgr self, long guid)
        {
            var exist = self.m_AllCountDown.TryGetValue(guid, out CountDownMgr.CountDownData data);
            if (!exist) return false;

            data.ElapseTime = data.TotalTime;
            self.Callback(data);

            return self.Remove(guid);
        }

        /// <summary>
        /// 让倒计时重新开始
        /// </summary>
        public static bool Restart(this CountDownMgr self, long guid)
        {
            var exist = self.m_AllCountDown.TryGetValue(guid, out CountDownMgr.CountDownData data);
            if (!exist) return false;

            return self.Restart(data);
        }
    }
}