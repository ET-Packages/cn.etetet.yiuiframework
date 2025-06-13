﻿using UnityEngine;
using YIUIFramework;
using UnityObject = UnityEngine.Object;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeLoadSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_Load, UnityObject>
    {
        public override UnityObject Handle(Entity entity, YIUIInvokeEntity_Load args)
        {
            EntityRef<YIUILoadComponent> loadRef = entity?.YIUILoad();

            if (loadRef.Entity == null) return null;

            var resName = args.ResName;

            var obj = loadRef.Entity.LoadAsset(args.PkgName, resName, args.LoadType);

            if (obj == null)
            {
                Log.Error($"加载失败 没有这个资源 请检查 {resName}");
                return null;
            }

            return obj;
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeLoadAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_Load, ETTask<UnityObject>>
    {
        public override async ETTask<UnityObject> Handle(Entity entity, YIUIInvokeEntity_Load args)
        {
            EntityRef<YIUILoadComponent> loadRef = entity?.YIUILoad();

            if (loadRef.Entity == null) return null;

            var resName = args.ResName;

            var obj = await loadRef.Entity.LoadAssetAsync(args.PkgName, resName, args.LoadType);

            if (obj == null)
            {
                Log.Error($"加载失败 没有这个资源 请检查 {resName}");
                return null;
            }

            return obj;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeLoadSpriteSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_LoadSprite, Sprite>
    {
        public override Sprite Handle(Entity entity, YIUIInvokeEntity_LoadSprite args)
        {
            EntityRef<YIUILoadComponent> loadRef = entity?.YIUILoad();

            if (loadRef.Entity == null) return null;

            var resName = args.ResName;

            #if UNITY_EDITOR
            if (!loadRef.Entity.VerifyAssetValidity(resName))
            {
                Log.Error($"验证资产有效性 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }
            #endif

            var sprite = loadRef.Entity.LoadAsset<Sprite>(resName);

            if (sprite == null)
            {
                Log.Error($"加载失败 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }

            return sprite;
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeLoadSpriteAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_LoadSprite, ETTask<Sprite>>
    {
        public override async ETTask<Sprite> Handle(Entity entity, YIUIInvokeEntity_LoadSprite args)
        {
            EntityRef<YIUILoadComponent> loadRef = entity?.YIUILoad();

            if (loadRef.Entity == null) return null;

            var resName = args.ResName;

            #if UNITY_EDITOR
            if (!loadRef.Entity.VerifyAssetValidity(resName))
            {
                Log.Error($"验证资产有效性 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }
            #endif

            var sprite = await loadRef.Entity.LoadAssetAsync<Sprite>(resName);

            if (sprite == null)
            {
                Log.Error($"加载失败 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }

            return sprite;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeLoadTexture2DSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_LoadTexture2D, Texture2D>
    {
        public override Texture2D Handle(Entity entity, YIUIInvokeEntity_LoadTexture2D args)
        {
            EntityRef<YIUILoadComponent> loadRef = entity?.YIUILoad();

            if (loadRef.Entity == null) return null;

            var resName = args.ResName;

            #if UNITY_EDITOR
            if (!loadRef.Entity.VerifyAssetValidity(resName))
            {
                Log.Error($"验证资产有效性 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }
            #endif

            var texture2D = loadRef.Entity.LoadAsset<Texture2D>(resName);

            if (texture2D == null)
            {
                Log.Error($"加载失败 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }

            return texture2D;
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeLoadTexture2DAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_LoadTexture2D, ETTask<Texture2D>>
    {
        public override async ETTask<Texture2D> Handle(Entity entity, YIUIInvokeEntity_LoadTexture2D args)
        {
            EntityRef<YIUILoadComponent> loadRef = entity?.YIUILoad();

            if (loadRef.Entity == null) return null;

            var resName = args.ResName;

            #if UNITY_EDITOR
            if (!loadRef.Entity.VerifyAssetValidity(resName))
            {
                Log.Error($"验证资产有效性 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }
            #endif

            var texture2D = await loadRef.Entity.LoadAssetAsync<Texture2D>(resName);

            if (texture2D == null)
            {
                Log.Error($"加载失败 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }

            return texture2D;
        }
    }
}