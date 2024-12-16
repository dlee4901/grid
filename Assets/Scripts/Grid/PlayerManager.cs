using System.Collections.Generic;

public class PlayerManager
{
    List<Player> _players;
    public int NumPlayers;
    public int MovementPoints;
    public int ActionPoints;

    public PlayerManager(int numPlayers, int movementPoints, int actionPoints)
    {
        NumPlayers = numPlayers;
        MovementPoints = movementPoints;
        ActionPoints = actionPoints;
        _players = new List<Player>();
        for (int id = 1; id <= numPlayers; id++)
        {
            Player player = new Player(id);
            player.MovementPoints = movementPoints;
            player.ActionPoints = actionPoints;
            _players.Add(player);
        }
    }

    public Player GetPlayer(int playerId)
    {
        if (playerId > 0 && playerId <= NumPlayers)
        {
            return _players[playerId-1];
        }
        return null;
    }

    public int GetPlayerUnitCostTotal(int playerId)
    {
        Player player = GetPlayer(playerId);
        if (player != null)
        {
            int unitCostTotal = 0;
            foreach (Unit unit in player.Units)
            {
                unitCostTotal += unit.Properties.Cost;
            }
            return unitCostTotal;
        }
        return -1;
    }

    public void ResetPlayerPoints(int playerId)
    {
        Player player = GetPlayer(playerId);
        if (player != null)
        {
            player.MovementPoints = MovementPoints;
            player.ActionPoints = ActionPoints;
        }
    }

    public void AddPlayerUnit(Unit unit, int unitCostTotal=0)
    {
        Player player = GetPlayer(unit.Stats.PlayerController);
        if (player != null)
        {
            List<Unit> playerUnits = player.Units;
            if (playerUnits != null && !playerUnits.Contains(unit) && (unitCostTotal == 0 || GetPlayerUnitCostTotal(unit.Stats.PlayerController) + unit.Properties.Cost <= unitCostTotal))
            {
                playerUnits.Add(unit);
            }
        }
    }

    public void DeletePlayerUnit(Unit unit)
    {
        Player player = GetPlayer(unit.Stats.PlayerController);
        if (player != null)
        {
            List<Unit> playerUnits = player.Units;
            if (playerUnits != null)
            {
                playerUnits.Remove(unit);
            }
        }
    }
}