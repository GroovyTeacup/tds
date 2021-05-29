using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace TDS.Patches
{
    public static class CustomAssetPatches
    {
        static List<string> PassingURLs = new List<string>();

        private static void CustomImage_StartWWWs(CustomImage __instance)
        {
            string realType = __instance.GetType().Name;
            TDSPlugin.Instance.LogInfo($"CustomImage Instance Type: {realType}");

            if (realType == "CustomToken")
            {
                if (__instance.CustomImageURL != "")
                {
                    PassingURLs.Add(__instance.CustomImageURL);
                }
                if (__instance.CustomImageSecondaryURL != "")
                {
                    PassingURLs.Add(__instance.CustomImageSecondaryURL);
                }
            }
        }

        private static void CustomToken_OnSetupImagePostfix(Texture T, CustomToken __instance)
        {
            __instance.GetComponent<Renderer>().sharedMaterial.mainTexture = TDSPlugin.Placeholder.TexturePlaceholder;
        }

        private static void CustomAssetBundle_SpawnGameObjectsPostfix(CustomAssetbundle __instance)
        {
            var renderers = __instance.GetComponentsInChildren<Renderer>(true);
            var meshRenderers = __instance.GetComponentsInChildren<MeshRenderer>(true);
            var effects = __instance.GetComponentsInChildren<TTSAssetBundleEffects>(true);
            var particleSystems = __instance.GetComponentsInChildren<ParticleSystem>(true);

            foreach (var renderer in renderers)
            {
                UnityEngine.Object.Destroy(renderer);
            }

            foreach (var effect in effects)
            {
                UnityEngine.Object.Destroy(effect);
            }

            foreach (var ps in particleSystems)
            {
                UnityEngine.Object.Destroy(ps);
            }

            foreach (var meshRenderer in meshRenderers)
            {
                UnityEngine.Object.Destroy(meshRenderer);
            }
        }

        static bool CustomLoadingTexture_Load(string url, Action<CustomTextureContainer> callback, CustomLoadingManager.CustomLoadingTexture __instance)
        {
            string caller = callback.Method.DeclaringType.Name;

            TDSPlugin.Instance.LogInfo($"Calling Type: {caller}");

            if (PassingURLs.Contains(url)) return true;

            if (caller == "UIGridMenuButton")
            {
                PassingURLs.Add(url);
                return true;
            }

            __instance.Load(url, callback, __instance.textureSettings);

            CustomTextureContainer customTextureContainer = new CustomTextureContainer(url, TDSPlugin.Placeholder.TexturePlaceholder, 1f);
            TDSPlugin.Instance.LogInfo($"Texture IsError: {customTextureContainer.IsError()}. URL: {url}");

            Singleton<CustomLoadingManager>.Instance.Texture.Finished(customTextureContainer);
            Singleton<UILoading>.Instance.RemoveLoading();

            return false;
        }

        static bool CustomLoadingTexture_OnLoad(string url)
        {
            if (PassingURLs.Contains(url))
            {
                PassingURLs.Remove(url);
                return true;
            }

            return false;
        }

        static bool CustomLoadingAudio_OnLoad(string url)
        {
            CustomAudioContainer customAudioContainer = new CustomAudioContainer(url, TDSPlugin.Placeholder.AudioPlaceholder);

            TDSPlugin.Instance.LogInfo($"Audio IsError: {customAudioContainer.IsError()}. URL: {url}");

            Singleton<CustomLoadingManager>.Instance.Audio.Finished(customAudioContainer);
            Singleton<UILoading>.Instance.RemoveLoading();
            return false;
        }

        static bool IsErrorPatch(bool __result, CustomTextureContainer __instance)
        {
            if (PassingURLs.Contains(__instance.url))
            {
                return true;
            }

            __result = false;
            return false;
        }
    }
}
