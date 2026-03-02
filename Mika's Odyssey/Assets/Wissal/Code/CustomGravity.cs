using UnityEngine;

public static class CustomGravity
{
    public static Vector3 GetGravity(Vector3 position)
    {
        return position.normalized * Physics.gravity.y;
    }
    public static Vector3 GetUpAxis(Vector3 position)
    {
        Vector3 up = position.normalized;
        return Physics.gravity.y < 0f ? up : -up;
    }
    public static Vector3 GetGravity(Vector3 position, out Vector3 upAxis)
    {
        Vector3 up = position.normalized;
        upAxis = Physics.gravity.y < 0f ? up : -up;
        return up * Physics.gravity.y;
    }
}

// for flat gravity:
//using UnityEngine;

//public static class CustomGravity
//{
//    public static Vector3 GetGravity(Vector3 position)
//    {
//        return Physics.gravity;
//    }

//    public static Vector3 GetUpAxis(Vector3 position)
//    {
//        return Vector3.up;
//    }

//    public static Vector3 GetGravity(Vector3 position, out Vector3 upAxis)
//    {
//        upAxis = Vector3.up;
//        return Physics.gravity;
//    }
//}
