namespace network.protocol;

// 房间信息数据结构。暂未使用。
[System.Serializable]
public sealed class RoomInfo
{
    // response
    public int id = 0;      // 房间id
    public int playerCnt = 0;   // 人数
    public int roomStatus = 0;	// 状态 0-准备中 1-战斗中
}

// 创建房间
public sealed class MsgCreateRoom : MsgBase
{
    public MsgCreateRoom() { protoName = "MsgCreateRoom"; }

    // request
    public string hostId = "";

    // response status code
    public int status = 0;
}

// 进入房间
public sealed class MsgEnterRoom : MsgBase
{
    public MsgEnterRoom() { protoName = "MsgEnterRoom"; }

    // request
    public string hostId = "";

    // response
    public int status = 0;
}