namespace network.model.util;

using System;
using System.Collections.Generic;

using network.model;
using network.protocol;

public static class RoomManager
{
    // 当前已创建的房间的最大id，非常简单的自动上升，不回收
    // private static int curMaxRoomId = 1;
    // 房间键值对 {id: Room}
    public static Dictionary<string, Room> rooms = new Dictionary<string, Room>();

    // 创建房间
    public static Room AddRoom(string hostId)
    {
        Room room = new Room();
        room.id = hostId;
        rooms[hostId] = room;
        return room;
    }

    public static bool RemoveRoom(string id)
    {
        return rooms.Remove(id);
    }

    public static Room? GetRoom(string id)
    {
        if (rooms.ContainsKey(id))
        {
            return rooms[id];
        }
        return null;
    }

    // 至多每秒调用一次，判断有战况的房间的获胜情况
    public static void Update()
    {
        foreach (Room room in rooms.Values)
        {
            room.Update();
        }
    }
}

