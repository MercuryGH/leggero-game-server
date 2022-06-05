namespace network.util;

using network.model;
using network.model.util;
using network.protocol;

public static partial class MsgHandler
{
    private static bool FindIfCheatMovement(InGamePlayer battlePlayer, MsgPlayerPosition msg)
    {
        if (Math.Abs(battlePlayer.x - msg.x) > 5 ||
    Math.Abs(battlePlayer.y - msg.y) > 5 ||
    Math.Abs(battlePlayer.z - msg.z) > 5)
        {
            return true;
        }
        return false;
    }

    // 同步位置协议
    public static void MsgPlayerPosition(ClientState c, MsgBase msgBase)
    {
        MsgPlayerPosition msg = (MsgPlayerPosition)msgBase;
        InGamePlayer? battlePlayer = c.inGamePlayer;
        if (battlePlayer == null)
        {
            return;
        }

        Room? room = RoomManager.GetRoom(battlePlayer.roomId);
        if (room == null)
        {
            return;
        }

        if (room.status != Room.Status.IN_GAME)
        {
            return;
        }

        // 检测是否作弊（粗略）
        // bool cheatFlag = FindIfCheatMovement(battlePlayer, msg);
        // if (cheatFlag == true)
        // {
        //     Console.WriteLine("Cheat detected: " + battlePlayer.id);
        // }

        // 更新信息
        battlePlayer.x = msg.x;
        battlePlayer.y = msg.y;
        battlePlayer.z = msg.z;
        battlePlayer.ex = msg.ex;
        battlePlayer.ey = msg.ey;
        battlePlayer.ez = msg.ez;

        msg.id = battlePlayer.id;
        room.BroadcastExceptPlayer(msg, msg.id);
    }

    public static void MsgEnemyPosition(ClientState c, MsgBase msgBase)
    {
        MsgEnemyPosition msg = (MsgEnemyPosition)msgBase;
        InGamePlayer? player = c.inGamePlayer;
        if (player == null)
        {
            return;
        }

        Room? room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            return;
        }

        if (room.status != Room.Status.IN_GAME)
        {
            return;
        }

        room.BroadcastExceptPlayer(msg, player.id);
    }

    public static void MsgAssassinBomb(ClientState c, MsgBase msgBase)
    {
        MsgAssassinBomb msg = (MsgAssassinBomb)msgBase;
        InGamePlayer? player = c.inGamePlayer;
        if (player == null)
        {
            return;
        }

        Room? room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            return;
        }
        if (room.status != Room.Status.IN_GAME)
        {
            return;
        }

        room.BroadcastExceptPlayer(msg, player.id);
    }

    public static void MsgMagicianTrigger(ClientState c, MsgBase msgBase)
    {
        MsgMagicianTrigger msg = (MsgMagicianTrigger)msgBase;
        InGamePlayer? player = c.inGamePlayer;
        if (player == null)
        {
            return;
        }

        Room? room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            return;
        }
        if (room.status != Room.Status.IN_GAME)
        {
            return;
        }

        room.BroadcastExceptPlayer(msg, player.id);
    }

    public static void MsgEnemyDied(ClientState c, MsgBase msgBase)
    {
        MsgEnemyDied msg = (MsgEnemyDied)msgBase;
        InGamePlayer? player = c.inGamePlayer;
        if (player == null)
        {
            return;
        }

        Room? room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            return;
        }
        if (room.status != Room.Status.IN_GAME)
        {
            return;
        }

        room.BroadcastExceptPlayer(msg, player.id);
    }

    public static void MsgOpenDoor(ClientState c, MsgBase msgBase)
    {
        MsgOpenDoor msg = (MsgOpenDoor)msgBase;
        InGamePlayer? player = c.inGamePlayer;
        if (player == null)
        {
            return;
        }

        Room? room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            return;
        }
        if (room.status != Room.Status.IN_GAME)
        {
            return;
        }

        msg.playerId = player.id;
        room.Broadcast(msg);
    }

    public static void MsgGetBomb(ClientState c, MsgBase msgBase)
    {
        MsgGetBomb msg = (MsgGetBomb)msgBase;
        InGamePlayer? player = c.inGamePlayer;
        if (player == null)
        {
            return;
        }

        Room? room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            return;
        }
        if (room.status != Room.Status.IN_GAME)
        {
            return;
        }

        msg.playerId = player.id;
        room.BroadcastExceptPlayer(msg, player.id);
    }

    public static void MsgImmobilized(ClientState c, MsgBase msgBase)
    {
        MsgImmobilized msg = (MsgImmobilized)msgBase;
        InGamePlayer? player = c.inGamePlayer;
        if (player == null)
        {
            return;
        }

        Room? room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            return;
        }
        if (room.status != Room.Status.IN_GAME)
        {
            return;
        }

        room.BroadcastExceptPlayer(msg, player.id);
    }

    // MsgLose也是广播型的
    public static void MsgLose(ClientState c, MsgBase msgBase)
    {
        MsgLose msg = (MsgLose)msgBase;
        InGamePlayer? player = c.inGamePlayer;
        if (player == null)
        {
            return;
        }

        Room? room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            return;
        }
        if (room.status != Room.Status.IN_GAME)
        {
            return;
        }

        room.Broadcast(msg);
    }

    public static void MsgWin(ClientState c, MsgBase msgBase)
    {
        MsgWin msg = (MsgWin)msgBase;
        InGamePlayer? player = c.inGamePlayer;
        if (player == null)
        {
            return;
        }

        Room? room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            return;
        }
        if (room.status != Room.Status.IN_GAME)
        {
            return;
        }

        room.Broadcast(msg);
    }

    public static void MsgRestart(ClientState c, MsgBase msgBase)
    {
        MsgRestart msg = (MsgRestart)msgBase;
        InGamePlayer? player = c.inGamePlayer;
        if (player == null)
        {
            return;
        }

        Room? room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            return;
        }
        if (room.status != Room.Status.IN_GAME)
        {
            return;
        }

        room.BroadcastExceptPlayer(msg, player.id);
    }
}


