using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace LetMeSleep
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInIncompatibility("com.rinsev.SkipSleep")]
    [BepInIncompatibility("Azumatt.SleepSkip")]
    [BepInIncompatibility("com.comoyi.valheim.SleepPlease")]
    [HarmonyPatch]
    public class LetMeSleepPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = "blockchaaain.LetMeSleep";
        public const string PluginName = "LetMeSleep";
        public const string PluginVersion = "1.0.2";

        private static readonly Harmony harmony = new Harmony(PluginGUID);

        private static ConfigFile configFile = new ConfigFile(Path.Combine(BepInEx.Paths.ConfigPath, "blockchaaain.LetMeSleep.cfg"), true);
        private static ConfigEntry<double> ratio = configFile.Bind("General", "ratio", 0.5, new ConfigDescription("Fraction of players needed in bed to skip the night.", new AcceptableValueRange<double>(0.01, 1.0)));
        private static ConfigEntry<bool> message = configFile.Bind("General", "showMessage", true, "Show a chat message with the number of players currently in bed.");

        private void Awake()
        {
            harmony.PatchAll();
        }

        private void OnDestroy()
        {
            harmony.UnpatchSelf();
        }

        private static int prevInBed = 0;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Game), "EverybodyIsTryingToSleep")]
        static bool LetMeSleep(ref Game __instance, ref bool __result)
        {
            // This HarmonyPrefix method runs before Game.EverybodyIsTryingToSleep()
            // Returning false prevents the original method from running.
            // The keyword argument __result overrides the return value of EverybodyIsTryingToSleep.
            // When __result is true, the game believes everyone is in bed.
            // This method does not override the game's check for appropriate time (afternoon or night).

            // Get all players
            List<ZDO> allCharacterZdos = ZNet.instance.GetAllCharacterZDOS();

            int playerCount = allCharacterZdos.Count;

            // Return false if none
            if (playerCount == 0)
            {
                __result = false;
                return false;
            }

            // Number of players in bed
            int numInBed = allCharacterZdos.Where(zdo => zdo.GetBool("inBed")).Count();

            // Calculate current ratio of people sleeping
            double sleepRatio = Convert.ToDouble(numInBed) / allCharacterZdos.Count;

            // If showMessage is true AND anyone is in bed AND number in bed changed
            if (message.Value && numInBed > 0 && numInBed != prevInBed)
            {
                string message = String.Format("{0:d}/{1:d} ({2:p0}) asleep", numInBed, playerCount, sleepRatio);

                // Send message to everyone, e.g. "Server: 2/5 (40 %) asleep"
                Chat.instance.SendText(Talker.Type.Shout, message);
            }

            prevInBed = numInBed;

            // If the ratio reaches the threshold, return true to sleep
            if (sleepRatio >= ratio.Value)
            {
                __result = true;
                return false;
            }

            // Otherwise, result false
            __result = false;

            return false;
        }
    }
}

