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
        InitFromTempSerializable();
    }

    void InitFromTempSerializable()
    {
        foreach (Unit unit in tempSerializable)
        {
            if (unit == null) 
            {
                units.Add(null);
                continue;
            }
            Unit unitCopy = Instantiate(unit, transform);
            unitCopy.Init();
            units.Add(unitCopy);
        }
    }

    public bool IsValidUnitID(int unitID)
    {
        return unitID > 0 && unitID < units.Count;
    }
}
