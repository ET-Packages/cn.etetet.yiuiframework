#if UNITY_EDITOR
using UnityEngine;

namespace YIUIFramework
{
    public sealed partial class UIBindComponentTable
    {
        public int EditorCollectComponent(Component component, YIUIBindEditorRemoveResult result)
        {
            return ProcessComponent(component, result, false);
        }

        public int EditorRemoveComponent(Component component, YIUIBindEditorRemoveResult result)
        {
            return ProcessComponent(component, result, true);
        }

        public int EditorCollectComponentsInHierarchy(GameObject target, YIUIBindEditorRemoveResult result)
        {
            return ProcessComponentsInHierarchy(target, result, false);
        }

        public int EditorRemoveComponentsInHierarchy(GameObject target, YIUIBindEditorRemoveResult result)
        {
            return ProcessComponentsInHierarchy(target, result, true);
        }

        private int ProcessComponent(Component component, YIUIBindEditorRemoveResult result, bool remove)
        {
            if (component == null || m_AllBindPair == null)
            {
                return 0;
            }

            var count = 0;
            for (var i = m_AllBindPair.Count - 1; i >= 0; i--)
            {
                var pair = m_AllBindPair[i];
                if (pair == null || pair.Component != component)
                {
                    continue;
                }

                AddComponentTableTarget(result, pair);
                count++;
                if (remove)
                {
                    m_AllBindPair.RemoveAt(i);
                }
            }

            if (remove && count > 0)
            {
                RebuildEditorComponentDictionary();
                if (result != null)
                {
                    result.RemovedComponentTableBindCount += count;
                }
            }

            if (result != null)
            {
                result.ComponentTableBindCount += count;
            }

            return count;
        }

        private int ProcessComponentsInHierarchy(GameObject target, YIUIBindEditorRemoveResult result, bool remove)
        {
            if (target == null || m_AllBindPair == null)
            {
                return 0;
            }

            var count = 0;
            for (var i = m_AllBindPair.Count - 1; i >= 0; i--)
            {
                var pair = m_AllBindPair[i];
                if (pair == null || pair.Component == null || !IsComponentInHierarchy(pair.Component, target))
                {
                    continue;
                }

                AddComponentTableTarget(result, pair);
                count++;
                if (remove)
                {
                    m_AllBindPair.RemoveAt(i);
                }
            }

            if (remove && count > 0)
            {
                RebuildEditorComponentDictionary();
                if (result != null)
                {
                    result.RemovedComponentTableBindCount += count;
                }
            }

            if (result != null)
            {
                result.ComponentTableBindCount += count;
            }

            return count;
        }

        private static bool IsComponentInHierarchy(Component component, GameObject target)
        {
            if (component == null || target == null)
            {
                return false;
            }

            var componentTransform = component.transform;
            var targetTransform = target.transform;
            return componentTransform == targetTransform || componentTransform.IsChildOf(targetTransform);
        }

        private static void AddComponentTableTarget(YIUIBindEditorRemoveResult result, UIBindPairData pair)
        {
            if (result == null || pair == null)
            {
                return;
            }

            var componentPath = pair.Component == null
                ? "<null>"
                : YIUIBindEditorRemoveUtility.GetComponentPath(pair.Component);
            result.ComponentTableTargets.Add($"{pair.Name}:{componentPath}");
        }

        private void RebuildEditorComponentDictionary()
        {
            m_AllBindDic.Clear();
            if (m_AllBindPair == null)
            {
                return;
            }

            for (var i = 0; i < m_AllBindPair.Count; i++)
            {
                var pair = m_AllBindPair[i];
                if (pair == null || pair.Component == null || string.IsNullOrEmpty(pair.Name))
                {
                    continue;
                }

                if (!m_AllBindDic.ContainsKey(pair.Name) && !m_AllBindDic.ContainsValue(pair.Component))
                {
                    m_AllBindDic.Add(pair.Name, pair.Component);
                }
            }

            m_Initialized = false;
        }
    }
}
#endif
