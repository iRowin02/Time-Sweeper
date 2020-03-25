using UnityEngine;

public class GunUsage : MonoBehaviour
{
    public AudioClip gunSound;
    [SerializeField]
    protected HUD_Manager HUD;

    [Header("Variables")]
    public float reloadTime;
    public float fireDelay = 0.2f;
    [ReadOnly]public float interval_;
    [ReadOnly]public bool canShoot = true;
    [ReadOnly]public bool isReloading;

    [Header("Usage")]
    public GameObject muzzleFlash;
    public Transform barrel;
    protected Camera cam;
    public LayerMask layer;

    [Header("Ammunition")]
    public int currentBullets;
    public int currentAmmo;

    protected int maxBullets;
    protected int maxAmmo;
}
