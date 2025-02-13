using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    public static IEnumerator LerpTransform(Transform transform, Vector3 src, Vector3 dst, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector3.Lerp(src, dst, (Time.time - startTime) / overTime);
            yield return null;
        }
        transform.position = dst;
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

    public static bool BinaryStringContainsEnum(string binaryString, int enumValue)
    {
        return binaryString[enumValue] == '1';
    }

    public static string IntToBinaryString(int input)
    {
        return Convert.ToString(input, 2);
    }

    public static int BinaryStringToInt(string input)
    {
        return Convert.ToInt32(input, 2);
    }

    public static HashSet<T> ListToHashSet<T>(List<T> list)
    {
        return new HashSet<T>(list);
    }

    public static bool IsValidOriginAndUnit(int origin, Position<Tile> tiles, Position<Entity> entities)
    {
        Entity entity = entities.Get(origin);
        return tiles.IsValidIndex(origin) && entity != null && entity.GetType() == typeof(Unit);
    }
    
    public static bool IsValidOriginAndUnit(Vector2Int origin, Position<Tile> tiles, Position<Entity> entities)
    {
        Entity entity = entities.Get(origin);
        return tiles.IsValidVector(origin) && entity != null && entity.GetType() == typeof(Unit);
    }

    public static FieldInfo[] GetFields(object src)
    {
        return src?.GetType()?.GetFields();
    }

    public static FieldInfo GetField(object src, string field)
    {
        return src?.GetType()?.GetField(field);
    }

    public static object GetFieldValue(object src, string field)
    {
        return GetField(src, field)?.GetValue(src);
    }
}