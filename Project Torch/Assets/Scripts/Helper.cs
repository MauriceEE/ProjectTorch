using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class contains useful methods that can be used in other scripts
/// </summary>
public class Helper{
    /// <summary>
    /// Handles AABB collision detection between two rects
    /// </summary>
    /// <param name="r1">First bounds</param>
    /// <param name="r2">Second bounds</param>
    /// <returns>True if collision, false otherwise</returns>
    public static bool AABB(Rect r1, Rect r2) {
        return (r1.x < r2.x + r2.width && r1.x + r1.width > r2.x && r1.y < r2.y + r2.height && r1.height + r1.y > r2.y);
    }
    /// <summary>
    /// Maps a value between two ranges
    /// </summary>
    /// <param name="n">Value to map</param>
    /// <param name="a1">From 1</param>
    /// <param name="b1">To 1</param>
    /// <param name="a2">From 2</param>
    /// <param name="b2">To 2</param>
    /// <returns>Remapped value of 'n'</returns>
    public static float Map(float n, float a1, float b1, float a2, float b2) {
        return (n - a1) / (b1 - a1) * (b2 - a2) + a2;
    }
    /// <summary>
    /// Takes in a rect in local coordinates and turns them into global coordinates
    /// </summary>
    /// <param name="r">The source rect</param>
    /// <param name="parentPosition">The position it's parented to</param>
    /// <returns>'r' in global coordinates</returns>
    public static Rect LocalToWorldRect(Rect r, Vector3 parentPosition) {
        return new Rect(new Vector3(parentPosition.x + r.x, parentPosition.y + r.y, parentPosition.z), new Vector2(r.width, r.height));
    }

    /// <summary>
    /// Calculates the midpoint between two vectors
    /// </summary>
    /// <param name="p1">Point 1</param>
    /// <param name="p2">Point 2</param>
    /// <returns>The midpoint as a vector2</returns>
    public static Vector2 Midpoint(Vector2 p1, Vector2 p2)
    {
        return new Vector2((p1.x + p2.x) / 2f, (p1.y + p2.y) / 2f);
    }

    /// <summary>
    /// Returns a vector3 with x and y of a vector2, and a z of zero
    /// </summary>
    /// <param name="v">Vector to convert</param>
    /// <returns>A vector of (v.x,v.y,0)</returns>
    public static Vector3 Vec2ToVec3(Vector2 v)
    {
        return new Vector3(v.x, v.y, 0f);
    }
}
