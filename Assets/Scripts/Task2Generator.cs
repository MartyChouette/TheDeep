using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Task2Generator : MonoBehaviour
{
    // 3D grid dimensions
    const int GRID_WIDTH = 5;
    const int GRID_HEIGHT = 3;
    const int GRID_DEPTH = 4;
    const int MAX_TRIES = 50;

    
    [SerializeField] List<RoomPiece> roomPrefabs;

    // the start and finish rooms (contain player in start and enemy at finish)
    [SerializeField] RoomPiece startRoom;
    [SerializeField] RoomPiece finishRoom;

    // 3D grid of cells superposition tracking possible room indices
    SuperPosition[,,] _grid;

    // global cell size determined by the max size of all room prefabs
    Vector3 globalCellSize;

    // cache calculated sizes for prefabs
    Dictionary<RoomPiece, Vector3> _prefabSizeCache = new Dictionary<RoomPiece, Vector3>();

    void Start()
    {
        CalculateGlobalCellSize();

        int tries = 0;
        bool result;
        do
        {
            tries++;
            result = RunWFC();
        } while (!result && tries < MAX_TRIES);

        if (!result)
        {
            Debug.Log("Unable to solve wave function collapse after " + tries + " tries.");
        }
        else
        {
            DrawRooms();
        }
    }

    bool RunWFC()
    {
        InitGrid();

        while (DoUnobservedNodesExist())
        {
            Vector3Int node = GetNextUnobservedNode();
            if (node.x == -1)
            {
                return false; 
            }

            int observedValue = Observe(node);
            PropagateNeighbors(node, observedValue);
        }

        return true; // success every cell chosen
    }

    // instantiate the room pieces in 3D space using the global cell size
    void DrawRooms()
    {
        for (int x = 0; x < GRID_WIDTH; x++)
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int z = 0; z < GRID_DEPTH; z++)
                {
                    RoomPiece roomPrefab = null;

                    // use fixed coordinates for start and finish
                    if (x == 0 && y == 0 && z == 0)
                    {
                        roomPrefab = startRoom;
                    }
                    else if (x == GRID_WIDTH - 1 && y == GRID_HEIGHT - 1 && z == GRID_DEPTH - 1)
                    {
                        roomPrefab = finishRoom;
                    }
                    else
                    {
                        int observed = _grid[x, y, z].GetObservedValue();
                        roomPrefab = roomPrefabs[observed];
                    }

                    GameObject room = Instantiate(roomPrefab.gameObject);
                    // space rooms by multiplying grid indices by globalCellSize
                    room.transform.position = new Vector3(x * globalCellSize.x, y * globalCellSize.y, z * globalCellSize.z);
                }
            }
        }
    }

    bool DoUnobservedNodesExist()
    {
        for (int x = 0; x < GRID_WIDTH; x++)
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int z = 0; z < GRID_DEPTH; z++)
                {
                    if (!_grid[x, y, z].IsObserved())
                        return true;
                }
            }
        }
        return false;
    }

    int Observe(Vector3Int node)
    {
        return _grid[node.x, node.y, node.z].Observe();
    }

    // initialize 3D grid with a superposition for each cell
    // each superposition initialized with roomPrefabs.Count possibilities
    void InitGrid()
    {
        _grid = new SuperPosition[GRID_WIDTH, GRID_HEIGHT, GRID_DEPTH];
        for (int x = 0; x < GRID_WIDTH; x++)
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int z = 0; z < GRID_DEPTH; z++)
                {
                    _grid[x, y, z] = new SuperPosition(roomPrefabs.Count);
                }
            }
        }
    }

    // propagate chosen result to 6 neighbors
    void PropagateNeighbors(Vector3Int node, int observedValue)
    {
        RoomPiece observedRoomPiece = roomPrefabs[observedValue];

        PropagateTo(node, new Vector3Int(-1, 0, 0), observedRoomPiece);
        PropagateTo(node, new Vector3Int(1, 0, 0), observedRoomPiece);
        PropagateTo(node, new Vector3Int(0, -1, 0), observedRoomPiece);
        PropagateTo(node, new Vector3Int(0, 1, 0), observedRoomPiece);
        PropagateTo(node, new Vector3Int(0, 0, -1), observedRoomPiece);
        PropagateTo(node, new Vector3Int(0, 0, 1), observedRoomPiece);
    }

    // propagate constraints from a chosen cell to neighbor
    // check connectors remove invalid choices from neighbor
    void PropagateTo(Vector3Int node, Vector3Int direction, RoomPiece mustWorkAdjacentTo)
    {
        Vector3Int neighbor = node + direction;

        if (neighbor.x < 0 || neighbor.x >= GRID_WIDTH ||
            neighbor.y < 0 || neighbor.y >= GRID_HEIGHT ||
            neighbor.z < 0 || neighbor.z >= GRID_DEPTH)
            return;

        if (_grid[neighbor.x, neighbor.y, neighbor.z].IsObserved())
            return;

        List<int> options = _grid[neighbor.x, neighbor.y, neighbor.z].GetPossibleValues();

        foreach (int option in options)
        {
            RoomPiece candidateRoomPiece = roomPrefabs[option];
            bool valid = true;

            // check connector compatibility between room pieces
            if (direction == new Vector3Int(-1, 0, 0))
            {
                valid = candidateRoomPiece._rightConnector == mustWorkAdjacentTo._leftConnector;
            }
            else if (direction == new Vector3Int(1, 0, 0))
            {
                valid = candidateRoomPiece._leftConnector == mustWorkAdjacentTo._rightConnector;
            }
            else if (direction == new Vector3Int(0, -1, 0))
            {
                valid = candidateRoomPiece._upConnector == mustWorkAdjacentTo._downConnector;
            }
            else if (direction == new Vector3Int(0, 1, 0))
            {
                valid = candidateRoomPiece._downConnector == mustWorkAdjacentTo._upConnector;
            }
            else if (direction == new Vector3Int(0, 0, -1))
            {
                valid = candidateRoomPiece._frontConnector == mustWorkAdjacentTo._backConnector;
            }
            else if (direction == new Vector3Int(0, 0, 1))
            {
                valid = candidateRoomPiece._backConnector == mustWorkAdjacentTo._frontConnector;
            }

            if (!valid)
            {
                _grid[neighbor.x, neighbor.y, neighbor.z].RemovePossibleValue(option);
            }
        }
    }

    // returns the next unobserved cell with the fewest choices
    Vector3Int GetNextUnobservedNode()
    {
        Vector3Int pick = new Vector3Int(-1, -1, -1);
        int minOptions = int.MaxValue;

        for (int x = 0; x < GRID_WIDTH; x++)
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int z = 0; z < GRID_DEPTH; z++)
                {
                    if (!_grid[x, y, z].IsObserved())
                    {
                        int options = _grid[x, y, z].NumOptions;
                        if (options < minOptions)
                        {
                            minOptions = options;
                            pick = new Vector3Int(x, y, z);
                        }
                    }
                }
            }
        }
        return pick;
    }


    // calculate and hold the max size of any room prefab from roomPrefabs
    void CalculateGlobalCellSize()
    {
        globalCellSize = Vector3.zero;
        foreach (RoomPiece roomPiece in roomPrefabs)
        {
            Vector3 size = GetPrefabSize(roomPiece);
            globalCellSize = Vector3.Max(globalCellSize, size);
        }
    }

    // retrieve the size of the prefab using its renderer or collider
    Vector3 GetPrefabSize(RoomPiece roomPiece)
    {
        if (_prefabSizeCache.ContainsKey(roomPiece))
        {
            return _prefabSizeCache[roomPiece];
        }

        GameObject temp = Instantiate(roomPiece.gameObject);
        temp.SetActive(false);

        Vector3 size = Vector3.zero;
        Renderer rend = temp.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            size = rend.bounds.size;
        }
        else
        {
            Collider col = temp.GetComponentInChildren<Collider>();
            if (col != null)
            {
                size = col.bounds.size;
            }
            else
            {
                size = Vector3.one;
            }
        }

        Destroy(temp);
        _prefabSizeCache[roomPiece] = size;
        return size;
    }
}
