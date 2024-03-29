using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
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
        public const string PluginVersion = "1.0.4";

        private static readonly Harmony harmony = new Harmony(PluginGUID);

        private static ConfigFile configFile = new ConfigFile(Path.Combine(BepInEx.Paths.ConfigPath, "blockchaaain.LetMeSleep.cfg"), true);
        private static ConfigEntry<double> ratio = configFile.Bind("General", "ratio", 0.5, new ConfigDescription("Fraction of players needed in bed to skip the night.", new AcceptableValueRange<double>(0.01, 1.0)));
        private static ConfigEntry<bool> showMessage = configFile.Bind("General", "showMessage", true, "Show a chat message with the number of players currently in bed.");

        private static new ManualLogSource Logger;

        private void Awake()
        {
            harmony.PatchAll();

            Logger = base.Logger;
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
            if (showMessage.Value && numInBed > 0 && numInBed != prevInBed)
            {
                try
                {
                    Vector3 position = Vector3.zero;

                    int talkerType = (int)Talker.Type.Shout;

                    UserInfo userInfo = UserInfo.GetLocalUser();
                    userInfo.Name = "Server";

                    string message = String.Format("{0:d}/{1:d} ({2:p0}) sleeping", numInBed, playerCount, sleepRatio);

                    string networkUserID = PrivilegeManager.GetNetworkUserId();

                    // Send chat message to everybody, e.g. "Server: 2/5 (40 %) SLEEPING"
                    ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, "ChatMessage", position, talkerType, userInfo, message, networkUserID);
                }
                catch (Exception e)
                {
                    // Warning and information
                    Logger.LogWarning("Exception while sending server message from LetMeSleep:" + Environment.NewLine + e.Message);
                    Logger.LogMessage("Consider disabling server messages in blockchaaain.LetMeSleep.cfg");
                    Logger.LogMessage("Bug reports can be submitted at: https://github.com/blockchaaain/LetMeSleep");

                    // Produce exception and warning only once
                    showMessage.Value = false;
                }
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

