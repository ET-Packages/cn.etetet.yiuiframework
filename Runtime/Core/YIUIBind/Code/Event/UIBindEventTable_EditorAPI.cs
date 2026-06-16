#if UNITY_EDITOR
namespace YIUIFramework
{
    public sealed partial class UIBindEventTable
    {
        public bool EditorTryRemoveEvent(string eventName, bool forceRemoveBinds, out YIUIBindEditorRemoveResult result, out string error)
        {
            result = new YIUIBindEditorRemoveResult();
            error = string.Empty;
            result.DeclarationKind = "Event";
            result.DeclarationName = EditorNormalizeEventName(eventName);
            result.NeedCheckGeneratedEventMethods = true;

            if (string.IsNullOrEmpty(eventName))
            {
                error = "Event 名称不能为空";
                return false;
            }

            m_Initialized = false;
            OnValidate();

            if (m_EventDic == null)
            {
                error = "EventTable 未初始化";
                return false;
            }

            var normalizedName = EditorNormalizeEventName(eventName);
            if (!m_EventDic.TryGetValue(normalizedName, out var uiEvent))
            {
                if (!m_EventDic.TryGetValue(eventName, out uiEvent))
                {
                    error = $"Event 不存在: {normalizedName}";
                    return false;
                }

                normalizedName = eventName;
            }

            result.BindCount = uiEvent.GetBindCount();
            uiEvent.EditorCollectBindTargets(result);

            if (result.BindCount > 0 && !forceRemoveBinds)
            {
                result.RequiredForceRemoveBinds = true;
                error = $"Event 已绑定 {result.BindCount} 个目标，删除需要 forceRemoveBinds=true: {normalizedName}";
                return false;
            }

            result.RemovedBindCount = result.BindCount;
            uiEvent.OnRemoveVariableCallBack();
            m_EventDic.Remove(normalizedName);
            result.Removed = true;

            m_Initialized = false;
            OnValidate();
            return true;
        }
    }
}
#endif
