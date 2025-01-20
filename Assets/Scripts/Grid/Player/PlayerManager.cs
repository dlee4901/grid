using System.Collections.Generic;

public class PlayerManager
{
    List<Player> _players;
    public int NumPlayers;
    public int MovePoints;
    public int Mana;

    public PlayerManager(int numPlayers, int movePoints, int mana)
    {
        NumPlayers = numPlayers;
        MovePoints = movePoints;
        Mana = mana;
        _players = new List<Player>();
        for (int id = 1; id <= numPlayers; id++)
        {
            Player player = new Player(id);
            player.MovePoints = movePoints;
            player.Mana = mana;
            _players.Add(player);
        }
    }

    public Player GetPlayer(int playerTurn)
    {
        if (playerTurn > 0 && playerTurn <= NumPlayers)
        {
            return _players[playerTurn-1];
        }
        return null;
    }

    public int GetPlayerUnitCostTotal(int playerTurn)
    {
        Player player = GetPlayer(playerTurn);
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

    public void ResetPlayerPoints(int playerTurn)
    {
        Player player = GetPlayer(playerTurn);
        if (player != null)
        {
            player.MovePoints = MovePoints;
            player.Mana = Mana;
        }
    }

    public void AddPlayerUnit(Unit unit, int unitCostTotal=0)
    {
        Player player = GetPlayer(unit.PlayerController);
        if (player != null)
        {
            List<Unit> playerUnits = player.Units;
            if (playerUnits != null && !playerUnits.Contains(unit) && (unitCostTotal == 0 || GetPlayerUnitCostTotal(unit.PlayerController) + unit.Properties.Cost <= unitCostTotal))
            {
                playerUnits.Add(unit);
            }
        }
    }

    public void DeletePlayerUnit(Unit unit)
    {
        Player player = GetPlayer(unit.PlayerController);
        if (player != null)
        {
            List<Unit> playerUnits = player.Units;
            if (playerUnits != null)
            {
                playerUnits.Remove(unit);
            }
        }
    }

    public void UpdatePlayerMovePoints(int playerTurn, int movePoints)
    {
        Player player = GetPlayer(playerTurn);
        if (player != null)
        {
            player.MovePoints += movePoints;
        }
    }

    public void UpdatePlayerMana(int playerTurn, int Mana)
    {
        Player player = GetPlayer(playerTurn);
        if (player != null)
        {
            player.Mana += Mana;
        }
    }
}