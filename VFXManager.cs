using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;

    [Header("VFX Prefabs")]
    public GameObject prestigeBurst;
    public GameObject milestoneFlash;
    public GameObject upgradePop;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void PlayVFX(GameObject prefab, Vector3 worldPosition)
    {
        if (prefab == null) return;

        GameObject fx = Instantiate(prefab, worldPosition, Quaternion.identity);
        Destroy(fx, 2f); // Clean up after play
    }
}