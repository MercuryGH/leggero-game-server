namespace network.protocol;

[System.Serializable]
public sealed class PlayerInfo
{
    public string playerId = "test";  // 玩家昵称
    public int roleId = 0;            // 阵营
    public int isOwner = 0;		      // 是否是房主
}

// 查询当前所在的房间信息
public sealed class MsgGetPlayerInfoInRoom : MsgBase
{
    public MsgGetPlayerInfoInRoom(int playerCnt)
    {
        protoName = "MsgGetPlayerInfoInRoom";
        players = new PlayerInfo[playerCnt];
    }

    public MsgGetPlayerInfoInRoom()
    {
        protoName = "MsgGetPlayerInfoInRoom";
    }

    // response
    public PlayerInfo[]? players;
}

// 选择职业 sync broadcast
public sealed class MsgSelectRole : MsgBase
{
    public MsgSelectRole() { protoName = "MsgSelectRole"; }

    public int roleId = 0;
}

// 离开房间
public sealed class MsgLeaveRoom : MsgBase
{
    public MsgLeaveRoom() { protoName = "MsgLeaveRoom"; }

    // response status code
    public int status = 0;
}

// 开战
public sealed class MsgStartGame : MsgBase
{
    public MsgStartGame() { protoName = "MsgStartGame"; }

    // request TODO: 指定stageId
    public int stageId = 0;

    // response status code: success: 0, other: 1
    public int status = 0;
}

// 进入战场
// 注意与 MsgEnterGame （房主开始战斗） 的区别
public sealed class MsgEnterGame : MsgBase
{
    public MsgEnterGame()
    {
        protoName = "MsgEnterGame";
    }

    public MsgEnterGame(int playerCnt, int stageId)
    {
        protoName = "MsgEnterGame";
        this.stageId = stageId;
    }

    // push
    public int stageId = 1;	 // 地图id
}