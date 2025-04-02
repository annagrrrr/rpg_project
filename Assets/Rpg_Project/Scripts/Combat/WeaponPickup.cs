using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private Weapon weaponPrefab;
    [SerializeField] private Transform weaponModel; 
    [SerializeField] private Transform weaponHoldPoint; 

    private bool isPickedUp = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isPickedUp) return;

        if (weaponPrefab == null)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                bool isMelee = weaponPrefab.damageType == DamageType.PHYSICAL;

                if (isMelee && player.HasMeleeWeapon())
                {
                    Destroy(player.GetMeleeWeapon().gameObject);
                }
                else if (!isMelee && player.HasMagicWeapon())
                {
                    Destroy(player.GetMagicWeapon().gameObject);
                }

                Weapon newWeapon = Instantiate(weaponPrefab, weaponHoldPoint.position, weaponHoldPoint.rotation);
                newWeapon.transform.SetParent(weaponHoldPoint);

                Rigidbody rb = newWeapon.GetComponent<Rigidbody>();
                Collider col = newWeapon.GetComponent<Collider>();
                if (rb != null) Destroy(rb);
                if (col != null) Destroy(col);

                newWeapon.transform.localPosition = Vector3.zero;
                newWeapon.transform.localRotation = Quaternion.identity;

                player.EquipWeapon(newWeapon, isMelee);

                if (weaponModel != null)
                {
                    weaponModel.gameObject.SetActive(false);
                }

                isPickedUp = true;
                Destroy(gameObject, 1f);
            }
        }
    }
}
