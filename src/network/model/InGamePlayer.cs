namespace network.model;

using System;
// using db.dao;
using protocol;

public class InGamePlayer
{
    public string id = "";
    
    public ClientState socketState; // 指向 ClientState

    public InGamePlayer(ClientState state)
    {
        this.socketState = state;
		// this.playerData = new Player();
    }

    // 坐标和旋转
    public float x;
    public float y;
    public float z;
    public float ex;
    public float ey;
    public float ez;

    public bool isInRoom = false; // 是否在房间里
    public string roomId = ""; // （仅isInRoom = true时有意义）所在房间的 id。
    public int roleId = 0;  // 扮演的角色

    // public int hp = 100; // 生命值

    // 指向该 BattlePlayer 的 dao
    // public Player playerData;

    // 通过 BattlePlayer 向 socket 发送信息

    public void SendToSocket(MsgBase msgBase)
    {
        NetManager.Send(socketState, msgBase);
    }
}
