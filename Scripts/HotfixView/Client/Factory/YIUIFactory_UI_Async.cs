﻿//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIFactory
    {
        public static async ETTask<T> InstantiateAsync<T>(Entity parentEntity, Transform parent = null) where T : Entity
        {
            var data = YIUIBindHelper.GetBindVoByType<T>();
            if (data == null) return null;
            var vo = data.Value;

            return await InstantiateAsync<T>(vo, parentEntity, parent);
        }

        public static async ETTask<T> InstantiateAsync<T>(YIUIBindVo vo, Entity parentEntity, Transform parent = null) where T : Entity
        {
            var uiCom = await CreateAsync(vo, parentEntity);
            SetParent(uiCom.GetParent<YIUIChild>().OwnerRectTransform, parent ? parent : YIUIMgrComponent.Inst.UICache);
            return (T)uiCom;
        }

        public static async ETTask<Entity> InstantiateAsync(YIUIBindVo vo, Entity parentEntity, Transform parent = null)
        {
            var uiCom = await CreateAsync(vo, parentEntity);
            SetParent(uiCom.GetParent<YIUIChild>().OwnerRectTransform, parent ? parent : YIUIMgrComponent.Inst.UICache);
            return uiCom;
        }

        public static async ETTask<Entity> InstantiateAsync(Type uiType, Entity parentEntity, Transform parent = null)
        {
            var data = YIUIBindHelper.GetBindVoByType(uiType);
            if (data == null) return null;
            var vo = data.Value;

            return await InstantiateAsync(vo, parentEntity, parent);
        }

        public static async ETTask<Entity> InstantiateAsync(string    pkgName, string resName, Entity parentEntity,
                                                            Transform parent = null)
        {
            var data = YIUIBindHelper.GetBindVoByPath(pkgName, resName);
            if (data == null) return null;
            var vo = data.Value;

            return await InstantiateAsync(vo, parentEntity, parent);
        }

        public static async ETTask<Entity> InstantiateAsync(string    resName, Entity parentEntity,
                                                            Transform parent = null)
        {
            var data = YIUIBindHelper.GetBindVoByResName(resName);
            if (data == null) return null;
            var vo = data.Value;

            return await InstantiateAsync(vo, parentEntity, parent);
        }

        public static async ETTask<Entity> CreatePanelAsync(PanelInfo panelInfo, Entity parentEntity)
        {
            var bingVo = YIUIBindHelper.GetBindVoByPath(panelInfo.PkgName, panelInfo.ResName);
            if (bingVo == null) return null;
            var uiCom = await CreateAsync(bingVo.Value, parentEntity);
            return uiCom;
        }

        public static async ETTask<Entity> CreateAsync(YIUIBindVo vo, Entity parentEntity)
        {
            EntityRef<Entity> parentEntityRef = parentEntity;
            var obj = await YIUILoadComponent.Inst?.LoadAssetAsyncInstantiate(vo.PkgName, vo.ResName);
            if (obj == null)
            {
                Debug.LogError($"没有加载到这个资源 {vo.PkgName}/{vo.ResName}");
                return null;
            }
            return CreateByObjVo(vo, obj, parentEntityRef);
        }
    }
}
