using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RenderTextureExtensions {
    public static Texture2D GetRTPixels(this RenderTexture rt)
    {
        // Remember currently active render texture
        var currentActiveRT = RenderTexture.active;

        // Set the supplied RenderTexture as the active one
        RenderTexture.active = rt;

        // Create a new Texture2D and read the RenderTexture image into it
        var tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();

        // Restorie previously active render texture
        RenderTexture.active = currentActiveRT;
        return tex;
    }
}
