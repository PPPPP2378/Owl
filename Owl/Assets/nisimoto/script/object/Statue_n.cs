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

    //public StatuePuzzle_n puzzle;

    public void SetWeapon(WeaponType_n weapon)
    {
        currentWeapon = weapon;

        // ëOÇÃïêäÌÇè¡Ç∑
        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        GameObject prefab = null;

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

        // ïêäÌÇê∂ê¨
        if (prefab != null)
        {
            currentWeaponObject = Instantiate(
                prefab,
                weaponPoint.position,
                weaponPoint.rotation,
                weaponPoint
            );

            currentWeaponObject.transform.localPosition = Vector3.zero;
            currentWeaponObject.transform.localRotation = Quaternion.identity;
        }

        Debug.Log("ëú" + statueID + " Ç… " + weapon + " ÇéùÇΩÇπÇΩ");

        // puzzle.CheckAnswer();
    }
}
