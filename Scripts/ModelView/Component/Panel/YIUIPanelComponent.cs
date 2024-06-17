//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// UI面板组件
    /// </summary>
    [ComponentOf(typeof(YIUIComponent))]
    public partial class YIUIPanelComponent : Entity, IAwake, IYIUIInitialize, IDestroy
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

        private EntityRef<Entity> _ownerUIEntity;
        private Entity            _OwnerUIEntity => _ownerUIEntity;

        public Entity OwnerUIEntity
        {
            get
            {
                if (_OwnerUIEntity == null)
                {
                    _ownerUIEntity = UIBase?.OwnerUIEntity;
                }

                return _OwnerUIEntity;
            }
        }

        private EntityRef<YIUIWindowComponent> _uiWindow;
        private YIUIWindowComponent            _UIWindow => _uiWindow;

        public YIUIWindowComponent UIWindow
        {
            get
            {
                if (_UIWindow == null)
                {
                    _uiWindow = UIBase?.GetComponent<YIUIWindowComponent>();
                }

                return _UIWindow;
            }
        }

        /// <summary>
        /// 所在层级
        /// </summary>
        public EPanelLayer Layer = EPanelLayer.Panel;

        /// <summary>
        /// 界面选项
        /// </summary>
        public EPanelOption PanelOption = EPanelOption.None;

        /// <summary>
        /// 堆栈操作
        /// </summary>
        public EPanelStackOption StackOption = EPanelStackOption.Visible;

        /// <summary>
        /// 优先级，用于同层级排序,
        /// 大的在前 小的在后
        /// 相同时 后添加的在前
        /// </summary>
        public int Priority = 0;
    }
}
