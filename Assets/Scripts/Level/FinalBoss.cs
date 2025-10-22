using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinalBoss : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public HealthSystem hs;
    public HealthSystem ps;
    public BoxCollider2D leftArm;
    public BoxCollider2D rightArm;
    public BoxCollider2D head;

    public GameObject bossHealthBar;
    public Slider bossSlider;
    public Transform fire1;
    public Transform fire2;
    public Transform fire3;
    public Transform fire4;
    public Transform fire5;
    public Transform fire6;

    public CircleCollider2D shockwave1;
    public CircleCollider2D shockwave2;

    [Header("Timing")]
    public float delayBetweenMoves = 7f;
    public float postAttackBuffer = .5f;
    public float attackCooldown = 1f;

    private Coroutine move;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        hs = GetComponent<HealthSystem>();
        bossHealthBar.SetActive(true);
        bossSlider.value = Mathf.Clamp01((float)hs.GetCurrentHealth() / hs.GetMaxHealth());
        

        move = StartCoroutine(GetMove());
    }

    IEnumerator GetMove()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            yield return new WaitForSeconds(delayBetweenMoves);

            int moveNumber = Random.Range(0, 3);
            switch (moveNumber)
            {
                case 0:
                    animator.SetTrigger("isShield");
                    break;

                case 1:
                    animator.SetTrigger("isFiring");
                    break;

                case 2:
                    animator.SetTrigger("isSlamming");
                    break;
            }
            yield return new WaitForSeconds(postAttackBuffer);
        }
    }
}
