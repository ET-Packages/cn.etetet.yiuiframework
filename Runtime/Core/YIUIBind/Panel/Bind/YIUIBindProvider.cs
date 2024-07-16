using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace YIUIFramework
{
    internal class YIUIBindProvider : ICodeGenerator<YIUIBindVo>
    {
        #region 扫描指定程序集

        #if !YIUIMACRO_BIND_BYUNITYDLL_ALL

        //业务代码相关程序集的名字
        //默认有Unity默认程序集 可以根据需求修改
        private static readonly string[] LogicAssemblyNames = { "ET.ModelView" };

        private static Type[] GetLogicTypes()
        {
            return AppDomain.CurrentDomain.GetTypesByAssemblyName(LogicAssemblyNames);
        }
        #else
        //找所有程序集 全部遍历一次 消耗比指定程序集大 但是简单 如果你有需求扫描指定的
        //使用上面的给的方法 写入你要的程序集
        //这个也仅限于编辑器模式下 正常打包出去后会使用生成的就没有消耗了 根据需求自行选择
        private static Type[] GetLogicTypes()
        {
            System.Reflection.Assembly[]               assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Dictionary<string, Type> types = AssemblyHelper.GetAssemblyTypes(assemblies);
            return types.Values.ToArray();
        }
        #endif

        #endregion

        #if UNITY_EDITOR
        /// <summary>
        /// 编辑器下加载热更dll的目录
        /// </summary>
        public const string CodeDir = "Packages/cn.etetet.loader/Bundles/Code";

        public Type[] GetLogicTypesByDll()
        {
            byte[] modelViewAssBytes = File.ReadAllBytes(Path.Combine(CodeDir, "ET.ModelView.dll.bytes"));
            byte[] modelViewPdbBytes = File.ReadAllBytes(Path.Combine(CodeDir, "ET.ModelView.pdb.bytes"));
            var    modelViewAssembly = Assembly.Load(modelViewAssBytes, modelViewPdbBytes);
            var    typesDic          = AssemblyHelper.GetAssemblyTypes(modelViewAssembly);
            return typesDic.Values.ToArray();
        }
        #endif

        public YIUIBindVo[] Get()
        {
            //使用反射时 有2种方式
            //1.使用DLL(ET工程编译出来的) 只能编辑器时用 一般情况下都不用
            //2.使用Unity的程序集(Unity编译的)
            #if UNITY_EDITOR && YIUIMACRO_BIND_BYETDLL
            var types = GetLogicTypesByDll();
            #else
            var types = GetLogicTypes();
            #endif

            var binds = new List<YIUIBindVo>();

            foreach (var type in types)
            {
                if (type.IsAbstract) continue;
                var attribute = type.GetCustomAttribute<YIUIAttribute>(true);
                if (attribute == null) continue;
                if (GetBindVo(out var bindVo, attribute, type))
                {
                    binds.Add(bindVo);
                }
            }

            return binds.ToArray();
        }

        private static bool GetBindVo(out YIUIBindVo bindVo,
                                      YIUIAttribute  attribute,
                                      Type           componentType)
        {
            bindVo = new YIUIBindVo();
            if (componentType == null ||
                !componentType.GetFieldValue("PkgName", out bindVo.PkgName) ||
                !componentType.GetFieldValue("ResName", out bindVo.ResName))
            {
                return false;
            }

            bindVo.ComponentType = componentType;
            bindVo.CodeType      = attribute.YIUICodeType;
            bindVo.PanelLayer    = attribute.YIUIPanelLayer;
            if (bindVo is { CodeType: EUICodeType.Panel, PanelLayer: EPanelLayer.Any })
            {
                Debug.LogError($"{componentType.Name} 错误的设定 既然是Panel 那必须设定所在层级 不能是Any 请检查重新导出");
            }

            return true;
        }

        public void WriteCode(YIUIBindVo info, StringBuilder sb)
        {
            sb.Append("            {\r\n");
            sb.AppendFormat("                PkgName       = {0}.PkgName,\r\n", info.ComponentType.FullName);
            sb.AppendFormat("                ResName       = {0}.ResName,\r\n", info.ComponentType.FullName);
            sb.AppendFormat("                CodeType      = EUICodeType.{0},\r\n", info.CodeType.ToString());
            sb.AppendFormat("                PanelLayer    = EPanelLayer.{0},\r\n", info.PanelLayer.ToString());
            sb.AppendFormat("                ComponentType = typeof({0}),\r\n", info.ComponentType.FullName);
            sb.Append("            };\r\n");
        }

        public void NewCode(YIUIBindVo info, StringBuilder sb)
        {
        }
    }
}