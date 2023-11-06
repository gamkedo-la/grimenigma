using UnityEngine;

public class PlayerWeaponCamera : MonoBehaviour
{
    [SerializeField] PlayerData pData;
    [SerializeField] Camera weaponCamera;

    void OnEnable()
    {
        pData.onRefreshWeaponScale += SetWeaponScale;
    }

    void OnDisable()
    {
        pData.onRefreshWeaponScale -= SetWeaponScale;
    }

    void SetWeaponScale(float scale)
    {
        weaponCamera.fieldOfView = scale;
    }
}
