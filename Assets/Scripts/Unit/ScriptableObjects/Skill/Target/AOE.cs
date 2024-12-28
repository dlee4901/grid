using UnityEngine;

[CreateAssetMenu(menuName = "TargetAOE")]
public class AOE : TargetBase
{
    [Header("AOE")]
    public TileSelection selectionArea;
    public TileSelection effectArea;
}