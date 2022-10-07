using System.Reflection;
using System;
using UnityEngine;
using HarmonyLib;
using TFBGames;
using System.Collections.Generic;

namespace MapMayhem {
    [HarmonyPatch(typeof(GroundChecker), "OnCollisionEnter")]
    class GroundPatcher {
        [HarmonyPrefix]
        public static bool Prefix(GroundChecker __instance, Collision collision) {

            if (__instance.ignoreRigidbodies && collision.transform.root.name == "Map")
            {
                __instance.ignoreRigidbodies = false;
            }
            return false;
        }
    }
}