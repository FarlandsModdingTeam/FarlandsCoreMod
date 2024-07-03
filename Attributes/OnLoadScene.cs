using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
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
            SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => 
                Debug.Log($"scene {scene.name} loaded!");

            Debug.Log("Scenes");
            assembly
                .GetTypes()
                .ToList()
                .ForEach(type => {

                    if (type.GetCustomAttributes<SceneOverride>().Count() >= 1)
                    {
                        SceneManager.sceneLoaded += type.GetCustomAttribute<SceneOverride>().InstantiateAllGameObjects;
                    }

                    type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .ToList().ForEach(x =>
                    {
                        if (type.GetCustomAttributes<SceneOverride>().Count() >= 1)
                        {
                            if (x.GetCustomAttributes<SceneOverride.OnLoad>().Count() >= 1)
                            {
                                Debug.Log("scn");
                                Debug.Log(x.Name);

                                SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
                                {
                                    if (scene.name == type.GetCustomAttribute<SceneOverride>().SceneName)
                                    {
                                        x.Invoke(null, [scene]);
                                    }
                                };
                            }
                        }
                        else
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
                        }
                    });

                    
                }
                );
        }
    }
}