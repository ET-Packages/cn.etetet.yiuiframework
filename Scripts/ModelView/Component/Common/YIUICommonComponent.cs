//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

namespace ET.Client
{
    /// <summary>
    /// UI通用组件
    /// </summary>
    [ComponentOf(typeof(YIUIComponent))]
    public partial class YIUICommonComponent : Entity, IAwake, IYIUIInitialize, IDestroy
    {
        private EntityRef<YIUIComponent> _uiBase;
        private YIUIComponent            _UIBase => _uiBase;

        public YIUIComponent UIBase
        {
            get
            {
                if (_UIBase == null)
                {
                    _uiBase = this.GetParent<YIUIComponent>();
                }

                return _UIBase;
            }
        }
    }
}
