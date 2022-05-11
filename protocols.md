# 协议设计

## 房间

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

### 根据房主id进入房间`MsgEnterRoom`

request:
```C
hostId: string
```

response:
```C
status: int
```

### 获取房间中第二名玩家的信息`MsgGetRoomInfo`

将在有玩家成功进入房间的情况下，由服务器自动向该房间中的所有玩家广播推送。

push:
```C
playerId: string // 第二名玩家的id
roleId: int   // 第二名玩家选择的职业
```

roleId编码规则：0为刺客，1为法师。

### 选择职业`MsgSelectRole`

sync:
```C
roleId: int
```

broadcast:
```C
roleId: int
```

### 开始游戏`MsgStartGame`

仅在来源为房主时有效。

sync:
```C
stageId: int // 关卡id
```

broadcast_include_src:
```C
status: int
```

默认的`broadcast`的对象不包含发送者自身，但`broadcast_include_src`将会包含发送者自身。

## 游戏逻辑

### 玩家的位置同步`MsgPlayerPosition`

包括Position。

sync:
```C
x, y, z, ex, ey, ez: float
```

broadcast:
```C
x, y, z, ex, ey, ez: float
playerId: string
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
```

### 物品的拾取`MsgGetItem`

sync:
```C
itemId: string
```

broadcast:
```C
itemId: string
```

### 不同角色不同技能的施放

TODO

### 游戏失败`MsgLose`

游戏失败数据包由客户端发送，然后服务端广播。没有额外字段。

sync:
```
```

broadcast:
```
```

### 游戏胜利`MsgWin`

游戏胜利数据由服务端广播推送。没有额外字段。

push_broadcast:
```
```