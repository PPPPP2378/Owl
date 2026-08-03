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
            Debug.Log("weaponPoint = " + weaponPoint.name);
            Debug.Log("Prefab = " + prefab.name);

            currentWeaponObject = Instantiate(
                prefab,
                weaponPoint.position,
                weaponPoint.rotation,
                weaponPoint
            );

            Debug.Log("world = " + currentWeaponObject.transform.position);
            Debug.Log("local = " + currentWeaponObject.transform.localPosition);
            Debug.Log("parent = " + currentWeaponObject.transform.parent.name);

            Debug.Log("生成 = " + currentWeaponObject);

            SpriteRenderer sr = currentWeaponObject.GetComponent<SpriteRenderer>();

            sr.sortingLayerName = "Default";
            sr.sortingOrder = 100;

            Debug.Log("SpriteRenderer = " + sr);

            if (sr != null)
                Debug.Log("Sprite = " + sr.sprite);

            currentWeaponObject.transform.localPosition = Vector3.zero;
            currentWeaponObject.transform.localRotation = Quaternion.identity;
            currentWeaponObject.transform.localScale = prefab.transform.localScale;
        }





        Debug.Log("像" + statueID + " に " + weapon + " を持たせた");

        Debug.Log(gameObject.name + " に " + weapon + " をセット");
    }
}
