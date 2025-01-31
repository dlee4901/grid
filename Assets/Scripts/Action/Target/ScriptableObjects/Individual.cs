using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TargetIndividual")]
public class Individual : TargetBase
{
    [Header("Individual")]
    public TileSelection SelectionArea;
}