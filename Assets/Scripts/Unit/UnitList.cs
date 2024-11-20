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
        //EventManager.Singleton.UnitUIDragEvent += UnitUIDrag;

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
            GameObject unitGO = new GameObject(unitProperties.title);
            unitGO.transform.parent = transform;
            Unit unit = unitGO.AddComponent<Unit>();
            unit.properties = unitProperties;
            unit.Init();
            unit.gameObject.SetActive(false);
            units.Add(unit);
        }
    }

    // void UnitUIDrag(int unitID, int playerController, int listUIPosition)
    // {
    //     if (unitID < units.Count)
    //     {
    //         Unit unit = Instantiate(units[unitID]);
    //         unit.properties.controller = playerController;
    //         unit.gameObject.SetActive(true);
    //         unit.listUIPosition = listUIPosition;
    //         unit.isDragging = true;
    //     }
    // }
}
