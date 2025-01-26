using System;
using System.Collections.Generic;
using UnityEngine;

// 1-based index
public class Position<T>
{
    private List<T> _list;
    private int _x;
    private int _y;
    private int _z;

    public Position(int x, int y, int z=1)
    {
        _list = new() {default};
        SetBounds(x, y, z);
    }

    public (int, int, int) Bounds()
    {
        return (_x, _y, _z);
    }

    public int Count()
    {
        return _list.Count;
    }

    public void SetBounds(int x, int y, int z=1)
    {
        _x = x;
        _y = y;
        _z = z;
    }

    public void Add(T val)
    {
        _list.Add(val);
    }

    public bool Set(int idx, T val)
    {
        if (IsValidIndex(idx))
        {
            _list[idx] = val;
            return true;
        }
        return false;
    }

    public bool Set(Vector3Int vec, T val)
    {
        if (IsValidVector(vec))
        {
            return Set(GetIndex(vec), val);
        }
        return false;
    }

    public T Get(int idx)
    {
        if (!IsValidIndex(idx))
        {
            return default;
        }
        return _list[idx];
    }

    public T Get(Vector3Int vec)
    {
        return Get(GetIndex(vec));
    }

    public T Get(Vector2Int vec)
    {
        return Get(GetIndex(vec));
    }

    public bool Move(int srcIndex, int dstIndex)
    {
        if (IsValidIndex(srcIndex) && IsValidIndex(dstIndex))
        {
            return Set(dstIndex, Get(srcIndex)) && Set(srcIndex, default);
        }
        return false;
    }

    public int GetIndex(Vector2Int vec)
    {
        return GetIndex(new Vector3Int(vec.x, vec.y, 1));
    }

    public int GetIndex(Vector3Int vec)
    {
        if (!IsValidVector(vec)) return 0;
        return (vec.z - 1) * _y * _x + (vec.y - 1) * _x + (vec.x - 1) + 1;
    }

    public HashSet<int> GetOccupiedIndices()
    {
        HashSet<int> indices = new();
        for (int i = 1; i < _list.Count; i++)
        {
            if (_list[i] != null)
            {
                indices.Add(i);
            }
        }
        return indices;
    }

    public HashSet<Vector2> GetOccupiedVectors2()
    {
        HashSet<Vector2> vectors = new();
        for (int i = 1; i < _list.Count; i++)
        {
            if (_list[i] != null)
            {
                vectors.Add(GetVector2(i));
            }
        }
        return vectors;
    }

    public HashSet<int> GetIndicesHashSet(HashSet<Vector2Int> vectors)
    {
        HashSet<int> indices = new();
        foreach (Vector2Int vec in vectors) indices.Add(GetIndex(vec));
        return indices;
    }

    public HashSet<int> GetIndicesHashSet(List<Vector2Int> vectors)
    {
        HashSet<int> indices = new();
        foreach (Vector2Int vec in vectors) indices.Add(GetIndex(vec));
        return indices;
    }

    public List<int> GetIndicesList(HashSet<Vector2Int> vectors)
    {
        List<int> indices = new();
        foreach (Vector2Int vec in vectors) indices.Add(GetIndex(vec));
        return indices;
    }

    public List<int> GetIndicesList(List<Vector2Int> vectors)
    {
        List<int> indices = new();
        foreach (Vector2Int vec in vectors) indices.Add(GetIndex(vec));
        return indices;
    }

    public Vector2Int GetVector2(int idx)
    {
        Vector3Int vector = GetVector3(idx);
        return new Vector2Int(vector.x, vector.y);
    }

    public Vector3Int GetVector3(int idx)
    {
        if (!IsValidIndex(idx)) return Vector3Int.zero;
        int z = (idx-1) / (_y * _x) + 1;
        int y = (idx-1) % (_y * _x) / _x + 1;
        int x = (idx-1) % (_y * _x) % _x + 1;
        return new Vector3Int(x, y, z);
    }

    public Vector3Int GetMinVector()
    {
        return new Vector3Int(1, 1, 1);
    }

    public Vector3Int GetMaxVector()
    {
        return new Vector3Int(_x, _y, _z);
    }

    public int GetMinIndex()
    {
        return 1;
    }

    public int GetMaxIndex()
    {
        return _x * _y * _z;
    }

    public bool IsValidVector(Vector2Int vec)
    {
        return IsValidVector(new Vector3Int(vec.x, vec.y, 1));
    }

    public bool IsValidVector(Vector3Int vec)
    {
        Vector3Int minVec = GetMinVector();
        Vector3Int maxVec = GetMaxVector();
        return vec.x >= minVec.x && vec.y >= minVec.y && vec.z >= minVec.z && vec.x <= maxVec.x && vec.y <= maxVec.y && vec.z <= maxVec.z;
    }

    public bool IsValidIndex(int idx)
    {
        return idx >= GetMinIndex() && idx <= GetMaxIndex();
    }

    public bool TestPosition()
    {
        for (int i = GetMinIndex()-1; i < GetMaxIndex()+1; i++)
        {
            Vector3Int vector = GetVector3(i);
            int index = GetIndex(vector);
            if (index != i) 
            {
                Debug.Log(i);
                Debug.Log(index);
                Debug.Log(vector);
                return false;
            }
        }
        return true;
    }
}

// 7 8
// 5 6

// 3 4
// 1 2

// 1 1 1 > 1
// 2 1 1 > 2
// 1 2 1 > 3
// 2 2 1 > 4
// 1 1 2 > 5
// 2 1 2 > 6
// 1 2 2 > 7
// 2 2 2 > 8
// 1 1 3 > 9
// 1 2 3 > 10
// 2 1 3 > 11
// 2 2 3 > 12

// _x = 2 
// _y = 2
// _z = 3

// z = idx-1 / _x * _y
// y = (idx-1 % _x * _y) / _x
// x = (idx-1 % _x * _y) % _x

