using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitList : MonoBehaviour
{
    public static UnitList Singleton;

    public List<Unit> units;

    public List<UnitProperties> tempSerializable;
    
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
        foreach (UnitProperties unitProperties in tempSerializable)
        {
            Unit unit = Util.CreateGameObject<Unit>();
            unit.Init(unitProperties, transform);
            units.Add(unit);
        }
    }
}
