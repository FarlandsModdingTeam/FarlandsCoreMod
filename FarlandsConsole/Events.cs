using FarlandsCoreMod.Attributes;
using HarmonyLib;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarlandsCoreMod.FarlandsConsole
{
    [Patcher]
    public static class Events
    {
        // dialog.portrair.*

        [HarmonyPatch(typeof(Sequencer), "HandleSetPortraitInternally")]
        [HarmonyPostfix]
        public static void DialogPortraitX(string commandName, string[] args)
        {
            Manager.ExecuteEvent("dialogue.portrait.*");
            Manager.ExecuteEvent($"dialogue.portrait.{args[1]}");
        }
    }
}
