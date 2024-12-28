using UnityEngine;

[CreateAssetMenu(menuName = "TargetIndividual")]
public class Individual : TargetBase
{
    [Header("Individual")]
    public TileSelection selectionArea;
    public int Amount;
}