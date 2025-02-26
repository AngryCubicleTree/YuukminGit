using System.Collections;
using UnityEngine;

public class YuukaJourneyManager : MonoBehaviour
{
    [SerializeField] AudioSource musicP;
    [SerializeField] YuukaMovement Yuuka;
    [SerializeField] YuukaItemsManager YIM;
    [SerializeField] YuukaFairyManager YuukFae;
    [SerializeField] CanvasGroup darkCanvas;
    [SerializeField] float appearTime;
    [SerializeField] float darkTime;
    [SerializeField] Vector3 destination;
    [SerializeField] Gap newGap;
    Coroutine transportCoroutine;
    public void Transport(Vector3 desti, Gap targetGap, AudioClip music)
    {
        newGap = targetGap;
        if (transportCoroutine != null)
            StopCoroutine(transportCoroutine);
        musicP.clip = music;
        musicP.Play();
        destination = desti;
        transportCoroutine =StartCoroutine(IETransport());
    }
    IEnumerator IETransport()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime / appearTime)
        {
            darkCanvas.alpha = i;
            yield return null;
        }
        darkCanvas.alpha = 1;

        Yuuka.move = false;
        Debug.LogError("Yuuka" + Yuuka.transform.position);
        Yuuka.transform.GetComponent<Rigidbody>().position = destination;
        Yuuka.transform.position = destination;
        Debug.LogError("Yuuka2" + Yuuka.transform.position);
        Yuuka.move = true;
        YuukFae.BringFairies(newGap);
        YIM.gap = newGap;

        yield return new WaitForSeconds(darkTime);
        for (float i = 1; i >= 0; i -= Time.deltaTime / appearTime)
        {
            darkCanvas.alpha = i;
            yield return null;
        }
        darkCanvas.alpha = 0;
    }
}
