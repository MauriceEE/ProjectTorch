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
        return new Rect(parentPosition.x + r.x, parentPosition.y + r.y, r.width, r.height);
        //return new Rect(new Vector3(parentPosition.x + r.x, parentPosition.y + r.y, parentPosition.z), new Vector2(r.width, r.height));
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

    /// <summary>
    /// Returns a vector2 with x and y of a vector3
    /// </summary>
    /// <param name="v">Vector to convert</param>
    /// <returns>A vector of (v.x,v.y)</returns>
    public static Vector2 Vec3ToVec2(Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    /// <summary>
    /// Displays a rectangle in the editor
    /// </summary>
    /// <param name="position">Position of the rect</param>
    /// <param name="box">The rect itself</param>
    public static void DebugDrawRect(Vector3 position, Rect box)
    {
        //Top
        Debug.DrawLine(
            new Vector3(position.x + box.min.x, position.y + box.min.y, position.z),
            new Vector3(position.x + box.max.x, position.y + box.min.y, position.z));
        //new Vector3(this.transform.boxsition.xransform.posiboxon.y + hform.position.z),
        //new Vector3(this.transform.boxsition.xdth, this.traboxform.pos this.transform.position.z));
        //Left
        Debug.DrawLine(                                      
            new Vector3(position.x + box.min.x, position.y + box.min.y, position.z),
            new Vector3(position.x + box.min.x, position.y + box.max.y, position.z));
        //new Vector3(thsition.x + hbbox, this.ton.y + hb.y, boxis.trans),
        //new Vector3(thsition.x + hbbox, this.ton.y + hb.y -boxb.heightm.position.z));
        //Bottom
        Debug.DrawLine(                                      
            new Vector3(position.x + box.min.x, position.y + box.max.y, position.z),
            new Vector3(position.x + box.max.x, position.y + box.max.y, position.z));
        //new Vector3(thsition.x + hbbox, this.ton.y + hb.y -boxb.heightm.position.z),
        //new Vector3(thsition.x + hbbox + hb.wiform.positionbox + hb.y is.transform.position.z));
        //Right
        Debug.DrawLine(                                      
            new Vector3(position.x + box.max.x, position.y + box.min.y, position.z),
            new Vector3(position.x + box.max.x, position.y + box.max.y, position.z));
        //new Vector3(this.transform.position.x + hb.x + hb.width, this.transform.position.y + hb.y, this.transform.position.z),
        //new Vector3(this.transform.position.x + hb.x + hb.width, this.transform.position.y + hb.y - hb.height, this.transform.position.z));
    }

    /// <summary>
    /// Puts a divider in the console so things can be easier to read sometimes
    /// </summary>
    public static void DebugLogDivider()
    {
        Debug.Log("<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>" + Time.fixedTime);
    }

    #region Constants
    //I'm just putting this here for ease of access, so "frame" data can be easily adjusted later
    public const float frame = 1f / 60f;
    #endregion
}