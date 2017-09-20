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
}
