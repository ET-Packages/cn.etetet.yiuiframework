#if UNITY_EDITOR
using System;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace YIUIFramework
{
    //Editor
    public sealed partial class UIBindCDETable
    {
        #region 界面参数

        [LabelText("组件类型")]
        [OnValueChanged("OnValueChangedEUICodeType")]
        [ReadOnly]
        public EUICodeType UICodeType = EUICodeType.Common;

        [BoxGroup("配置", true, true)]
        [HideIf("UICodeType", EUICodeType.Common)]
        [LabelText("窗口选项")]
        [GUIColor(0, 1, 1)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EWindowOption WindowOption = EWindowOption.None;

        [ShowIf("UICodeType", EUICodeType.Panel)]
        [BoxGroup("配置", true, true)]
        [OnValueChanged("OnValueChangedEPanelLayer")]
        [GUIColor(0, 1, 1)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EPanelLayer PanelLayer = EPanelLayer.Panel;

        [ShowIf("UICodeType", EUICodeType.Panel)]
        [BoxGroup("配置", true, true)]
        [GUIColor(0, 1, 1)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EPanelOption PanelOption = EPanelOption.None;

        [ShowIf("UICodeType", EUICodeType.Panel)]
        [BoxGroup("配置", true, true)]
        [GUIColor(0, 1, 1)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EPanelStackOption PanelStackOption = EPanelStackOption.VisibleTween;

        [ShowIf("UICodeType", EUICodeType.View)]
        [BoxGroup("配置", true, true)]
        [GUIColor(0, 1, 1)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EViewWindowType ViewWindowType = EViewWindowType.View;

        [ShowIf("UICodeType", EUICodeType.View)]
        [BoxGroup("配置", true, true)]
        [GUIColor(0, 1, 1)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EViewStackOption ViewStackOption = EViewStackOption.VisibleTween;

        [ShowIf("ShowCachePanelTime", EUICodeType.Panel)]
        [BoxGroup("配置", true, true)]
        [GUIColor(0, 1, 1)]
        [LabelText("缓存时间")]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        [MinValue(0.01)]
        public float CachePanelTime = 10;

        private bool ShowCachePanelTime => PanelOption.HasFlag(EPanelOption.TimeCache) && !PanelOption.HasFlag(EPanelOption.ForeverCache);

        [LabelText("同层级时 优先级高的在前面")] //相同时后开的在前
        [ShowIf("UICodeType", EUICodeType.Panel)]
        [BoxGroup("配置", true, true)]
        [GUIColor(0, 1, 1)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public int Priority = 0;

        private void OnValueChangedEUICodeType()
        {
            if (name.EndsWith(YIUIConstHelper.Const.UIPanelName) || name.EndsWith(YIUIConstHelper.Const.UIPanelSourceName))
            {
                if (UICodeType != EUICodeType.Panel)
                {
                    Debug.LogWarning($"{name} 结尾{YIUIConstHelper.Const.UIPanelName} 必须设定为{YIUIConstHelper.Const.UIPanelName}类型");
                }

                UICodeType = EUICodeType.Panel;
            }
            else if (name.EndsWith(YIUIConstHelper.Const.UIViewName))
            {
                if (UICodeType != EUICodeType.View)
                {
                    Debug.LogWarning($"{name} 结尾{YIUIConstHelper.Const.UIViewName} 必须设定为{YIUIConstHelper.Const.UIViewName}类型");
                }

                UICodeType = EUICodeType.View;
            }
            else
            {
                if (UICodeType != EUICodeType.Common)
                {
                    Debug.LogWarning($"{name} 想设定为其他类型 请按照规则设定 请勿强行修改");
                }

                UICodeType = EUICodeType.Common;
            }
        }

        private void OnValueChangedEPanelLayer()
        {
            if (PanelLayer >= EPanelLayer.Cache)
            {
                Debug.LogError($" {name} 层级类型 选择错误 请重新选择");
                PanelLayer = EPanelLayer.Panel;
            }
        }

        #endregion

        private bool ShowAutoCheckBtn()
        {
            if (!UIOperationHelper.CheckUIOperation(false)) return false;
            return true;
        }

        [GUIColor(1, 1, 0)]
        [Button("自动检查所有", 30)]
        [PropertyOrder(-100)]
        [ShowIf("ShowAutoCheckBtn")]
        private void AutoCheckBtn()
        {
            AutoCheck();
        }

        [GUIColor(1, 1, 1)]
        [Button("重置子预制", 20)]
        [PropertyOrder(-100)]
        [ShowIf("ShowAutoCheckBtn")]
        private void RevertPrefabInstance()
        {
            UnityTipsHelper.CallBack(
                "将会重置所有子CDE 还原到预制初始状态 \n(防止嵌套预制修改)",
                () =>
                {
                    InvokeTargetMethod(CreateModuleType, "RefreshChildCdeTable", this);
                    foreach (var cdeTable in AllChildCdeTable)
                    {
                        try
                        {
                            PrefabUtility.RevertPrefabInstance(cdeTable.gameObject, InteractionMode.AutomatedAction);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"这个CDETable 不是预制体 请检查 {cdeTable.name}\n{e.Message}", cdeTable.gameObject);
                        }
                    }
                });
        }

        [GUIColor(0, 1, 1)]
        [Button("保存选中", 50)]
        [PropertyOrder(-100)]
        [ShowIf("ShowSaveSelectSelf")]
        private void SaveSelectSelf()
        {
            var stage = PrefabStageUtility.GetPrefabStage(gameObject);

            if (stage == null)
            {
                Debug.LogError($"未知错误 没有找到预制 {gameObject.name}");
                return;
            }

            var methodInfo = stage.GetType().GetMethod("Save", BindingFlags.Instance | BindingFlags.NonPublic);

            if (methodInfo != null)
            {
                bool result = (bool)methodInfo.Invoke(stage, null);
                if (!result)
                {
                    Debug.LogError("自动保存失败 注意请手动保存");
                }
            }
            else
            {
                Debug.LogError("Save方法不存在 自动保存失败 注意请手动保存");
            }

            var assetObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(stage.assetPath);
            EditorGUIUtility.PingObject(assetObj);
            Selection.activeObject = assetObj;
        }

        private bool ShowSaveSelectSelf()
        {
            var parentName = gameObject.transform.parent?.name ?? "";
            if (!parentName.Contains("Canvas"))
            {
                return false;
            }

            return !UIOperationHelper.CheckUIIsPackages(this, false);
        }

        internal bool AutoCheck()
        {
            if (!UIOperationHelper.CheckUIOperation(this)) return false;

            var (result1, result2) = InvokeTargetMethod<bool>(CreateModuleType, "InitVoName", this);

            if (!result1 || !result2) return false;

            ShowPackagesCreateBtn();
            OnValueChangedEUICodeType();
            OnValueChangedEPanelLayer();
            if (UICodeType == EUICodeType.Panel)
            {
                PanelSplitData.Panel = gameObject;
                if (!PanelSplitData.AutoCheck()) return false;
            }

            InvokeTargetMethod(CreateModuleType, "RefreshChildCdeTable", this);
            if (ComponentTable != null)
                ComponentTable.AutoCheck();
            if (DataTable != null)
                DataTable.AutoCheck();
            if (EventTable != null)
                EventTable.AutoCheck();

            EditorUtility.SetDirty(this);
            return true;
        }

        private bool ShowCreateBtn()
        {
            if (IsSplitData) return false;
            if (gameObject.transform.parent != null) return false;
            if (UIOperationHelper.CheckUIIsPackages(this, false)) return false;
            return UIOperationHelper.CheckUIOperationAll(this, false);
        }

        private bool ShowPackagesCreateBtn()
        {
            if (IsSplitData) return false;
            if (gameObject.transform.parent != null) return false;
            var result = UIOperationHelper.CheckUIIsPackages(this, false);
            if (result && string.IsNullOrEmpty(m_PackagesName))
            {
                m_PackagesName = UIOperationHelper.GetETPackagesName(this, false);
            }

            return result;
        }

        [GUIColor(0.7f, 0.4f, 0.8f)]
        [Button("生成", 50)]
        [ShowIf("ShowCreateBtn")]
        internal void CreateUICode()
        {
            if (!UIOperationHelper.CheckUIOperation(this)) return;

            if (!InvokeTargetMethod(CreateModuleType, "CreateCommon", this, false, false)) return;

            AssetDatabase.Refresh();
        }

        [LabelText("指定生成包名")]
        [ShowIf("ShowPackagesCreateBtn")]
        [ShowInInspector]
        [OdinSerialize]
        private string m_PackagesName;

        public string PackagesName => m_PackagesName;

        [GUIColor(0.7f, 0.4f, 0.8f)]
        [Button("Packages生成", 50)]
        [ShowIf("ShowPackagesCreateBtn")]
        internal void CreatePackagesUICode()
        {
            if (!UIOperationHelper.CheckUIIsPackages(this)) return;

            if (!InvokeTargetMethod(CreateModuleType, "CreatePackages", this, false, false, m_PackagesName)) return;

            AssetDatabase.Refresh();
        }

        private bool ShowPanelSourceSplit()
        {
            if (!UIOperationHelper.CheckUIOperationAll(this, false)) return false;
            return IsSplitData;
        }

        [GUIColor(0f, 0.4f, 0.8f)]
        [Button("源数据拆分", 50)]
        [ShowIf("ShowPanelSourceSplit")]
        internal void PanelSourceSplit()
        {
            if (!UIOperationHelper.CheckUIOperation(this)) return;

            if (IsSplitData)
            {
                if (AutoCheck())
                {
                    InvokeTargetMethod(SourceSplitType, "Do", this);
                }
            }
            else
            {
                UnityTipsHelper.ShowError($"{name} 当前数据不是源数据 无法进行拆分 请检查数据");
            }
        }

        private void OnValidate()
        {
            ComponentTable ??= GetComponent<UIBindComponentTable>();
            DataTable      ??= GetComponent<UIBindDataTable>();
            EventTable     ??= GetComponent<UIBindEventTable>();
        }

        private void AddComponentTable()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            ComponentTable = gameObject.GetOrAddComponent<UIBindComponentTable>();
        }

        private void AddDataTable()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            DataTable = gameObject.GetOrAddComponent<UIBindDataTable>();
        }

        private void AddEventTable()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            EventTable = gameObject.GetOrAddComponent<UIBindEventTable>();
        }

        #region 反射

        private Type m_CreateModuleType;

        private Type CreateModuleType => m_CreateModuleType ??= GetTargetType("ET.YIUIFramework.Editor", "YIUIFramework.Editor.UICreateModule");

        private Type m_SourceSplitType;

        private Type SourceSplitType => m_SourceSplitType ??= GetTargetType("ET.YIUIFramework.Editor", "YIUIFramework.Editor.UIPanelSourceSplit");

        private Type GetTargetType(string assemblyName, string className)
        {
            var assembly = AssemblyHelper.GetAssembly(assemblyName);
            if (assembly == null)
            {
                Debug.LogError($"没有这个程序集 {assemblyName}");
                return null;
            }

            var targetType = assembly.GetType(className);

            if (targetType == null)
            {
                Debug.LogError($"没有这个类 {className}");
            }

            return targetType;
        }

        private bool InvokeTargetMethod(Type targetType, string methodName, params object[] parameters)
        {
            if (targetType == null)
            {
                Debug.LogError($"targetType == null");
                return false;
            }

            MethodInfo method = targetType?.GetMethod(methodName);
            if (method == null)
            {
                Debug.LogError($"没有这个方法 {methodName}");
                return false;
            }

            try
            {
                method.Invoke(null, parameters);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"{e}");
            }

            return false;
        }

        private (bool, T) InvokeTargetMethod<T>(Type targetType, string methodName, params object[] parameters)
        {
            if (targetType == null)
            {
                Debug.LogError($"targetType == null");
                return (false, default);
            }

            MethodInfo method = targetType.GetMethod(methodName);
            if (method == null)
            {
                Debug.LogError($"没有这个方法 {methodName}");
                return (false, default);
            }

            try
            {
                object result = method.Invoke(null, parameters);
                return (true, (T)result);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e}");
            }

            return (false, default);
        }

        #endregion
    }
}
#endif
