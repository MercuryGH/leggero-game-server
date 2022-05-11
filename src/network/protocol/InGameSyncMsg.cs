namespace network.protocol;

// 同步玩家信息
public sealed class MsgPlayerPosition : MsgBase
{
    public MsgPlayerPosition() { protoName = "MsgPlayerPosition"; }

    // sync 位置、旋转、炮塔旋转
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float ex = 0f;
    public float ey = 0f;
    public float ez = 0f;
    // public float turretY = 0f;
    // public float gunX = 0f;

    // broadcast
    public string id = ""; // 服务端广播时，补充发送 MsgSyncTank 的玩家id
}

public sealed class MsgOpenDoor : MsgBase
{
    public MsgOpenDoor() { protoName = "MsgOpenDoor"; }

    public int doorId = 0;
    public string playerId = "";
}

public sealed class MsgGetItem : MsgBase
{
    public MsgGetItem() { protoName = "MsgGetItem"; }

    public int itemId = 0;
    public string playerId = "";
}

// 开火
public sealed class MsgFire : MsgBase
{
    public MsgFire() { protoName = "MsgFire"; }

    // sync 初始位置、旋转
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float ex = 0f;
    public float ey = 0f;
    public float ez = 0f;

    // broadcast
    public string id = "";
}

// 击中（由shooter发送，可以作弊）
// [System.Serializable]
// public sealed class MsgHit : MsgBase
// {
//     public MsgHit() { protoName = "MsgHit"; }

//     // sync
//     public string targetId = ""; 
//     // 击中点（该信息可以用于服务端计算伤害、辅助反作弊等）
//     public float x = 0f;
//     public float y = 0f;
//     public float z = 0f;

//     // broadcast
//     public string id = ""; // shooter id
//     public int hp = 0;     // target HP before being shooted
//     public int damage = 0; // hit damage
// }