using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace GalacticScale
{
    public class PatchOnStationComponent
    {
        // [HarmonyDebug]
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(StationComponent), "InternalTickRemote")]
        public static IEnumerable<CodeInstruction> BuildTool_Click_DeterminePreviews_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var codeMatcher = new CodeMatcher(instructions, il).MatchForward(false, 
                new CodeMatch(op => op.opcode == OpCodes.Ldc_I4_S && op.OperandIs(10)));
            
            if (codeMatcher.IsInvalid)
            {
                GS2.Error("InternalTickRemote Transpiler Failed");
                return instructions;
            }
            instructions = codeMatcher.Repeat( z => z
                    .Set(OpCodes.Ldc_I4_S, 99))
                
                .InstructionEnumeration();
            return instructions;
            return new CodeMatcher(instructions, il).MatchForward(false, 
                new CodeMatch(op => op.opcode == OpCodes.Ldc_I4_S && (int)op.operand == 10)).Repeat( z => z
            .SetAndAdvance(OpCodes.Ldc_I4_S, 99))
                 
            .InstructionEnumeration();
            
            
        }
        // public static bool InternalTickRemote(StationComponent __instance, PlanetFactory factory, int timeGene, double dt, float shipSailSpeed, float shipWarpSpeed, int shipCarries, StationComponent[] gStationPool, AstroPose[] astroPoses, VectorLF3 relativePos, Quaternion relativeRot, bool starmap, int[] consumeRegister)
        // {
        //     var canWarp = shipWarpSpeed > shipSailSpeed + 1.0;
        //     __instance.warperFree = DSPGame.IsMenuDemo;
        //     if (__instance.warperCount < __instance.warperMaxCount)
        //         lock (__instance.storage)
        //         {
        //             for (var index = 0; index < __instance.storage.Length; ++index)
        //                 if (__instance.storage[index].itemId == 1210 && __instance.storage[index].count > 0)
        //                 {
        //                     ++__instance.warperCount;
        //                     var num = (int)(__instance.storage[index].inc / (double)__instance.storage[index].count + 0.5);
        //                     --__instance.storage[index].count;
        //                     __instance.storage[index].inc -= num;
        //                     break;
        //                 }
        //         }
        //
        //     var num1 = 0;
        //     var num2 = 0;
        //     var num3 = 0;
        //     var num4 = 0;
        //     var num5 = 0;
        //     var num6 = 0;
        //     var num7 = 0;
        //     var num8 = 0;
        //     var num9 = 0;
        //     var num10 = 0;
        //     var num11 = 0;
        //     var num12 = 0;
        //     var num13 = 0;
        //     var num14 = 0;
        //     var num15 = 0;
        //     var num16 = 0;
        //     if (timeGene == __instance.gene)
        //     {
        //         ++__instance._tmp_iter_remote;
        //         if (__instance.remotePairCount > 0 && __instance.idleShipCount > 0)
        //         {
        //             __instance.remotePairProcess %= __instance.remotePairCount;
        //             var remotePairProcess1 = __instance.remotePairProcess;
        //             do
        //             {
        //                 var num17 = (shipCarries - 1) * __instance.deliveryShips / 100;
        //                 var remotePair1 = __instance.remotePairs[__instance.remotePairProcess];
        //                 if (remotePair1.supplyId == __instance.gid)
        //                     lock (__instance.storage)
        //                     {
        //                         num1 = __instance.storage[remotePair1.supplyIndex].max;
        //                         num2 = __instance.storage[remotePair1.supplyIndex].count;
        //                         num3 = __instance.storage[remotePair1.supplyIndex].inc;
        //                         num4 = __instance.storage[remotePair1.supplyIndex].remoteSupplyCount;
        //                         num5 = __instance.storage[remotePair1.supplyIndex].totalSupplyCount;
        //                         num6 = __instance.storage[remotePair1.supplyIndex].itemId;
        //                     }
        //
        //                 if (remotePair1.supplyId == __instance.gid && num1 <= num17)
        //                     num17 = num1 - 1;
        //                 if (num17 < 0)
        //                     num17 = 0;
        //                 if (remotePair1.supplyId == __instance.gid && num2 > num17 && num4 > num17 && num5 > num17)
        //                 {
        //                     var stationComponent = gStationPool[remotePair1.demandId];
        //                     if (stationComponent != null)
        //                     {
        //                         var trip = (astroPoses[__instance.planetId].uPos - astroPoses[stationComponent.planetId].uPos).magnitude + astroPoses[__instance.planetId].uRadius + astroPoses[stationComponent.planetId].uRadius;
        //                         var flag1 = trip < __instance.tripRangeShips;
        //                         var flag2 = trip >= __instance.warpEnableDist;
        //                         if (__instance.warperNecessary & flag2 && (__instance.warperCount < 2 || !canWarp))
        //                             flag1 = false;
        //                         if (flag1)
        //                             lock (stationComponent.storage)
        //                             {
        //                                 num10 = stationComponent.storage[remotePair1.demandIndex].remoteDemandCount;
        //                                 num11 = stationComponent.storage[remotePair1.demandIndex].totalDemandCount;
        //                             }
        //
        //                         if (flag1 && num10 > 0 && num11 > 0)
        //                         {
        //                             var num18 = __instance.CalcTripEnergyCost(trip, shipSailSpeed, canWarp);
        //                             if (__instance.energy >= num18)
        //                             {
        //                                 var num19 = shipCarries < num2 ? shipCarries : num2;
        //                                 var num20 = (int)(num3 / (double)num2 * num19 + 0.5);
        //                                 var index = __instance.QueryIdleShip(__instance.nextShipIndex);
        //                                 if (index >= 0)
        //                                 {
        //                                     __instance.nextShipIndex = (index + 1) % __instance.workShipDatas.Length;
        //                                     __instance.workShipDatas[__instance.workShipCount].stage = -2;
        //                                     __instance.workShipDatas[__instance.workShipCount].planetA = __instance.planetId;
        //                                     __instance.workShipDatas[__instance.workShipCount].planetB = stationComponent.planetId;
        //                                     __instance.workShipDatas[__instance.workShipCount].otherGId = stationComponent.gid;
        //                                     __instance.workShipDatas[__instance.workShipCount].direction = 1;
        //                                     __instance.workShipDatas[__instance.workShipCount].t = 0.0f;
        //                                     __instance.workShipDatas[__instance.workShipCount].itemId = __instance.workShipOrders[__instance.workShipCount].itemId = num6;
        //                                     __instance.workShipDatas[__instance.workShipCount].itemCount = num19;
        //                                     __instance.workShipDatas[__instance.workShipCount].inc = num20;
        //                                     __instance.workShipDatas[__instance.workShipCount].gene = __instance._tmp_iter_remote;
        //                                     __instance.workShipDatas[__instance.workShipCount].shipIndex = index;
        //                                     __instance.workShipOrders[__instance.workShipCount].otherStationGId = stationComponent.gid;
        //                                     __instance.workShipOrders[__instance.workShipCount].thisIndex = remotePair1.supplyIndex;
        //                                     __instance.workShipOrders[__instance.workShipCount].otherIndex = remotePair1.demandIndex;
        //                                     __instance.workShipOrders[__instance.workShipCount].thisOrdered = 0;
        //                                     __instance.workShipOrders[__instance.workShipCount].otherOrdered = num19;
        //                                     if (flag2)
        //                                         lock (consumeRegister)
        //                                         {
        //                                             if (__instance.warperCount >= 2)
        //                                             {
        //                                                 __instance.workShipDatas[__instance.workShipCount].warperCnt += 2;
        //                                                 __instance.warperCount -= 2;
        //                                                 consumeRegister[1210] += 2;
        //                                             }
        //                                             else if (__instance.warperCount >= 1)
        //                                             {
        //                                                 ++__instance.workShipDatas[__instance.workShipCount].warperCnt;
        //                                                 --__instance.warperCount;
        //                                                 ++consumeRegister[1210];
        //                                             }
        //                                             else if (__instance.warperFree)
        //                                             {
        //                                                 __instance.workShipDatas[__instance.workShipCount].warperCnt += 2;
        //                                             }
        //                                         }
        //
        //                                     lock (stationComponent.storage)
        //                                     {
        //                                         stationComponent.storage[remotePair1.demandIndex].remoteOrder += num19;
        //                                     }
        //
        //                                     ++__instance.workShipCount;
        //                                     --__instance.idleShipCount;
        //                                     __instance.IdleShipGetToWork(index);
        //                                     lock (__instance.storage)
        //                                     {
        //                                         __instance.storage[remotePair1.supplyIndex].count -= num19;
        //                                         __instance.storage[remotePair1.supplyIndex].inc -= num20;
        //                                     }
        //
        //                                     __instance.energy -= num18;
        //                                 }
        //                             }
        //
        //                             break;
        //                         }
        //                     }
        //                 }
        //
        //                 if (remotePair1.demandId == __instance.gid)
        //                     lock (__instance.storage)
        //                     {
        //                         num7 = __instance.storage[remotePair1.demandIndex].remoteDemandCount;
        //                         num8 = __instance.storage[remotePair1.demandIndex].totalDemandCount;
        //                     }
        //
        //                 if (remotePair1.demandId == __instance.gid && num7 > 0 && num8 > 0)
        //                 {
        //                     var stationComponent = gStationPool[remotePair1.supplyId];
        //                     if (stationComponent != null)
        //                     {
        //                         var trip = (astroPoses[__instance.planetId].uPos - astroPoses[stationComponent.planetId].uPos).magnitude + astroPoses[__instance.planetId].uRadius + astroPoses[stationComponent.planetId].uRadius;
        //                         var flag1 = trip < __instance.tripRangeShips;
        //                         if (flag1 && !__instance.includeOrbitCollector && stationComponent.isCollector)
        //                             flag1 = false;
        //                         var flag2 = trip >= __instance.warpEnableDist;
        //                         if (__instance.warperNecessary & flag2 && (__instance.warperCount < 2 || !canWarp))
        //                             flag1 = false;
        //                         lock (stationComponent.storage)
        //                         {
        //                             num12 = stationComponent.storage[remotePair1.supplyIndex].max;
        //                             num13 = stationComponent.storage[remotePair1.supplyIndex].count;
        //                             num14 = stationComponent.storage[remotePair1.supplyIndex].inc;
        //                             num15 = stationComponent.storage[remotePair1.supplyIndex].remoteSupplyCount;
        //                             num16 = stationComponent.storage[remotePair1.supplyIndex].totalSupplyCount;
        //                         }
        //
        //                         if (num12 <= num17)
        //                             num17 = num12 - 1;
        //                         if (num17 < 0)
        //                             num17 = 0;
        //                         if (flag1 && num13 > num17 && num15 > num17 && num16 > num17)
        //                         {
        //                             var num18 = __instance.CalcTripEnergyCost(trip, shipSailSpeed, canWarp);
        //                             if (!stationComponent.isCollector && !stationComponent.isVeinCollector)
        //                             {
        //                                 var flag3 = false;
        //                                 __instance.remotePairProcess %= __instance.remotePairCount;
        //                                 var num19 = __instance.remotePairProcess + 1;
        //                                 var remotePairProcess2 = __instance.remotePairProcess;
        //                                 var index1 = num19 % __instance.remotePairCount;
        //                                 do
        //                                 {
        //                                     var remotePair2 = __instance.remotePairs[index1];
        //                                     if (remotePair2.supplyId == __instance.gid && remotePair2.demandId == stationComponent.gid)
        //                                         lock (__instance.storage)
        //                                         {
        //                                             num2 = __instance.storage[remotePair2.supplyIndex].count;
        //                                             num3 = __instance.storage[remotePair2.supplyIndex].inc;
        //                                             num4 = __instance.storage[remotePair2.supplyIndex].remoteSupplyCount;
        //                                             num5 = __instance.storage[remotePair2.supplyIndex].totalSupplyCount;
        //                                             num6 = __instance.storage[remotePair2.supplyIndex].itemId;
        //                                         }
        //
        //                                     if (remotePair2.supplyId == __instance.gid && remotePair2.demandId == stationComponent.gid)
        //                                         lock (stationComponent.storage)
        //                                         {
        //                                             num10 = stationComponent.storage[remotePair2.demandIndex].remoteDemandCount;
        //                                             num11 = stationComponent.storage[remotePair2.demandIndex].totalDemandCount;
        //                                         }
        //
        //                                     if (remotePair2.supplyId == __instance.gid && remotePair2.demandId == stationComponent.gid && num2 >= num17 && num4 >= num17 && num5 >= num17 && num10 > 0 && num11 > 0)
        //                                     {
        //                                         if (__instance.energy >= num18)
        //                                         {
        //                                             var num20 = shipCarries < num2 ? shipCarries : num2;
        //                                             var num21 = (int)(num3 / (double)num2 * num20 + 0.5);
        //                                             var index2 = __instance.QueryIdleShip(__instance.nextShipIndex);
        //                                             if (index2 >= 0)
        //                                             {
        //                                                 __instance.nextShipIndex = (index2 + 1) % __instance.workShipDatas.Length;
        //                                                 __instance.workShipDatas[__instance.workShipCount].stage = -2;
        //                                                 __instance.workShipDatas[__instance.workShipCount].planetA = __instance.planetId;
        //                                                 __instance.workShipDatas[__instance.workShipCount].planetB = stationComponent.planetId;
        //                                                 __instance.workShipDatas[__instance.workShipCount].otherGId = stationComponent.gid;
        //                                                 __instance.workShipDatas[__instance.workShipCount].direction = 1;
        //                                                 __instance.workShipDatas[__instance.workShipCount].t = 0.0f;
        //                                                 __instance.workShipDatas[__instance.workShipCount].itemId = __instance.workShipOrders[__instance.workShipCount].itemId = num6;
        //                                                 __instance.workShipDatas[__instance.workShipCount].itemCount = num20;
        //                                                 __instance.workShipDatas[__instance.workShipCount].inc = num21;
        //                                                 __instance.workShipDatas[__instance.workShipCount].gene = __instance._tmp_iter_remote;
        //                                                 __instance.workShipDatas[__instance.workShipCount].shipIndex = index2;
        //                                                 __instance.workShipOrders[__instance.workShipCount].otherStationGId = stationComponent.gid;
        //                                                 __instance.workShipOrders[__instance.workShipCount].thisIndex = remotePair2.supplyIndex;
        //                                                 __instance.workShipOrders[__instance.workShipCount].otherIndex = remotePair2.demandIndex;
        //                                                 __instance.workShipOrders[__instance.workShipCount].thisOrdered = 0;
        //                                                 __instance.workShipOrders[__instance.workShipCount].otherOrdered = num20;
        //                                                 if (flag2)
        //                                                     lock (consumeRegister)
        //                                                     {
        //                                                         if (__instance.warperCount >= 2)
        //                                                         {
        //                                                             __instance.workShipDatas[__instance.workShipCount].warperCnt += 2;
        //                                                             __instance.warperCount -= 2;
        //                                                             consumeRegister[1210] += 2;
        //                                                         }
        //                                                         else if (__instance.warperCount >= 1)
        //                                                         {
        //                                                             ++__instance.workShipDatas[__instance.workShipCount].warperCnt;
        //                                                             --__instance.warperCount;
        //                                                             ++consumeRegister[1210];
        //                                                         }
        //                                                         else if (__instance.warperFree)
        //                                                         {
        //                                                             __instance.workShipDatas[__instance.workShipCount].warperCnt += 2;
        //                                                         }
        //                                                     }
        //
        //                                                 lock (stationComponent.storage)
        //                                                 {
        //                                                     stationComponent.storage[remotePair2.demandIndex].remoteOrder += num20;
        //                                                 }
        //
        //                                                 ++__instance.workShipCount;
        //                                                 --__instance.idleShipCount;
        //                                                 __instance.IdleShipGetToWork(index2);
        //                                                 lock (__instance.storage)
        //                                                 {
        //                                                     __instance.storage[remotePair2.supplyIndex].count -= num20;
        //                                                     __instance.storage[remotePair2.supplyIndex].inc -= num21;
        //                                                 }
        //
        //                                                 __instance.energy -= num18;
        //                                                 flag3 = true;
        //                                             }
        //                                         }
        //
        //                                         break;
        //                                     }
        //
        //                                     index1 = (index1 + 1) % __instance.remotePairCount;
        //                                 } while (remotePairProcess2 != index1);
        //
        //                                 if (flag3)
        //                                     break;
        //                             }
        //
        //                             if (__instance.energy >= num18)
        //                             {
        //                                 var index = __instance.QueryIdleShip(__instance.nextShipIndex);
        //                                 if (index >= 0)
        //                                 {
        //                                     lock (__instance.storage)
        //                                     {
        //                                         num9 = __instance.storage[remotePair1.demandIndex].itemId;
        //                                     }
        //
        //                                     __instance.nextShipIndex = (index + 1) % __instance.workShipDatas.Length;
        //                                     __instance.workShipDatas[__instance.workShipCount].stage = -2;
        //                                     __instance.workShipDatas[__instance.workShipCount].planetA = __instance.planetId;
        //                                     __instance.workShipDatas[__instance.workShipCount].planetB = stationComponent.planetId;
        //                                     __instance.workShipDatas[__instance.workShipCount].otherGId = stationComponent.gid;
        //                                     __instance.workShipDatas[__instance.workShipCount].direction = 1;
        //                                     __instance.workShipDatas[__instance.workShipCount].t = 0.0f;
        //                                     __instance.workShipDatas[__instance.workShipCount].itemId = __instance.workShipOrders[__instance.workShipCount].itemId = num9;
        //                                     __instance.workShipDatas[__instance.workShipCount].itemCount = 0;
        //                                     __instance.workShipDatas[__instance.workShipCount].inc = 0;
        //                                     __instance.workShipDatas[__instance.workShipCount].gene = __instance._tmp_iter_remote;
        //                                     __instance.workShipDatas[__instance.workShipCount].shipIndex = index;
        //                                     __instance.workShipOrders[__instance.workShipCount].otherStationGId = stationComponent.gid;
        //                                     __instance.workShipOrders[__instance.workShipCount].thisIndex = remotePair1.demandIndex;
        //                                     __instance.workShipOrders[__instance.workShipCount].otherIndex = remotePair1.supplyIndex;
        //                                     __instance.workShipOrders[__instance.workShipCount].thisOrdered = shipCarries;
        //                                     __instance.workShipOrders[__instance.workShipCount].otherOrdered = -shipCarries;
        //                                     if (flag2)
        //                                         lock (consumeRegister)
        //                                         {
        //                                             if (__instance.warperCount >= 2)
        //                                             {
        //                                                 __instance.workShipDatas[__instance.workShipCount].warperCnt += 2;
        //                                                 __instance.warperCount -= 2;
        //                                                 consumeRegister[1210] += 2;
        //                                             }
        //                                             else if (__instance.warperCount >= 1)
        //                                             {
        //                                                 ++__instance.workShipDatas[__instance.workShipCount].warperCnt;
        //                                                 --__instance.warperCount;
        //                                                 ++consumeRegister[1210];
        //                                             }
        //                                             else if (__instance.warperFree)
        //                                             {
        //                                                 __instance.workShipDatas[__instance.workShipCount].warperCnt += 2;
        //                                             }
        //                                         }
        //
        //                                     lock (__instance.storage)
        //                                     {
        //                                         __instance.storage[remotePair1.demandIndex].remoteOrder += shipCarries;
        //                                     }
        //
        //                                     lock (stationComponent.storage)
        //                                     {
        //                                         stationComponent.storage[remotePair1.supplyIndex].remoteOrder -= shipCarries;
        //                                     }
        //
        //                                     ++__instance.workShipCount;
        //                                     --__instance.idleShipCount;
        //                                     __instance.IdleShipGetToWork(index);
        //                                     __instance.energy -= num18;
        //                                 }
        //                             }
        //
        //                             break;
        //                         }
        //                     }
        //                 }
        //
        //                 ++__instance.remotePairProcess;
        //                 __instance.remotePairProcess %= __instance.remotePairCount;
        //             } while (remotePairProcess1 != __instance.remotePairProcess);
        //
        //             ++__instance.remotePairProcess;
        //             __instance.remotePairProcess %= __instance.remotePairCount;
        //         }
        //     }
        //
        //     var num22 = Mathf.Sqrt(shipSailSpeed / 600f);
        //     var f = num22;
        //     if (f > 1.0)
        //         f = Mathf.Log(f) + 1f;
        //     var astroPose1 = astroPoses[__instance.planetId];
        //     var num23 = shipSailSpeed * 0.03f * f;
        //     var num24 = shipSailSpeed * 0.12f * f;
        //     var num25 = shipSailSpeed * 0.4f * num22;
        //     var num26 = (float)(num22 * (3.0 / 500.0) + 9.99999974737875E-06);
        //     for (var destinationIndex = 0; destinationIndex < __instance.workShipCount; ++destinationIndex)
        //     {
        //         var workShipData = __instance.workShipDatas[destinationIndex];
        //         var flag1 = false;
        //         var urot = Quaternion.identity;
        //         if (workShipData.otherGId <= 0)
        //         {
        //             workShipData.direction = -1;
        //             if (workShipData.stage > 0)
        //                 workShipData.stage = 0;
        //         }
        //
        //         VectorLF3 vectorLf3_1;
        //         if (workShipData.stage < -1)
        //         {
        //             if (workShipData.direction > 0)
        //             {
        //                 workShipData.t += 0.03335f;
        //                 if (workShipData.t > 1.0)
        //                 {
        //                     workShipData.t = 0.0f;
        //                     workShipData.stage = -1;
        //                 }
        //             }
        //             else
        //             {
        //                 workShipData.t -= 0.03335f;
        //                 if (workShipData.t < 0.0)
        //                 {
        //                     workShipData.t = 0.0f;
        //                     __instance.AddItem(workShipData.itemId, workShipData.itemCount, workShipData.inc);
        //                     factory.NotifyShipDelivery(workShipData.planetB, gStationPool[workShipData.otherGId], workShipData.planetA, __instance, workShipData.itemId, workShipData.itemCount);
        //                     if (__instance.workShipOrders[destinationIndex].itemId > 0)
        //                     {
        //                         lock (__instance.storage)
        //                         {
        //                             if (__instance.storage[__instance.workShipOrders[destinationIndex].thisIndex].itemId == __instance.workShipOrders[destinationIndex].itemId)
        //                                 __instance.storage[__instance.workShipOrders[destinationIndex].thisIndex].remoteOrder -= __instance.workShipOrders[destinationIndex].thisOrdered;
        //                         }
        //
        //                         __instance.workShipOrders[destinationIndex].ClearThis();
        //                     }
        //
        //                     Array.Copy(__instance.workShipDatas, destinationIndex + 1, __instance.workShipDatas, destinationIndex, __instance.workShipDatas.Length - destinationIndex - 1);
        //                     Array.Copy(__instance.workShipOrders, destinationIndex + 1, __instance.workShipOrders, destinationIndex, __instance.workShipOrders.Length - destinationIndex - 1);
        //                     --__instance.workShipCount;
        //                     ++__instance.idleShipCount;
        //                     __instance.WorkShipBackToIdle(workShipData.shipIndex);
        //                     Array.Clear(__instance.workShipDatas, __instance.workShipCount, __instance.workShipDatas.Length - __instance.workShipCount);
        //                     Array.Clear(__instance.workShipOrders, __instance.workShipCount, __instance.workShipOrders.Length - __instance.workShipCount);
        //                     --destinationIndex;
        //                     continue;
        //                 }
        //             }
        //
        //             workShipData.uPos = astroPose1.uPos + Maths.QRotateLF(astroPose1.uRot, __instance.shipDiskPos[workShipData.shipIndex]);
        //             workShipData.uVel.x = 0.0f;
        //             workShipData.uVel.y = 0.0f;
        //             workShipData.uVel.z = 0.0f;
        //             workShipData.uSpeed = 0.0f;
        //             workShipData.uRot = astroPose1.uRot * __instance.shipDiskRot[workShipData.shipIndex];
        //             workShipData.uAngularVel.x = 0.0f;
        //             workShipData.uAngularVel.y = 0.0f;
        //             workShipData.uAngularVel.z = 0.0f;
        //             workShipData.uAngularSpeed = 0.0f;
        //             workShipData.pPosTemp = Vector3.zero;
        //             workShipData.pRotTemp = Quaternion.identity;
        //             __instance.shipRenderers[workShipData.shipIndex].anim.z = 0.0f;
        //         }
        //         else if (workShipData.stage == -1)
        //         {
        //             if (workShipData.direction > 0)
        //             {
        //                 workShipData.t += num26;
        //                 var num17 = workShipData.t;
        //                 if (workShipData.t > 1.0)
        //                 {
        //                     workShipData.t = 1f;
        //                     num17 = 1f;
        //                     workShipData.stage = 0;
        //                 }
        //
        //                 __instance.shipRenderers[workShipData.shipIndex].anim.z = num17;
        //                 var num18 = (3f - num17 - num17) * num17 * num17;
        //                 workShipData.uPos = astroPose1.uPos + Maths.QRotateLF(astroPose1.uRot, __instance.shipDiskPos[workShipData.shipIndex] + __instance.shipDiskPos[workShipData.shipIndex].normalized * (25f * num18));
        //                 workShipData.uRot = astroPose1.uRot * __instance.shipDiskRot[workShipData.shipIndex];
        //             }
        //             else
        //             {
        //                 workShipData.t -= num26 * 0.6666667f;
        //                 var num17 = workShipData.t;
        //                 if (workShipData.t < 0.0)
        //                 {
        //                     workShipData.t = 1f;
        //                     num17 = 0.0f;
        //                     workShipData.stage = -2;
        //                 }
        //
        //                 __instance.shipRenderers[workShipData.shipIndex].anim.z = num17;
        //                 var num18 = (3f - num17 - num17) * num17 * num17;
        //                 var vectorLf3_2 = astroPose1.uPos + Maths.QRotateLF(astroPose1.uRot, __instance.shipDiskPos[workShipData.shipIndex]);
        //                 var vectorLf3_3 = astroPose1.uPos + Maths.QRotateLF(astroPose1.uRot, workShipData.pPosTemp);
        //                 workShipData.uPos = vectorLf3_2 * (1.0 - num18) + vectorLf3_3 * num18;
        //                 workShipData.uRot = astroPose1.uRot * Quaternion.Slerp(__instance.shipDiskRot[workShipData.shipIndex], workShipData.pRotTemp, (float)(num18 * 2.0 - 1.0));
        //             }
        //
        //             workShipData.uVel.x = 0.0f;
        //             workShipData.uVel.y = 0.0f;
        //             workShipData.uVel.z = 0.0f;
        //             workShipData.uSpeed = 0.0f;
        //             workShipData.uAngularVel.x = 0.0f;
        //             workShipData.uAngularVel.y = 0.0f;
        //             workShipData.uAngularVel.z = 0.0f;
        //             workShipData.uAngularSpeed = 0.0f;
        //         }
        //         else if (workShipData.stage == 0)
        //         {
        //             var astroPose2 = astroPoses[workShipData.planetB];
        //             var vectorLf3_2 = (workShipData.direction <= 0 ? astroPose1.uPos + Maths.QRotateLF(astroPose1.uRot, __instance.shipDiskPos[workShipData.shipIndex] + __instance.shipDiskPos[workShipData.shipIndex].normalized * 25f) : astroPose2.uPos + Maths.QRotateLF(astroPose2.uRot, gStationPool[workShipData.otherGId].shipDockPos + gStationPool[workShipData.otherGId].shipDockPos.normalized * 25f)) - workShipData.uPos;
        //             var num17 = Math.Sqrt(vectorLf3_2.x * vectorLf3_2.x + vectorLf3_2.y * vectorLf3_2.y + vectorLf3_2.z * vectorLf3_2.z);
        //             var vectorLf3_3 = workShipData.direction > 0 ? astroPose1.uPos - workShipData.uPos : astroPose2.uPos - workShipData.uPos;
        //             var num18 = vectorLf3_3.x * vectorLf3_3.x + vectorLf3_3.y * vectorLf3_3.y + vectorLf3_3.z * vectorLf3_3.z;
        //             var flag2 = num18 <= astroPose1.uRadius * (double)astroPose1.uRadius * 2.25;
        //             var flag3 = false;
        //             if (num17 < 6.0)
        //             {
        //                 workShipData.t = 1f;
        //                 workShipData.stage = workShipData.direction;
        //                 flag3 = true;
        //             }
        //
        //             var num19 = 0.0f;
        //             if (canWarp)
        //             {
        //                 vectorLf3_1 = astroPose1.uPos - astroPose2.uPos;
        //                 var num20 = vectorLf3_1.magnitude * 2.0;
        //                 var num21 = shipWarpSpeed < num20 ? shipWarpSpeed : num20;
        //                 var num27 = __instance.warpEnableDist * 0.5;
        //                 if (workShipData.warpState <= 0.0)
        //                 {
        //                     workShipData.warpState = 0.0f;
        //                     if (num18 > 25000000.0 && num17 > num27 && workShipData.uSpeed >= (double)shipSailSpeed && (workShipData.warperCnt > 0 || __instance.warperFree))
        //                     {
        //                         --workShipData.warperCnt;
        //                         workShipData.warpState += (float)dt;
        //                     }
        //                 }
        //                 else
        //                 {
        //                     num19 = (float)(num21 * ((Math.Pow(1001.0, workShipData.warpState) - 1.0) / 1000.0));
        //                     var num28 = num19 * 0.0449 + 5000.0 + shipSailSpeed * 0.25;
        //                     var num29 = num17 - num28;
        //                     if (num29 < 0.0)
        //                         num29 = 0.0;
        //                     if (num17 < num28)
        //                         workShipData.warpState -= (float)(dt * 4.0);
        //                     else
        //                         workShipData.warpState += (float)dt;
        //                     if (workShipData.warpState < 0.0)
        //                         workShipData.warpState = 0.0f;
        //                     else if (workShipData.warpState > 1.0)
        //                         workShipData.warpState = 1f;
        //                     if (workShipData.warpState > 0.0)
        //                     {
        //                         num19 = (float)(num21 * ((Math.Pow(1001.0, workShipData.warpState) - 1.0) / 1000.0));
        //                         if (num19 * dt > num29)
        //                             num19 = (float)(num29 / dt * 1.01);
        //                     }
        //                 }
        //             }
        //
        //             var num30 = num17 / (workShipData.uSpeed + 0.1) * 0.382 * f;
        //             float num31;
        //             if (workShipData.warpState > 0.0)
        //             {
        //                 num31 = workShipData.uSpeed = shipSailSpeed + num19;
        //                 if (num31 > (double)shipSailSpeed)
        //                     num31 = shipSailSpeed;
        //             }
        //             else
        //             {
        //                 var num20 = workShipData.uSpeed * (float)num30 + 6f;
        //                 if (num20 > (double)shipSailSpeed)
        //                     num20 = shipSailSpeed;
        //                 var num21 = (float)(dt * (flag2 ? num23 : (double)num24));
        //                 if (workShipData.uSpeed < num20 - (double)num21)
        //                     workShipData.uSpeed += num21;
        //                 else if (workShipData.uSpeed > num20 + (double)num25)
        //                     workShipData.uSpeed -= num25;
        //                 else
        //                     workShipData.uSpeed = num20;
        //                 num31 = workShipData.uSpeed;
        //             }
        //
        //             var index1 = -1;
        //             var num32 = 0.0;
        //             var d = 1E+40;
        //             var num33 = workShipData.planetA / 100 * 100;
        //             var num34 = workShipData.planetB / 100 * 100;
        //             for (var index2 = num33; index2 < num33 + 99; ++index2)
        //             {
        //                 var uRadius = astroPoses[index2].uRadius;
        //                 if (uRadius >= 1.0)
        //                 {
        //                     var vectorLf3_4 = workShipData.uPos - astroPoses[index2].uPos;
        //                     var num20 = vectorLf3_4.x * vectorLf3_4.x + vectorLf3_4.y * vectorLf3_4.y + vectorLf3_4.z * vectorLf3_4.z;
        //                     var num21 = -(workShipData.uVel.x * vectorLf3_4.x + workShipData.uVel.y * vectorLf3_4.y + workShipData.uVel.z * vectorLf3_4.z);
        //                     if ((num21 > 0.0 || num20 < uRadius * (double)uRadius * 7.0) && num20 < d)
        //                     {
        //                         num32 = num21 < 0.0 ? 0.0 : num21;
        //                         index1 = index2;
        //                         d = num20;
        //                     }
        //                 }
        //             }
        //
        //             if (num34 != num33)
        //                 for (var index2 = num34; index2 < num34 + 99; ++index2)
        //                 {
        //                     var uRadius = astroPoses[index2].uRadius;
        //                     if (uRadius >= 1.0)
        //                     {
        //                         var vectorLf3_4 = workShipData.uPos - astroPoses[index2].uPos;
        //                         var num20 = vectorLf3_4.x * vectorLf3_4.x + vectorLf3_4.y * vectorLf3_4.y + vectorLf3_4.z * vectorLf3_4.z;
        //                         var num21 = -(workShipData.uVel.x * vectorLf3_4.x + workShipData.uVel.y * vectorLf3_4.y + workShipData.uVel.z * vectorLf3_4.z);
        //                         if ((num21 > 0.0 || num20 < uRadius * (double)uRadius * 7.0) && num20 < d)
        //                         {
        //                             num32 = num21 < 0.0 ? 0.0 : num21;
        //                             index1 = index2;
        //                             d = num20;
        //                         }
        //                     }
        //                 }
        //
        //             var zero = VectorLF3.zero;
        //             var vectorLf3_5 = VectorLF3.zero;
        //             var num35 = 0.0f;
        //             VectorLF3 vectorLf3_6 = Vector3.zero;
        //             if (index1 > 0)
        //             {
        //                 var uRadius = astroPoses[index1].uRadius;
        //                 if (index1 % 100 == 0)
        //                     uRadius *= 2.5f;
        //                 vectorLf3_1 = astroPoses[index1].uPosNext - astroPoses[index1].uPos;
        //                 var num20 = Math.Max(1.0, (vectorLf3_1.magnitude - 0.5) * 0.6);
        //                 var num21 = 1.0 + 1600.0 / uRadius;
        //                 var num27 = 1.0 + 250.0 / uRadius;
        //                 var num28 = num21 * (num20 * num20);
        //                 var num29 = index1 == workShipData.planetA || index1 == workShipData.planetB ? 1.25 : 1.5;
        //                 var num36 = Math.Sqrt(d);
        //                 var num37 = uRadius / num36 * 1.6 - 0.1;
        //                 if (num37 > 1.0)
        //                     num37 = 1.0;
        //                 else if (num37 < 0.0)
        //                     num37 = 0.0;
        //                 var num38 = num36 - uRadius * 0.82;
        //                 if (num38 < 1.0)
        //                     num38 = 1.0;
        //                 var num39 = (num31 - 6.0) / (num38 * f) * 0.6 - 0.01;
        //                 if (num39 > 1.5)
        //                     num39 = 1.5;
        //                 else if (num39 < 0.0)
        //                     num39 = 0.0;
        //                 var vectorLf3_4 = workShipData.uPos + (VectorLF3)workShipData.uVel * num32 - astroPoses[index1].uPos;
        //                 var num40 = vectorLf3_4.magnitude / uRadius;
        //                 if (num40 < num29)
        //                 {
        //                     var num41 = (num40 - 1.0) / (num29 - 1.0);
        //                     if (num41 < 0.0)
        //                         num41 = 0.0;
        //                     var num42 = 1.0 - num41 * num41;
        //                     vectorLf3_5 = vectorLf3_4.normalized * (num39 * num39 * num42 * 2.0 * (1.0 - workShipData.warpState));
        //                 }
        //
        //                 var v1 = workShipData.uPos - astroPoses[index1].uPos;
        //                 var vectorLf3_7 = new VectorLF3(v1.x / num36, v1.y / num36, v1.z / num36);
        //                 zero += vectorLf3_7 * num37;
        //                 num35 = (float)num37;
        //                 var num43 = num36 / uRadius;
        //                 var num44 = num43 * num43;
        //                 var num45 = (num28 - num44) / (num28 - num27);
        //                 if (num45 > 1.0)
        //                     num45 = 1.0;
        //                 else if (num45 < 0.0)
        //                     num45 = 0.0;
        //                 if (num45 > 0.0)
        //                 {
        //                     var v2 = Maths.QInvRotateLF(astroPoses[index1].uRot, v1);
        //                     var vectorLf3_8 = Maths.QRotateLF(astroPoses[index1].uRotNext, v2) + astroPoses[index1].uPosNext;
        //                     var num41 = (3.0 - num45 - num45) * num45 * num45;
        //                     var uPos = workShipData.uPos;
        //                     vectorLf3_6 = (vectorLf3_8 - uPos) * num41;
        //                 }
        //             }
        //
        //             Vector3 up;
        //             workShipData.uRot.ForwardUp(out workShipData.uVel, out up);
        //             var lhs = up * (1f - num35) + (Vector3)zero * num35;
        //             var vector3_1 = lhs - Vector3.Dot(lhs, workShipData.uVel) * workShipData.uVel;
        //             vector3_1.Normalize();
        //             Vector3 vector3_2 = vectorLf3_2.normalized + vectorLf3_5;
        //             var vector3_3 = Vector3.Cross(workShipData.uVel, vector3_2);
        //             var num46 = (float)(workShipData.uVel.x * (double)vector3_2.x + workShipData.uVel.y * (double)vector3_2.y + workShipData.uVel.z * (double)vector3_2.z);
        //             var vector3_4 = Vector3.Cross(up, vector3_1);
        //             var num47 = up.x * (double)vector3_1.x + up.y * (double)vector3_1.y + up.z * (double)vector3_1.z;
        //             if (num46 < 0.0)
        //                 vector3_3 = vector3_3.normalized;
        //             if (num47 < 0.0)
        //                 vector3_4 = vector3_4.normalized;
        //             var num48 = num30 < 3.0 ? (float)((3.25 - num30) * 4.0) : (float)(num31 / (double)shipSailSpeed * (flag2 ? 0.200000002980232 : 1.0));
        //             vector3_3 = vector3_3 * num48 + vector3_4 * 2f;
        //             var vector3_5 = vector3_3 - workShipData.uAngularVel;
        //             var num49 = vector3_5.sqrMagnitude < 0.100000001490116 ? 1f : 0.05f;
        //             workShipData.uAngularVel += vector3_5 * num49;
        //             var num50 = workShipData.uSpeed * dt;
        //             workShipData.uPos.x = workShipData.uPos.x + workShipData.uVel.x * num50 + vectorLf3_6.x;
        //             workShipData.uPos.y = workShipData.uPos.y + workShipData.uVel.y * num50 + vectorLf3_6.y;
        //             workShipData.uPos.z = workShipData.uPos.z + workShipData.uVel.z * num50 + vectorLf3_6.z;
        //             var normalized1 = workShipData.uAngularVel.normalized;
        //             var num51 = workShipData.uAngularVel.magnitude * dt * 0.5;
        //             var w = (float)Math.Cos(num51);
        //             var num52 = (float)Math.Sin(num51);
        //             var quaternion = new Quaternion(normalized1.x * num52, normalized1.y * num52, normalized1.z * num52, w);
        //             workShipData.uRot = quaternion * workShipData.uRot;
        //             if (workShipData.warpState > 0.0)
        //             {
        //                 var t = workShipData.warpState * workShipData.warpState * workShipData.warpState;
        //                 workShipData.uRot = Quaternion.Slerp(workShipData.uRot, Quaternion.LookRotation(vector3_2, vector3_1), t);
        //                 workShipData.uAngularVel *= 1f - t;
        //             }
        //
        //             if (num17 < 100.0)
        //             {
        //                 var num20 = (float)(1.0 - num17 / 100.0);
        //                 var num21 = (3f - num20 - num20) * num20 * num20;
        //                 var t = num21 * num21;
        //                 if (workShipData.direction > 0)
        //                 {
        //                     urot = Quaternion.Slerp(workShipData.uRot, astroPose2.uRot * (gStationPool[workShipData.otherGId].shipDockRot * new Quaternion(0.7071068f, 0.0f, 0.0f, -0.7071068f)), t);
        //                 }
        //                 else
        //                 {
        //                     vectorLf3_1 = workShipData.uPos - astroPose1.uPos;
        //                     Vector3 normalized2 = vectorLf3_1.normalized;
        //                     var normalized3 = (workShipData.uVel - Vector3.Dot(workShipData.uVel, normalized2) * normalized2).normalized;
        //                     urot = Quaternion.Slerp(workShipData.uRot, Quaternion.LookRotation(normalized3, normalized2), t);
        //                 }
        //
        //                 flag1 = true;
        //             }
        //
        //             if (flag3)
        //             {
        //                 workShipData.uRot = urot;
        //                 if (workShipData.direction > 0)
        //                 {
        //                     workShipData.pPosTemp = Maths.QInvRotateLF(astroPose2.uRot, workShipData.uPos - astroPose2.uPos);
        //                     workShipData.pRotTemp = Quaternion.Inverse(astroPose2.uRot) * workShipData.uRot;
        //                 }
        //                 else
        //                 {
        //                     workShipData.pPosTemp = Maths.QInvRotateLF(astroPose1.uRot, workShipData.uPos - astroPose1.uPos);
        //                     workShipData.pRotTemp = Quaternion.Inverse(astroPose1.uRot) * workShipData.uRot;
        //                 }
        //
        //                 urot = Quaternion.identity;
        //                 flag1 = false;
        //             }
        //
        //             if (__instance.shipRenderers[workShipData.shipIndex].anim.z > 1.0)
        //                 __instance.shipRenderers[workShipData.shipIndex].anim.z -= (float)dt * 0.3f;
        //             else
        //                 __instance.shipRenderers[workShipData.shipIndex].anim.z = 1f;
        //             __instance.shipRenderers[workShipData.shipIndex].anim.w = workShipData.warpState;
        //         }
        //         else if (workShipData.stage == 1)
        //         {
        //             var astroPose2 = astroPoses[workShipData.planetB];
        //             float num17;
        //             if (workShipData.direction > 0)
        //             {
        //                 workShipData.t -= num26 * 0.6666667f;
        //                 var num18 = workShipData.t;
        //                 if (workShipData.t < 0.0)
        //                 {
        //                     workShipData.t = 1f;
        //                     num18 = 0.0f;
        //                     workShipData.stage = 2;
        //                 }
        //
        //                 num17 = (3f - num18 - num18) * num18 * num18;
        //                 var num19 = num17 * 2f;
        //                 var num20 = (float)(num17 * 2.0 - 1.0);
        //                 var vectorLf3_2 = astroPose2.uPos + Maths.QRotateLF(astroPose2.uRot, gStationPool[workShipData.otherGId].shipDockPos + gStationPool[workShipData.otherGId].shipDockPos.normalized * 7.27f);
        //                 if (num17 > 0.5)
        //                 {
        //                     var vectorLf3_3 = astroPose2.uPos + Maths.QRotateLF(astroPose2.uRot, workShipData.pPosTemp);
        //                     workShipData.uPos = vectorLf3_2 * (1.0 - num20) + vectorLf3_3 * num20;
        //                     workShipData.uRot = astroPose2.uRot * Quaternion.Slerp(gStationPool[workShipData.otherGId].shipDockRot * new Quaternion(0.7071068f, 0.0f, 0.0f, -0.7071068f), workShipData.pRotTemp, (float)(num20 * 1.5 - 0.5));
        //                 }
        //                 else
        //                 {
        //                     var vectorLf3_3 = astroPose2.uPos + Maths.QRotateLF(astroPose2.uRot, gStationPool[workShipData.otherGId].shipDockPos + gStationPool[workShipData.otherGId].shipDockPos.normalized * -14.4f);
        //                     workShipData.uPos = vectorLf3_3 * (1.0 - num19) + vectorLf3_2 * num19;
        //                     workShipData.uRot = astroPose2.uRot * (gStationPool[workShipData.otherGId].shipDockRot * new Quaternion(0.7071068f, 0.0f, 0.0f, -0.7071068f));
        //                 }
        //             }
        //             else
        //             {
        //                 workShipData.t += num26;
        //                 var num18 = workShipData.t;
        //                 if (workShipData.t > 1.0)
        //                 {
        //                     workShipData.t = 1f;
        //                     num18 = 1f;
        //                     workShipData.stage = 0;
        //                 }
        //
        //                 num17 = (3f - num18 - num18) * num18 * num18;
        //                 workShipData.uPos = astroPose2.uPos + Maths.QRotateLF(astroPose2.uRot, gStationPool[workShipData.otherGId].shipDockPos + gStationPool[workShipData.otherGId].shipDockPos.normalized * (float)(39.4000015258789 * num17 - 14.3999996185303));
        //                 workShipData.uRot = astroPose2.uRot * (gStationPool[workShipData.otherGId].shipDockRot * new Quaternion(0.7071068f, 0.0f, 0.0f, -0.7071068f));
        //             }
        //
        //             workShipData.uVel.x = 0.0f;
        //             workShipData.uVel.y = 0.0f;
        //             workShipData.uVel.z = 0.0f;
        //             workShipData.uSpeed = 0.0f;
        //             workShipData.uAngularVel.x = 0.0f;
        //             workShipData.uAngularVel.y = 0.0f;
        //             workShipData.uAngularVel.z = 0.0f;
        //             workShipData.uAngularSpeed = 0.0f;
        //             __instance.shipRenderers[workShipData.shipIndex].anim.z = (float)(num17 * 1.70000004768372 - 0.699999988079071);
        //         }
        //         else
        //         {
        //             if (workShipData.direction > 0)
        //             {
        //                 workShipData.t -= 0.0334f;
        //                 if (workShipData.t < 0.0)
        //                 {
        //                     workShipData.t = 0.0f;
        //                     var dstStation = gStationPool[workShipData.otherGId];
        //                     var storage = dstStation.storage;
        //                     vectorLf3_1 = astroPoses[workShipData.planetA].uPos - astroPoses[workShipData.planetB].uPos;
        //                     if (vectorLf3_1.sqrMagnitude > __instance.warpEnableDist * __instance.warpEnableDist && workShipData.warperCnt == 0 && dstStation.warperCount > 0)
        //                         lock (consumeRegister)
        //                         {
        //                             ++workShipData.warperCnt;
        //                             --dstStation.warperCount;
        //                             ++consumeRegister[1210];
        //                         }
        //
        //                     if (workShipData.itemCount > 0)
        //                     {
        //                         dstStation.AddItem(workShipData.itemId, workShipData.itemCount, workShipData.inc);
        //                         factory.NotifyShipDelivery(workShipData.planetA, __instance, workShipData.planetB, dstStation, workShipData.itemId, workShipData.itemCount);
        //                         workShipData.itemCount = 0;
        //                         workShipData.inc = 0;
        //                         if (__instance.workShipOrders[destinationIndex].otherStationGId > 0)
        //                         {
        //                             lock (storage)
        //                             {
        //                                 if (storage[__instance.workShipOrders[destinationIndex].otherIndex].itemId == __instance.workShipOrders[destinationIndex].itemId)
        //                                     storage[__instance.workShipOrders[destinationIndex].otherIndex].remoteOrder -= __instance.workShipOrders[destinationIndex].otherOrdered;
        //                             }
        //
        //                             __instance.workShipOrders[destinationIndex].ClearOther();
        //                         }
        //
        //                         if (__instance.remotePairCount > 0)
        //                         {
        //                             __instance.remotePairProcess %= __instance.remotePairCount;
        //                             var remotePairProcess = __instance.remotePairProcess;
        //                             var index = __instance.remotePairProcess;
        //                             do
        //                             {
        //                                 var remotePair = __instance.remotePairs[index];
        //                                 if (remotePair.demandId == __instance.gid && remotePair.supplyId == dstStation.gid)
        //                                     lock (__instance.storage)
        //                                     {
        //                                         num7 = __instance.storage[remotePair.demandIndex].remoteDemandCount;
        //                                         num8 = __instance.storage[remotePair.demandIndex].totalDemandCount;
        //                                         num9 = __instance.storage[remotePair.demandIndex].itemId;
        //                                     }
        //
        //                                 if (remotePair.demandId == __instance.gid && remotePair.supplyId == dstStation.gid)
        //                                     lock (storage)
        //                                     {
        //                                         num13 = storage[remotePair.supplyIndex].count;
        //                                         num14 = storage[remotePair.supplyIndex].inc;
        //                                         num15 = storage[remotePair.supplyIndex].remoteSupplyCount;
        //                                         num16 = storage[remotePair.supplyIndex].totalSupplyCount;
        //                                     }
        //
        //                                 if (remotePair.demandId == __instance.gid && remotePair.supplyId == dstStation.gid && num7 > 0 && num8 > 0 && num13 >= shipCarries && num15 >= shipCarries && num16 >= shipCarries)
        //                                 {
        //                                     var num17 = shipCarries < num13 ? shipCarries : num13;
        //                                     var num18 = (int)(num14 / (double)num13 * num17 + 0.5);
        //                                     workShipData.itemId = __instance.workShipOrders[destinationIndex].itemId = num9;
        //                                     workShipData.itemCount = num17;
        //                                     workShipData.inc = num18;
        //                                     lock (storage)
        //                                     {
        //                                         storage[remotePair.supplyIndex].count -= num17;
        //                                         storage[remotePair.supplyIndex].inc -= num18;
        //                                     }
        //
        //                                     __instance.workShipOrders[destinationIndex].otherStationGId = dstStation.gid;
        //                                     __instance.workShipOrders[destinationIndex].thisIndex = remotePair.demandIndex;
        //                                     __instance.workShipOrders[destinationIndex].otherIndex = remotePair.supplyIndex;
        //                                     __instance.workShipOrders[destinationIndex].thisOrdered = num17;
        //                                     __instance.workShipOrders[destinationIndex].otherOrdered = 0;
        //                                     lock (__instance.storage)
        //                                     {
        //                                         __instance.storage[remotePair.demandIndex].remoteOrder += num17;
        //                                         break;
        //                                     }
        //                                 }
        //
        //                                 index = (index + 1) % __instance.remotePairCount;
        //                             } while (remotePairProcess != index);
        //                         }
        //                     }
        //                     else
        //                     {
        //                         var itemId = workShipData.itemId;
        //                         var count = shipCarries;
        //                         int inc;
        //                         dstStation.TakeItem(ref itemId, ref count, out inc);
        //                         workShipData.itemCount = count;
        //                         workShipData.inc = inc;
        //                         if (__instance.workShipOrders[destinationIndex].otherStationGId > 0)
        //                         {
        //                             lock (storage)
        //                             {
        //                                 if (storage[__instance.workShipOrders[destinationIndex].otherIndex].itemId == __instance.workShipOrders[destinationIndex].itemId)
        //                                     storage[__instance.workShipOrders[destinationIndex].otherIndex].remoteOrder -= __instance.workShipOrders[destinationIndex].otherOrdered;
        //                             }
        //
        //                             __instance.workShipOrders[destinationIndex].ClearOther();
        //                         }
        //
        //                         lock (__instance.storage)
        //                         {
        //                             if (__instance.storage[__instance.workShipOrders[destinationIndex].thisIndex].itemId == __instance.workShipOrders[destinationIndex].itemId)
        //                                 if (__instance.workShipOrders[destinationIndex].thisOrdered != count)
        //                                 {
        //                                     var num17 = count - __instance.workShipOrders[destinationIndex].thisOrdered;
        //                                     __instance.storage[__instance.workShipOrders[destinationIndex].thisIndex].remoteOrder += num17;
        //                                     __instance.workShipOrders[destinationIndex].thisOrdered += num17;
        //                                 }
        //                         }
        //                     }
        //
        //                     workShipData.direction = -1;
        //                 }
        //             }
        //             else
        //             {
        //                 workShipData.t += 0.0334f;
        //                 if (workShipData.t > 1.0)
        //                 {
        //                     workShipData.t = 0.0f;
        //                     workShipData.stage = 1;
        //                 }
        //             }
        //
        //             var astroPose2 = astroPoses[workShipData.planetB];
        //             workShipData.uPos = astroPose2.uPos + Maths.QRotateLF(astroPose2.uRot, gStationPool[workShipData.otherGId].shipDockPos + gStationPool[workShipData.otherGId].shipDockPos.normalized * -14.4f);
        //             workShipData.uVel.x = 0.0f;
        //             workShipData.uVel.y = 0.0f;
        //             workShipData.uVel.z = 0.0f;
        //             workShipData.uSpeed = 0.0f;
        //             workShipData.uRot = astroPose2.uRot * (gStationPool[workShipData.otherGId].shipDockRot * new Quaternion(0.7071068f, 0.0f, 0.0f, -0.7071068f));
        //             workShipData.uAngularVel.x = 0.0f;
        //             workShipData.uAngularVel.y = 0.0f;
        //             workShipData.uAngularVel.z = 0.0f;
        //             workShipData.uAngularSpeed = 0.0f;
        //             workShipData.pPosTemp = Vector3.zero;
        //             workShipData.pRotTemp = Quaternion.identity;
        //             __instance.shipRenderers[workShipData.shipIndex].anim.z = 0.0f;
        //         }
        //
        //         __instance.workShipDatas[destinationIndex] = workShipData;
        //         if (flag1)
        //         {
        //             __instance.shipRenderers[workShipData.shipIndex].SetPose(workShipData.uPos, urot, relativePos, relativeRot, workShipData.uVel * workShipData.uSpeed, workShipData.itemCount > 0 ? workShipData.itemId : 0);
        //             if (starmap)
        //             {
        //                 ref var local = ref __instance.shipUIRenderers[workShipData.shipIndex];
        //                 var uPos = workShipData.uPos;
        //                 var _urot = urot;
        //                 vectorLf3_1 = astroPoses[workShipData.planetA].uPos - astroPoses[workShipData.planetB].uPos;
        //                 var magnitude = vectorLf3_1.magnitude;
        //                 double uSpeed = workShipData.uSpeed;
        //                 var _itemId = workShipData.itemCount > 0 ? workShipData.itemId : 0;
        //                 local.SetPose(uPos, _urot, (float)magnitude, (float)uSpeed, _itemId);
        //             }
        //         }
        //         else
        //         {
        //             __instance.shipRenderers[workShipData.shipIndex].SetPose(workShipData.uPos, workShipData.uRot, relativePos, relativeRot, workShipData.uVel * workShipData.uSpeed, workShipData.itemCount > 0 ? workShipData.itemId : 0);
        //             if (starmap)
        //             {
        //                 ref var local = ref __instance.shipUIRenderers[workShipData.shipIndex];
        //                 var uPos = workShipData.uPos;
        //                 var uRot = workShipData.uRot;
        //                 vectorLf3_1 = astroPoses[workShipData.planetA].uPos - astroPoses[workShipData.planetB].uPos;
        //                 var magnitude = vectorLf3_1.magnitude;
        //                 double uSpeed = workShipData.uSpeed;
        //                 var _itemId = workShipData.itemCount > 0 ? workShipData.itemId : 0;
        //                 local.SetPose(uPos, uRot, (float)magnitude, (float)uSpeed, _itemId);
        //             }
        //         }
        //
        //         if (__instance.shipRenderers[workShipData.shipIndex].anim.z < 0.0)
        //             __instance.shipRenderers[workShipData.shipIndex].anim.z = 0.0f;
        //     }
        //
        //     __instance.ShipRenderersOnTick(astroPoses, relativePos, relativeRot);
        //     return false;
        // }
        // Strategy: find where  replace all ldc.i4.s 10 instructions with dynamic references to the relevant star's planetCount
        //
        // GameMain.galaxy.PlanetById(int) returns null if not a planet, otherwise PlanetData
        /* 0x0002D43D 7E05130004   */ // IL_000D: callVirt    class GalaxyData GameMain::get_galaxy
        // IL_xxxx: See below for how we find the right planet ID
        // IL_xxxx: callVirt class PlanetData GalaxyData::PlanetById(int)
        //
        // From there, PlanetData.star.planetCount gets what we need.
        //
        // Finding the planetID:
        // First, the C# code at this time:
        //   int num43 = shipData.planetA / 100 * 100; //this is later passed to astroPoses[i] and it basically represents a planet ID.
        //   int num44 = shipData.planetB / 100 * 100; //this is later passed to astroPoses[i] and it basically represents a planet ID.
        //	 for (int k = num43; k<num43 + 10; k++) {...}
        //   if (num44 != num43) {
        //     for (int l = num44; l<num44 + 10; l++) {...}}
        //
        // Note that "basically represents" is important - astroPoses is a zero-indexed array, but IDs start at 1.
        //
        // For loops are kind of backwards in CIL (compared to C# anyhow). Here's original IL for the planet IDs and prep...
        /* 0x0002EFFD 1221         */ // IL_1BCD: ldloca.s V_33                  // shipData.
        /* 0x0002EFFF 7B28050004   */ // IL_1BCF: ldfld int32 ShipData::planetA  // load planetA ID from prior
        /* 0x0002F004 1F64         */ // IL_1BD4: ldc.i4.s  100                  // load 100
        /* 0x0002F006 5B           */ // IL_1BD6: div                            // divide planetA ID by 100
        /* 0x0002F007 1F64         */ // IL_1BD7: ldc.i4.s  100                  // load 100
        /* 0x0002F009 5A           */ // IL_1BD9: mul                            // multiply result by 100
        /* 0x0002F00A 1340         */ // IL_1BDA: stloc.s V_64                   // store into a variable (!)
        /* 0x0002F00C 1221         */ // IL_1BDC: ldloca.s V_33                  // shipData. again
        /* 0x0002F00E 7B29050004   */ // IL_1BDE: ldfld int32 ShipData::planetB  // load planetB ID from prior
        /* 0x0002F013 1F64         */ // IL_1BE3: ldc.i4.s  100                  // load 100
        /* 0x0002F015 5B           */ // IL_1BE5: div                            // divide planetB ID by 100
        /* 0x0002F016 1F64         */ // IL_1BE6: ldc.i4.s  100                  // load 100
        /* 0x0002F018 5A           */ // IL_1BE8: mul                            // multiply result by 100
        /* 0x0002F019 1341         */ // IL_1BE9: stloc.s V_65                   // store into a different variable (!!)
        /* 0x0002F01B 1140         */
        // IL_1BEB: ldloc.s V_64                   // loop prep - load the planetA ID stored prior
        /* 0x0002F01D 1342         */
        // IL_1BED: stloc.s V_66                   // loop prep - save that ID into a new temp variable (i)
        /* 0x0002F01F 380E010000   */
        // IL_1BEF: br IL_1D02                     //skip to the for loop's limit checking line
        // ...                                     //skipping the code inside the loop
        /* 0x0002F132 1142         */ // IL_1D02: ldloc.s   V_66                 // load in the loop variable
        /* 0x0002F134 1140         */ // IL_1D04: ldloc.s   V_64                 // load planet A's ID
        /* 0x0002F136 1F0A         */ // IL_1D06: ldc.i4.s  10                   // load the value 10
        /* 0x0002F138 58           */
        // IL_1D08: add                            // add the last two loads - planet A's ID + 10
        /* 0x0002F139 3FE6FEFFFF   */
        // IL_1D09: blt       IL_1BF4              // skip back to the actual loop code if the result is less than the loop variable loaded before it
        //
        // Unfortunately we can't guarantee that V_64 and V_65 are consistently going to be the variables we need, especially between patches and with other mods.
        // But, thankfully, we know that the line immediately before ldc.i4.s 10 is the variable we want to refer to.
        // [HarmonyTranspiler]
        // [HarmonyPatch(typeof(StationComponent), "InternalTickRemote")]
        // public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        // {
        //     var codes = new List<CodeInstruction>(instructions);
        //     for (var i = 0; i < codes.Count; i++)
        //         if (codes[i].opcode == OpCodes.Ldc_I4_S && codes[i].OperandIs(10))
        //         {
        //             var newInstructions = new List<CodeInstruction>();
        //             newInstructions.Add(new CodeInstruction(codes[i - 1])); //The line before adding 10 is the line which loads in the planet ID we care about, so copy it
        //             newInstructions.Add(Transpilers.EmitDelegate<Del>(bodyID =>
        //                 // We add 1 to the body ID because it was originally an index in the astroPoses array but we need the actual ID of it.
        //                 // We add 1 to the planet count because the loop is <, not <=
        //                 GameMain.galaxy.PlanetById(bodyID + 1).star.planetCount + 1));
        //             codes.RemoveAt(i); // Remove the original loading of 10
        //             codes.InsertRange(i, newInstructions); //Instead, load the count of planets around the target star (plus one)
        //         }
        //
        //     return codes.AsEnumerable();
        // }

        // private delegate int Del(int bodyID);
    }
}