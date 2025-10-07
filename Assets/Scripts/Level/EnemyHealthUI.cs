using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public GameObject barPrefab;
    public Canvas UICanvas;
    public Vector3 worldOffset = new Vector3(0f, 1.2f, 0f);

    private GameObject barInstance;
    private RectTransform barRect;
    private Slider barSlider;
    private TMP_Text barText;
    private Camera mainCam;
    private HealthSystem healthSystem;

    void Awake()
    {
        mainCam = Camera.main;
        healthSystem = GetComponent<HealthSystem>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        barInstance = Instantiate(barPrefab, UICanvas.transform, false);
        barRect = barInstance.GetComponent<RectTransform>();
        barSlider = barInstance.GetComponentInChildren<Slider>();

        if (healthSystem != null)
        {
            UpdateBar(healthSystem.currentHealth, healthSystem.maxHealth);
            healthSystem.OnHealthChange += UpdateBar;
        }
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

        barRect.position = screenPos;
    }

    private void UpdateBar(int current, int max)
    {
        if (barSlider != null)
        {
            barSlider.value = (max > 0) ? Mathf.Clamp01((float)current / max) : 0f;
        }
    }

    private void UpdateBar(float current, float max)
    {
        UpdateBar(Mathf.RoundToInt(current), Mathf.RoundToInt(max));
    }

    void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnHealthChange -= UpdateBar;
        }
        if (barInstance != null)
        {
            Destroy(barInstance);
        }
    }
}
