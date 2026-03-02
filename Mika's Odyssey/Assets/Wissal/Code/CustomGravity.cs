//using UnityEngine;

//public static class CustomGravity
//{
//    public static Vector3 GetGravity(Vector3 position)
//    {
//        return position.normalized * Physics.gravity.y;
//    }
//    public static Vector3 GetUpAxis (Vector3 position)
//    {
//        return position.normalized;
//    }
//    public static Vector3 GetGravity (Vector3 position, out Vector3 upAxis)
//    {
//        upAxis = position.normalized;
//        return upAxis * Physics.gravity.y;
//    }
//}

// for flat gravity:
using UnityEngine;

public static class CustomGravity
{
    public static Vector3 GetGravity(Vector3 position)
    {
        return Physics.gravity;
    }

    public static Vector3 GetUpAxis(Vector3 position)
    {
        return Vector3.up;
    }

    public static Vector3 GetGravity(Vector3 position, out Vector3 upAxis)
    {
        upAxis = Vector3.up;
        return Physics.gravity;
    }
}
