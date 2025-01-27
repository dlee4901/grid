using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitList : MonoBehaviour
{
    public static UnitList Singleton;

    [field: SerializeField] public List<Unit> Units { get; private set; }

    [Header("Unit Prefabs")]
    [SerializeField] private List<Unit> UnitPrefabs1Indexed;
    [SerializeField] private UnitHealthCounter UnitHealthCounterPrefab;
    
    private void Awake()
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

    private void InitFromUnitPrefabs()
    {
        foreach (Unit unitPrefab in UnitPrefabs1Indexed)
        {
            Debug.Log(unitPrefab);
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
