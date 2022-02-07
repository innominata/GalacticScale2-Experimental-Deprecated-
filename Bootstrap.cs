using System.Collections;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using GSSerializer;
using HarmonyLib;
using UnityEngine;

namespace GalacticScale
{
    public partial class GS2
    {
        public static int PreferencesVersion = 2104;
    }


    [BepInPlugin("dsp.galactic-scale.2", "Galactic Scale 2 Plug-In", VERSION)]
    [BepInDependency("space.customizing.console", BepInDependency.DependencyFlags.SoftDependency)]
    // [BepInDependency(NebulaAPI.NebulaModAPI.API_GUID)]
    public class Bootstrap : BaseUnityPlugin
    {
        public const string VERSION = "2.3.0";

        public new static ManualLogSource Logger;

        // Internal Variables
        public static Queue buffer = new();

        internal void Awake()
        {
            var v = Assembly.GetExecutingAssembly().GetName().Version;
            GS2.Version = $"{v.Major}.{v.Minor}.{v.Build}";
            BCE.Console.Init();
            Logger = new ManualLogSource("GS2");
            BepInEx.Logging.Logger.Sources.Add(Logger);
            GS2.ConsoleSplash();
            ApplyHarmonyPatches();
            if (GS2.TP == null) GS2.TP = gameObject.AddComponent<TeleportComponent>();
        }

