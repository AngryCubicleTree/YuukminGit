using UnityEngine;

public class YuukaAnims : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float blinkInterval;
    public bool walking;
    float elapsedTime;
    void Update()
    {
        animator.SetBool("Walk", walking);

        elapsedTime += Time.deltaTime;
        if(elapsedTime > blinkInterval)
        {
            animator.SetTrigger("Blink");
            blinkInterval = Random.Range(4, 10);
            elapsedTime = 0;
        }
    }
}
