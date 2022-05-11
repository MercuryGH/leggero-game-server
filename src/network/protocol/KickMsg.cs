namespace network.protocol;

public sealed class MsgKick : MsgBase
{
    public MsgKick() { protoName = "MsgKick"; }

    // push 被踢原因（0-其他人登陆同一账号）
    public int reason = 0;
}