using UnityEngine;
//When constructing a level, you’d generally use these booleans as a kind of “blueprint” for each room. Here’s a high‐level approach:
//1.Room Representation and Layout

//    Grid/Graph Structure:
//    Think of your level as a grid or graph where each node represents a room. Each room’s data (with its exit booleans) tells you which sides can connect to neighboring rooms.

//    Exits vs. Internal Paths:
//    The exit booleans (hasNorthExit, etc.) indicate where a room can connect to adjacent rooms. The path booleans (hasNSPath, hasNWPath, etc.) determine if there’s an internal corridor or passage linking two exits within the same room (for example, a corridor connecting the north and south exits).

//2. Procedural Generation Flow

//    Layout Generation:
//    First, decide on your overall level layout. When placing a room, check the surrounding rooms to ensure consistency. For instance, if a room has a north exit, the room above should have a corresponding south exit.

//    Room Instantiation:
//    Use the exit information to select or modify a room prefab. For example, if hasNorthExit is true, your prefab might have a door or opening on its north wall.

//    Connecting Rooms:
//    Once the rooms are placed, use the path booleans to create corridors or hallways that connect the exits within a room. For example:
//        hasNSPath: If true, carve or place corridor geometry that directly connects the north and south exits.
//        hasNWPath, hasNEPath, etc.: Similarly, these can be used to design angled or diagonal corridors linking the respective exits.

//3. Implementation Considerations

//    Prefab Variations:
//    Depending on the values of your booleans, you might have several variations of room prefabs. A room with just a north and south exit might use a simpler design than one with multiple diagonal connections.

//    Dynamic Geometry:
//    Alternatively, you can design a modular system where the room is built up from pieces (walls, door segments, corridor pieces). The boolean flags then trigger which pieces to add or remove at runtime.

//    Validation:
//    Make sure to verify that the connections are consistent. For instance, if one room has a north exit, the room above it must have a south exit, or else you risk creating isolated spaces.

//4. Example Workflow

//    Generate Map Data:
//    Create a 2D array (or graph) representing your rooms. For each room, assign the exit booleans based on your procedural algorithm (like a maze generator or random walk).

//    Room Placement:
//    Loop through your grid and instantiate room objects at the proper coordinates. Use the exit booleans to determine which walls to “open” (e.g., remove or replace with door models).

//    Construct Internal Paths:
//    Inside each room, check for the internal path booleans.If, say, hasNSPath is true, modify the room’s geometry (or spawn a corridor segment) that connects the north and south exits. Repeat for the other path booleans.

//    Connect Adjacent Rooms:
//    Finally, add corridor or doorway geometry between adjacent rooms if their corresponding exits match.

//Summary

//By treating your boolean flags as directives for both room connectivity (which room touches which) and internal routing (how exits connect within a room), you can build a flexible system for procedurally generating levels. This modular approach lets you easily tweak the layout and room design while ensuring that all parts of the level interconnect seamlessly.

//This high-level plan provides a framework, but you’ll need to iterate and adjust based on the specifics of your engine, room geometry, and gameplay requirements.
public class Room : MonoBehaviour
{

    public bool hasNorthExit;
    public bool hasSouthExit;
    public bool hasWestExit;
    public bool hasEastExit;

    public bool hasNSPath;
    public bool hasNWPath;
    public bool hasNEPath;
    public bool hasSWPath;
    public bool hasSEPath;
    public bool hasEWPath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
