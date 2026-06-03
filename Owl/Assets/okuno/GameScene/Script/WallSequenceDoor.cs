using UnityEngine;
using UnityEngine.Tilemaps;

public class WallSequenceDoor_n : MonoBehaviour
{
    public Tilemap doorTilemap;
    public Vector3Int doorTilePosition;
    public TileBase openDoorTile;

    public MysteryWall_n[] walls;

    private int currentStep = 1;

    private int[] correctOrder = { 2, 3, 1 };

    private int currentIndex = 0;

    public void CheckWall(int wallNumber)
    {
        if (wallNumber == correctOrder[currentIndex])
        {
            Debug.Log("ê≥â");

            currentIndex++;

            if (currentIndex >= correctOrder.Length)
            {
                OpenDoor();
            }
        }
        else
        {
            Debug.Log("èáî‘Ç™à·Ç§");

            ResetWalls();
        }
    }

    void OpenDoor()
    {
        doorTilemap.SetTile(doorTilePosition, openDoorTile);
    }

    public void ResetWalls()
    {
        currentIndex = 0;

        foreach (MysteryWall_n wall in walls)
        {
            wall.isChecked = false;
        }

        Debug.Log("ï«ÉäÉZÉbÉgäÆóπ");
    }
}