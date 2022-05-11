using System;

using network;
using network.protocol;
using network.util;

using System.Reflection;

namespace GameServer
{
    public class GameServerEntry
    {
        private const int PORT = 25695; // bind port = CLMXJ

        public static void Main(string[] args)
        {
            try {
                NetManager.StartLoop(PORT);
            } catch (Exception e) {
                Console.WriteLine("Uncaught exception: " + e);
            }
        }

        private static void TestMsg()
        {
            string protoName = "MsgRegister";
            // 分发消息
            MethodInfo mi = typeof(MsgHandler).GetMethod(protoName)!;
            MethodInfo m = typeof(MsgHandler).GetMethod(nameof(MsgHandler.MsgLeaveRoom))!;
            Console.WriteLine("Receive " + protoName);
            if (mi != null)
            {
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine("Bad + " + protoName);
            }

            if (m != null)
            {
                Console.WriteLine(nameof(MsgHandler.MsgLeaveRoom));
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine("Bad + " + protoName);
            }

            // MsgMove msgMove = (MsgMove)msgBase;
            // Console.WriteLine(msgMove.x);
            // msgMove.x++;
            // NetManager.Send(c, msgMove);
        }

        private static void TestSerialize()
        {
            // 测试JSON串行化反串行化
            MsgPlayerPosition msgSyncTank = new MsgPlayerPosition();
            msgSyncTank.x = 100;
            msgSyncTank.y = -20;
            byte[] bytes = MsgManager.Encode(msgSyncTank);

            MethodInfo decodeMethod = typeof(MsgBase).GetMethod(nameof(MsgManager.Decode))!;
            MethodInfo generic = decodeMethod.MakeGenericMethod(Type.GetType("network.protocol.MsgSyncTank")!);
            MsgBase msg = (MsgBase)generic.Invoke(null, new object[] { bytes, 0, bytes.Length })!;

            byte[] secondBytes = MsgManager.Encode(msg);

            string s = System.Text.Encoding.UTF8.GetString(bytes);
            Console.WriteLine(s);
            string t = System.Text.Encoding.UTF8.GetString(secondBytes);
            Console.WriteLine(t);
        }
    }
}
