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
    public static class TexturesModifier
    {
        public static Texture2D GetTexture(string texture) => UnityEngine.Resources.FindObjectsOfTypeAll<Texture2D>().First(x=>x.name == texture);

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

        public static void ReplaceTexture(string id, byte[] raw)
        {
            var texture = GetTexture(id);
            texture.LoadImage(raw);
        }
    }
}
