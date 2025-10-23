using System.Collections;
using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;
using UnityEngine.UI;

public class FinalBoss : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public HealthSystem hs;
    public HealthSystem ps;
    public BoxCollider2D leftArm;
    public BoxCollider2D rightArm;
    public CircleCollider2D head;

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

    [Header("Projectile")]
    public GameObject projPrefab;
    public int projDMG = 20;

    private Coroutine move;
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        hs = GetComponentInParent<HealthSystem>();
        bossHealthBar.SetActive(true);
        bossSlider.value = Mathf.Clamp01((float)hs.GetCurrentHealth() / hs.GetMaxHealth());
        player = GameObject.FindWithTag("Player").transform;

        rightArm = GetComponentsInParent<BoxCollider2D>()[0];
        leftArm = GetComponentsInParent<BoxCollider2D>()[1];
        head = GetComponentsInParent<CircleCollider2D>()[0];
        shockwave1 = GetComponentsInParent<CircleCollider2D>()[1];
        shockwave2 = GetComponentsInParent<CircleCollider2D>()[2];

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

    public void FireProjectiles()
    {
        var p1 = Instantiate(projPrefab, fire1.position, Quaternion.identity).GetComponent<Projectile>();
        p1.Fire((player.position - fire1.position).normalized, projDMG, Projectile.ProjectileOwner.Enemy);

        var p2 = Instantiate(projPrefab, fire2.position, Quaternion.identity).GetComponent<Projectile>();
        p2.Fire((player.position - fire2.position).normalized, projDMG, Projectile.ProjectileOwner.Enemy);

        var p3 = Instantiate(projPrefab, fire3.position, Quaternion.identity).GetComponent<Projectile>();
        p3.Fire((player.position - fire3.position).normalized, projDMG, Projectile.ProjectileOwner.Enemy);

        var p4 = Instantiate(projPrefab, fire4.position, Quaternion.identity).GetComponent<Projectile>();
        p4.Fire((player.position - fire4.position).normalized, projDMG, Projectile.ProjectileOwner.Enemy);

        var p5 = Instantiate(projPrefab, fire5.position, Quaternion.identity).GetComponent<Projectile>();
        p5.Fire((player.position - fire5.position).normalized, projDMG, Projectile.ProjectileOwner.Enemy);

        var p6 = Instantiate(projPrefab, fire6.position, Quaternion.identity).GetComponent<Projectile>();
        p6.Fire((player.position - fire6.position).normalized, projDMG, Projectile.ProjectileOwner.Enemy);
    }

    public void EnableHitBoxes()
    {
        head.enabled = true;
        leftArm.enabled = true;
        rightArm.enabled = true;
    }
    public void DisableHitBoxes()
    {
        head.enabled = false;
        leftArm.enabled = false;
        rightArm.enabled = false;
    }

    public void EnableFiringPoints()
    {
        fire1.gameObject.SetActive(true);
        fire2.gameObject.SetActive(true);
        fire3.gameObject.SetActive(true);
        fire4.gameObject.SetActive(true);
        fire5.gameObject.SetActive(true);
        fire6.gameObject.SetActive(true);
    }
    public void DisableFiringPoints()
    {
        fire1.gameObject.SetActive(false);
        fire2.gameObject.SetActive(false);
        fire3.gameObject.SetActive(false);
        fire4.gameObject.SetActive(false);
        fire5.gameObject.SetActive(false);
        fire6.gameObject.SetActive(false);
    }

    public void EnableSlamCircles()
    {
        shockwave1.enabled = true;
        shockwave2.enabled = true;
    }

    public void DisableSlamCircles()
    {
        shockwave1.enabled = false;
        shockwave2.enabled = false;
    }
}
