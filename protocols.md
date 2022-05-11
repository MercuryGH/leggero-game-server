# 协议设计

> 默认的`broadcast`的对象不包含发送者自身，但`broadcast_include_src`将会包含发送者自身。

## 大厅`HallMsg`

### 创建房间`MsgCreateRoom`

request:
```C
hostId: string // 房主id
```

response:
```C
status: int // 状态码
```

有关状态码的含义：0为成功，1为失败，其他非零整数可表示失败详细信息（其他封装协议的状态码设计同理）

当`hostId`冲突时，创建房间将会失败，`status=1`。

### 根据房主id进入房间`MsgEnterRoom`

request:
```C
hostId: string
```

response:
```C
status: int
```

房间满人（已有两人）或者房间已在游戏中时，返回`status = 1`。

## 房间`RoomMsg`

### 获取房间中玩家的信息`MsgGetPlayerInfoInRoom`

将在以下三种情况中，发生任意一种后

* 其他玩家成功进入当前房间后
* 当前房间的某个玩家退出房间后
* 当前玩家中的某个玩家选择了职业后

由服务器自动向该房间中的所有玩家广播推送。

结构体定义：
```c
struct PlayerInfo {
    playerId: string // 玩家的id
    roleId: int      // 玩家选择的职业
    isOwner: bool    // 是否是房主
};
```

push_broadcast:
```c
players: PlayerInfo[]
```

roleId编码规则：0为刺客，1为法师。

### 选择职业`MsgSelectRole`

sync:
```C
roleId: int
```

broadcast:
```C
playerId: string
roleId: int
```

### 离开房间`MsgLeaveRoom`

仅涉及单个玩家离开房间。其他玩家获知该信息将通过服务器自行推送`MsgGetPlayerInfoInRoom`。

request:
```c
```

response:
```c
status: int
```

必定成功。因此`status=0`恒成立。

**离开房间后，房间名会更改为新的房主的房间名。**

### 请求开始游戏`MsgStartGame`

request:
```C
stageId: int // 关卡id
```

response:
```C
status: int
```

若来源不为房主，服务端返回`status=1`。

当两名玩家选择的职业相同时，服务端返回`status=1`。否则，服务端返回`status=0`，并广播`MsgEnterGame`。

### 被动进入游戏`MsgEnterGame`

push:
```c
stageId: int
```

## 游戏逻辑`InGameMsg`

### 玩家的位置同步`MsgPlayerPosition`

sync:
```C
x, y, z, ex, ey, ez: float 
```

broadcast:
```C
x, y, z, ex, ey, ez: float
id: string
```

### 门的开启`MsgOpenDoor`

门只能开启不能关闭。

sync:
```C
doorId: string
```

broadcast:
```C
doorId: string
playerId: string  // 谁开的门
```

### 物品的拾取`MsgGetItem`

sync:
```C
itemId: string
```

broadcast:
```C
itemId: string
playerId: string  // 谁拾取的物品
```

### 不同角色不同技能的施放

TODO

### 游戏失败`MsgLose`

游戏失败数据包由客户端发送，然后服务端广播。没有任何字段。

sync:
```
```

broadcast:
```
```

### 游戏胜利`MsgWin`

游戏胜利数据由服务端广播推送。没有任何字段。

push_broadcast:
```
```

### 玩家中途掉线或离开`MsgEscape`

由服务端检测掉线逻辑。

broadcast:
```c
playerId: string // 掉线的玩家id
```

## 其他

### Ping-Pong心跳`MsgPing`、`MsgPong`

### 服务器检测到故障，主动断开连接之前的踢下线`MsgKick`

