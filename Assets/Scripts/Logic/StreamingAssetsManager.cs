using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Logic
{
    public static class StreamingAssetsManager
    {
        private static Dictionary<string, Sprite> spritesDictionary = new ();

        public static void OpenStreamingAssetsDirectory()
        {
            Application.OpenURL(Path.Combine("file://", Application.streamingAssetsPath));
        }

        public static Sprite GetSprite(string path)
        {
            if(spritesDictionary.TryGetValue(path, out Sprite sprite))
            {
                return sprite;
            }

            byte[] pngBytes = File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, "Image", path));

            var tex = new Texture2D(2, 2);
            tex.LoadImage(pngBytes);
            var fromTex = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            spritesDictionary.Add(path, fromTex);
            return fromTex;
        }
    }
}
