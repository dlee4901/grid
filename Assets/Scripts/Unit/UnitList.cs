using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitList : MonoBehaviour
{
    public static UnitList Singleton;

    public List<Unit> units;

    public List<Unit> tempSerializable;
    
    void Awake()
    {
        units = new List<Unit>();
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
        foreach (Unit unit in tempSerializable)
        {
            units.Add(unit);
        }
    }

    void InitFromJson()
    {

    }

    Unit getUnitById(int id)
    {
        return units[id-1];
    }
}
