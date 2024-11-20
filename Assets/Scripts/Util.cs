using UnityEngine;
using UnityEngine.InputSystem;

public static class Util
{
    public static Vector3 GetMousePosition(bool zeroed=false)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (zeroed) mousePosition.z = 0f;
        return mousePosition;
    }

    public static T CreateGameObject<T>() where T : MonoBehaviour
    {
        GameObject go = new GameObject();
        T t = go.AddComponent<T>();
        return t;
    }
}