        private void ApplyHarmonyPatches()
        {
            var _ = new Harmony("dsp.galactic-scale.2");
            Harmony.CreateAndPatchAll(typeof(PatchOnWhatever));
            Harmony.CreateAndPatchAll(typeof(PatchOnBlueprintUtils));
            Harmony.CreateAndPatchAll(typeof(PatchOnBuildingGizmo));
            Harmony.CreateAndPatchAll(typeof(PatchOnBuildTool_BlueprintCopy));
            Harmony.CreateAndPatchAll(typeof(PatchOnBuildTool_BlueprintPaste));
            Harmony.CreateAndPatchAll(typeof(PatchOnBuildTool_Click));
            Harmony.CreateAndPatchAll(typeof(PatchOnBuildTool_Path));
            Harmony.CreateAndPatchAll(typeof(PatchOnGameData));
            Harmony.CreateAndPatchAll(typeof(PatchOnGameDesc));
            Harmony.CreateAndPatchAll(typeof(PatchOnGameHistoryData));
            Harmony.CreateAndPatchAll(typeof(PatchOnGameLoader));
            Harmony.CreateAndPatchAll(typeof(PatchOnGameMain));
            Harmony.CreateAndPatchAll(typeof(PatchOnGameOption));
            Harmony.CreateAndPatchAll(typeof(PatchOnGraticulePoser));
            Harmony.CreateAndPatchAll(typeof(PatchOnGuideMissionStandardMode));
            Harmony.CreateAndPatchAll(typeof(PatchOnNearColliderLogic));
            Harmony.CreateAndPatchAll(typeof(PatchOnPlanetAtmoBlur));
            Harmony.CreateAndPatchAll(typeof(PatchOnPlanetData));
            Harmony.CreateAndPatchAll(typeof(PatchOnPlanetFactory));
            Harmony.CreateAndPatchAll(typeof(PatchOnPlanetGrid));
            Harmony.CreateAndPatchAll(typeof(PatchOnPlanetModelingManager));
            Harmony.CreateAndPatchAll(typeof(PatchOnPlanetRawData));
            Harmony.CreateAndPatchAll(typeof(PatchOnPlanetSimulator));
            Harmony.CreateAndPatchAll(typeof(PatchOnPlatformSystem));
            Harmony.CreateAndPatchAll(typeof(PatchOnPlayerFootsteps));
            Harmony.CreateAndPatchAll(typeof(PatchOnPlayerMove_Fly));
            Harmony.CreateAndPatchAll(typeof(PatchOnPlayerMove_Sail));
            Harmony.CreateAndPatchAll(typeof(PatchOnPowerSystem));
            Harmony.CreateAndPatchAll(typeof(PatchOnStarGen));
            Harmony.CreateAndPatchAll(typeof(PatchOnStationComponent));
            Harmony.CreateAndPatchAll(typeof(PatchOnTrashSystem));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIAdvisorTip));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIBuildingGrid));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIEscMenu));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIGalaxySelect));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIGameLoadingSplash));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIMainMenu));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIOptionWindow));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIPlanetDetail));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIResearchResultsWindow));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIRoot));
            Harmony.CreateAndPatchAll(typeof(PatchOnUISpaceGuide));
            Harmony.CreateAndPatchAll(typeof(PatchOnUISpaceGuideEntry));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIStarDetail));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIStarmap));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIStarmapPlanet));
            Harmony.CreateAndPatchAll(typeof(PatchOnUITutorialTip));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIVeinDetail));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIVersionText));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIVirtualStarmap));
            Harmony.CreateAndPatchAll(typeof(PatchOnUniverseGen));
            Harmony.CreateAndPatchAll(typeof(PatchOnVFPreload));
            Harmony.CreateAndPatchAll(typeof(PatchOnUIAchievementPanel));
        }
        private void FixedUpdate()
        {
            if (VFInput.alt && VFInput.control && VFInput._openMechLight) GS2.WarnJson(HandleLocalStarPlanets.TransitionRadii);
            if (VFInput.control && VFInput.alt && VFInput.shift && VFInput._moveRight) GS2.Config.EnableDevMode();

            if (GS2.Config.Dev && VFInput.control && VFInput.shift && VFInput._rotate && GameMain.localPlanet != null)
            {
                var filename = Path.Combine(GS2.DataDir, "WorkingTheme.json");
                if (!File.Exists(filename))
                {
                    var oldTheme = GS2.GetGSPlanet(GameMain.localPlanet).GsTheme;
                    var fs = new fsSerializer();
                    fs.TrySerialize(oldTheme, out var data);
                    var json = fsJsonPrinter.PrettyJson(data);
                    File.WriteAllText(filename, json);
                    GS2.ShowMessage("WorkingTheme.json has been exported. Use the same key combination to reload it");
                    return;
                }

                GameMain.mainPlayer.controller.movementStateInFrame = EMovementState.Sail;
                GameMain.mainPlayer.controller.actionSail.ResetSailState();
                GameCamera.instance.SyncForSailMode();

                var p = GameMain.localStar.planets[GameMain.localPlanet.index];
                var gsPlanet = GS2.GetGSPlanet(p);
                GS2.GetGSStar(p.star).counter = p.index;

                var newTheme = GS2.LoadJsonTheme(filename);
                GS2.Warn($"LOADED THEME {newTheme.Name} CustomGen:{newTheme.CustomGeneration} TA:{newTheme.TerrainSettings.Algorithm}");
                newTheme.Process();
                gsPlanet.Theme = newTheme.Name;

                p.Free();
                p.data = null;
                p.factory = null;
                p.terrainMaterial = null;
                p.oceanMaterial = null;
                p.atmosMaterial = null;
                p.minimapMaterial = null;
                GS2.SetPlanetTheme(p, gsPlanet);
                GameMain.localStar.planets[p.index] = p;
                gsPlanet.planetData = p;
                PlanetModelingManager.RequestLoadPlanet(p);
                GameMain.data.LeavePlanet();
            }

            //GS2.Warn("FixedUpdate");

        }

        public static void Debug(object data, LogLevel logLevel, bool isActive)
        {
            if (isActive && Logger != null)
                {
                    while (buffer.Count > 0)
                    {
                        var o = buffer.Dequeue();
                        var l = ((object data, LogLevel loglevel, bool isActive))o;
                        if (l.isActive) Logger.Log(l.loglevel, "Q:" + l.data);
                    }

                    Logger.Log(logLevel, data);
                }
                else
                {
                    buffer.Enqueue((data, logLevel, true));
                }
            
        }

        public static void Debug(object data)
        {
            Debug(data, LogLevel.Message, true);
        }


    }
}