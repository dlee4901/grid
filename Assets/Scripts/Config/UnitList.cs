using System.Collections.Generic;
using UnityEngine;

public class UnitList : MonoBehaviour
{
    public static UnitList Singleton;

    public List<Unit> units;

    public List<Unit> tempSerializable;
    
    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        InitFromSerializable();
    }

    void InitFromSerializable()
    {
        units = tempSerializable;
    }

    void InitFromJson()
    {

    }

    Unit getUnitById(int id)
    {
        return units[id-1];
    }
}
