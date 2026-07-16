using UnityEngine;
using UnityEngine.Tilemaps;

public class PaintingPuzzle_n : MonoBehaviour
{
    public Painting_n[] paintings;

    public Tilemap doorTilemap;
    public Vector3Int doorTilePosition;
    public TileBase openDoorTile;

    public GameObject doorTriggerObject;

    public void CheckAnswer()
    {
        foreach (Painting_n painting in paintings)
        {
            if (!painting.isSolved)
            {
                return;
            }
        }

        OpenDoor();
    }

    void OpenDoor()
    {
        doorTilemap.SetTile(doorTilePosition, openDoorTile);

        if (doorTriggerObject != null)
        {
            doorTriggerObject.SetActive(true);
        }

        Debug.Log("絵画パズルクリア！");
    }
}
