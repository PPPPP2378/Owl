using UnityEngine;
using UnityEngine.Tilemaps;

public class WallSequenceDoor_n : MonoBehaviour
{
    public Tilemap doorTilemap;
    public Vector3Int doorTilePosition;
    public TileBase openDoorTile;

    public GameObject doorTriggerObject;

    public MysteryWall_n[] walls;

    private int[] correctOrder = { 2, 3, 1 };

    private int currentIndex = 0;

    public void CheckWall(int wallNumber)
    {
        if (wallNumber == correctOrder[currentIndex])
        {
            Debug.Log("正解");

            currentIndex++;

            if (currentIndex >= correctOrder.Length)
            {
                OpenDoor();
            }
        }
        else
        {
            Debug.Log("順番が違う");

            ResetWalls();
        }
    }

    void OpenDoor()
    {
        doorTilemap.SetTile(doorTilePosition, openDoorTile);

        if (doorTriggerObject != null)
        {
            doorTriggerObject.SetActive(true);
        }

        Debug.Log("扉が開いた");
    }

    public void ResetWalls()
    {
        currentIndex = 0;

        foreach (MysteryWall_n wall in walls)
        {
            wall.isChecked = false;
        }

        Debug.Log("壁リセット完了");
    }
}