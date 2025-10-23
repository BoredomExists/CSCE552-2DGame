using UnityEngine;

/// <summary>
/// Hit Box Manager for enabling/disabling the player sword attack hitbox
/// </summary>
public class HitBoxManager : MonoBehaviour
{
    [Header("References")]
    public GameObject playerHitbox;                 // Hitbox that represents the sword swing
    public PlayerAnimator playerAnimator;           // Animator for the player

    void Awake()
    {
        playerAnimator = GetComponentInParent<PlayerAnimator>();
    }


    // Enables the hitbox when the player swings the sword
    public void EnablePlayerHitBox()
    {
        playerHitbox.SetActive(true);

        // Moves the hitbox based on the direction the player is facing
        if (playerAnimator.GetLastFacing() > 0)
        {
            playerHitbox.GetComponent<BoxCollider2D>().offset = new Vector2(0.15f, -0.05f);
        }
        else
        {
            playerHitbox.GetComponent<BoxCollider2D>().offset = new Vector2(-0.15f, -0.05f);
        }
    }

    // Disables the hitbox when sword swing animation is done
    public void DisablePlayerHitBox()
    {
        playerHitbox.SetActive(false);
    }

}
