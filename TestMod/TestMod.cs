using BepInEx;
using BepInEx.Logging;
using GLTFast;
using GLTFast.Materials;
using GLTFast.Schema;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BepInExGLTFTestMod;

[HarmonyPatch(typeof(BuiltInMaterialGenerator), nameof(BuiltInMaterialGenerator.FinderShaderMetallicRoughness))]
public static class FinderShaderMetallicRoughnessPatch
{
    public static bool Prefix(BuiltInMaterialGenerator __instance, ref Shader __result)
    {
        BepInExGLTFTestMod.Instance.Logger.LogInfo("Loading FinderShaderMetallicRoughness");
        __result = BepInExGLTFTestMod.Instance.GetShader("assets/atlyss/shader/gltf/built-in/gltfpbrmetallicroughness.shader");
        if (__result == null)
            BepInExGLTFTestMod.Instance.Logger.LogInfo("Load failed FinderShaderMetallicRoughness");
        return false;
    }
}

[HarmonyPatch(typeof(BuiltInMaterialGenerator), nameof(BuiltInMaterialGenerator.FinderShaderSpecularGlossiness))]
public static class FinderShaderSpecularGlossinessPatch
{
    public static bool Prefix(BuiltInMaterialGenerator __instance, ref Shader __result)
    {
        BepInExGLTFTestMod.Instance.Logger.LogInfo("Loading FinderShaderSpecularGlossiness");
        __result = BepInExGLTFTestMod.Instance.GetShader("assets/atlyss/shader/gltf/built-in/gltfpbrspecularglossiness.shader");
        if (__result == null)
            BepInExGLTFTestMod.Instance.Logger.LogInfo("Load failed FinderShaderSpecularGlossiness");
        return false;
    }
}

[HarmonyPatch(typeof(BuiltInMaterialGenerator), nameof(BuiltInMaterialGenerator.FinderShaderUnlit))]
public static class FinderShaderUnlitPatch
{
    public static bool Prefix(BuiltInMaterialGenerator __instance, ref Shader __result)
    {
        BepInExGLTFTestMod.Instance.Logger.LogInfo("Loading FinderShaderUnlit");
        __result = BepInExGLTFTestMod.Instance.GetShader("assets/atlyss/shader/gltf/built-in/gltfunlit.shader");
        if (__result == null)
            BepInExGLTFTestMod.Instance.Logger.LogInfo("Load failed FinderShaderUnlit");
        return false;
    }
}

[BepInPlugin("BepInExGLTFTestMod", "BepInExGLTFTestMod", "1.0.0")]
public class BepInExGLTFTestMod : BaseUnityPlugin
{
    public static BepInExGLTFTestMod Instance { get; private set; }
    public new ManualLogSource Logger { get; private set; }

    private AssetBundle _loadedBundle;
    private Harmony _harmony;

    internal Shader GetShader(string name)
    {
        return _loadedBundle.LoadAsset<Shader>(name);
    }

    private void Awake()
    {
        Instance = this;
        Logger = base.Logger;

        _harmony = new Harmony("BepInExGLTFTestMod");
        _harmony.PatchAll();

        _loadedBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Info.Location), "gltf"));

        Logger.LogInfo("Loaded assets:");

        foreach (var name in _loadedBundle.GetAllAssetNames())
        {
            Logger.LogInfo(" - " + name);
        }

        Logger.LogInfo("Initialized!");
    }

    //public void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.N))
    //    {
    //        Test();
    //    }
    //}

    //private void Test()
    //{
    //    try
    //    {
    //        var gameObj = new GameObject();

    //        var gltf = gameObj.AddComponent<GLTFast.GltfAsset>();
    //        gltf.Url = "file://" + Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Info.Location), "test.glb"));

    //        Logger.LogInfo("Object made.");

    //        gameObj.transform.position = Player._mainPlayer.transform.position;

    //        Logger.LogInfo("Spawned.");
    //        var collider = gameObj.AddComponent<MeshCollider>();
    //        collider.enabled = true;
    //        collider.sharedMesh =

    //        /*
    //        byte[] data = File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(Info.Location), "test.glb"));
    //        var gltf = new GltfImport();

    //        bool success = gltf.LoadGltfBinary(data).GetAwaiter().GetResult();

    //        if (success)
    //        {
    //            var gameObj = new GameObject();
    //            gameObj.transform.position = Player._mainPlayer.transform.position;

    //            success = gltf.InstantiateMainScene(gameObj.transform);
    //        }

    //        Logger.LogInfo("Object made: " + success);


    //        Logger.LogInfo("Spawned.");
    //        */
    //    }
    //    catch (Exception e)
    //    {
    //        Logger.LogError(e);
    //    }
    //}
}
