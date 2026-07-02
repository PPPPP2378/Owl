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
        Debug.Log("石像数：" + statues.Length);

        for (int i = 0; i < statues.Length; i++)
        {
            Debug.Log("石像" + i +
                      " 武器：" + statues[i].currentWeapon +
                      " 正解：" + answer[i]);

            if (statues[i].currentWeapon != answer[i])
            {
                Debug.Log("石像" + i + " が違う");
                return;
            }
        }

        Debug.Log("全部正解");
        OpenDoor();
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
