#if UNITY_EDITOR
using System.Collections.Generic;

namespace YIUIFramework
{
    public abstract partial class UIDataBindSelectBase
    {
        public bool EditorSetDataSelects(IReadOnlyList<UIData> datas, out string error)
        {
            error = string.Empty;

            if (datas == null || datas.Count <= 0)
            {
                error = $"{name} 必须至少绑定一个 Data";
                return false;
            }

            if (datas.Count > SelectMax())
            {
                error = $"{name} 当前组件最多允许绑定 {SelectMax()} 个 Data，请检查写入请求";
                return false;
            }

            var names = new HashSet<string>();
            for (var i = 0; i < datas.Count; i++)
            {
                var data = datas[i];
                if (data == null || data.DataValue == null)
                {
                    error = $"{name} 第 {i} 个 Data 为空";
                    return false;
                }

                if (!IsValid(data.DataValue.UIBindDataType))
                {
                    error = $"{name} 不支持绑定 Data {data.Name} 的类型 {data.DataValue.UIBindDataType}";
                    return false;
                }

                if (!names.Add(data.Name))
                {
                    error = $"{name} 重复绑定 Data: {data.Name}";
                    return false;
                }
            }

            UnBindData();
            m_DataSelectDic.Clear();

            foreach (var data in datas)
            {
                m_DataSelectDic.Add(data.Name, new UIDataSelect(data));
            }

            base.OnValidate();
            return true;
        }
    }
}
#endif
