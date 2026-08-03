//Statue_n.cs
using UnityEngine;

public class Statue_n : MonoBehaviour
{
    public int statueID;
    public Transform weaponPoint;

    public GameObject swordPrefab;
    public GameObject axePrefab;
    public GameObject shieldPrefab;
    public GameObject spearPrefab;
    public GameObject bowPrefab;

    public ItemData_n currentItem;

    private GameObject currentWeaponObject;

    public WeaponType_n currentWeapon = WeaponType_n.None;

    public StatuePuzzle_n puzzle;

    public void SetWeapon(WeaponType_n weapon)
    {
        if (weapon == WeaponType_n.None)
        {
            currentWeapon = WeaponType_n.None;

            if (currentWeaponObject != null)
            {
                Destroy(currentWeaponObject);
                currentWeaponObject = null;
            }

            return;
        }

        Debug.Log("SetWeapon 引数 = " + weapon);

        currentWeapon = weapon;
        Debug.Log("currentWeapon = " + currentWeapon);

        // 前の武器を消す
        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        GameObject prefab = null;

        Debug.Log("switchに入る weapon = " + weapon);

        switch (weapon)
        {
            case WeaponType_n.Sword:
                prefab = swordPrefab;
                break;

            case WeaponType_n.Axe:
                prefab = axePrefab;
                break;

            case WeaponType_n.Shield:
                prefab = shieldPrefab;
                break;

            case WeaponType_n.Spear:
                prefab = spearPrefab;
                break;

            case WeaponType_n.Bow:
                prefab = bowPrefab;
                break;
        }
        Debug.Log("生成するPrefab = " + (prefab != null ? prefab.name : "NULL"));


        // 武器を生成
        if (prefab != null)
        {
            // 先にワールド座標で生成
            currentWeaponObject = Instantiate(prefab,weaponPoint.position,weaponPoint.rotation );

            // WeaponPointを有効化
            weaponPoint.gameObject.SetActive(true);
            // 見た目のワールドサイズを維持したままWeaponPointの子にする
            currentWeaponObject.transform.SetParent(weaponPoint, true);

            // 追加
            currentWeaponObject.SetActive(true);

            // 表示サイズ調整
            currentWeaponObject.transform.localScale =
                new Vector3(1.0f, 1.0f, 1.0f);

            Debug.Log( "武器 activeSelf=" + currentWeaponObject.activeSelf + " / activeInHierarchy=" + currentWeaponObject.activeInHierarchy + " / parentActive=" + weaponPoint.gameObject.activeInHierarchy);

            Debug.Log("生成位置 world = " + currentWeaponObject.transform.position);
            Debug.Log("生成位置 local = " + currentWeaponObject.transform.localPosition);
            Debug.Log("scale = " + currentWeaponObject.transform.lossyScale);
            Debug.Log("active = " + currentWeaponObject.activeInHierarchy);

            SpriteRenderer[] renderers = currentWeaponObject.GetComponentsInChildren<SpriteRenderer>(true);

            Debug.Log("Renderer数 = " + renderers.Length);

            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.enabled = true;
                renderer.sortingLayerName = "Default";
                renderer.sortingOrder = 100;

                Debug.Log(
                    "Renderer: " + renderer.name +
                    " / enabled=" + renderer.enabled +
                    " / sprite=" + renderer.sprite
                );
            }

            Item_n itemComponent =
                currentWeaponObject.GetComponent<Item_n>();

            if (itemComponent != null)
            {
                itemComponent.enabled = false;
            }

            Collider2D[] colliders =
                currentWeaponObject.GetComponentsInChildren<Collider2D>(true);

            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
            }

            Debug.Log(
                "武器生成完了：" + currentWeaponObject.name +
                " / worldPos=" + currentWeaponObject.transform.position +
                " / worldScale=" + currentWeaponObject.transform.lossyScale
            );
        }

        Debug.Log("像" + statueID + " に " + weapon + " を持たせた");

        Debug.Log(gameObject.name + " に " + weapon + " をセット");
    }
}
