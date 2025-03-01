using UnityEngine;

public class RoomGenerator : MonoBehaviour
{

    public GameObject[] rooms ;
    public int[] weight ;
    public string[] doorPositions;
    public GameObject roomPlacerLocation;
    public float roomSize = 2f ;
    public int howManyRooms = 10;


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateDungeon();

    }

    // Update is called once per frame
   
    void GenerateDungeon()
    {
        for (int i = 0; i < howManyRooms; i++)
        {
            GameObject chosenRoom = RandomRoom();
            PlaceRoom(chosenRoom);
            MovePlacer();
        }
    }

    void PlaceRoom(GameObject room)
    {
        Instantiate(room, roomPlacerLocation.transform.position,Quaternion.identity);
        Debug.Log("CREATE ROOM" + roomPlacerLocation.transform.position);
    }

    void MovePlacer()
    {
        roomPlacerLocation.transform.position += new Vector3(0f, 0f, roomSize);
        Debug.Log("MOVE ROOM PLACER" + roomPlacerLocation.transform.position);
    }

    GameObject RandomRoom()
    {
        int randomIndex = Random.Range(0, rooms.Length);
        return rooms[randomIndex];
    }
}
