using BepInEx;
using BepInEx.Logging;
using GLTFast;
using GLTFast.Jobs;
using GLTFast.Materials;
using GLTFast.Schema;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Burst.LowLevel;
using UnityEngine;
using static GLTFast.Jobs.CachedFunction;

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

[HarmonyPatch(typeof(BurstCompiler), "Compile", typeof(object), typeof(bool))]
public static class BurstRemove
{
    private unsafe static bool Prefix(object delegateObj, out void* __result)
    {
        // Let's skip any Burst stuff for GLTF for now
        if (delegateObj.GetType().Assembly == typeof(GLTFast.GltfAsset).Assembly)
        {
            __result = (void*)Marshal.GetFunctionPointerForDelegate(delegateObj);
            return false;
        }

        __result = null;
        return true;
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

        //_loadedBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Info.Location), "gltf"));

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
    //        var gameobj = new GameObject();

    //        var gltf = gameobj.AddComponent<GLTFast.GltfAsset>();
    //        gltf.url = "file://" + Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Info.Location), "AnimationExportTest.glb"));

    //        Logger.LogInfo("object made.");

    //        gameobj.transform.position = Player._mainPlayer.transform.position;

    //        Logger.LogInfo("spawned.");
    //        //var collider = gameobj.AddComponent<MeshCollider>();
    //        //collider.enabled = true;
    //        //collider.sharedMesh =

    //        /*
    //        byte[] data = file.readallbytes(path.combine(path.getdirectoryname(info.location), "test.glb"));
    //        var gltf = new gltfimport();

    //        bool success = gltf.loadgltfbinary(data).getawaiter().getresult();

    //        if (success)
    //        {
    //            var gameobj = new gameobject();
    //            gameobj.transform.position = player._mainplayer.transform.position;

    //            success = gltf.instantiatemainscene(gameobj.transform);
    //        }

    //        logger.loginfo("object made: " + success);


    //        logger.loginfo("spawned.");
    //        */
    //    }
    //    catch (Exception e)
    //    {
    //        Logger.LogError(e);
    //    }
    //}
}
