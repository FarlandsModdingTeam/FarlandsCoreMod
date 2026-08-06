using Farlands.Dev;
using HarmonyLib;
using UnityEngine;
using FarlandsCoreMod.Core.Extensors;
using System.Collections.Generic;

namespace FarlandsCoreMod.Core.Debug;

public class DebugManager : AbstractManager
{
    public override string ConfigSection => "Debug";

    public List<DebugCommandBase> Commands = new();
    public List<string> PrintedMessages = new();
    public Vector2 LogScrollPos;
    public bool PendingScrollToBottom { get; private set; }

    public bool ShowLog => PrintedMessages.Count > 0;

    private const int MaxMessages = 200;

    public override void OnLoad()
    {
        RegisterCommand(new DebugCommand("clear_log", "Limpia el panel de log", "clear_log",
            () => PrintedMessages.Clear()));
    }

    public void RegisterCommand(DebugCommandBase command) => Commands.Add(command);

    public void Print(string message)
    {
        PrintedMessages.Add(message);
        if (PrintedMessages.Count > MaxMessages)
            PrintedMessages.RemoveAt(0);

        PendingScrollToBottom = true;
    }

    public void Clear()
    {
        PrintedMessages.Clear();
    }

    public void ConsumeScrollToBottom() => PendingScrollToBottom = false;
}

[HarmonyPatch(typeof(DebugController))]
public static class DebugControllerPatch
{
    private const float MaxPanelHeight = 300f;

    [HarmonyPatch("Update")]
    [HarmonyPrefix]
    public static bool UpdatePrefix(DebugController __instance)
    {
        __instance.InvokePrivateMethod("HandleConsoleToggle");
        __instance.InvokePrivateMethod("HandleEnterKey");
        __instance.InvokePrivateMethod("HandleTimeScale");
        __instance.InvokePrivateMethod("HandleSceneReload");
        if ((double)__instance.GetPrivateField<float>("feedbackTimer") <= 0.0)
            return false;
        __instance.SetPrivateField("feedbackTimer", __instance.GetPrivateField<float>("feedbackTimer") - Time.unscaledDeltaTime);
        return false;
    }

    [HarmonyPatch("Awake")]
    [HarmonyPostfix]
    public static void AddNewCommands(DebugController __instance)
    {
        var fcm = FCMPlugin.GetFCM();
        fcm.Debug.Commands.ForEach(d => __instance.commandList.Add(d));
    }

    [HarmonyPatch("HandleInput")]
    [HarmonyPrefix]
    public static bool HandleInputPrefix(DebugController __instance)
    {
        string input = __instance.GetPrivateField<string>("input");
        if (string.IsNullOrWhiteSpace(input))
            return true;

        string commandId = input.Trim().Split(' ')[0].ToLower();
        if (commandId != "help")
            return true;

        var debug = FCMPlugin.GetFCM().Debug;
        debug.PrintedMessages.Clear();
        foreach (var cmd in __instance.commandList)
        {
            var c = cmd as DebugCommandBase;
            debug.Print($"{c.commandFormat}  —  {c.commandDescription}");
        }

        return false;
    }

    [HarmonyPatch("OnGUI")]
    [HarmonyPrefix]
    public static bool OnGUIPrefix(DebugController __instance)
    {
        var debug = FCMPlugin.GetFCM().Debug;

        if (__instance.GetPrivateField<float>("feedbackTimer") > 0f)
            __instance.InvokePrivateMethod("DrawFeedback");

        bool showConsole = __instance.GetPrivateField<bool>("showConsole");
        if (!showConsole)
            return false;

        float y = 0f;
        GUIStyle style1 = __instance.InvokePrivateMethod<GUIStyle>("MonoStyle", 28);
        GUIStyle style2 = __instance.InvokePrivateMethod<GUIStyle>("MonoStyle", 20);
        int lineHeight = style2.fontSize + 8;

        if (debug.ShowLog)
        {
            Vector2 scroll = debug.LogScrollPos;

            // Solo forzamos el scroll al fondo una vez por print, no en cada frame,
            // para no impedir que el usuario pueda subir a mano a leer atrás.
            if (debug.PendingScrollToBottom)
            {
                scroll.y = float.MaxValue; // IMGUI lo clampa automáticamente al máximo válido
                debug.ConsumeScrollToBottom();
            }

            y = DrawPanel(0f, style2, lineHeight, debug.PrintedMessages, ref scroll, MaxPanelHeight);
            debug.LogScrollPos = scroll;
        }

        GUI.Box(new Rect(0f, y, Screen.width, 52f), "");
        GUI.backgroundColor = Color.black;

        bool inputFocused = __instance.GetPrivateField<bool>("inputFocused");
        if (!inputFocused)
            return false;

        GUI.SetNextControlName("InputTextField");
        GUI.FocusControl("InputTextField");
        string input = __instance.GetPrivateField<string>("input");
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 100f), input, style1);
        __instance.SetPrivateField("input", input);

        if (Event.current.keyCode != KeyCode.Return && Event.current.keyCode != KeyCode.KeypadEnter)
            return false;

        GUI.FocusControl(null);
        __instance.SetPrivateField("inputFocused", false);
        __instance.InvokePrivateMethod("HandleInput");
        __instance.SetPrivateField("input", "");

        return false;
    }

    private static float DrawPanel(float yStart, GUIStyle style, int lineHeight, List<string> lines, ref Vector2 scrollPos, float maxHeight)
    {
        const float padding = 10f;
        float panelHeight = Mathf.Min(lineHeight * lines.Count + padding, maxHeight);

        GUI.Box(new Rect(0f, yStart, Screen.width, panelHeight), "");
        Rect viewRect = new Rect(0f, 0f, Screen.width - 20, lineHeight * lines.Count);
        scrollPos = GUI.BeginScrollView(new Rect(0f, yStart + 5f, Screen.width, panelHeight - 10f), scrollPos, viewRect);

        for (int i = 0; i < lines.Count; i++)
        {
            GUI.Label(new Rect(8f, lineHeight * i, viewRect.width - 20f, lineHeight), lines[i], style);
        }

        GUI.EndScrollView();
        return yStart + panelHeight;
    }
}
