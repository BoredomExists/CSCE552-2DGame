using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    [Header("References")]
    public GameObject playerHitbox;
    public PlayerAnimator playerAnimator;

    void Awake()
    {
        playerAnimator = GetComponentInParent<PlayerAnimator>();
    }


    public void EnablePlayerHitBox()
    {
        playerHitbox.SetActive(true);
        if (playerAnimator.GetLastFacing() > 0)
        {
            playerHitbox.GetComponent<BoxCollider2D>().offset = new Vector2(0.15f, -0.05f);
        }
        else
        {
            playerHitbox.GetComponent<BoxCollider2D>().offset = new Vector2(-0.15f, -0.05f);
        }
    }

    public void DisablePlayerHitBox()
    {
        playerHitbox.SetActive(false);
    }
    
}
