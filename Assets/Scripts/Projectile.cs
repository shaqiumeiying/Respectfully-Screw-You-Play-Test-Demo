using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damage = 10f;
    public string enemyTag = "Enemy";
    public float destroyDelay = 0.05f;

    void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag(enemyTag))
        {
            Debug.Log($"Bullet triggered {other.name}");
            var reaction = other.GetComponent<EnemyReactionC>();
            if (reaction)
                reaction.ReactToHit();

            Destroy(gameObject, destroyDelay);
        }

        
    }
}
