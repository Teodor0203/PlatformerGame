using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBehaviour : MonoBehaviour
{
    [SerializeField] private Light2D itemLight;

    [Header("Light radius info")]
    [SerializeField] private float startOuterRadius;
    [SerializeField] private float endOuterRadius;

    [Header("Light intensity info")]
    [SerializeField] private float startLightIntensity;
    [SerializeField] private float endLightIntensity;

    [Space]
    [SerializeField] private float lightFadeDuration;

    private Animator anim => GetComponent<Animator>();

    [SerializeField] private GameObject flowerLight;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            anim.SetTrigger("activate");
            anim.SetBool("flowerActive", true);
            flowerLight.SetActive(true);
            StartCoroutine(FadeInLightCoroutine());
        }

    }

    private IEnumerator FadeInLightCoroutine()
    {
        float elapsedTime = 0f;

        float initialOuterRadius = startOuterRadius;
        float initalLightIntensity = startLightIntensity;

        while (elapsedTime < lightFadeDuration)
        {
            float t = elapsedTime / lightFadeDuration;

            startOuterRadius = Mathf.Lerp(initialOuterRadius, endOuterRadius, t);
            startLightIntensity = Mathf.Lerp(initalLightIntensity, endLightIntensity, t);

            itemLight.intensity = startLightIntensity;
            itemLight.pointLightOuterRadius = startOuterRadius;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        itemLight.intensity = endLightIntensity;
        itemLight.pointLightOuterRadius = endOuterRadius; itemLight.intensity = endLightIntensity;
    }
}
