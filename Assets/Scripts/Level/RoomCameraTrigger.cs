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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Vector2 playerPos = collision.transform.position;
        Door closestDoor = null;
        float bestDist = float.MaxValue;

        foreach (var door in doors)
        {
            if (door.doorPoint == null) continue;
            float dist = Vector2.SqrMagnitude((Vector2)door.doorPoint.position - playerPos);
            if (dist < bestDist)
            {
                bestDist = dist;
                closestDoor = door;
            }
        }

        if (closestDoor != null)
        {
            currentOffset = closestDoor.roomOffset;

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

        StopAllCoroutines();
        StartCoroutine(Zoom(targetOrthoSize));
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (camPos != null)
        {
            camPos.TargetOffset = Vector2.zero;
        }

        if (cam != null)
        {
            cam.Follow = originalFollowTarget;
        }

        StopAllCoroutines();
        StartCoroutine(Zoom(7f));
    }

    System.Collections.IEnumerator Zoom(float zoom)
    {
        if (cam == null) yield break;

        while (Mathf.Abs(cam.Lens.OrthographicSize - zoom) > 0.01f)
        {
            cam.Lens.OrthographicSize = Mathf.Lerp(
                cam.Lens.OrthographicSize, zoom, Time.deltaTime * transitionSpeed);
            yield return null;
        }

        cam.Lens.OrthographicSize = zoom;
    }
}
