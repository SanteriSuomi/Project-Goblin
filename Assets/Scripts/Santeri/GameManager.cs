using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> rooms;
    [SerializeField]
    GameObject bossRoom;

    Transform player;

    int amountRoomsTravelled = 0;
    GameObject previousRoom;
    GameObject currentRoom;

    [SerializeField]
    float xPositionForNewRoom = 42.5f;
    [SerializeField]
    int amountOfRoomsUntilBoss = 10;

    bool canSpawn = true;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        AdvanceRoom();
    }

    void AdvanceRoom()
    {
        amountRoomsTravelled++;
        previousRoom = currentRoom;
        if (amountRoomsTravelled >= amountOfRoomsUntilBoss)
        {
            currentRoom = bossRoom;
            canSpawn = false;
        }
        else
        {
            currentRoom = PickRandomRoom();
        }
        if (previousRoom == null)
        {
            currentRoom = Instantiate(currentRoom, new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            currentRoom = Instantiate(currentRoom, new Vector3(previousRoom.transform.position.x + 75, 0, 0), Quaternion.identity);
        }
        currentRoom.SetActive(true);
    }

    GameObject PickRandomRoom()
    {
        return rooms[Random.Range(0, rooms.Count)];
    }

    void SetDoorCollider(GameObject room, bool val)
    {
        var children = room.GetComponentsInChildren<Collider>();
        foreach (var child in children)
        {
            if (child.name == "DoorCollider")
            {
                child.enabled = val;
                return;
            }
        }
    }

    private void Update()
    {
        if (canSpawn)
        {
            if (player.position.x - currentRoom.transform.position.x >= xPositionForNewRoom)
            {
                AdvanceRoom();
                SetDoorCollider(currentRoom, false);
            }
        }
        if (player.position.x >= currentRoom.transform.position.x - 0.5f)
        {
            SetDoorCollider(currentRoom, true);
            Destroy(previousRoom);
        }
    }
}
