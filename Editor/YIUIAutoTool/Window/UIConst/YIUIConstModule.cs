using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    /// <summary>
    ///  常量
    /// </summary>
    [YIUIAutoMenu("常量", int.MaxValue)]
    public class YIUIConstModule : BaseYIUIToolModule
    {
        [Button("文档", 30, Icon = SdfIconType.Link45deg, IconAlignment = IconAlignment.LeftOfText)]
        [PropertyOrder(-99999)]
        public void OpenDocument()
        {
            Application.OpenURL("https://lib9kmxvq7k.feishu.cn/wiki/KjMgw5woPi4iOFkH3wrcUIZtnKd");
        }

        [ShowInInspector]
        [HideLabel]
        private YIUIConstAsset YIUIConstAsset;

        public override void Initialize()
        {
            YIUIConstAsset = YIUIConstHelper.Const;
        }

        public override void OnDestroy()
        {
            Save();
        }

        // [Button("保存", 50)]
        // [PropertyOrder(-999)]
        // [GUIColor(0.5f, 0.95f, 0.7f)]
        private void Save()
        {
            if (YIUIConstAsset == null)
            {
                return;
            }

            OdinSerializationUtility.Save(YIUIConstAsset, YIUIConstHelper.YIUIConstAssetPath);
            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(YIUIConstHelper.YIUIConstAssetPath);
            if (textAsset != null)
            {
                EditorUtility.SetDirty(textAsset);
            }

            AssetDatabase.SaveAssets();
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
        }
    }
}