using UnityEngine;

public class PuzzleRoomSolution : MonoBehaviour
{
    [Header("References")]
    public Transform door;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Goal")) return;
        
        if (collision.collider.CompareTag("Goal"))
        {
            door.gameObject.SetActive(false);
        }
    }

}
