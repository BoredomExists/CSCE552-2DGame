using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Creates the health bar for the enemies when spawned in
/// </summary>
public class EnemyHealthBar : MonoBehaviour
{
    [Header("Health Bar References")]
    public GameObject barPrefab;                                                // Health Bar Prefab representing the enemies health bar
    public Canvas UICanvas;                                                     // UI Canvas to show the health bar for player to see
    public Vector3 worldOffset = new Vector3(0f, 1.5f, 0f);                     // Sets the offset of the health bar to be above the enemy

    private GameObject barInstance;                                             // The Game Object of the health bar when created
    private RectTransform barRect;                                              // The Rect Transform of the health bar Game Object
    private Slider barSlider;                                                   // The slide of the health bar Game Object
    private Camera mainCam;                                                     // The main camera
    private HealthSystem hs;                                                    // The enemy Health System script

    void Awake()
    {
        mainCam = Camera.main;                                                      // Gets the current main camera set
        hs = GetComponent<HealthSystem>();                                          // Gets the HealthSystem of the enemy
        UICanvas = FindFirstObjectByType<Canvas>();                                 // Gets the Canvas to place Health Bar
    }

    void OnEnable()
    {
        if (!barInstance && barPrefab)
        {
            barInstance = Instantiate(barPrefab, UICanvas.transform, false);        // Creates a new health bar for the enemy
            barRect = barInstance.GetComponent<RectTransform>();                    // Gets the health bar transform
            barSlider = barInstance.GetComponent<Slider>();                         // Gets the slider for the health
            UpdateEnemyBar();                                                       // Updates damage counter of health bar when damage is taken
        }
        if (barInstance && !barInstance.activeSelf) barInstance.SetActive(true);    // Sets the health bar to be active if one is created
    }

    void OnDisable()
    {
        if (barInstance && barInstance.activeSelf) barInstance.SetActive(false);    // Disables the health bar for when the enemy dies
    }

    void LateUpdate()
    {
        if (barInstance == null || mainCam == null) return;

        Vector3 worldPos = transform.position + worldOffset;                        // Gets the world position
        Vector3 screenPos = mainCam.WorldToScreenPoint(worldPos);                   // Gets the screen position (What the player sees)

        if (screenPos.z < 0f)
        {
            if (barInstance.activeSelf) barInstance.SetActive(false);               // If the health bar is not on the current screen, disable it
            return;
        }

        if (!barInstance.activeSelf) barInstance.SetActive(true);                   // Sets the bar to be true if the enemy is on screen
        float screenZ = Mathf.DeltaAngle(0f, transform.eulerAngles.z - mainCam.transform.eulerAngles.z);    // Gets the shortest difference between the enemy and the main camera

        barRect.position = screenPos;                                               // Sets the health bar to the enemies position on screen
        barRect.rotation = Quaternion.Euler(0f, 0f, screenZ);                       // Sets the rotation of the bar to match the enemy
        UpdateEnemyBar();                                                           // Updates Health Bar if damage is taken
    }

    // Destroys the health bar when enemy is defeated
    void OnDestroy()
    {
        if (barInstance)
        {
            Destroy(barInstance);
        }
    }

    // Updates the health counter of the health bar and if 0, destroy the enemy
    public void UpdateEnemyBar()
    {
        barSlider.value = Mathf.Clamp01((float)hs.GetCurrentHealth() / hs.GetMaxHealth());
        if (hs.GetCurrentHealth() <= 0)
        {
            Destroy(gameObject);
        }
    }
}
