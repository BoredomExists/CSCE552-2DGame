using UnityEngine;

public class BossDoorAnimation : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public BoxCollider2D colBox;

    public GameObject key1;
    public GameObject key2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        colBox.gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (checkKeys() && collision.CompareTag("Player"))
        {
            animator.SetBool("bossDoorOpen", true);
        }
    }

    private bool checkKeys()
    {
        return key1.activeSelf && key2.activeSelf;
    }

    public void DisableBossDoorCollider()
    {
        colBox.GetComponents<BoxCollider2D>()[1].enabled = false;
    }
}
