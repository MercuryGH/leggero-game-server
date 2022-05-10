# 协议设计

## 房间

### 创建房间`MsgCreateRoom`

request:
```java
hostId: string // 房主id
```

response:
```java
status: number // 状态码
```

有关状态码的含义：0为成功，1为失败，其他非零整数可表示失败详细信息（其他封装协议的状态码设计同理）

### 根据房主id进入房间`MsgEnterRoom`

request:
```java
hostId: string
```

response:
```java
status: int
```

### 获取房间中第二名玩家的信息`MsgGetRoomInfo`

将在有玩家成功进入房间的情况下，由服务器自动向该房间中的所有玩家广播推送。

push:
```java
playerId: string // 第二名玩家的id
roleId: number   // 第二名玩家选择的职业
```

roleId编码规则：0为刺客，1为法师。

### 选择职业`MsgSelectRole`

sync:
```java
roleId: number
```

broadcast:
```java
roleId: number
```

### 开始游戏`MsgStartGame`

仅在来源为房主时有效。

sync:
```java
stageId: number // 关卡id
```

broadcast_include_src:
```java
status: number
```

默认的`broadcast`的对象不包含发送者自身，但`broadcast_include_src`将会包含发送者自身。

## 游戏逻辑

### 位置与朝向的同步

### 门的开启

### 物品的拾取

### 不同角色不同技能的施放

### 游戏失败

### 游戏胜利

