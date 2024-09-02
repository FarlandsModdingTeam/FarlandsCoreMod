using Farlands.DataBase;
using Farlands.Inventory;
using Farlands.PlaceableObjectsSystem;
using Farlands.PlantSystem;
using Farlands.WorldResources;
using FarlandsCoreMod.Utiles.Loaders;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace FarlandsCoreMod.Utiles
{
    public static class Source
    {
        public static ScriptableObjectsDB DB;

        public static InventoryItem GetInventory(int id) => DB.GetInventoryItem(id);
        public static InventoryItem GetInventory(string name)
        {
            foreach (var i in DB.inventoryItems)
            { 
                if(i.itemIcon.texture.name == name) return i;
            }

            return null;
        }

        public static PlaceableScriptableObject GetPlaceable(int id) => DB.getPlaceablesData(id);
        public static PlaceableScriptableObject GetPlaceable(string name)
        {
            foreach (var i in DB.placeables)
            {
                if (i.worldSprite.texture.name == name) return i;
            }

            return null;
        }

        public static PlantScriptableObject GetPlant(int id) => DB.getPlantData(id);
        // TODO cambiar a que vea el nombre de la textura
        public static PlantScriptableObject GetPlant(string name)
        {
            foreach (var i in DB.plants)
            {
                if (i.name == name) return i;
            }

            return null;
        }

        public static WorldResource GetWorldResource(int id) => DB.getWorldResourceData(id);
        public static WorldResource GetWorldResource(string name)
        {
            foreach (var i in DB.worldResources)
            {
                if (i.resourceSprite.texture.name == name) return i;
            }

            return null;
        }

        public static Texture2D GetTexture(string texture) => UnityEngine.Resources.FindObjectsOfTypeAll<Texture2D>().First(x=>x.name == texture);

        public static void Init()
        {
            DB = GameObject.FindObjectOfType<ScriptableObjectsDB>();
        }

        public static class Replace
        {
            public static Texture2D DuplicateTexture(Texture2D source)
            {
                RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

                Graphics.Blit(source, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;

                Texture2D readableText = new Texture2D(source.width, source.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();

                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);
                return readableText;
            }

            public static void ReplaceSprite(string id, Vector2Int position, byte[] raw)
            {
                var originalTexture = GetTexture(id);

                // Crear una nueva textura legible y modificable
                Texture2D readableTexture = DuplicateTexture(originalTexture);

                // Crear una textura temporal para cargar los nuevos datos de imagen
                Texture2D newTexture = new Texture2D(1, 1);
                newTexture.LoadImage(raw);

                // Aplicar los píxeles de la nueva textura a la textura legible
                for (int i = 0; i < newTexture.width; i++)
                    for (int j = 0; j < newTexture.height; j++)
                        readableTexture.SetPixel(position.x + i, position.y + j, newTexture.GetPixel(i, j));

                // Aplicar los cambios
                readableTexture.Apply();

                // Opcional: convertir la textura legible modificada a byte[]
                byte[] modifiedTextureData = readableTexture.EncodeToPNG();

                // Ahora puedes cargar los datos modificados en otra textura o hacer lo que necesites
                originalTexture.LoadImage(modifiedTextureData);
            }

            public static void OtherTexture(string id, byte[] raw)
            {
                var texture = GetTexture(id);
                texture.LoadImage(raw); 
            }

            public static void PlaceableTexture(string id, byte[] raw)
            { 
                var placeable = GetPlaceable(id);
                placeable.worldSprite.texture.LoadImage(raw);
            }

            public static void InventoryTexture(string id, byte[] raw)
            { 
                var inventory = GetInventory(id);
                inventory.itemIcon.texture.LoadImage(raw);
            }

            public static void WorldResourceTexture(string id, byte[] raw)
            {
                var wr = GetWorldResource(id);
                wr.resourceSprite.texture.LoadImage(raw);
            }

            public static void PlantTextue(string id, byte[] raw)
            {
                var plant = GetPlant(id);
                plant.seedSprite.texture.LoadImage(raw);
            }

            public static void DialogueTexture(string id, byte[] raw)
            {
                var asset = (Sprite)DialogueManager.instance.LoadAsset(id, typeof(Sprite));
                if (asset == null) return;

                asset.texture.LoadImage(raw);
            }
        }
    }
}
