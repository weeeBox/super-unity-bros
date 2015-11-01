using UnityEngine;
using System.Collections;

public static class Extensions
{
    public static Vector2 CalculateOverlap(this BoxCollider2D a, BoxCollider2D b)
    {
        float dx = 0.5f * (a.size.x + b.size.x) - Mathf.Abs(a.bounds.center.x - b.bounds.center.x);
        float dy = 0.5f * (a.size.y + b.size.y) - Mathf.Abs(a.bounds.center.y - b.bounds.center.y);
        return new Vector2(dx, dy);
    }

    public static void Translate(this Transform transform, float dx, float dy)
    {
        transform.Translate(new Vector3(dx, dy, 0.0f));
    }
}
