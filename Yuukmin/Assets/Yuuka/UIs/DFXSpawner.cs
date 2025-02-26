using UnityEngine;

public class DFXSpawner : MonoBehaviour
{
    public static DFXSpawner instance;
    [SerializeField] DamageIndi dama;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void SpawnDFX(Transform spawnTran, int damage, Color colors)
    {
        DamageIndi indicat = Instantiate(dama, spawnTran.position, Quaternion.identity);
        dama.text.text = damage.ToString();
        dama.text.color = colors;
        Destroy(indicat.gameObject, 3);
    }
}
