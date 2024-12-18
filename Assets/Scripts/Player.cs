using System.Collections.Generic;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public int TurnId;
    public int MovePoints;
    public int SkillPoints;
    public List<Unit> Units;

    public Player(int turnId)
    {
        TurnId = turnId;
    }
}
