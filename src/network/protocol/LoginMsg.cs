namespace network.protocol;

public sealed class MsgLogin : MsgBase
{
    public MsgLogin() { protoName = "MsgLogin"; }

    // request
    public string playerId = "";

    // response
    public int status = 0;
}