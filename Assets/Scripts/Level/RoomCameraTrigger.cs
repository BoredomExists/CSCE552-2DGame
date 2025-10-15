using Unity.Cinemachine;
using UnityEngine;

public class RoomCameraTrigger : MonoBehaviour
{
    [Header("Camera Framing")]
    public Transform roomAnchor;         // place this at the room center (or wherever you want the camera to sit around)
    public Vector2  flatOffset = Vector2.zero; // flat offset relative to roomAnchor (NOT the player)
    public float targetOrthoSize = 12f;
    public float transitionSpeed = 2f;
    public static bool roomEntered;

    private CinemachineCamera cam;
    private CinemachinePositionComposer camPos;
    private Transform originalFollowTarget;

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

        // Switch camera to "follow" the room itself
        if (cam != null)
        {
            originalFollowTarget = cam.Follow;

            if (roomAnchor != null)
                cam.Follow = roomAnchor;
        }

        if (camPos != null)
        {
            Vector3 cur = camPos.TargetOffset;
            cur.x = flatOffset.x;
            cur.y = flatOffset.y;
            camPos.TargetOffset = cur;
        }

        roomEntered = true;
        StopAllCoroutines();
        StartCoroutine(Zoom(targetOrthoSize));
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (camPos != null)
            camPos.TargetOffset = Vector2.zero;

        if (cam != null)
            cam.Follow = originalFollowTarget;

        roomEntered = false;
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
