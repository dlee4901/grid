using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitList : MonoBehaviour
{
    public static UnitList Singleton;

    public List<Unit> Units;

    [Header("Unit Prefabs")]
    public List<Unit> UnitPrefabs1Indexed;
    public UnitHealthCounter UnitHealthCounterPrefab;
    
    void Awake()
    {
        Units = new List<Unit>();

        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        InitFromUnitPrefabs();
    }

    void InitFromUnitPrefabs()
    {
        foreach (Unit unitPrefab in UnitPrefabs1Indexed)
        {
            if (unitPrefab == null) 
            {
                Units.Add(null);
                continue;
            }
            Unit unit = Instantiate(unitPrefab, transform);
            unit.Init(UnitHealthCounterPrefab);
            Units.Add(unit);
        }
    }

    public bool IsValidUnitID(int unitID)
    {
        return unitID > 0 && unitID < Units.Count;
    }
}
