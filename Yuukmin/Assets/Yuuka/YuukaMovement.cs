using UnityEngine;
using UnityEngine.InputSystem;

public class YuukaMovement : MonoBehaviour
{
    public bool move;
    InputAction moveInput;
    Rigidbody rb;
    public float Speed;

    [SerializeField] bool walking;
    [SerializeField] YuukaAnims anima;

    void Start()
    {
        moveInput = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(move)
        {
            if (moveInput.ReadValue<Vector2>().x != 0)
            {

                if (moveInput.ReadValue<Vector2>().x < 0)
                    transform.localScale = new Vector3(0.1f, 0.1f, 1);
                else
                    transform.localScale = new Vector3(-0.1f, 0.1f, 1);
            }
            rb.linearVelocity = new Vector3(moveInput.ReadValue<Vector2>().x * Speed, rb.linearVelocity.y, moveInput.ReadValue<Vector2>().y * Speed);

            if(moveInput.ReadValue<Vector2>().magnitude != 0)
            {
                walking = true;
                anima.walking = walking;
            }
            if (moveInput.ReadValue<Vector2>().magnitude == 0)
            {
                walking = false;
                anima.walking = walking;
            }
        }
    }
}
