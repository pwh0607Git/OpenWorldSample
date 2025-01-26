using UnityEngine;

public static class Vector3Extensions
{
    /// <summary>
    /// 벡터의 Y값을 0으로 설정합니다.
    /// </summary>
    public static Vector3 FlattenY(this Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }
}
