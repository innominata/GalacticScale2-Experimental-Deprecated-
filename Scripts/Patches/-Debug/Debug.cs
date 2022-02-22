using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace GalacticScale

{
    public class PatchOnWhatever
    {
        public static bool pressSpamProtector = false;




        /*
        if (flag2 && flag)
        {
            if (pressing)
            {
                this.starPool[j].nameText.text = this.starPool[j].textContent + "\r\n" + this.clickText.Translate();
            }
            this.starPool[j].nameText.rectTransform.sizeDelta = new Vector2(this.starPool[j].nameText.preferredWidth, this.starPool[j].nameText.preferredHeight);
        }
        to
        if (flag2 && pressing)
        {
            own logic
        }
        NOTE: the game does not use UIVirtualStarmap.clickText yet so the original logic would never be called anyways.
        Also change iteration over stars to start at 0 instead of 1 to also have a detailed solar system view of the default starting system
        By default the game always marks the first star as birth point, but as we can change that we also need to adapt the code for the visualisation
         */
        // [HarmonyTranspiler]
        // [HarmonyPatch(typeof(UIVirtualStarmap), "_OnLateUpdate")]
        // public static IEnumerable<CodeInstruction> _OnLateUpdate_Transpiler(IEnumerable<CodeInstruction> instructions)
        // {
        //     CodeMatcher matcher = new CodeMatcher(instructions).MatchForward(true, new CodeMatch(OpCodes.Ldarg_0), new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(UIVirtualStarmap), "starPool")), new CodeMatch(OpCodes.Ldloc_S), new CodeMatch(i => i.opcode == OpCodes.Callvirt && ((MethodInfo)i.operand).Name == "get_Item"), new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(UIVirtualStarmap.StarNode), "nameText")), new CodeMatch(i => i.opcode == OpCodes.Callvirt && ((MethodInfo)i.operand).Name == "get_gameObject"), new CodeMatch(OpCodes.Ldloc_S), new CodeMatch(i => i.opcode == OpCodes.Callvirt && ((MethodInfo)i.operand).Name == "SetActive"), new CodeMatch(OpCodes.Ldloc_S));
        //     if (matcher.IsInvalid)
        //     {
        //         GS2.Warn("UIVirtualStarmap transpiler could not find injection point, not patching!");
        //         return instructions;
        //     }
        //
        //     matcher.Advance(1).SetAndAdvance(OpCodes.Ldloc_2, null) // change 'if (flag2 && flag)' to 'if (flag2 && pressing)'
        //         .Advance(2);
        //
        //     // now remove original logic in this if(){}
        //     for (int i = 0; i < 39; i++)
        //     {
        //         matcher.SetAndAdvance(OpCodes.Nop, null);
        //     }
        //
        //     // add own logic
        //     matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0), new CodeInstruction(OpCodes.Ldloc_S, 12), Transpilers.EmitDelegate<ShowSolarsystemDetails>((UIVirtualStarmap starmap, int starIndex) =>
        //     {
        //         SystemDisplay.OnUpdate(starmap, starIndex);
        //         
        //     }));
        //
        //     // change for loop to start at 0 instead of 1
        //     matcher.Start();
        //     matcher.MatchForward(true, new CodeMatch(OpCodes.Stloc_2), new CodeMatch(OpCodes.Ldarg_0), new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(UIVirtualStarmap), "clickText")), new CodeMatch(i => i.opcode == OpCodes.Call && ((MethodInfo)i.operand).Name == "IsNullOrEmpty"), new CodeMatch(OpCodes.Ldc_I4_0), new CodeMatch(OpCodes.Ceq), new CodeMatch(OpCodes.Stloc_3)).Advance(1).SetInstruction(new CodeInstruction(OpCodes.Ldc_I4_0));
        //
        //     // mark the correct star as birth point
        //     matcher.Start();
        //     matcher.MatchForward(true, new CodeMatch(OpCodes.Ldc_R4), new CodeMatch(OpCodes.Stloc_1), new CodeMatch(OpCodes.Br), new CodeMatch(OpCodes.Ldloc_S), new CodeMatch(OpCodes.Stloc_1), new CodeMatch(OpCodes.Ldloc_S), new CodeMatch(OpCodes.Stloc_0), new CodeMatch(OpCodes.Ldloc_S), new CodeMatch(OpCodes.Brtrue)).Advance(-1).SetAndAdvance(OpCodes.Nop, null).InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0)).InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_S, 5)).Insert(Transpilers.EmitDelegate<IsBirthStar>((UIVirtualStarmap starmap, int starIndex) => { return starmap.starPool[starIndex].starData.id != starmap._galaxyData.birthStarId && starmap.starPool[starIndex].starData.id != starmap._galaxyData.birthPlanetId; }));
        //
        //     // listen for general mouse clicks to deselect planet / solar system
        //     // matcher.Start();
        //     // matcher.MatchForward(true, new CodeMatch(OpCodes.Br), new CodeMatch(OpCodes.Ldarg_0), new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(UIVirtualStarmap), "starPool")), new CodeMatch(OpCodes.Ldloc_S), new CodeMatch(i => i.opcode == OpCodes.Callvirt && ((MethodInfo)i.operand).Name == "get_Item"), new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(UIVirtualStarmap.StarNode), "active")), new CodeMatch(OpCodes.Brfalse), new CodeMatch(OpCodes.Ldloc_S), new CodeMatch(OpCodes.Ldloc_0), new CodeMatch(OpCodes.Ceq)).Advance(3).InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0)).InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_0)).InsertAndAdvance(Transpilers.EmitDelegate<TrackPlayerClick>((UIVirtualStarmap starmap, int starIndex) =>
        //     // {
        //     //     // SystemDisplay.OnStarMapClick(starmap, starIndex);
        //     // }
        //         // ));
        //
        //     return matcher.InstructionEnumeration();
        // }

        

        

        

        // mark correct star with the '>> Mission start <<' text
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(UIVirtualStarmap), "OnGalaxyDataReset")]
        public static IEnumerable<CodeInstruction> OnGalaxyDataReset_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher matcher = new CodeMatcher(instructions).MatchForward(true, new CodeMatch(i => i.opcode == OpCodes.Call && ((MethodInfo)i.operand).Name == "Translate"), new CodeMatch(i => i.opcode == OpCodes.Call && ((MethodInfo)i.operand).Name == "Concat"), new CodeMatch(OpCodes.Stloc_S), new CodeMatch(OpCodes.Ldloc_S), new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(StarData), "index")), new CodeMatch(OpCodes.Brtrue)).Advance(-1).SetAndAdvance(OpCodes.Ldarg_0, null).InsertAndAdvance(Transpilers.EmitDelegate<IsBirthStar2>((starData, starmap) =>
            {
                GS2.Warn("OnGalaxyDataReset");
                if (starData == null || starmap == null)
                {
                    return true;
                }

                return starData.index != ((SystemDisplay.customBirthStar != -1) ? SystemDisplay.customBirthStar - 1 : starmap._galaxyData.birthStarId - 1);
            }));
            return matcher.InstructionEnumeration();
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIGalaxySelect), "_OnUpdate")]
        public static void _OnUpdate_Postfix()
        {
            // as we need to load and generate planets for the detail view in the lobby, update the loading process here
            PlanetModelingManager.ModelingPlanetCoroutine();
            UIRoot.instance.uiGame.planetDetail._OnUpdate();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UIVirtualStarmap), "_OnLateUpdate")]
        public static bool _OnLateUpdate_Prefix(UIVirtualStarmap __instance)
        {
            // reset the spam protector if no press is recognized to enable solar system details again.
            if (!VFInput.rtsConfirm.pressing)
            {
                pressSpamProtector = false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UIVirtualStarmap), "OnGalaxyDataReset")]
        public static bool OnGalaxyDataReset_Prefix(UIVirtualStarmap __instance)
        {
            __instance.clickText = ""; // reset to vanilla

            foreach (UIVirtualStarmap.ConnNode connNode in __instance.connPool)
            {
                connNode.lineRenderer.positionCount = 2;
            }

            return true;
        }
        // [HarmonyPrefix, HarmonyPatch(typeof(BuildTool_PathAddon), "CheckBuildConditions")]
        // public static bool CheckBuildConditions(ref bool __result, BuildTool_PathAddon __instance)
        // {
        //     if (__instance.cursorValid)
        //     {
        //         // GS2.Warn("CursorValid");
        //         Vector3 position = __instance.player.position;
        //         float num = __instance.player.mecha.buildArea * __instance.player.mecha.buildArea;
        //         if ((__instance.handbp.lpos - position).sqrMagnitude > num)
        //         {
        //             __instance.handbp.condition = EBuildCondition.OutOfReach;
        //         }
        //     }
        //     if (__instance.handbp != null && __instance.handbp.desc.isSpraycoster)
        //     {
        //         // GS2.Warn("IsSprayCoster");
        //         Vector3 reshapeData = SpraycoaterComponent.GetReshapeData(__instance.handbp.lpos, __instance.handbp.lrot);
        //         if (Mathf.Abs(reshapeData.x) > 0.265f || Mathf.Abs(reshapeData.y) > 0.265f)
        //         {
        //             // GS2.Warn("IsSprayCosterTooSkew");
        //             __instance.handbp.condition = EBuildCondition.TooSkew;
        //         }
        //     }
        //     for (int i = 0; i < __instance.potentialBeltObjIdArray.Length; i++)
        //     {
        //         int num2 = __instance.potentialBeltObjIdArray[i];
        //         int num3 = (int)Mathf.Sign((float)num2) * (Mathf.Abs(num2) >> 4);
        //         if (num3 != 0 && __instance.GetBeltInputCount(num3) > 1 && !__instance.HasAddonConn(num3))
        //         {
        //             GS2.Warn("Coolide");
        //             __instance.handbp.condition = EBuildCondition.Collide;
        //         }
        //     }
        //     Pose[] addonAreaColPoses = __instance.handbp.desc.addonAreaColPoses;
        //     Vector3[] addonAreaSize = __instance.handbp.desc.addonAreaSize;
        //     for (int j = 0; j < __instance.potentialBeltCursor; j++)
        //     {
        //         int num4 = __instance.potentialBeltObjIdArray[j];
        //         int objId = (int)Mathf.Sign((float)num4) * (Mathf.Abs(num4) >> 4);
        //         int num5 = Mathf.Abs(num4) & 15;
        //         Vector3 b = __instance.handbp.lpos + __instance.handbp.lrot * __instance.handbp.desc.addonAreaPoses[num5].position;
        //         Quaternion b2 = __instance.handbp.lrot * addonAreaColPoses[num5].rotation;
        //         Pose objectPose = __instance.GetObjectPose(objId);
        //         bool flag;
        //         Pose beltOutputBeltPose = __instance.GetBeltOutputBeltPose(objId, out flag);
        //         bool flag2;
        //         Pose beltInputBeltPose = __instance.GetBeltInputBeltPose(objId, out flag2);
        //         bool flag3 = true;
        //         if (flag)
        //         {
        //             Vector3 normalized = (beltOutputBeltPose.position - objectPose.position).normalized;
        //             Vector3 normalized2 = objectPose.position.normalized;
        //             float num6 = Quaternion.Angle(Quaternion.LookRotation(normalized, normalized2), b2);
        //             flag3 &= (num6 < 20.5f || num6 > 159.5f);
        //             flag3 &= (Mathf.Abs(objectPose.position.magnitude - beltOutputBeltPose.position.magnitude) < 0.6f);
        //         }
        //         if (flag2)
        //         {
        //             Vector3 normalized3 = (objectPose.position - beltInputBeltPose.position).normalized;
        //             Vector3 normalized4 = objectPose.position.normalized;
        //             float num7 = Quaternion.Angle(Quaternion.LookRotation(normalized3, normalized4), b2);
        //             flag3 &= (num7 < 20.5f || num7 > 159.5f);
        //             flag3 &= (Mathf.Abs(objectPose.position.magnitude - beltInputBeltPose.position.magnitude) < 0.6f);
        //         }
        //         bool flag4 = true;
        //         Vector3 lineStart = __instance.handbp.lpos + __instance.handbp.lrot * (addonAreaColPoses[num5].position + addonAreaColPoses[num5].forward * addonAreaSize[num5].z * 2.5f);
        //         Vector3 lineEnd = __instance.handbp.lpos + __instance.handbp.lrot * (addonAreaColPoses[num5].position - addonAreaColPoses[num5].forward * addonAreaSize[num5].z * 2.5f);
        //         float num8 = Maths.DistancePointLine(objectPose.position, lineStart, lineEnd);
        //         if (Mathf.Pow((objectPose.position - b).sqrMagnitude + num8 * num8, 0.5f) < addonAreaSize[num5].z)
        //         {
        //             flag4 = false;
        //         }
        //         if (!flag3 && !flag4){
        //         
        //             GS2.Warn("Collide2");
        //             __instance.handbp.condition = EBuildCondition.Collide;
        //         }
        //     }
        //     int id = __instance.handbp.item.ID;
        //     int num9 = 1;
        //     if (__instance.tmpInhandId == id && __instance.tmpInhandCount > 0)
        //     {
        //         num9 = 1;
        //         __instance.tmpInhandCount--;
        //     }
        //     else
        //     {
        //         int num10;
        //         __instance.tmpPackage.TakeTailItems(ref id, ref num9, out num10, false);
        //     }
        //     if (num9 == 0)
        //     {
        //         __instance.handbp.condition = EBuildCondition.NotEnoughItem;
        //     }
        //     if (__instance.handbp.condition == EBuildCondition.Ok && __instance.handPrefabDesc.isSpraycoster && Mathf.Abs(Vector3.Dot(__instance.handbp.lrot.Forward(), __instance.handbp.lpos.normalized)) > 0.05f)
        //     {
        //         __instance.handbp.condition = EBuildCondition.TooSkew;
        //     }
        //     if (__instance.handbp.condition == EBuildCondition.Ok)
        //     {
        //         ColliderData[] buildColliders = __instance.handbp.desc.buildColliders;
        //         for (int k = 0; k < buildColliders.Length; k++)
        //         {
        //             ColliderData colliderData = __instance.handbp.desc.buildColliders[k];
        //             colliderData.pos = __instance.handbp.lpos + __instance.handbp.lrot * colliderData.pos;
        //             colliderData.q = __instance.handbp.lrot * colliderData.q;
        //             int mask = 428032;
        //             Array.Clear(BuildTool._tmp_cols, 0, BuildTool._tmp_cols.Length);
        //             int num11 = Physics.OverlapBoxNonAlloc(colliderData.pos, colliderData.ext, BuildTool._tmp_cols, colliderData.q, mask, QueryTriggerInteraction.Collide);
        //             if (num11 > 0)
        //             {
        //                 bool flag5 = false;
        //                 PlanetPhysics physics = __instance.player.planetData.physics;
        //                 for (int l = 0; l < num11; l++)
        //                 {
        //                     ColliderData colliderData3;
        //                     bool colliderData2 = physics.GetColliderData(BuildTool._tmp_cols[l], out colliderData3);
        //                     int num12 = 0;
        //                     if (colliderData2 && colliderData3.isForBuild)
        //                     {
        //                         if (colliderData3.objType == EObjectType.Entity)
        //                         {
        //                             num12 = colliderData3.objId;
        //                         }
        //                         else if (colliderData3.objType == EObjectType.Prebuild)
        //                         {
        //                             num12 = -colliderData3.objId;
        //                         }
        //                     }
        //                     if (!__instance.IsPotentialBeltObj(num12))
        //                     {
        //                         PrefabDesc prefabDesc = __instance.GetPrefabDesc(num12);
        //                         Collider collider = BuildTool._tmp_cols[l];
        //                         if (collider.gameObject.layer == 18)
        //                         {
        //                             BuildPreviewModel component = collider.GetComponent<BuildPreviewModel>();
        //                             if ((component != null && component.index == __instance.handbp.previewIndex) || (__instance.handbp.desc.isInserter && !component.buildPreview.desc.isInserter) || (!__instance.handbp.desc.isInserter && component.buildPreview.desc.isInserter) || (!__instance.handbp.desc.isBelt && component.buildPreview.desc.isBelt))
        //                             {
        //                                 goto IL_711;
        //                             }
        //                         }
        //                         if (prefabDesc == null || !prefabDesc.isBelt || (!__instance.IsPotentialBeltConn(num12) && !__instance.HasAddonConn(num12))) // <- last two are true...so no potentialbeltconn or hasaddonconn
        //                         {
        //                             // GS2.Warn(num12.ToString());
        //                             // GS2.Warn($"Prefabdesc == null:{prefabDesc == null} || !prefabDesc.isBelt:{!prefabDesc.isBelt} || ispotentialbeltconn(num12):{!__instance.IsPotentialBeltConn(num12)} && HasAddonConn: {!__instance.HasAddonConn(num12)}");
        //                             GS2.WarnJson(__instance.factory.GetEntityData(num12));
        //                             //flag5 = true;
        //                         }
        //                     }
        //                     IL_711:;
        //                 }
        //                 if (flag5)
        //                 {
        //                     GS2.Warn("Collide3");
        //                     __instance.handbp.condition = EBuildCondition.Collide;
        //                     break;
        //                 }
        //             }
        //         }
        //         if (__instance.planet != null)
        //         {
        //             float num13 = 64f;
        //             float num14 = __instance.actionBuild.history.buildMaxHeight + 0.5f + __instance.planet.realRadius;
        //             if (__instance.handbp.lpos.sqrMagnitude > num14 * num14)
        //             {
        //                 if (__instance.actionBuild.history.buildMaxHeight + 0.5f <= num13)
        //                 {
        //                     BuildModel model = __instance.actionBuild.model;
        //                     model.cursorText = model.cursorText + "垂直建造可升级".Translate() + "\r\n";
        //                 }
        //                 __instance.handbp.condition = EBuildCondition.OutOfVerticalConstructionHeight;
        //             }
        //         }
        //         bool flag6 = false;
        //         Vector3 b3 = Vector3.zero;
        //         if (__instance.planet.id == __instance.planet.galaxy.birthPlanetId && __instance.actionBuild.history.SpaceCapsuleExist())
        //         {
        //             b3 = __instance.planet.birthPoint;
        //             flag6 = true;
        //         }
        //         if (flag6 && __instance.handbp.lpos.magnitude < __instance.planet.realRadius + 3f)
        //         {
        //             Vector3 ext = __instance.handbp.desc.buildCollider.ext;
        //             float num15 = Mathf.Sqrt(ext.x * ext.x + ext.z * ext.z);
        //             if ((__instance.handbp.lpos - b3).magnitude - num15 < 3.7f)
        //             {
        //                 GS2.Warn("Collide4");
        //                 __instance.handbp.condition = EBuildCondition.Collide;
        //             }
        //         }
        //     }
        //     if ((__instance.handPrefabDesc.isMonitor && __instance.handBpParams[0] == 0) || (__instance.handPrefabDesc.isSpraycoster && __instance.handBpParams[0] == 0 && __instance.handBpParams[1] == 0))
        //     {
        //         int m = 0;
        //         while (m < __instance.handbp.desc.landPoints.Length)
        //         {
        //             Vector3 point = __instance.handbp.desc.landPoints[m];
        //             point.y = 0f;
        //             Vector3 vector = __instance.handbp.lpos + __instance.handbp.lrot * point;
        //             Vector3 normalized5 = vector.normalized;
        //             vector += normalized5 * 3f;
        //             Vector3 direction = -normalized5;
        //             float num16 = 0f;
        //             RaycastHit raycastHit;
        //             if (!Physics.Raycast(new Ray(vector, direction), out raycastHit, 100f, 8704, QueryTriggerInteraction.Collide))
        //             {
        //                 GS2.Warn("Collide5");
        //                 goto IL_9E8;
        //             }
        //             num16 = raycastHit.distance;
        //             if (raycastHit.point.magnitude - __instance.factory.planet.realRadius >= -0.3f)
        //             {
        //                 GS2.Warn("Collide6");
        //                 goto IL_9E8;
        //             }
        //             __instance.handbp.condition = EBuildCondition.NeedGround;
        //             IL_A2D:
        //             m++;
        //             continue;
        //             IL_9E8:
        //             float num17;
        //             if (Physics.Raycast(new Ray(vector, direction), out raycastHit, 100f, 16, QueryTriggerInteraction.Collide))
        //             {
        //                 GS2.Warn("Collide7");
        //                 num17 = raycastHit.distance;
        //             }
        //             else
        //             {
        //                 num17 = 1000f;
        //             }
        //             if (num16 - num17 > 0.27f)
        //             {
        //                 __instance.handbp.condition = EBuildCondition.NeedGround;
        //                 goto IL_A2D;
        //             }
        //             goto IL_A2D;
        //         }
        //     }
        //     if (__instance.handbp.condition != EBuildCondition.Ok)
        //     {
        //         __instance.actionBuild.model.cursorState = -1;
        //         BuildModel model2 = __instance.actionBuild.model;
        //         model2.cursorText += BuildPreview.GetConditionText(__instance.handbp.condition);
        //     }
        //     return __instance.handbp.condition == EBuildCondition.Ok;
        // }
        // // [HarmonyPrefix, HarmonyPatch(typeof(PhysicsScene), "Internal_Raycast")]
        // // public static bool Internal_Raycast(PhysicsScene physicsScene, Ray ray, float maxDistance, ref RaycastHit hit, int layerMask, QueryTriggerInteraction queryTriggerInteraction, ref bool __result)
        // // {
        // //     if (maxDistance == 200f && GameMain.localPlanet != null) maxDistance = GameMain.localPlanet.realRadius;
        // //     __result = PhysicsScene.Internal_Raycast_Injected(ref physicsScene, ref ray, maxDistance, ref hit, layerMask, queryTriggerInteraction);
        // //     return false;
        // // }
        // // [HarmonyPrefix, HarmonyPatch(typeof(Ray), "GetPoint")]
        // // public static bool GetPoint(Ray __instance, float distance, ref Vector3 __result)
        // // {
        // //     if (distance == 200f && GameMain.localPlanet != null) distance = GameMain.localPlanet.realRadius;
        // //     __result = __instance.m_Origin + __instance.m_Direction * distance;
        // //     return false;
        // // }
        // [HarmonyPostfix, HarmonyPatch(typeof(BuildPreview), "ResetAll")]
        // public static void ResetAll(BuildPreview __instance)
        // {
        //     __instance.genNearColliderArea2 = GameMain.localPlanet.realRadius;
        // }
        //
        // [HarmonyPostfix, HarmonyPatch(typeof(BuildPreview), "ResetInfos")]
        // public static void ResetInfos(BuildPreview __instance)
        // {
        //     __instance.genNearColliderArea2 = GameMain.localPlanet.realRadius;
        // }
        //
        // [HarmonyPrefix, HarmonyPatch(typeof(BuildingParameters), "GenerateBuildPreviews")]
        // public static bool GenerateBuildPreviews(BuildingParameters __instance, List<BuildPreview> bplist)
        // {
        //     bplist.Clear();
        //     if (__instance.type == BuildingType.None)
        //     {
        //         return false;
        //     }
        //     ItemProto itemProto = LDB.items.Select(__instance.itemId);
        //     ModelProto modelProto = LDB.models.Select(__instance.modelIndex);
        //     if (itemProto == null || modelProto == null)
        //     {
        //         return false;
        //     }
        //     PrefabDesc prefabDesc = modelProto.prefabDesc;
        //     if (!itemProto.IsEntity)
        //     {
        //         return false;
        //     }
        //     BuildPreview buildPreview = new BuildPreview();
        //     buildPreview.item = itemProto;
        //     buildPreview.desc = prefabDesc;
        //     buildPreview.lpos = Vector3.zero;
        //     buildPreview.lrot = Quaternion.identity;
        //     buildPreview.lpos2 = Vector3.zero;
        //     buildPreview.lrot2 = buildPreview.lrot;
        //     buildPreview.needModel = (prefabDesc.lodCount > 0 && prefabDesc.lodMeshes[0] != null);
        //     buildPreview.recipeId = __instance.recipeId;
        //     buildPreview.filterId = __instance.filterId;
        //     __instance.ToParamsArray(ref buildPreview.parameters, ref buildPreview.paramCount);
        //     bplist.Add(buildPreview);
        //     if (__instance.inserterItemIds != null)
        //     {
        //         int num = __instance.inserterItemIds.Length;
        //         for (int i = 0; i < num; i++)
        //         {
        //             ItemProto itemProto2 = LDB.items.Select(__instance.inserterItemIds[i]);
        //             if (itemProto2 != null && __instance.inserterLengths[i] != 0)
        //             {
        //                 bool flag = __instance.inserterLengths[i] < 0;
        //                 int num2 = __instance.inserterLengths[i];
        //                 if (num2 < 0)
        //                 {
        //                     num2 = -num2;
        //                 }
        //                 float d = 1f * (float)num2;
        //                 Pose pose = prefabDesc.slotPoses[i];
        //                 Vector3 vec = flag ? pose.right : (-pose.right);
        //                 BuildPreview buildPreview2 = new BuildPreview();
        //                 buildPreview2.item = itemProto2;
        //                 buildPreview2.desc = itemProto2.prefabDesc;
        //                 buildPreview2.lpos = pose.position;
        //                 buildPreview2.lrot = pose.rotation * Quaternion.Euler(0f, (float)(flag ? 0 : 180), 0f);
        //                 buildPreview2.lpos2 = pose.position + pose.forward * d;
        //                 var r = GameMain.localPlanet.realRadius + 0.2f;
        //                 VectorLF3 lhs = (buildPreview2.lpos2 + new Vector3(0.0f, r, 0.0f)).normalized * r;
        //                 buildPreview2.lpos2 = lhs - new VectorLF3(0.0, r, 0.0);
        //                 buildPreview2.lrot2 = Quaternion.LookRotation(VectorLF3.Cross(vec, lhs.normalized), lhs.normalized);
        //                 if (!flag)
        //                 {
        //                     Vector3 lpos = buildPreview2.lpos;
        //                     buildPreview2.lpos = buildPreview2.lpos2;
        //                     buildPreview2.lpos2 = lpos;
        //                     Quaternion lrot = buildPreview2.lrot;
        //                     buildPreview2.lrot = buildPreview2.lrot2;
        //                     buildPreview2.lrot2 = lrot;
        //                 }
        //                 buildPreview2.genNearColliderArea2 = 0f;
        //                 if (flag)
        //                 {
        //                     buildPreview2.input = buildPreview;
        //                     buildPreview2.inputFromSlot = i;
        //                     buildPreview2.inputToSlot = 1;
        //                 }
        //                 else
        //                 {
        //                     buildPreview2.output = buildPreview;
        //                     buildPreview2.outputToSlot = i;
        //                     buildPreview2.outputFromSlot = 0;
        //                 }
        //                 buildPreview2.needModel = (prefabDesc.lodCount > 0 && prefabDesc.lodMeshes[0] != null);
        //                 buildPreview2.recipeId = 0;
        //                 buildPreview2.filterId = __instance.inserterFilters[i];
        //                 buildPreview2.paramCount = 1;
        //                 buildPreview2.parameters = new int[1];
        //                 buildPreview2.parameters[0] = num2;
        //                 buildPreview2.condition = EBuildCondition.NeedConn;
        //                 bplist.Add(buildPreview2);
        //             }
        //         }
        //     }
        //     return false;
        // }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlanetData), "AddHeightMapModLevel")]
        public static bool AddHeightMapModLevel(int index, int level, PlanetData __instance)
        {
            if (__instance.data.AddModLevel(index, level))
            {
                var num = __instance.precision / __instance.segment;
                var num2 = index % __instance.data.stride;
                var num3 = index / __instance.data.stride;
                var num4 = (num2 < __instance.data.substride ? 0 : 1) + (num3 < __instance.data.substride ? 0 : 2);
                var num5 = num2 % __instance.data.substride;
                var num6 = num3 % __instance.data.substride;
                var num7 = (num5 - 1) / num;
                var num8 = (num6 - 1) / num;
                var num9 = num5 / num;
                var num10 = num6 / num;
                if (num9 >= __instance.segment) num9 = __instance.segment - 1;
                if (num10 >= __instance.segment) num10 = __instance.segment - 1;
                var num11 = num4 * __instance.segment * __instance.segment;
                var num12 = num7 + num8 * __instance.segment + num11;
                var num13 = num9 + num8 * __instance.segment + num11;
                var num14 = num7 + num10 * __instance.segment + num11;
                var num15 = num9 + num10 * __instance.segment + num11;
                num12 = Mathf.Clamp(num12, 0, 99);
                num13 = Mathf.Clamp(num13, 0, 99);
                num14 = Mathf.Clamp(num14, 0, 99);
                num15 = Mathf.Clamp(num15, 0, 99);
                __instance.dirtyFlags[num12] = true;
                __instance.dirtyFlags[num13] = true;
                __instance.dirtyFlags[num14] = true;
                __instance.dirtyFlags[num15] = true;
            }

            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BuildTool_Inserter), "CheckBuildConditions")]
        public static void BuildToolInserter(BuildTool_Inserter __instance, ref bool __result)
        {
            if (__instance.buildPreviews.Count == 0) return;
            // if (__instance.buildPreviews == null) return;
            var preview = __instance.buildPreviews[0];
            // GS2.Warn(preview?.condition.ToString());

            if (__instance.planet.realRadius < 20)
                if (preview.condition == EBuildCondition.TooSkew)
                {
                    preview.condition = EBuildCondition.Ok;
                    // GS2.Warn("TooSkew");
                    __instance.cursorValid = true; // Prevent red text
                    __result = true; // Override the build condition check
                    UICursor.SetCursor(ECursor.Default); // Get rid of that ban cursor
                    __instance.actionBuild.model.cursorText = "Click to build";
                    __instance.actionBuild.model.cursorState = 0;
                }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UILoadGameWindow), "_OnOpen")]
        public static bool UILoadGameWindow_OnOpen()
        {
            GS2.Warn("Disabled Import");
            GS2.SaveOrLoadWindowOpen = true;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UILoadGameWindow), "LoadSelectedGame")]
        public static bool UILoadGameWindow_LoadSelectedGame()
        {
            GS2.Warn("Enabled Import");
            GS2.SaveOrLoadWindowOpen = false;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UILoadGameWindow), "_OnClose")]
        public static bool UILoadGameWindow_OnClose()
        {
            GS2.Warn("Enabled Import");

            GS2.SaveOrLoadWindowOpen = false;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UISaveGameWindow), "_OnOpen")]
        public static bool UISaveGameWindow_OnOpen()
        {
            GS2.Warn("Disabled Import");

            GS2.SaveOrLoadWindowOpen = true;
            return true;
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(UISaveGameWindow), "_OnClose")]
        public static bool UISaveGameWindow_OnClose()
        {
            GS2.Warn("Enabled Import");

            GS2.SaveOrLoadWindowOpen = false;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UIAchievementPanel), "LoadData")]
        public static bool LoadData(UIAchievementPanel __instance)
        {
            // __instance.unlockedEntries.Clear();
            // __instance.lockedEntries.Clear();
            // __instance.inProgressEntries.Clear();
            __instance.uiEntries.Clear();
            // foreach (KeyValuePair<int, AchievementState> keyValuePair in DSPGame.achievementSystem.achievements)
            // {
            //     if (keyValuePair.Value.unlocked)
            //     {
            //         UIAchievementEntry uiachievementEntry = UnityEngine.Object.Instantiate<UIAchievementEntry>(__instance.entryPrefab, __instance.unlockedContainerRect);
            //         uiachievementEntry._Create();
            //         uiachievementEntry._Init(null);
            //         uiachievementEntry.SetAchievementData(keyValuePair.Value.id);
            //         if (!__instance.unlockedEntries.Contains(uiachievementEntry)) __instance.unlockedEntries.Add(uiachievementEntry);
            //         uiachievementEntry.index = __instance.unlockedEntries.IndexOf(uiachievementEntry);
            //         __instance.uiEntries.Add(keyValuePair.Key, uiachievementEntry);
            //         uiachievementEntry._Open();
            //     }
            //     else if (keyValuePair.Value.targetValue > 1L && keyValuePair.Value.progressValue > 0L)
            //     {
            //         UIAchievementEntry uiachievementEntry2 = UnityEngine.Object.Instantiate<UIAchievementEntry>(__instance.entryPrefab, __instance.inProgressContainerRect);
            //         uiachievementEntry2._Create();
            //         uiachievementEntry2._Init(null);
            //         uiachievementEntry2.SetAchievementData(keyValuePair.Value.id);
            //         __instance.inProgressEntries.Add(uiachievementEntry2);
            //         uiachievementEntry2.index = __instance.inProgressEntries.IndexOf(uiachievementEntry2);
            //         __instance.uiEntries.Add(keyValuePair.Key, uiachievementEntry2);
            //         uiachievementEntry2._Open();
            //     }
            //     else
            //     {
            //         UIAchievementEntry uiachievementEntry3 = UnityEngine.Object.Instantiate<UIAchievementEntry>(__instance.entryPrefab, __instance.lockedContainerRect);
            //         uiachievementEntry3._Create();
            //         uiachievementEntry3._Init(null);
            //         uiachievementEntry3.SetAchievementData(keyValuePair.Value.id);
            //         __instance.lockedEntries.Add(uiachievementEntry3);
            //         uiachievementEntry3.index = __instance.lockedEntries.IndexOf(uiachievementEntry3);
            //         __instance.uiEntries.Add(keyValuePair.Key, uiachievementEntry3);
            //         uiachievementEntry3._Open();
            //     }
            // }
            // __instance.inProgressEntries.Sort(new AchievementProgressComparer());
            // foreach (UIAchievementEntry uiachievementEntry4 in __instance.inProgressEntries)
            // {
            //     uiachievementEntry4.index = __instance.inProgressEntries.IndexOf(uiachievementEntry4);
            // }

            return true;
        }

        // [HarmonyPrefix, HarmonyPatch(typeof(VegeRenderer), "AddInst")]
        // public static bool AddInst(VegeRenderer __instance, int __result, int objId, Vector3 pos, Quaternion rot, bool setBuffer = true)
        // {
        //     return false;
        // }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(WarningSystem), "Init")]
        public static void Init(ref WarningSystem __instance)
        {
            GS2.Warn("Warning System Initializing");
            GS2.Warn($"Star Count: {GSSettings.StarCount}");
            var planetCount = GSSettings.PlanetCount;
            GS2.Warn($"Planet Count: {planetCount}");
            GS2.Warn($"Factory Length: {__instance.gameData.factories.Length}");
            if (__instance.gameData.factories.Length > planetCount) planetCount = __instance.gameData.factories.Length;
            __instance.tmpEntityPools = new EntityData[planetCount][];
            __instance.tmpPrebuildPools = new PrebuildData[planetCount][];
            __instance.tmpSignPools = new SignData[planetCount][];
            __instance.warningCounts = new int[GameMain.galaxy.starCount * 1024];
            __instance.warningSignals = new int[GameMain.galaxy.starCount * 32];
            __instance.focusDetailCounts = new int[GameMain.galaxy.starCount * 1024];
            __instance.focusDetailSignals = new int[GameMain.galaxy.starCount * 32];
            var l = GameMain.galaxy.starCount * 400;
            __instance.astroArr = new AstroPoseR[l];
            __instance.astroBuffer = new ComputeBuffer(l, 32, ComputeBufferType.Default);
            GS2.Warn($"Pool Length: {__instance.tmpEntityPools.Length}");
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ThemeProto), "Preload")]
        public static bool Preload(ref ThemeProto __instance)
        {
            __instance.displayName = __instance.DisplayName.Translate();
            __instance.terrainMat = Utils.ResourcesLoadArray<Material>(__instance.MaterialPath + "terrain", "{0}-{1}", true);
            __instance.oceanMat = Utils.ResourcesLoadArray<Material>(__instance.MaterialPath + "ocean", "{0}-{1}", true);
            __instance.atmosMat = Utils.ResourcesLoadArray<Material>(__instance.MaterialPath + "atmosphere", "{0}-{1}", true);
            __instance.lowMat = Utils.ResourcesLoadArray<Material>(__instance.MaterialPath + "low", "{0}-{1}", true);
            __instance.thumbMat = Utils.ResourcesLoadArray<Material>(__instance.MaterialPath + "thumb", "{0}-{1}", true);
            __instance.minimapMat = Utils.ResourcesLoadArray<Material>(__instance.MaterialPath + "minimap", "{0}-{1}", true);
            __instance.ambientDesc = Utils.ResourcesLoadArray<AmbientDesc>(__instance.MaterialPath + "ambient", "{0}-{1}", true);
            __instance.ambientSfx = Utils.ResourcesLoadArray<AudioClip>(__instance.SFXPath, "{0}-{1}", true);
            if (__instance.RareSettings.Length != __instance.RareVeins.Length * 4) Debug.LogError("稀有矿物数组长度有误 " + __instance.displayName);
            return false;
        }
        // [HarmonyPrefix, HarmonyPatch(typeof(LandPlanetCountCondition), "Check")]
        // public static bool Check(ref LandPlanetCountCondition __instance, ref bool __result)
        // {
        //     int num = 0;
        //     int num2 = 0;
        //     for (int i = 0; i < __instance.landTypeArr.Length; i++)
        //     {
        //         __instance.landTypeArr[i] = 0;
        //     }
        //     StarData[] stars = __instance.gameData.galaxy.stars;
        //     for (int j = 0; j < stars.Length; j++)
        //     {
        //         if (stars[j] != null)
        //         {
        //             PlanetData[] planets = stars[j].planets;
        //             for (int k = 0; k < planets.Length; k++)
        //             {
        //                 GS2.Log($"Checking Planet {k} {planets[k].name} with theme {planets[k].theme}");
        //                 if (planets[k] != null && planets[k].factory != null && planets[k].factory.landed)
        //                 {
        //                     num++;
        //                     if (__instance.landTypeArr[planets[k].theme] == 0)
        //                     {
        //                         GS2.Log($"theme == 0");
        //                         __instance.landTypeArr[planets[k].theme] = 1;
        //                         num2++;
        //                     }
        //                 }
        //             }
        //         }
        //     }
        //     GS2.Log($"Finshed That part. Setting Refarances");
        //     __instance.trigger.SetReferances(num, num2);
        //     GS2.Log("Returning");
        //     __result = Utility.Compare(num, __instance.count1, __instance.c1) && Utility.Compare(num2, __instance.count2, __instance.c2);
        //     return false;
        // }
        //[HarmonyPrefix, HarmonyPatch(typeof(LandPlanetCountCondition), "OnCreate")]
        //public static bool OnCreate(ref LandPlanetCountCondition __instance)
        //{
        //    __instance.c1 = Utility.ToCompare(__instance.parameters["c1"]);
        //    __instance.count1 = Utility.ToInt(__instance.parameters["count1"]);
        //    __instance.c2 = Utility.ToCompare(__instance.parameters["c2"]);
        //    __instance.count2 = Utility.ToInt(__instance.parameters["count2"]);
        //    if (__instance.landTypeArr == null)
        //    {
        //        __instance.landTypeArr = new byte[512]; //Also in galaxy generation the LDB.themes array is truncated to 128, leaving 384 theme slots avail. if DSP adds more themes this will need to be tweaked
        //    }

        //    return false;
        //}
        //[HarmonyPrefix, HarmonyPatch(typeof(CommonUtils), "ResourcesLoadArray")]
        //public static bool ResourcesLoadArray<T>(ref T[] __result, string path, string format, bool emptyNull) where T : UnityEngine.Object
        //{
        //    List<T> list = new List<T>();

        //    T t = Resources.Load<T>(path);
        //    if (t == null)
        //    {
        //        GS2.Log("Resource returned null, exiting");
        //        __result = null;
        //        return false;
        //    }
        //    GS2.Log("Resource loaded");
        //    int num = 0;
        //    if (t != null)
        //    {
        //        list.Add(t);
        //        num = 1;
        //    }
        //    do
        //    {
        //        t = Resources.Load<T>(string.Format(format, path, num));
        //        if (t == null || ((num == 1 || num == 2) && list.Contains(t)))
        //        {
        //            break;
        //        }
        //        list.Add(t);
        //        num++;
        //    }
        //    while (num < 1024);
        //    if (emptyNull && list.Count == 0)
        //    {
        //        __result = null;
        //        return false;
        //    }
        //    __result = list.ToArray();
        //    return false;
        //}
        //[HarmonyPatch(typeof(WorkerThreadExecutor), "InserterPartExecute")]
        //[HarmonyPrefix]
        //public static bool InserterPartExecute(ref WorkerThreadExecutor __instance)
        //{
        //    if (__instance.inserterFactories == null) return true;
        //    for (var i=0;i<__instance.inserterFactoryCnt;i++)
        //    {

        //        if (__instance.inserterFactories[i].factorySystem == null)
        //        {
        //            Warn("Creating Factory");
        //            __instance.inserterFactories[i] = GameMain.data.GetOrCreateFactory(__instance.inserterFactories[i].planet);
        //        }
        //    }
        //    return true;
        //}
        [HarmonyPatch(typeof(UIReplicatorWindow), "OnPlusButtonClick")]
        [HarmonyPrefix]
        public static bool OnPlusButtonClick(ref UIReplicatorWindow __instance, int whatever)
        {
            // GS2.Log("Test");
            if (__instance.selectedRecipe != null)
            {
                if (!__instance.multipliers.ContainsKey(__instance.selectedRecipe.ID)) __instance.multipliers[__instance.selectedRecipe.ID] = 1;

                var num = __instance.multipliers[__instance.selectedRecipe.ID];
                if (VFInput.control) num += 10;
                else if (VFInput.shift) num += 100;
                else if (VFInput.alt) num = 999;
                else num++;
                if (num > 999) num = 999;

                __instance.multipliers[__instance.selectedRecipe.ID] = num;
                __instance.multiValueText.text = num + "x";
            }

            return false;
        }

        [HarmonyPatch(typeof(UIReplicatorWindow), "OnMinusButtonClick")]
        [HarmonyPrefix]
        public static bool OnMinusButtonClick(ref UIReplicatorWindow __instance, int whatever)
        {
            if (__instance.selectedRecipe != null)
            {
                if (!__instance.multipliers.ContainsKey(__instance.selectedRecipe.ID)) __instance.multipliers[__instance.selectedRecipe.ID] = 1;
                var num = __instance.multipliers[__instance.selectedRecipe.ID];
                if (VFInput.control) num -= 10;
                else if (VFInput.shift) num -= 100;
                else if (VFInput.alt) num = 1;
                else num--;
                if (num < 1) num = 1;
                __instance.multipliers[__instance.selectedRecipe.ID] = num;
                __instance.multiValueText.text = num + "x";
            }


            return false;
        }

        [HarmonyPatch(typeof(UIReplicatorWindow), "OnOkButtonClick")]
        [HarmonyPrefix]
        public static bool OnOkButtonClick(ref UIReplicatorWindow __instance, int whatever, bool button_enable)
        {
            // GS2.Log("Test2");
            if (__instance.selectedRecipe != null)
            {
                if (!__instance.selectedRecipe.Handcraft)
                {
                    UIRealtimeTip.Popup("该配方".Translate() + __instance.selectedRecipe.madeFromString + "生产".Translate());
                    return false;
                }

                var id = __instance.selectedRecipe.ID;
                if (!GameMain.history.RecipeUnlocked(id))
                {
                    UIRealtimeTip.Popup("配方未解锁".Translate());
                    return false;
                }

                var num = 1;
                if (__instance.multipliers.ContainsKey(id)) num = __instance.multipliers[id];

                if (num < 1)
                    num = 1;
                else if (num > 999) num = 1000;

                var num2 = __instance.mechaForge.PredictTaskCount(__instance.selectedRecipe.ID, 999);
                // GS2.Log($"{num} - {num2}");
                if (num > num2) num = num2;

                if (num == 0)
                {
                    UIRealtimeTip.Popup("材料不足".Translate());
                    return false;
                }

                if (__instance.mechaForge.AddTask(id, num) == null)
                {
                    UIRealtimeTip.Popup("材料不足".Translate());
                    return false;
                }

                GameMain.history.RegFeatureKey(1000104);
            }

            return false;
        }

        private delegate void ShowSolarsystemDetails(UIVirtualStarmap starmap, int starIndex);

        private delegate bool IsBirthStar(UIVirtualStarmap starmap, int starIndex);

        private delegate bool IsBirthStar2(StarData starData, UIVirtualStarmap starmap);

        private delegate void TrackPlayerClick(UIVirtualStarmap starmap, int starIndex);
    }
}