using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    public GameObject levelCanvas;

    void Awake()
    {
        levelCanvas.SetActive(true);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
