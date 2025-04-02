using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Menu_LightBehaviour : MonoBehaviour
{
    [SerializeField] private Light2D spotLight;
    [Space]
    [SerializeField] private float[] waitTime;


    private IEnumerator Start()
    {
        spotLight.intensity = 1;

        yield return new WaitForSeconds(Random.Range(1, 5));

        RandomBlinking();
    }

    private void RandomBlinking()
    {
        int randomInt = Random.Range(1, 101);

        if (randomInt <= 50)
            StartCoroutine(LightBlinkCoroutine());
    }

    private IEnumerator LightBlinkCoroutine()
    {
        foreach (float seconds in waitTime)
        {
            spotLight.intensity = .2f;

            yield return new WaitForSeconds(seconds);

            spotLight.intensity = 1;

            yield return new WaitForSeconds(seconds);
        }

        /*if (Random.Range(1, 101) <= 30)
        {
            spotLight.intensity = 1;
        }*/
    }
}
