using System.Collections.Generic;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public int Id;
    public int MovementPoints;
    public int ActionPoints;
    public List<Unit> Units;

    public Player(int id)
    {
        Id = id;
    }
}
