using System;
using System.Reflection;

using UnityEngine;
using UnityEditor;

public static class SpriteUtility
{
    static MethodInfo s_RenderStaticPreviewMethod;

    static SpriteUtility()
    {
        var spriteUtilityType = GetSpriteUtilityType();
        s_RenderStaticPreviewMethod = spriteUtilityType.GetMethod("RenderStaticPreview", new Type[] { 
            typeof(Sprite), 
            typeof(Color), 
            typeof(int), 
            typeof(int), 
            typeof(Matrix4x4) 
        });
    }

    public static Texture2D RenderStaticPreview(Sprite sprite, Color color, int width, int height)
    {
        return RenderStaticPreview (sprite, color, width, height, Matrix4x4.identity);
    }

    public static Texture2D RenderStaticPreview(Sprite sprite, Color color, int width, int height, Matrix4x4 transform)
    {
        return s_RenderStaticPreviewMethod.Invoke(null, new object[] { sprite, color, width, height, transform }) as Texture2D;
    }

    static Type GetSpriteUtilityType()
    {
        var assembly = typeof(UnityEditor.Editor).Assembly;
        foreach (Type type in assembly.GetTypes())
        {
            if (type.FullName == "UnityEditor.SpriteUtility")
            {
                return type;
            }
        }

        return null;
    }
}

