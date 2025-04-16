public class WeaponInventory
{
    private IWeapon _rightHandWeapon;
    private IWeapon _leftHandWeapon;

    public void EquipRightHand(IWeapon weapon) => _rightHandWeapon = weapon;
    public void EquipLeftHand(IWeapon weapon) => _leftHandWeapon = weapon;

    public IWeapon GetRightHandWeapon() => _rightHandWeapon;
    public IWeapon GetLeftHandWeapon() => _leftHandWeapon;
}
