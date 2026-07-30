using UnityEngine;
using UnityEngine.Tilemaps;

public class StatuePuzzle_n : MonoBehaviour
{
    public Tilemap doorTilemap;
    public Vector3Int doorTilePosition;
    public TileBase openDoorTile;
    public GameObject doorTriggerObject;

    public Statue_n[] statues;

    public WeaponType_n[] answer =
    {
        WeaponType_n.Sword,
        WeaponType_n.Axe,
        WeaponType_n.Shield,
        WeaponType_n.Spear,
        WeaponType_n.Bow
    };

    public void CheckAnswer()
    {
        bool allCorrect = true;

        for (int i = 0; i < statues.Length; i++)
        {
            if (statues[i].currentWeapon != answer[i])
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            Debug.Log("全部正解");
            OpenDoor();
        }
    }

    public bool IsAllPlaced()
    {
        foreach (Statue_n statue in statues)
        {
            if (statue.currentWeapon == WeaponType_n.None)
                return false;
        }
        return true;
    }

    void OpenDoor()
    {
        doorTilemap.SetTile(doorTilePosition, openDoorTile);

        if (doorTriggerObject != null)
        {
            doorTriggerObject.SetActive(true);
        }

        Debug.Log("石像パズルクリア！");
    }
}
