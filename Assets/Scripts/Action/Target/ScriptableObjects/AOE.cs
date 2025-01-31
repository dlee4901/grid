using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TargetAOE")]
public class AOE : TargetBase
{
    [Header("AOE")]
    public TileSelection SelectionArea;
    public TileSelection EffectArea;
}