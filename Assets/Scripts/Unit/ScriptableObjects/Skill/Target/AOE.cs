using UnityEngine;

[CreateAssetMenu(menuName = "TargetAOE")]
public class AOE : TargetBase
{
    [Header("AOE")]
    public TileSelection SelectionArea;
    public TileSelection EffectArea;
}