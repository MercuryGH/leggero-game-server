namespace network.util;

using network.model;
using network.model.util;
using network.protocol;

public static partial class MsgHandler
{
    // 创建房间
    // TODO: 并发控制
    public static void MsgCreateRoom(ClientState c, MsgBase msgBase)
    {
        MsgCreateRoom msg = (MsgCreateRoom)msgBase;
        InGamePlayer? player = c.inGamePlayer;
        if (player == null)
        {
            return;
        }

        // 已经在房间里
        if (player.isInRoom)
        {
            msg.status = 1; // 创建失败
            player.SendToSocket(msg);
            return;
        }
        Console.WriteLine(msg.hostId + " " + player.id);
        Room room = RoomManager.AddRoom(msg.hostId);
        bool flag = room.AddPlayer(player.id);
        if (flag == false)
        {
            msg.status = 1; // 创建失败
            player.SendToSocket(msg);
            return;
        }

        msg.status = 0;
        player.SendToSocket(msg);
    }

    // 进入房间
    public static void MsgEnterRoom(ClientState c, MsgBase msgBase)
    {
        MsgEnterRoom msg = (MsgEnterRoom)msgBase;
        InGamePlayer? player = c.inGamePlayer;
        if (player == null)
        {
            return;
        }

        // 已经在房间里
        if (player.isInRoom)
        {
            msg.status = 3; // 进入失败
            player.SendToSocket(msg);
            return;
        }
        // 获取房间
        Room? room = RoomManager.GetRoom(msg.hostId);
        if (room == null)
        {
            msg.status = 1; // 找不到这个房间
            player.SendToSocket(msg);
            return;
        }
        // 房间的房主就是自己，不知道出了什么BUG
        if (room.playerIds.Count == 0 || room.isOwner(player))
        {
            msg.status = 4;
            player.SendToSocket(msg);
            return;
        }
        // 进入
        bool flag = room.AddPlayer(player.id); // room.AddPlayer 会自动广播
        if (flag == false) // 某种原因导致加入失败
        {
            msg.status = 2;
            player.SendToSocket(msg);
            return;
        }
        msg.status = 0;
        player.SendToSocket(msg);
    }

}