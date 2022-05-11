/// ServerInGameMsg 都是 push 型协议，因此不需要Handler
namespace network.protocol;

// 客户端出生点信息
// [System.Serializable]
// public sealed class PlayerPositionInfo
// {
//     public string playerId = "";  // 玩家id

//     public float x = 0;     // 位置
//     public float y = 0;
//     public float z = 0;
//     public float ex = 0;    // 旋转
//     public float ey = 0;
//     public float ez = 0;
// }

public sealed class MsgLose : MsgBase
{
    public MsgLose() { protoName = "MsgLose"; }
}

public sealed class MsgWin : MsgBase
{
    public MsgWin() { protoName = "MsgWin"; }
}

// 玩家退出
public sealed class MsgEscape : MsgBase
{
    public MsgEscape() { protoName = "MsgEscape"; }

    // push
    public string playerId = "";
}