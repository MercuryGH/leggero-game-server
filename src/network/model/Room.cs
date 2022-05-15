namespace network.model;

using System;
using System.Collections.Generic;

using network.util;
using network.model.util;
using network.protocol;

public class Room
{
    private const int maxPlayer = 2; // 最大玩家数

    public string id = "Default Room Name"; // 房间 id
    public Dictionary<string, bool> playerIds { get; private set; } = new Dictionary<string, bool>(); // 玩家列表，其实是HashSet

    private string ownerId = ""; // 房主 id

    public enum Status // 房间状态
    {
        WAITING = 0, // 等待开始
        IN_GAME = 1, // 正在战斗
    }
    public Status status = Status.WAITING;

    // 硬编码出生点
    private static readonly float[,,] birthConfig = new float[2, 3, 6] {
		// 阵营1出生点
		{
            {262.3f, -8.0f, 342.7f, 0, -151.0f, 0f},
            {229.7f, -5.5f, 354.4f, 0, -164.2f, 0f},
            {197.1f, -3.6f, 347.7f, 0, -193.0f, 0f},
        },
		// 阵营2出生点
		{
            {-80.3f,  9.5f, 114.6f, 0, -294.0f,  0f},
            {-91.1f, 15.5f, 139.1f, 0, -294.2f, 0f},
            {-62.3f,  1.2f, 76.1f,  0, -315.4f, 0f},
        },
    };

    private bool isSinglePlay = false; // 是否为训练房 (TODO: implements it)

    private long lastJudgeWinLoseTime = 0; // 上一次判断胜负的时间
    private const long JUDGE_WINLOSE_INTERVAL = 5; // 两次判断胜负的时间间隔

    // 添加玩家
    public bool AddPlayer(string id)
    {
        InGamePlayer? inGamePlayer = InGamePlayerManager.GetPlayerById(id);
        if (inGamePlayer == null)
        {
            Console.WriteLine("room.AddPlayer failed, player is null");
            return false;
        }
        if (playerIds.Count >= maxPlayer)
        {
            Console.WriteLine("room.AddPlayer failed, reach maxPlayer");
            return false;
        }
        if (status != Status.WAITING)
        {
            Console.WriteLine("room.AddPlayer failed, room status is not WAITING");
            return false;
        }
        if (playerIds.ContainsKey(id) == true)
        {
            Console.WriteLine("room.AddPlayer fail, already in this room");
            return false;
        }
        // 自动分配阵营，设置房间id
        inGamePlayer.roleId = AutoSetTeam();
        inGamePlayer.roomId = this.id;

        // 设置房主
        if (ownerId == "")
        {
            ownerId = inGamePlayer.id;
        }

        // 加入玩家集合
        // TODO: 并发控制（可能在同一时间有很多人加入房间）
        playerIds[id] = true; // playerIds.insert(id);

        MsgBase broadCastedRoomInfo = GenerateGetPlayerInfoInRoomMsg();
        BroadcastExceptPlayer(broadCastedRoomInfo, id);

        return true;
    }

