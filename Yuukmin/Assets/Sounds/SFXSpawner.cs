using UnityEngine;

public class SFXSpawner : MonoBehaviour
{
    public static SFXSpawner instance;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void PlaySFX(AudioClip AClip, Transform spawnTran, float AVol)
    {
        AudioSource aS = Instantiate(audioSource,spawnTran.position,Quaternion.identity);
        aS.clip = AClip;
        aS.volume = AVol;
        aS.Play();

        Destroy(aS.gameObject, AClip.length);
    }
}
