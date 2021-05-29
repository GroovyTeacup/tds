using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using NewNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDS
{
    /// <summary>
    /// Base plugin class for TDS.
    /// This plugin is intended for use on makeshift dedicated server for TTS. 
    /// The idea is to strip the game of things that wouldn't be needed in a headless server client and yet is still loaded/running in the background. (Textures, sounds, etc). This adds up to a overhead and can be cut out.
    /// We're using harmony in our case to just disable a great many functions. https://github.com/BepInEx/HarmonyX
    /// This is, obviously, about as destructive as a patching mod can get and isn't meant to be used with any other plugins not developed with this in mind.
    /// We're using BepInEx to handle loading our plugin.
    /// Launch server with command line options: -batchmode -nographics -nosound -nosubscription -nointro -novid -novoicechat
    /// </summary>
    [BepInPlugin("com.tea.tds", "DediHelper", "1.0.0.0")]
    [BepInProcess("Tabletop Simulator.exe")]
    public class TDSPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            if (!Utilities.IsLaunchOption("-batchmode"))
            {
                //Logger.LogWarning("Game is not in batchmode. Not loading TDS.");
                //return;
            }

            Instance = this;

            placeholderObject = new GameObject();
            Placeholder = placeholderObject.AddComponent<Placeholders>();
            UnityEngine.Object.DontDestroyOnLoad(placeholderObject);

            this.DoPatches();

            Logger.LogInfo("Dedicated server helper loaded.");

            this.StartCoroutine(SetMisc());
        }

        private IEnumerator SetMisc()
        {
            yield return new WaitUntil(() => NetworkUI.Instance != null);

            // Disable auto saving of table. (This crashes the game in batchmode)
            SaveManager.Instance.AutoSaveCount = 0;
            SaveManager.Instance.AutoSaveInterval = 0;

            foreach (var camera in Camera.allCameras)
            {
                camera.enabled = false;
                camera.SetTargetBuffers(new RenderBuffer[0], new RenderBuffer());
                UnityEngine.Object.Destroy(camera);
            }

            AudioListener.volume = 0;


            LogInfo("SetMisc");
            yield break;
        }


        public void LogInfo(string str)
        {
            Logger.LogInfo(str);
        }

        public void LogError(string str)
        {
            Logger.LogError(str);
        }

        private void DoPatches()
        {
            // Prevent custom textures from loading. 
            AccessTools.Method(typeof(CustomLoadingManager.CustomLoadingTexture), "OnLoad").HarmonyPatch(AccessTools.Method(typeof(TDS.Patches.CustomAssetPatches), "CustomLoadingTexture_OnLoad"));
            AccessTools.Method(typeof(CustomLoadingManager.CustomLoadingTexture), "Load", Utils.TypeArray<string, Action<CustomTextureContainer>, bool, bool, bool, bool, bool, bool, int, CustomLoadingManager.LoadType>()).HarmonyPatch(AccessTools.Method(typeof(TDS.Patches.CustomAssetPatches), "CustomLoadingTexture_Load"));
            AccessTools.Method(typeof(CustomImage), "StartWWWs").HarmonyPatch(AccessTools.Method(typeof(TDS.Patches.CustomAssetPatches), "CustomImage_StartWWWs"));
            AccessTools.Method(typeof(CustomToken), "OnSetupImage").HarmonyPatch(null, AccessTools.Method(typeof(TDS.Patches.CustomAssetPatches), "CustomToken_OnSetupImagePostfix"));
            AccessTools.Method(typeof(CustomAssetbundle), "SpawnGameObjects").HarmonyPatch(null, AccessTools.Method(typeof(TDS.Patches.CustomAssetPatches), "CustomAssetBundle_SpawnGameObjectsPostfix"));
            AccessTools.Method(typeof(CustomLoadingManager.CustomLoadingAudio), "OnLoad").HarmonyPatch(AccessTools.Method(typeof(TDS.Patches.CustomAssetPatches), "CustomLoadingAudio_OnLoad"));
            AccessTools.Method(typeof(CustomTextureContainer), "IsError").HarmonyPatch(AccessTools.Method(typeof(TDS.Patches.CustomAssetPatches), "IsErrorPatch"));

            // Prevent IRC chat from loading and doing its thing
            AccessTools.Method(typeof(ChatIRC), "Awake").DisableMethod();

            // Disable expensive functions that are not needed and probably won't break anything.
            AccessTools.Method(typeof(UIPanel), "LateUpdate").DisableMethod();
            AccessTools.Method(typeof(TextureScale), "ThreadedScale").DisableMethod();
            AccessTools.Method(typeof(SaveManager), "CheckSave").DisableMethod(); // Make sure saving doesn't happen
            AccessTools.Method(typeof(HoverScript), "Update").DisableMethod();
            AccessTools.Method(typeof(HandCamera), "Update").DisableMethod();
            AccessTools.Method(typeof(CameraController), "Update").DisableMethod();
            AccessTools.Method(typeof(UICamera), "Update").DisableMethod();
            AccessTools.Method(typeof(UIPointerMode), "Update").DisableMethod();
            AccessTools.Method(typeof(Dissonance.DissonanceComms), "Update").DisableMethod();
        }

        private GameObject placeholderObject;
        public static Placeholders Placeholder;
        public static TDSPlugin Instance;
        public static readonly Harmony HarmonyInstance = new Harmony("com.tea.tds");
    }
}