    // 自动分配阵营
    public int AutoSetTeam()
    {
        int count1 = 0;
        int count2 = 0;
        foreach (string id in playerIds.Keys)
        {
            InGamePlayer player = InGamePlayerManager.GetPlayerById(id)!;
            if (player.roleId == 0) { count1++; }
            if (player.roleId == 1) { count2++; }
        }

        Console.WriteLine("DEBUG: " + count1 + " " + count2);

        // 选择人数少的阵营。若人数一致，则选择阵营1
        if (count1 <= count2)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    // 判断传入的 BattlePlayer 是不是房主
    public bool isOwner(InGamePlayer battlePlayer)
    {
        return battlePlayer.id == ownerId;
    }

    // 删除玩家
    public bool RemovePlayer(string id)
    {
        // 获取玩家
        InGamePlayer? player = InGamePlayerManager.GetPlayerById(id);
        if (player == null)
        {
            Console.WriteLine("room.RemovePlayer fail, player is null");
            return false;
        }
        if (!playerIds.ContainsKey(id))
        {
            Console.WriteLine("room.RemovePlayer fail, not in this room");
            return false;
        }

        playerIds.Remove(id); // playerIds.remove(id)
        player.roleId = 0;
        player.isInRoom = false;

        // 自动设置房主
        if (ownerId == player.id)
        {
            ownerId = AutoSwitchOwner();

            // 房间id = 房主id
            this.id = ownerId;
        }

        // 退出后，房间为空（后续广播逻辑也不必再做）
        Console.WriteLine("******* Room Exited ********");
        if (playerIds.Count == 0)
        {
            Console.WriteLine("Remove room");
            RoomManager.RemoveRoom(this.id);
            return true;
        }

        // 战斗状态退出
        if (status == Status.IN_GAME)
        {
            // player.playerData.lose++;
            MsgEscape msg = new MsgEscape();
            msg.playerId = player.id;
            Broadcast(msg);
        }
        else // 非状态战斗退出
        {
            // 对除了退出者的全体玩家广播
            MsgBase broadCastedRoomInfo = GenerateGetPlayerInfoInRoomMsg();
            BroadcastExceptPlayer(broadCastedRoomInfo, id);
        }

        return true;
    }
    
    public void SetPlayerRole(string playerId, int roleId)
    {
        InGamePlayer? player = InGamePlayerManager.GetPlayerById(playerId);
        if (player == null)
        {
            return;
        }

        player.roleId = roleId;

        MsgBase broadCastedRoomInfo = GenerateGetPlayerInfoInRoomMsg();
        Broadcast(broadCastedRoomInfo);
    }

    // 选择房主
    public string AutoSwitchOwner()
    {
        // 选择第一个玩家
        foreach (string id in playerIds.Keys)
        {
            return id;
        }
        // 房间没人
        return "";
    }

    // 广播消息
    public void Broadcast(MsgBase msg)
    {
        foreach (string id in playerIds.Keys)
        {
            InGamePlayer battlePlayer = InGamePlayerManager.GetPlayerById(id)!;
            battlePlayer.SendToSocket(msg);
        }
    }

    // 除了 exceptId，广播消息
    public void BroadcastExceptPlayer(MsgBase msg, string exceptId)
    {
        foreach (string id in playerIds.Keys)
        {
            if (id == exceptId)
            {
                continue;
            }
            InGamePlayer battlePlayer = InGamePlayerManager.GetPlayerById(id)!;
            battlePlayer.SendToSocket(msg);
        }
    }

    // 生成 MsgGetPlayerInfoInRoom 协议
    public MsgBase GenerateGetPlayerInfoInRoomMsg()
    {
        MsgGetPlayerInfoInRoom msg = new MsgGetPlayerInfoInRoom(playerIds.Count);

        int i = 0;
        foreach (string id in playerIds.Keys)
        {
            InGamePlayer battlePlayer = InGamePlayerManager.GetPlayerById(id)!;

            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.playerId = battlePlayer.id;
            playerInfo.roleId = battlePlayer.roleId;
            playerInfo.isOwner = isOwner(battlePlayer) ? 1 : 0;

            msg.players![i] = playerInfo;
            i++;
        }
        return msg;
    }

    // 能否开始游戏
    // 两个职业都必须有人
    private bool CanStartGame()
    {
        // 已经是战斗状态
        if (status == Status.IN_GAME)
        {
            return false;
        }

        bool hasRole0 = false;
        bool hasRole1 = false;
        foreach (string id in playerIds.Keys)
        {
            InGamePlayer player = InGamePlayerManager.GetPlayerById(id)!;
            if (player.roleId == 1) { hasRole1 = true; }
            else { hasRole0 = true; }
        }
        if (hasRole0 && hasRole1)
        {
            return true;
        }

        isSinglePlay = true;
        return false;
    }

    // 根据传来的玩家索引 index，初始化 battlePlayer 的位置（出生点）
    // private void SetBirthPos(InGamePlayer battlePlayer, int index)
    // {
    //     int camp = battlePlayer.roleId;

    //     battlePlayer.x = birthConfig[camp - 1, index, 0];
    //     battlePlayer.y = birthConfig[camp - 1, index, 1];
    //     battlePlayer.z = birthConfig[camp - 1, index, 2];
    //     battlePlayer.ex = birthConfig[camp - 1, index, 3];
    //     battlePlayer.ey = birthConfig[camp - 1, index, 4];
    //     battlePlayer.ez = birthConfig[camp - 1, index, 5];
    // }

    // 玩家数据转成TankInfo，便于发送数据包
    // private PlayerPositionInfo Player2PositionInfo(InGamePlayer player)
    // {
    //     PlayerPositionInfo tankInfo = new PlayerPositionInfo();
    //     tankInfo.playerId = player.id;
    //     // tankInfo.hp = player.hp;

    //     tankInfo.x = player.x;
    //     tankInfo.y = player.y;
    //     tankInfo.z = player.z;
    //     tankInfo.ex = player.ex;
    //     tankInfo.ey = player.ey;
    //     tankInfo.ez = player.ez;

    //     return tankInfo;
    // }

    // 重置玩家战斗属性
    // private void ResetPlayers()
    // {
    //     //位置和旋转
    //     int count1 = 0;
    //     int count2 = 0;
    //     foreach (string id in playerIds.Keys)
    //     {
    //         InGamePlayer player = InGamePlayerManager.GetPlayerById(id)!;
    //         if (player.roleId == 1)
    //         {
    //             SetBirthPos(player, count1);
    //             count1++;
    //         }
    //         else
    //         {
    //             SetBirthPos(player, count2);
    //             count2++;
    //         }
    //         // player.hp = 100;
    //     }
    // }

    // 开战，并广播 MsgEnterBattle
    public bool StartGame()
    {
        if (CanStartGame() == false)
        {
            return false;
        }
        status = Status.IN_GAME;

        const int stageId = 1; // 目前只有一个地图，硬编码mapId

        // ResetPlayers();

        MsgBase msg = GenerateEnterGameMsg(stageId);
        Broadcast(msg);
        return true;
    }

    private MsgBase GenerateEnterGameMsg(int stageId)
    {
        MsgEnterGame msg = new MsgEnterGame(playerIds.Count, stageId);

        // 组装数据包
        // int i = 0;
        // foreach (string id in playerIds.Keys)
        // {
        //     InGamePlayer player = InGamePlayerManager.GetPlayerById(id)!;

        //     // TODO: 改为服务端指定出生点
        //     // msg.tanks[i] = PlayerToTankInfo(player);
        //     i++;
        // }
        return msg;
    }

    public bool IsDie(InGamePlayer player)
    {
        // return player.hp <= 0;
        throw new NotImplementedException();
    }

    // 定时更新
    public void Update()
    {
        if (status != Status.IN_GAME)
        {
            return;
        }

        // 两次更新的时间间隔至少是 5s，避免服务端压力过大
        if (NetManager.GetTimeStamp() - lastJudgeWinLoseTime < JUDGE_WINLOSE_INTERVAL)
        {
            return;
        }
        lastJudgeWinLoseTime = NetManager.GetTimeStamp();

        // 胜负判断
        int winLose = JudgeWinLose();
        // 尚未分出胜负
        if (winLose == 0)
        {
            return;
        }

        // 某一方胜利，结束战斗，房间变为等待状态
        status = Status.WAITING;
        // 统计信息
        // foreach (string id in playerIds.Keys)
        // {
        //     BattlePlayer player = BattlePlayerManager.GetPlayerById(id)!;
        //     if (player.team == winCamp) { player.playerData.win++; }
        //     else { player.playerData.lose++; }
        // }

        // 发送 Result
        if (winLose == 1) {
            MsgWin msg = new MsgWin();
            Broadcast(msg);
        }
    }

    /**
     * 定时调用的胜负判断
     * @return 0 if 还未分出胜负; 1 if 胜利; 2 if 失败
     */
    private int JudgeWinLose()
    {
        return 0;
        // return 1;
    }
}
