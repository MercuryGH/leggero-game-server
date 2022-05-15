namespace network.util;

using network.model;
using network.model.util;
using network.protocol;

public static partial class MsgHandler
{
    // 获取房间信息
    // TODO: 确定客户端
    public static void MsgGetPlayerInfoInRoom(ClientState c, MsgBase msgBase)
    {
        MsgGetPlayerInfoInRoom msg = (MsgGetPlayerInfoInRoom)msgBase;
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

        player.SendToSocket(room.GenerateGetPlayerInfoInRoomMsg());
    }

    public static void MsgSelectRole(ClientState c, MsgBase msgBase)
    {
        MsgSelectRole msg = (MsgSelectRole)msgBase;
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
        room.SetPlayerRole(player.id, msg.roleId);
    }

    public static void MsgLeaveRoom(ClientState c, MsgBase msgBase)
    {
        MsgLeaveRoom msg = (MsgLeaveRoom)msgBase;
        InGamePlayer? player = c.inGamePlayer;
        if (player == null)
        {
            return;
        }

        Room? room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            msg.status = 1;
            player.SendToSocket(msg);
            return;
        }

        room.RemovePlayer(player.id);

        msg.status = 0;
        player.SendToSocket(msg);
    }

    // 请求开始战斗
    public static void MsgStartGame(ClientState c, MsgBase msgBase)
    {
        MsgStartGame msg = (MsgStartGame)msgBase;
        InGamePlayer? player = c.inGamePlayer;
        if (player == null)
        {
            return;
        }

        Room? room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            msg.status = 1;
            player.SendToSocket(msg);
            return;
        }

        if (!room.isOwner(player)) // 是否是房主
        {
            msg.status = 1;
            player.SendToSocket(msg);
            return;
        }

        bool flag = room.StartGame();
        if (flag == false) // 开战失败
        {
            msg.status = 1;
            player.SendToSocket(msg);
            return;
        }

        msg.status = 0;
        player.SendToSocket(msg);
    }
}

