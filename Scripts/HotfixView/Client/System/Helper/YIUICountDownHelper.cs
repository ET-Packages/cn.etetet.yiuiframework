﻿using ET.Client;

namespace ET
{
    public static class YIUICountDownHelper
    {
        public static CountDownMgr YIUICountDown(this Entity entity)
        {
            return entity.YIUIMgr()?.GetComponent<CountDownMgr>(true);
        }

        public static CountDownMgr YIUICountDown(this Scene scene)
        {
            return scene.YIUIMgr()?.GetComponent<CountDownMgr>(true);
        }
    }
}