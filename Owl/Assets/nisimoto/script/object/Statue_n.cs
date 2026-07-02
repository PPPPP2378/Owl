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

    private GameObject currentWeaponObject;

    public WeaponType_n currentWeapon = WeaponType_n.None;

    public StatuePuzzle_n puzzle;

    public void SetWeapon(WeaponType_n weapon)
    {
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
            currentWeaponObject = Instantiate(
                prefab,
                weaponPoint.position,
                weaponPoint.rotation,
                weaponPoint
            );
            Debug.Log("生成されたオブジェクト = " + currentWeaponObject.name);
            currentWeaponObject.transform.localPosition = Vector3.zero;
            currentWeaponObject.transform.localRotation = Quaternion.identity;
        }

        if (puzzle != null)
        {
            puzzle.CheckAnswer();
        }

        Debug.Log("像" + statueID + " に " + weapon + " を持たせた");

        Debug.Log(gameObject.name + " に " + weapon + " をセット");
    }
}
