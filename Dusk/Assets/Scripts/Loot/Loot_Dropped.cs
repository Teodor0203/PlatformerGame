using System.Collections;
using UnityEngine;

public class Loot_Dropped : Items
{
    [SerializeField] private GameObject dissapearingVFX;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Color transperentColor;
    [SerializeField] private float[] blinkWaitTime;

    [Space]
    [SerializeField] private float[] dissapearingBlinkWaitTime;

    private bool canPickUp;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(ItemBlinkCoroutine());
    }

    private void Update()
    {
        transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;
    }

    public void SetLootType(ItemType itemOfType)
    {
        itemType = itemOfType;
    }

    private IEnumerator ItemBlinkCoroutine()
    {
        foreach (float seconds in blinkWaitTime)
        {
            ItemToggleColorAndPosition(transperentColor);

            yield return new WaitForSeconds(seconds);

            ItemToggleColorAndPosition(Color.white);

            yield return new WaitForSeconds(seconds);
        }

        velocity.y = 0;
        velocity.x = 0;
        canPickUp = true;

        yield return new WaitForSeconds(2);

        StartCoroutine(ItemDissapearingBlinkCoroutine());

        yield return new WaitForSeconds(1);

        GameObject newDissapearingVFX = Instantiate(dissapearingVFX, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private IEnumerator ItemDissapearingBlinkCoroutine()
    {
        foreach (float seconds in dissapearingBlinkWaitTime)
        {
            ItemDissapearingColor(transperentColor);

            yield return new WaitForSeconds(seconds);

            ItemDissapearingColor(Color.white);

            yield return new WaitForSeconds(seconds);
        }
    }

    private void ItemToggleColorAndPosition(Color color)
    {
        velocity.y = velocity.y * -1;
        sr.color = color;
    }

    private void ItemDissapearingColor(Color color)
    {
        sr.color = color;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (canPickUp == false)
            return;

        base.OnTriggerEnter2D(collision);
    }
}
