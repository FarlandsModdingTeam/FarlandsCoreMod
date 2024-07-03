using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SceneOverride: Attribute
    {
        public string SceneName;
        public Type type;
        public SceneOverride(string sceneName, Type type)
        {
            SceneName = sceneName;
            this.type = type;
        }

        [AttributeUsage(AttributeTargets.Method)]
        public class OnLoad : Attribute{}

        [AttributeUsage(AttributeTargets.Method)]
        public class NewGameObject : Attribute
        {
            public string name;
            public NewGameObject(string name) 
            {
                this.name = name;
            }
        }

        [AttributeUsage(AttributeTargets.Field)]
        public class GameObject : Attribute
        {
            public string name;
            public GameObject (string name)
            {
                this.name = name;
            }
        }
        
        public void InstantiateAllGameObjects(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"InstantiateAll: {scene.name} :: {SceneName}");
            if(scene.name != SceneName) return;

            type.GetMethods(BindingFlags.Public | BindingFlags.Static).ToList()
                .Where(x => x.GetCustomAttributes<NewGameObject>().Count() >= 1).ToList()
                .ForEach(x =>
                {
                    var str = x.GetCustomAttribute<NewGameObject>().name;
                    x.Invoke(null, [new UnityEngine.GameObject(str)]); 
                });
        }

        public void GetRequestedGameObjects(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"RequestedGO: {scene.name} :: {SceneName}");
            if(scene.name != SceneName) return;

            type.GetFields(BindingFlags.Public | BindingFlags.Static).ToList()
                .Where(x => x.GetCustomAttributes<GameObject>().Count() >= 1).ToList()
                .ForEach(x =>
                {
                    var str = x.GetCustomAttribute<GameObject>().name;
                    Debug.Log($"str: {str}");
                    
                    UnityEngine.GameObject gameObject = null;
                    
                    foreach(var go in SceneManager.GetSceneByName(this.SceneName).GetRootGameObjects())
                    {
                        if(go.name == str)
                        {
                            gameObject = go;
                            break;
                        }
                    }

                    x.SetValue(null, gameObject);
                });
        }
    }
}
