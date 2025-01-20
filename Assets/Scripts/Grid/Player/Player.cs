using System.Collections.Generic;
using Unity.Netcode;

public class Player
{
    public int TurnId;
    public int MovePoints;
    public int Mana;
    public List<Unit> Units;

    public Player(int turnId)
    {
        TurnId = turnId;
    }
}
