using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Light2D checkpointLight;
    [SerializeField] private Transform moveLightWaypoint;
    [SerializeField] private float blinkRate;

    private Animator anim => GetComponent<Animator>();
    private bool active;

    [SerializeField] private bool canBeReactivated;

    private void Start()
    {
        canBeReactivated = GameManager.instance.canReactivate;
        StartCoroutine(InactiveBlinkCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && canBeReactivated == false)
            return;

        Player player = collision.GetComponent<Player>();

        if (player != null)
            ActivateCheckpoint();
    }

    private IEnumerator InactiveBlinkCoroutine() //when the checkpoint is inactive
    {
        while (!active)
        {
            checkpointLight.intensity = 2f;

            yield return new WaitForSeconds(blinkRate);

            checkpointLight.intensity = .5f;

            yield return new WaitForSeconds(blinkRate);
        }
    }

    private IEnumerator ActiveBlinkCoroutine() //when the checkpoint is active
    {
        checkpointLight.intensity = 0f;

        while (active)
        {
            checkpointLight.intensity = .5f;

            yield return new WaitForSeconds(blinkRate);

            checkpointLight.intensity = 2.5f;

            yield return new WaitForSeconds(blinkRate);
        }
    }

    private void ActivateCheckpoint()
    {
        active = true;

        checkpointLight.intensity = 0f;
        checkpointLight.color = Color.green;
        checkpointLight.transform.position = Vector2.MoveTowards(transform.position, moveLightWaypoint.position, 5f);

        StartCoroutine(ActiveBlinkCoroutine());

        anim.SetTrigger("activate");
        PlayerManager.instance.UpdateRespawnPosition(transform);
    }
}
