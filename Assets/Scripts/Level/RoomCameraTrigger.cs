using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Manages the camera when entering and leaving rooms
/// </summary>
public class RoomCameraTrigger : MonoBehaviour
{
    [Header("Camera Framing")]
    public Transform roomAnchor;         // Anchor for the camera to view it at a specific offset
    public Vector2 flatOffset = Vector2.zero; // flat offset relative to roomAnchor
    public float targetOrthoSize = 12f;     // Default size for rooms
    public float transitionSpeed = 2f;      // Default speed
    public static bool roomEntered;         // Boolean to determine if the player has entered a room or not

    private Coroutine zoomCoroutine;        // Coroutine for changing the zoom of the camera

    private CinemachineCamera cam;          // Cinemachine Camera
    private CinemachinePositionComposer camPos; // Position composition for Cinemachine Camera
    private Transform originalFollowTarget;     // Transform of the player for camera

    void Start()
    {
        cam = FindFirstObjectByType<CinemachineCamera>();                   // Gets the camera
        camPos = FindFirstObjectByType<CinemachinePositionComposer>();      // Gets the position composition of the camera
        if (cam != null)
            originalFollowTarget = cam.Follow;
    }

    // Check if the player enters a room
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

        // Changes the camera POV to be focused on the room anchor to view the whole room
        if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);

        if (isActiveAndEnabled && gameObject.activeInHierarchy)
            zoomCoroutine = StartCoroutine(Zoom(targetOrthoSize));
    }

    // Checks if the player exits a room
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Sets the camera to focus back on the player
        if (camPos != null)
            camPos.TargetOffset = Vector2.zero;

        if (cam != null)
            cam.Follow = originalFollowTarget;

        roomEntered = false;

        // "Resets" the camera when focusing back on the player
        StopAllCoroutines();
        StartCoroutine(Zoom(7f));
    }

    // Enumerator to change the zoom of the camera depending on a player entering and leaving a room
    IEnumerator Zoom(float zoom)
    {
        if (cam == null) yield break;

        while (isActiveAndEnabled && gameObject.activeInHierarchy && Mathf.Abs(cam.Lens.OrthographicSize - zoom) > 0.01f)
        {
            cam.Lens.OrthographicSize = Mathf.Lerp(
                cam.Lens.OrthographicSize, zoom, Time.deltaTime * transitionSpeed);
            yield return null;
        }
        cam.Lens.OrthographicSize = zoom;
        zoomCoroutine = null;
    }
}
