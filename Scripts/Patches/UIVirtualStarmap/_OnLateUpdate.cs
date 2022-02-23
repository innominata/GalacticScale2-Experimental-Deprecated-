﻿using HarmonyLib;
using NebulaAPI;
using NGPT;
using UnityEngine;

namespace GalacticScale
{
    public partial class PatchOnUIVirtualStarmap
    {
        // GSSettings.Instance.birthStar =
        //     __instance.starPool[GSSettings.BirthPlanet.planetData.star.index].starData;
        // GS2.ActiveGenerator.Generate(GSSettings.StarCount,__instance.starPool[GSSettings.BirthPlanet.planetData.star.index].starData );
        // __instance.galaxyData = GS2.ProcessGalaxy(GS2.gameDesc, true);
        // __instance.OnGalaxyDataReset();

        public static bool deBounce = false;
        [HarmonyPrefix]
        [HarmonyPatch(typeof(UIVirtualStarmap), "_OnLateUpdate")]
        public static bool _OnLateUpdate(ref UIVirtualStarmap __instance)
        {
            if (Input.mouseScrollDelta.y < 0) UIRoot.instance.galaxySelect.cameraPoser.distRatio += 0.1f;
            if (Input.mouseScrollDelta.y > 0) UIRoot.instance.galaxySelect.cameraPoser.distRatio -= 0.1f;
            if (VFInput._moveRight) GameCamera.instance.transform.localPosition += GameCamera.instance.galaxySelectPoser.transform.localRotation * (0.1f * Vector3.right);
            if (VFInput._moveLeft) GameCamera.instance.transform.localPosition += GameCamera.instance.galaxySelectPoser.transform.localRotation * (0.1f * Vector3.left);
            if (VFInput._moveForward) GameCamera.instance.transform.localPosition += GameCamera.instance.galaxySelectPoser.transform.localRotation * (0.1f * Vector3.up);
            if (VFInput._moveBackward) GameCamera.instance.transform.localPosition += GameCamera.instance.galaxySelectPoser.transform.localRotation * (0.1f * Vector3.down);
            if (!(VFInput.rtsConfirm.pressing || VFInput.rtsCancel.pressing))
            {
                deBounce = false;
                //GS2.Log($"Nope {VFInput.rtsConfirm.pressing}{VFInput.rtsCancel.pressing}{VFInput.rtsConfirm.onDown}{VFInput.rtsCancel.onDown}{VFInput.axis_button.down[0]}{VFInput.axis_button.down[1]}");
                return true;
            }
            if (deBounce) return false;
            deBounce = true;
            var starIndex = -1;
            var clickTolerance = 1.7f;
            for (var i = 0; i < __instance.starPool.Count; ++i)
                if (__instance.starPool[i].active)
                {
                    var starData = __instance.starPool[i].starData;
                    // var rectPoint = Vector2.zero;
                    // UIRoot.ScreenPointIntoRect(Camera.main.WorldToScreenPoint(starData.position), __instance.textGroup, out _);
                    // rectPoint.x += 18f;
                    // rectPoint.y += 6f;
                    // __instance.starPool[i].nameText.rectTransform.anchoredPosition = rectPoint;
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    var num2 = Kit.ClosestPoint2Straight(ray.origin, ray.GetPoint(300f), starData.position);
                    var distanceFromClickToStar = Vector3.Distance(ray.GetPoint(300f * num2), starData.position);
                    if (distanceFromClickToStar < (double)clickTolerance)
                    {
                        clickTolerance = distanceFromClickToStar >= __instance.starPool[i].pointRenderer.transform.localScale.x * 0.25 ? distanceFromClickToStar : 0.0f;
                        starIndex = i;
                    }

                    //GS2.Warn($"index2 = {index2} GSSettings.birthStarId:{GSSettings.birthStarId}");
                    // if (i == GSSettings.BirthPlanet.planetData.star.index)
                    // {
                    //     var color = __instance.starColors.Evaluate(starData.color);
                    //     __instance.starPointBirth.gameObject.SetActive(true);
                    //     __instance.starPointBirth.material.SetColor("_TintColor", color);
                    //     __instance.starPointBirth.transform.localPosition = starData.position;
                    // }
                }

            if (VFInput.rtsConfirm.pressing)
            {
                SystemDisplay.OnStarMapClick(__instance, starIndex);
            }
            if (VFInput.rtsCancel.pressing)
            {
                SystemDisplay.OnStarMapRightClick(__instance, starIndex);
            }
            return false;
            // return;
            // if (GS2.Vanilla) return;
            // if (GS2.NebulaClient && NebulaModAPI.MultiplayerSession != null) return; // use new lobby feature in multiplayer but preserve existing functionality in single player
            // GS2.Warn("*");
            // var index1 = -1;
            // var num1 = 1.7f;
            // for (var index2 = 0; index2 < __instance.starPool.Count; ++index2)
            //     if (__instance.starPool[index2].active)
            //     {
            //         var starData = __instance.starPool[index2].starData;
            //         var rectPoint = Vector2.zero;
            //         UIRoot.ScreenPointIntoRect(Camera.main.WorldToScreenPoint(starData.position), __instance.textGroup, out rectPoint);
            //         rectPoint.x += 18f;
            //         rectPoint.y += 6f;
            //         __instance.starPool[index2].nameText.rectTransform.anchoredPosition = rectPoint;
            //         var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //         var num2 = Kit.ClosestPoint2Straight(ray.origin, ray.GetPoint(300f), starData.position);
            //         var num3 = Vector3.Distance(ray.GetPoint(300f * num2), starData.position);
            //         if (num3 < (double)num1)
            //         {
            //             num1 = num3 >= __instance.starPool[index2].pointRenderer.transform.localScale.x * 0.25 ? num3 : 0.0f;
            //             index1 = index2;
            //         }
            //
            //         //GS2.Warn($"index2 = {index2} GSSettings.birthStarId:{GSSettings.birthStarId}");
            //         if (index2 == GSSettings.BirthPlanet.planetData.star.index)
            //         {
            //             var color = __instance.starColors.Evaluate(starData.color);
            //             __instance.starPointBirth.gameObject.SetActive(true);
            //             __instance.starPointBirth.material.SetColor("_TintColor", color);
            //             __instance.starPointBirth.transform.localPosition = starData.position;
            //         }
            //     }
            //
            // var pressing = VFInput.rtsConfirm.pressing;
            // var flag1 = !string.IsNullOrEmpty(__instance.clickText);
            // for (var index2 = 0; index2 < __instance.starPool.Count; ++index2)
            // {
            //     var flag2 = __instance.starPool[index2].active && index2 == index1;
            //     __instance.starPool[index2].nameText.gameObject.SetActive(flag2);
            //
            //     if (flag2)
            //     {
            //         // GS2.Log("0");
            //         if (pressing && !GS2.GetGSStar(__instance.starPool[index1].starData).Decorative)
            //         {
            //             if (GS2.ActiveGenerator.Config.enableStarSelector)
            //             {
            //                 GS2.ActiveGenerator.Generate(GSSettings.StarCount, __instance.starPool[index1].starData);
            //                 __instance.galaxyData = GS2.ProcessGalaxy(GS2.gameDesc, true);
            //                 __instance.OnGalaxyDataReset();
            //             }
            //             else
            //             {
            //                 __instance.starPool[index2].nameText.text = __instance.starPool[index2].textContent + "\r\n" + __instance.clickText.Translate();
            //             }
            //         }
            //
            //         // GS2.Log(__instance.starPool[index1].starData.name + " - " +
            //         //         __instance.starPool[index2].starData.name);
            //         var sd = __instance.starPool[index2]?.starData;
            //         // GS2.Log("1");
            //         if (__instance.starPool[index2]?.nameText?.text != null && !GS2.GetGSStar(__instance.starPool[index1].starData).Decorative) __instance.starPool[index2].nameText.text = $"{__instance.starPool[index2].textContent}\r\n{Utils.GetStarDetail(sd)}";
            //
            //         // $"{__instance.starPool[index2].textContent}\r\n{"Gas Giants".Translate()}:{Utils.GetStarDataGasCount(sd)}\r\n{"Planets".Translate()}:{Utils.GetStarDataTelluricCount(sd)}\r\n{"Moons".Translate()}:{Utils.GetStarDataMoonCount(sd)}";
            //         // GS2.Log("2");
            //         // GS2.Log($"{sd?.planetCount}");
            //         if (GS2.GetGSStar(__instance.starPool[index1].starData).Decorative) __instance.starPool[index2].nameText.rectTransform.gameObject.SetActive(false);
            //         else __instance.starPool[index2].nameText.rectTransform.sizeDelta = new Vector2(__instance.starPool[index2].nameText.preferredWidth, __instance.starPool[index2].nameText.preferredHeight);
            //     }
            //     else if (!flag2 & flag1)
            //     {
            //         __instance.starPool[index2].nameText.text = __instance.starPool[index2].textContent;
            //         __instance.starPool[index2].nameText.rectTransform.sizeDelta = new Vector2(__instance.starPool[index2].nameText.preferredWidth, __instance.starPool[index2].nameText.preferredHeight);
            //     }
            // }
            //
            // var flag3 = index1 >= 0 && __instance.starPool[index1].active;
            // __instance.starPointSelection.gameObject.SetActive(flag3);
            // __instance.starPool[GSSettings.BirthPlanet.planetData.star.index].nameText.gameObject.SetActive(true);
            // if (!flag3) return;
            //
            // var starData1 = __instance.starPool[index1].starData;
            // var color1 = __instance.starColors.Evaluate(starData1.color);
            // if (starData1.type == EStarType.NeutronStar)
            //     color1 = __instance.neutronStarColor;
            // else if (starData1.type == EStarType.WhiteDwarf)
            //     color1 = __instance.whiteDwarfColor;
            // else if (starData1.type == EStarType.BlackHole) color1 = __instance.blackholeColor;
            //
            // var num4 = 1.2f;
            // if (starData1.type == EStarType.GiantStar)
            //     num4 = 3f;
            // else if (starData1.type == EStarType.WhiteDwarf)
            //     num4 = 0.6f;
            // else if (starData1.type == EStarType.NeutronStar)
            //     num4 = 0.6f;
            // else if (starData1.type == EStarType.BlackHole) num4 = 0.8f;
            //
            // __instance.starPointSelection.material.SetColor("_TintColor", color1);
            // __instance.starPointSelection.transform.localPosition = starData1.position;
            // __instance.starPointSelection.transform.localScale = Vector3.one * (float)(num4 * 0.600000023841858 + 0.600000023841858);
        }
    }
}