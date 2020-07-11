﻿using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;
using Verse.AI;

namespace ChangeCaravanLoadout.Patches
{
    [HarmonyPatch(typeof(JobDriver), "ReadyForNextToil")]
    public class PatchJobDriver_ReadyForNextToil
    {
        static void Prefix(JobDriver __instance)
        {
            var pos = __instance?.job?.targetA.Cell;
            var pawnNamespace = __instance?.pawn?.GetType()?.Namespace;
            if (pos.HasValue && pos.Value.IsValid
                && __instance is JobDriver_Mine
                && pawnNamespace == "AIRobot"
                && __instance.pawn.Map != null)
            {
                var things = pos.Value.GetThingList(__instance.pawn.Map);
                var forbidden = things
                    .Where(t => t.IsForbidden(Faction.OfPlayer))
                    .ToList();
                forbidden.ForEach(f => f.SetForbidden(false, false));
            }
        }
    }
}