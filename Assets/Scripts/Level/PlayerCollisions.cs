using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    // Checks the collisions of the player

    // If the player entered the room, disable the "cover" and enable all objects in the room
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

    // Checks if the player exited the room, if so, enable the "cover" while disabling all objects in the room
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
