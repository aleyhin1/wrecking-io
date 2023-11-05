using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformUtils
{
    public static Vector3 GetOffsetWorldVector(float distance, Quaternion rotation)
    {
        return new Vector3(-distance * Mathf.Cos(rotation.eulerAngles.y * Mathf.Deg2Rad), 0, distance * Mathf.Sin(rotation.eulerAngles.y * Mathf.Deg2Rad));
    }
}
