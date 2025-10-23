using UnityEngine;

/// <summary>
/// Manager for the puzzle room to unlock the secret room and door
/// </summary>
public class PuzzleRoomSolution : MonoBehaviour
{
    [Header("References")]
    public GameObject secretWall;               // Secret Wall Game Object
    public GameObject secretDoor;               // Secret Door Game Object
    
    // If box gets to goal, deactivate secret wall and activate secret door
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Goal")) return;
        if (collision.collider.CompareTag("Goal"))
        {
            secretWall.SetActive(false);
            secretDoor.SetActive(true);
        }
    }

}
