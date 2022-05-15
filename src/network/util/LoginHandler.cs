namespace network.util;

using network.protocol;
using network.model;
using network.model.util;
// using db.util;
// using db.dao;

public static partial class MsgHandler
{
    // public static void MsgRegister(ClientState c, BaseMsg msgBase)
    // {
    //     MsgRegister msg = (MsgRegister)msgBase;

    //     int flag = DbManager.Register(msg.id, msg.pw);
    //     if (flag == 0)
    //     {
    //         DbManager.RegisterPlayer(msg.id);
    //         msg.result = 0;
    //     }
    //     else 
    //     {
    //         msg.result = flag;
    //     }
    //     NetManager.Send(c, msg); // response
    // }

    public static void MsgLogin(ClientState c, MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin)msgBase;

        if (c.inGamePlayer != null) // 通过此 socket 重复登录
        {
            msg.status = 1;
            NetManager.Send(c, msg);
            return;
        }

        // duplicate id
        if (InGamePlayerManager.IsOnline(msg.playerId))
        {
            msg.status = 1;
            NetManager.Send(c, msg);
            return;
        }

        InGamePlayer player = new InGamePlayer(c);
        player.id = msg.playerId;

        InGamePlayerManager.AddPlayer(msg.playerId, player);
        c.inGamePlayer = player;

        // status = OK
        msg.status = 0;
        NetManager.Send(c, msg);
    }
}
