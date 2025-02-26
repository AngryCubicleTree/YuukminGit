using UnityEngine;
using UnityEngine.InputSystem;

public class CursorVisuals : MonoBehaviour
{
    public bool decoMode;
    [Header("CursorInteraction")]
    [SerializeField] InputAction inputLeftClick;
    [SerializeField] InputAction inputRightClick;

    [Header("CursorVisuals")]
    [SerializeField] ParticleSystem particleWhistle;
    [SerializeField] ParticleSystem particleIdle;
    [SerializeField] ParticleSystem particleThrow;
    [SerializeField] ParticleSystem particleDecoIdle;

    [Header("MoveCusor")]
    [SerializeField] Vector3 mousePos;
    [SerializeField] Vector3 worldPos;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float maxMouseDist;

    private void OnEnable()
    {
        inputLeftClick = InputSystem.actions.FindAction("LeftClick");
        inputRightClick = InputSystem.actions.FindAction("RightClick");
    }
    void LateUpdate()
    {
        if (!decoMode)
            ReadCursor();

        MoveCursor();
    }
    public void DecoStart()
    {
        decoMode = true;
        DecoModeReadCursor();
    }
    public void DecoEnd()
    {
        decoMode = false;
        ParticleStopClear();
        particleIdle.Play();
    }
    void DecoModeReadCursor()
    {
        ParticleStopClear();
        particleDecoIdle.Play();
    }
    void ReadCursor()
    {
        if (inputLeftClick.WasPressedThisFrame()) //Throw
        {
            ParticleStopClear();
            particleThrow.Play();
        }
        if (inputRightClick.WasPressedThisFrame()) //Call
        {
            ParticleStopClear();
            particleWhistle.Play();
        }

        if (inputLeftClick.WasReleasedThisFrame()) //Idle
        {
            ParticleStopClear();
            particleIdle.Play();
        }
        if (inputRightClick.WasReleasedThisFrame()) //Idle
        {
            ParticleStopClear();
            particleIdle.Play();
        }
    }
    void ParticleStopClear()
    {
        particleDecoIdle.Clear();
        particleIdle.Clear();
        particleThrow.Clear();
        particleWhistle.Clear();

        particleDecoIdle.Stop();
        particleIdle.Stop();
        particleThrow.Stop();
        particleWhistle.Stop();
    }
    void MoveCursor()
    {
        mousePos = Mouse.current.position.ReadValue(); //Screen mouse position

        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos); //Raymoo, i mean.. raycast from mouse position

        if (Physics.Raycast(mouseRay, out RaycastHit hit, maxMouseDist, targetLayer)) //if hit targeted layer,
        {
            worldPos = new Vector3(hit.point.x,0, hit.point.z); //get the point
        }
        transform.position = worldPos; //move this transform
    }
}
