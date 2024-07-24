using FarlandsCoreMod.Utiles.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

namespace FarlandsCoreMod.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    
    public class SO: Attribute
    {
        public string SceneName;
        public Type type;

        /// <summary>
        /// Modifica la escena utilizando una clase
        /// </summary>
        /// <param name="sceneName">nombre de la escena</param>
        /// <param name="type">clase a utilizar</param>
        public SO(string sceneName, Type type)
        {
            SceneName = sceneName;
            this.type = type;
        }

        [AttributeUsage(AttributeTargets.Method)]
        public class OnLoad : Attribute{}

        

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
        public class GameObject : Attribute
        {
            public string name;
            public GameObject (string name)
            {
                this.name = name;
            }

            [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
            public class New : Attribute
            {
                public string name;
                public New(string name)
                {
                    this.name = name;
                }
            }
        }

        [AttributeUsage(AttributeTargets.Field)]
        public class Component : Attribute
        {
            public Type Type;

            public Component(Type type)
            { 
                Type = type;
            }

            [AttributeUsage(AttributeTargets.Field)]
            public class Image : Attribute
            {
                public Sprite sprite;
                public Vector2 size;
                public Image(string spriteName, float x, float y)
                {
                    this.size = new(x, y);
                    this.sprite = SpriteLoader.FromTexture(FarlandsCoreMod.Resources.Get(spriteName) as Texture2D);
                }
            }

            public class Transform : Attribute
            {
                public float x;
                public float y;
                public bool isGlobal;

                public Transform(float x, float y, bool isGlobal = false)
                {
                    this.isGlobal = isGlobal;

                    this.x = x;
                    this.y = y;
                }
            }

            public class PText : Attribute
            {
                public string text;
                public float size;
                public TMP_FontAsset font;
                public PText(string text, float size, string font)
                {
                    this.text = text;
                    this.size = size;
                    this.font = FarlandsCoreMod.Resources.Get(font) as TMP_FontAsset;
                }
            }
        }

        [AttributeUsage(AttributeTargets.Field)]
        public class Awake : Attribute
        {
            public string action;

            public Awake(string action)
            { 
                this.action = action;
            }
        }

        public void InstantiateAllGameObjects(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"InstantiateAll: {scene.name} :: {SceneName}");
            if(scene.name != SceneName) return;
            InstantiateAllGameObjects();
        }

        private void InstantiateAllGameObjects(Transform parent = null)
        {
            type.GetFields(BindingFlags.Public | BindingFlags.Static).ToList()
                .Where(x => x.GetCustomAttributes<GameObject.New>().Count() >= 1).ToList()
                .ForEach(x =>
                {
                    var str = x.GetCustomAttribute<GameObject.New>().name;
                    var gameObject = new UnityEngine.GameObject(str);

                    gameObject.transform.SetParent(parent);

                    x.SetValue(null, gameObject);

                    instantiateOne(gameObject, x);
                });

            type.GetNestedTypes(BindingFlags.Public | BindingFlags.Static).ToList()
                .Where(x => x.GetCustomAttributes<GameObject.New>().Count() >= 1).ToList()
                .ForEach(x =>
                {
                    var str = x.GetCustomAttribute<GameObject.New>().name;
                    var gameObject = new UnityEngine.GameObject(str);

                    gameObject.transform.SetParent(parent);

                    x.GetField("gameObject").SetValue(null, gameObject);

                    instantiateOne(gameObject, x);

                    var s = new SO(SceneName, x);
                    s.InstantiateAllGameObjects(gameObject.transform);
                });
        }

        private void instantiateOne(UnityEngine.GameObject gameObject, MemberInfo x) =>
            x.GetCustomAttributes().ToList()
                    .ForEach(a =>
                    {
                        if (a is Component) gameObject.AddComponent((a as Component).Type);
                        else if (a is Component.Image)
                        {
                            var att = a as Component.Image;
                            var img = gameObject.AddComponent<UnityEngine.UI.Image>();
                            img.sprite = att.sprite;
                            img.rectTransform.sizeDelta = att.size;

                            Canvas.ForceUpdateCanvases();
                        }
                        else if (a is Component.Transform)
                        {
                            var att = a as Component.Transform;
                            var transform = gameObject.transform;

                            if (att.isGlobal) transform.position = new(att.x, att.y);
                            else transform.localPosition = new(att.x, att.y);

                            Canvas.ForceUpdateCanvases();
                        }
                        else if (a is Component.PText)
                        {
                            var att = a as Component.PText;
                            var tmp = gameObject.AddComponent<TextMeshProUGUI>();

                            tmp.text = att.text;
                            tmp.font = att.font;
                        }
                        else if (a is Awake) type.GetMethod((a as Awake).action).Invoke(null, []);
                    });
        
        public void GetRequestedGameObjects(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"RequestedGO: {scene.name} :: {SceneName}");
            if(scene.name != SceneName) return;

            type.GetFields(BindingFlags.Public | BindingFlags.Static).ToList()
                .Where(x => x.GetCustomAttributes<GameObject>().Count() >= 1).ToList()
                .ForEach(x =>
                {
                    var str = x.GetCustomAttribute<GameObject>().name;
                    Debug.Log($"classname: {x.GetCustomAttribute<GameObject>().GetType().FullName}");
                    
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

            type.GetNestedTypes(BindingFlags.Public | BindingFlags.Static).ToList()
                .Where(x => x.GetCustomAttributes<GameObject>().Count() >= 1).ToList()
                .ForEach(x =>
                {
                    var str = x.GetCustomAttribute<GameObject>().name;
                    Debug.Log($"classname: {x.GetCustomAttribute<GameObject>().GetType().FullName}");
                    UnityEngine.GameObject gameObject = null;

                    foreach (var go in SceneManager.GetSceneByName(this.SceneName).GetRootGameObjects())
                    {
                        if (go.name == str)
                        {
                            gameObject = go;
                            break;
                        }
                    }

                    x.GetField("gameObject").SetValue(null, gameObject);
                    var s = new SO(SceneName, x);
                    s.InstantiateAllGameObjects(gameObject.transform);
                });
        }
    }
}
