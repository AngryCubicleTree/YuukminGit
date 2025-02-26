using TMPro;
using UnityEngine;

public class DamageIndi : MonoBehaviour
{
    [SerializeField] float upSpeed;
    public TextMeshPro text;
    void Update()
    {
        transform.position += new Vector3(0, upSpeed * Time.deltaTime,0);
    }
}
