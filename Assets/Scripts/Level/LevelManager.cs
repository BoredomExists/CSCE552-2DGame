using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    public Canvas levelCanvas;

    void Awake()
    {
        levelCanvas = FindFirstObjectByType<Canvas>();
        levelCanvas.enabled = true;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
