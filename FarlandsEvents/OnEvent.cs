using PixelCrushers.DialogueSystem.Articy.Articy_1_4;
using PixelCrushers.DialogueSystem.SequencerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.FarlandsEvents
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OnEvent : Attribute
    {
        public string eventString;
        public OnEvent() { }
        public OnEvent(string eventString) => this.eventString = eventString;

        public static void LoadAll(Assembly assembly)
        {
            Debug.Log("Loadding Events from: " + assembly.FullName);
            assembly
               .GetTypes()
               .Where(t => t.GetCustomAttributes(typeof(OnEvent), false).Length > 0)
               .ToList()
               .ForEach(t =>
               {
                    t.GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Where(m => m.GetCustomAttributes(typeof(OnEvent), false).Length > 0).ToList()
                    .ForEach(m =>
                    {
                        Debug.Log("Loadding Method: " + m.Name);

                        var ev = m.GetCustomAttribute <OnEvent>().eventString;
                        if (EventsManager.onEvents.ContainsKey(ev))
                            EventsManager.onEvents[ev].Add(() => m.Invoke(null, []));
                        else EventsManager.onEvents.Add(ev, [() => m.Invoke(null, [])]);
                    });
               });
        }

    }
}
