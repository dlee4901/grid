using UnityEngine;

// 1-based index
public class Position
{
    int _x;
    int _y;
    int _z;

    public Position(int x, int y, int z=1)
    {
        SetBounds(x, y, z);
    }

    public bool TestPosition(int x, int y, int z=1)
    {
        SetBounds(x, y, z);
        for (int i = GetMinIndex()-1; i < GetMaxIndex()+1; i++)
        {
            Vector3Int vector = GetVector(i);
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

    public void SetBounds(int x, int y, int z=1)
    {
        _x = x;
        _y = y;
        _z = z;
    }

    public int GetIndex(int x, int y, int z=1)
    {
        return GetIndex(new Vector3Int(x, y, z));
    }

    public int GetIndex(Vector2Int vec)
    {
        return GetIndex(new Vector3Int(vec.x, vec.y, 1));
    }

    public int GetIndex(Vector3Int vec)
    {
        if (!IsValidVector(vec)) return 0;
        return (vec.z-1) * _y * _x + (vec.y-1) * _x + (vec.x-1) + 1;
    }

    public Vector3Int GetVector(int idx)
    {
        if (!IsValidIndex(idx)) return Vector3Int.zero;
        int z = (idx-1) / (_y * _x) + 1;
        int y = (idx-1) % (_y * _x) / _x + 1;
        int x = (idx-1) % (_y * _x) % _x + 1;
        return new Vector3Int(x, y, z);
    }

    public Vector3 Get2DWorldPos(Vector3Int vec, float tileScale)
    {
        float x = vec.x * tileScale / 0.1f;
        float y = vec.y * tileScale / 0.1f;
        return new Vector3(x, y, 1);
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
