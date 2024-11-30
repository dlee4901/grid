using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class Util
{
    public static Vector3 GetMousePosition(bool zeroed=true)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (zeroed) mousePosition.z = 0f;
        return mousePosition;
    }

    public static Vector3 Get2DWorldPos(Vector3Int vec, float scale)
    {
        float x = vec.x * scale / 0.1f;
        float y = vec.y * scale / 0.1f;
        return new Vector3(x, y, 0);
    }

    public static T CreateGameObject<T>() where T : MonoBehaviour
    {
        GameObject go = new GameObject();
        T t = go.AddComponent<T>();
        return t;
    }

    public static void PrintList<T>(HashSet<T> set)
    {
        foreach (T item in set)
        {
            Debug.Log(item);
        }
    }

    public static void PrintList<T>(List<T> set)
    {
        foreach (T item in set)
        {
            Debug.Log(item);
        }
    }
}