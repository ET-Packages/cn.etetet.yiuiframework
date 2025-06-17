using ET.Client;

namespace ET
{
    public static class YIUIEventHelper
    {
        public static YIUIEventComponent YIUIEvent(this Entity entity)
        {
            return entity.YIUIMgr().GetComponent<YIUIEventComponent>();
        }

        public static YIUIEventComponent YIUIEvent(this Scene scene)
        {
            return scene.YIUIMgr().GetComponent<YIUIEventComponent>();
        }
    }
}