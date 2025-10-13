using Unity.Cinemachine;
using UnityEngine;

public class RoomCameraTrigger : MonoBehaviour
{
    [System.Serializable]
    public class Door
    {
        public string doorName;
        public Transform doorPoint;   // assign door position in Inspector
        public Vector2 roomOffset;    // camera offset for this door
    }
    public float targetOrthoSize = 12f;
    public float transitionSpeed = 2f;
    public Door[] doors;          // assign all doors here
    public static bool roomEntered;

    private CinemachineCamera cam;
    private CinemachinePositionComposer camPos;
    private Transform originalFollowTarget;
    private Vector2 currentOffset;

    void Start()
    {
        cam = FindFirstObjectByType<CinemachineCamera>();
        camPos = FindFirstObjectByType<CinemachinePositionComposer>();
        if (cam != null)
            originalFollowTarget = cam.Follow;
    }

    // Checks if the user entered the room
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Vector2 playerPos = collision.transform.position;                                       // Gets the player's current position
        Door closestDoor = null;                                                                // Check for which door was entered
        float bestDist = float.MaxValue;                                                        // Gets the max value of the float variable

        foreach (var door in doors)
        {
            if (door.doorPoint == null) continue;
            float dist = Vector2.SqrMagnitude((Vector2)door.doorPoint.position - playerPos);    // Gets the closest door to the player (normally the one entered)
            if (dist < bestDist)
            {
                bestDist = dist;
                closestDoor = door;
            }
        }

        if (closestDoor != null)
        {
            currentOffset = closestDoor.roomOffset;                                             // Door found, so get the offset of the camera from the player to see the whole room

            // Sets the offset
            if (camPos != null)
            {
                Vector3 cur = camPos.TargetOffset;
                cur.x = currentOffset.x;
                cur.y = currentOffset.y;
                camPos.TargetOffset = cur;
            }

            if (cam != null)
            {
                originalFollowTarget = cam.Follow;
                cam.Follow = null;

                var camT = cam.transform;
                camT.position = new Vector3(playerPos.x + currentOffset.x, playerPos.y + currentOffset.y, camT.position.z);
            }
        }

        // Changes the camera's orthographic size to view the whole room
        roomEntered = true;
        StopAllCoroutines();
        StartCoroutine(Zoom(targetOrthoSize));
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Changes the camera back to focus on the player when leaving the room
        if (camPos != null)
        {
            camPos.TargetOffset = Vector2.zero;
        }

        if (cam != null)
        {
            cam.Follow = originalFollowTarget;
        }

        roomEntered = false;
        StopAllCoroutines();
        StartCoroutine(Zoom(7f));
    }

    System.Collections.IEnumerator Zoom(float zoom)
    {
        if (cam == null) yield break;

        // Changes the orthographic size of the camera when entering and leaving a room
        while (Mathf.Abs(cam.Lens.OrthographicSize - zoom) > 0.01f)
        {
            cam.Lens.OrthographicSize = Mathf.Lerp(
                cam.Lens.OrthographicSize, zoom, Time.deltaTime * transitionSpeed);
            yield return null;
        }

        cam.Lens.OrthographicSize = zoom;
    }
}
