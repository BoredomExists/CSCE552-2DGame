using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("Health Bar References")]
    public GameObject barPrefab;
    public Canvas UICanvas;
    public Vector3 worldOffset = new Vector3(0f, 1.5f, 0f);

    private GameObject barInstance;
    private RectTransform barRect;
    private Slider barSlider;
    private Camera mainCam;
    private HealthSystem hs;

    void Awake()
    {
        mainCam = Camera.main;
        hs = GetComponent<HealthSystem>();
        UICanvas = FindFirstObjectByType<Canvas>();
    }

    void OnEnable()
    {
        if (!barInstance && barPrefab)
        {
            barInstance = Instantiate(barPrefab, UICanvas.transform, false);
            barRect = barInstance.GetComponent<RectTransform>();
            barSlider = barInstance.GetComponent<Slider>();
            UpdateEnemyBar();
        }
        if (barInstance && !barInstance.activeSelf) barInstance.SetActive(true);
    }

    void OnDisable()
    {
        if (barInstance && barInstance.activeSelf) barInstance.SetActive(false);
    }

    void LateUpdate()
    {
        if (barInstance == null || mainCam == null) return;

        Vector3 worldPos = transform.position + worldOffset;
        Vector3 screenPos = mainCam.WorldToScreenPoint(worldPos);

        if (screenPos.z < 0f)
        {
            if (barInstance.activeSelf) barInstance.SetActive(false);
            return;
        }

        if (!barInstance.activeSelf) barInstance.SetActive(true);
        float screenZ = Mathf.DeltaAngle(0f, transform.eulerAngles.z - mainCam.transform.eulerAngles.z);

        barRect.position = screenPos;
        barRect.rotation = Quaternion.Euler(0f, 0f, screenZ);
        UpdateEnemyBar();
    }

    void OnDestroy()
    {
        if (barInstance)
        {
            Destroy(barInstance);
        }
    }

    public void UpdateEnemyBar()
    {
        barSlider.value = Mathf.Clamp01((float)hs.GetCurrentHealth() / hs.GetMaxHealth());
    }
}
