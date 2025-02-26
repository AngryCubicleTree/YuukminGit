using UnityEngine;

public class CirnoAnima : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float blinkInterval;
    float elapsedTime;
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > blinkInterval)
        {
            animator.SetTrigger("Blink");
            blinkInterval = Random.Range(4, 10);
            elapsedTime = 0;
        }
    }
}
