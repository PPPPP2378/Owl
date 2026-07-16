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

        // 間違っている石像だけリセット
        for (int i = 0; i < statues.Length; i++)
        {
            if (statues[i].currentWeapon != answer[i])
            {
                allCorrect = false;

                if (statues[i].currentItem != null)
                {
                    Debug.Log("戻す武器：" + statues[i].currentItem.itemName);
                    // 武器をインベントリに戻す
                    statues[i].currentItem.isPlaced = false;
                    statues[i].currentItem = null;
                }

                // 石像の武器を消す
                statues[i].SetWeapon(WeaponType_n.None);

                // インベントリ表示を更新
                InventoryUI_n.Instance.RefreshInventory();
            }
        }

        // インベントリ表示を更新
       // InventoryUI_n.Instance.RefreshInventory();

        // 全部正解なら扉を開ける
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
