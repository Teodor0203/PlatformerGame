using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.Knockback(transform.position.x);
            player.Damage(damage);
        }
    }
}
