using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class OnLoadScene: Attribute
    {
        public string SceneName;

        public OnLoadScene(string sceneName)
        { 
            SceneName = sceneName;
        }
        public static void onLoadScene() => onLoadScene(Assembly.GetCallingAssembly());
        public static void onLoadScene(Assembly assembly)
        {
            Debug.Log("Scenes");
            assembly
                .GetTypes()
                .ToList()
                .ForEach(type => type.GetMethods(BindingFlags.Public |BindingFlags.Static)
                    .ToList()
                    .ForEach(x => 
                    {
                        if (x.GetCustomAttributes<OnLoadScene>().Count() >= 1)
                        {
                            Debug.Log("scn");
                            Debug.Log(x.Name);
                            SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
                            {
                                if (scene.name == x.GetCustomAttribute<OnLoadScene>().SceneName)
                                {
                                    x.Invoke(null, [scene]);
                                }
                            };
                        }
                    })
                );
        }
    }
}