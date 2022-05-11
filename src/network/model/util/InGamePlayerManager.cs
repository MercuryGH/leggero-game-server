namespace network.model.util;

using System.Collections.Generic;

using network.model;

public static class InGamePlayerManager
{
    // {playerId: BattlePlayer}
    static Dictionary<string, InGamePlayer> players = new Dictionary<string, InGamePlayer>();

    public static bool IsOnline(string id)
    {
        return players.ContainsKey(id);
    }

    public static InGamePlayer? GetPlayerById(string id)
    {
        if (players.ContainsKey(id))
        {
            return players[id];
        }
        return null;
    }

    public static void AddPlayer(string id, InGamePlayer player)
    {
        players.Add(id, player);
    }

    public static void RemovePlayer(string id)
    {
        players.Remove(id);
    }
}
