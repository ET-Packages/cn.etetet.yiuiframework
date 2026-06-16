#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace YIUIFramework
{
    public sealed class YIUIBindEditorRemoveResult
    {
        public bool Removed;
        public string DeclarationKind;
        public string DeclarationName;
        public int BindCount;
        public int RemovedBindCount;
        public int NodeCount;
        public int ComponentCount;
        public int DataBindComponentCount;
        public int EventBindComponentCount;
        public int ComponentTableBindCount;
        public int RemovedComponentTableBindCount;
        public bool RequiredForceRemoveBinds;
        public bool NeedCheckGeneratedEventMethods;
        public readonly List<string> BindTargets = new List<string>();
        public readonly List<string> ComponentTargets = new List<string>();
        public readonly List<string> DataBindTargets = new List<string>();
        public readonly List<string> EventBindTargets = new List<string>();
        public readonly List<string> ComponentTableTargets = new List<string>();
    }

    public static class YIUIBindEditorRemoveAPI
    {
        public static YIUIBindEditorRemoveResult CollectHierarchy(GameObject target, UIBindCDETable cdeTable)
        {
            var result = new YIUIBindEditorRemoveResult();
            ScanHierarchy(target, cdeTable, result, false);
            return result;
        }

        public static YIUIBindEditorRemoveResult PrepareRemoveHierarchy(GameObject target, UIBindCDETable cdeTable)
        {
            var result = new YIUIBindEditorRemoveResult();
            ScanHierarchy(target, cdeTable, result, true);
            return result;
        }

        public static YIUIBindEditorRemoveResult CollectComponent(Component component, UIBindCDETable cdeTable)
        {
            var result = new YIUIBindEditorRemoveResult();
            ScanComponent(component, result, false);
            CollectComponentTable(component, cdeTable, result, false);
            return result;
        }

        public static YIUIBindEditorRemoveResult PrepareRemoveComponent(Component component, UIBindCDETable cdeTable)
        {
            var result = new YIUIBindEditorRemoveResult();
            ScanComponent(component, result, true);
            CollectComponentTable(component, cdeTable, result, true);
            return result;
        }

        private static void ScanHierarchy(GameObject target, UIBindCDETable cdeTable, YIUIBindEditorRemoveResult result, bool prepare)
        {
            if (target == null || result == null)
            {
                return;
            }

            var transforms = target.GetComponentsInChildren<Transform>(true);
            result.NodeCount = transforms == null ? 0 : transforms.Length;

            var components = target.GetComponentsInChildren<Component>(true);
            if (components != null)
            {
                result.ComponentCount = components.Length;
                for (var i = 0; i < components.Length; i++)
                {
                    ScanComponent(components[i], result, prepare);
                }
            }

            if (cdeTable != null && cdeTable.ComponentTable != null)
            {
                if (prepare)
                {
                    cdeTable.ComponentTable.EditorRemoveComponentsInHierarchy(target, result);
                }
                else
                {
                    cdeTable.ComponentTable.EditorCollectComponentsInHierarchy(target, result);
                }
            }
        }

        private static void ScanComponent(Component component, YIUIBindEditorRemoveResult result, bool prepare)
        {
            if (component == null || result == null)
            {
                return;
            }

            result.ComponentTargets.Add(YIUIBindEditorRemoveUtility.GetComponentPath(component));

            if (component is UIDataBind dataBind)
            {
                result.DataBindComponentCount++;
                result.DataBindTargets.Add(YIUIBindEditorRemoveUtility.GetComponentPath(component));
                if (prepare)
                {
                    dataBind.EditorPrepareRemove();
                }
            }

            if (component is UIEventBind eventBind)
            {
                result.EventBindComponentCount++;
                result.EventBindTargets.Add(YIUIBindEditorRemoveUtility.GetComponentPath(component));
                if (prepare)
                {
                    eventBind.EditorPrepareRemove();
                }
            }
        }

        private static void CollectComponentTable(Component component, UIBindCDETable cdeTable, YIUIBindEditorRemoveResult result, bool remove)
        {
            if (component == null || cdeTable == null || cdeTable.ComponentTable == null || result == null)
            {
                return;
            }

            if (remove)
            {
                cdeTable.ComponentTable.EditorRemoveComponent(component, result);
            }
            else
            {
                cdeTable.ComponentTable.EditorCollectComponent(component, result);
            }
        }
    }

    internal static class YIUIBindEditorRemoveUtility
    {
        public static string GetComponentPath(Component component)
        {
            if (component == null)
            {
                return "<null>";
            }

            var transform = component.transform;
            var names = new List<string>();
            while (transform != null)
            {
                names.Add(transform.name);
                transform = transform.parent;
            }

            names.Reverse();
            return $"{string.Join("/", names)}:{component.GetType().Name}";
        }
    }
}
#endif
