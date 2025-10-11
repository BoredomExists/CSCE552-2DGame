using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject room = collision.gameObject;
        if (room.CompareTag("Cover"))
        {
            Debug.Log("Room Entered");
            room.GetComponent<SpriteRenderer>().enabled = false;
            foreach (Transform child in room.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        GameObject room = collision.gameObject;
        if (room.CompareTag("Cover"))
        {
            Debug.Log("Room Exited");
            room.GetComponent<SpriteRenderer>().enabled = true;
            foreach (Transform child in room.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
