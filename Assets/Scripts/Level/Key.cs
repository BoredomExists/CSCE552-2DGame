using UnityEngine;
using UnityEngine.UI;


public class Key : MonoBehaviour
{
    [Header("References")]
    public GameObject key1;
    public GameObject key2;

    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!key1.activeSelf)
                key1.SetActive(true);
            else
                key2.SetActive(true); ;

            Destroy(gameObject);
        }
    }
}
