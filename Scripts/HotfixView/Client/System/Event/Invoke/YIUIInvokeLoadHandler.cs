using UnityEngine;
using YIUIFramework;
using UnityObject = UnityEngine.Object;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeLoadSyncHandler : AInvokeHandler<YIUIInvokeLoad, UnityObject>
    {
        public override UnityObject Handle(YIUIInvokeLoad args)
        {
            if (YIUILoadComponent.Inst == null) return null;

            var resName = args.ResName;

            var obj = YIUILoadComponent.Inst.LoadAsset(args.PkgName, resName, args.LoadType);

            if (obj == null)
            {
                Log.Error($"加载失败 没有这个资源 请检查 {resName}");
                return null;
            }

            return obj;
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeLoadAsyncHandler : AInvokeHandler<YIUIInvokeLoad, ETTask<UnityObject>>
    {
        public override async ETTask<UnityObject> Handle(YIUIInvokeLoad args)
        {
            if (YIUILoadComponent.Inst == null) return null;

            var resName = args.ResName;

            var obj = await YIUILoadComponent.Inst.LoadAssetAsync(args.PkgName, resName, args.LoadType);

            if (obj == null)
            {
                Log.Error($"加载失败 没有这个资源 请检查 {resName}");
                return null;
            }

            return obj;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeLoadSpriteSyncHandler : AInvokeHandler<YIUIInvokeLoadSprite, Sprite>
    {
        public override Sprite Handle(YIUIInvokeLoadSprite args)
        {
            if (YIUILoadComponent.Inst == null) return null;

            var resName = args.ResName;

            #if UNITY_EDITOR
            if (!YIUILoadComponent.Inst.VerifyAssetValidity(resName))
            {
                Log.Error($"验证资产有效性 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }
            #endif

            var sprite = YIUILoadComponent.Inst.LoadAsset<Sprite>(resName);

            if (sprite == null)
            {
                Log.Error($"加载失败 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }

            return sprite;
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeLoadSpriteAsyncHandler : AInvokeHandler<YIUIInvokeLoadSprite, ETTask<Sprite>>
    {
        public override async ETTask<Sprite> Handle(YIUIInvokeLoadSprite args)
        {
            if (YIUILoadComponent.Inst == null) return null;

            var resName = args.ResName;

            #if UNITY_EDITOR
            if (!YIUILoadComponent.Inst.VerifyAssetValidity(resName))
            {
                Log.Error($"验证资产有效性 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }
            #endif

            var sprite = await YIUILoadComponent.Inst.LoadAssetAsync<Sprite>(resName);

            if (sprite == null)
            {
                Log.Error($"加载失败 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }

            return sprite;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeLoadTexture2DSyncHandler : AInvokeHandler<YIUIInvokeLoadTexture2D, Texture2D>
    {
        public override Texture2D Handle(YIUIInvokeLoadTexture2D args)
        {
            if (YIUILoadComponent.Inst == null) return null;

            var resName = args.ResName;

            #if UNITY_EDITOR
            if (!YIUILoadComponent.Inst.VerifyAssetValidity(resName))
            {
                Log.Error($"验证资产有效性 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }
            #endif

            var texture2D = YIUILoadComponent.Inst.LoadAsset<Texture2D>(resName);

            if (texture2D == null)
            {
                Log.Error($"加载失败 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }

            return texture2D;
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeLoadTexture2DAsyncHandler : AInvokeHandler<YIUIInvokeLoadTexture2D, ETTask<Texture2D>>
    {
        public override async ETTask<Texture2D> Handle(YIUIInvokeLoadTexture2D args)
        {
            if (YIUILoadComponent.Inst == null) return null;

            var resName = args.ResName;

            #if UNITY_EDITOR
            if (!YIUILoadComponent.Inst.VerifyAssetValidity(resName))
            {
                Log.Error($"验证资产有效性 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }
            #endif

            var texture2D = await YIUILoadComponent.Inst.LoadAssetAsync<Texture2D>(resName);

            if (texture2D == null)
            {
                Log.Error($"加载失败 没有这个资源 图片无法加载 请检查 {resName}");
                return null;
            }

            return texture2D;
        }
    }
}