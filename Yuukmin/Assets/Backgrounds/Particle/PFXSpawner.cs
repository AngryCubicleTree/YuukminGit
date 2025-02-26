using UnityEngine;

public class PFXSpawner : MonoBehaviour
{
    public static PFXSpawner instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void PlayPFX(ParticleSystem ps, Vector3 pos ,float deathTime = 1)
    {
        ParticleSystem party = Instantiate(ps, pos, Quaternion.identity);
        party.Play();

        Destroy(party.gameObject, deathTime);
    }
}
