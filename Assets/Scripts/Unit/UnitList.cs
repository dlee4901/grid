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
        Debug.Log(units);
        EventManager.Singleton.UnitDragEvent += UnitDrag;

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
            GameObject unitGO = new GameObject(unitProperties.title);
            unitGO.transform.parent = transform;
            Unit unit = unitGO.AddComponent<Unit>();
            unit.properties = unitProperties;
            unit.Init();
            units.Add(unit);
        }
    }

    void UnitDrag(int id)
    {
        if (id < units.Count)
        {
            Unit unit = Instantiate(units[id]);
            unit.isDragging = true;
        }
    }

    Unit getUnitById(int id)
    {
        return units[id-1];
    }
}
