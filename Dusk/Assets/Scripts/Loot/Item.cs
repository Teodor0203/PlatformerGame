using UnityEngine;

public enum ItemType { Coin, Diamond, Fruit }

public class Items : MonoBehaviour
{
    [SerializeField] protected ItemType itemType;
    [SerializeField] private GameObject pickupVfx;
    protected Animator anim;
    protected SpriteRenderer sr;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        GameManager gameManager = GameManager.instance;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            switch (itemType)
            {
                case ItemType.Coin:
                    GameManager.instance.AddCoin(1); // You can set a value for the coin
                    break;

                /*case ItemType.Diamond:
                    GameManager.instance.AddDiamond(1); // You can set a value for the diamond
                    break;*/

                case ItemType.Fruit:
                    GameManager.instance.AddBattery();
                    break;
            }

            AudioManager.instance.PlaySFX(8);
            Destroy(gameObject);

            GameObject newFx = Instantiate(pickupVfx, transform.position, Quaternion.identity);
        }
    }
}
