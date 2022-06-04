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

// 同步敌人位置
public sealed class MsgEnemyPosition : MsgBase
{
    public MsgEnemyPosition() { protoName = "MsgEnemyPosition"; }

    public int wayPointsId = 0;
    public bool wayReverse = false;

    public int enemyId = 0;
}

// 炸弹投掷的坐标、角度、力度等
public sealed class MsgAssassinBomb : MsgBase
{
    public MsgAssassinBomb() { protoName = "MsgAssassinBomb"; }

    // position
    public float x;
    public float y;
    public float z;

    // force
    public float fx;
    public float fy;
    public float fz;
}

// 炸弹引爆，需要引爆的实际位置
public sealed class MsgMagicianTrigger : MsgBase
{
    public MsgMagicianTrigger() { protoName = "MsgMagicianTrigger"; }

    public float x;
    public float y;
    public float z;
}

// 哪个敌人死了
public sealed class MsgEnemyDied : MsgBase
{
    public MsgEnemyDied() { protoName = "MsgEnemyDied"; }

    public int enemyId = 0;
}

public sealed class MsgOpenDoor : MsgBase
{
    public MsgOpenDoor() { protoName = "MsgOpenDoor"; }

    public int doorId = 0;
    public string playerId = "";
}

public sealed class MsgGetBomb : MsgBase
{
    public MsgGetBomb() { protoName = "MsgGetBomb"; }

    public int itemId = 0;
    public string playerId = "";
}

// 敌人定身效果（技能）
public sealed class MsgImmobilized : MsgBase
{
    public MsgImmobilized() { protoName = "MsgImmobilized"; }

    public int enemyId = 0;
}