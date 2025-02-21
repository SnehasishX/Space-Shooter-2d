using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float maxTravelDistance = 20f;
    public GameObject explosionEffectPrefab;

    private Vector3 startPosition;
    private ParticleSystem particleSystem;
    private Vector3 originalScale;
    public string shooterTag;

    public void SetShooterTag(string tag)
    {
        shooterTag = tag;
    }

    void Start()
    {
        startPosition = transform.position;
        particleSystem = GetComponentInChildren<ParticleSystem>();

        if (particleSystem != null)
        {
            originalScale = particleSystem.transform.localScale;
        }
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;

        if (Vector3.Distance(startPosition, transform.position) >= maxTravelDistance)
        {
            DestroyProjectile();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (shooterTag == "Player" && other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                ShowDamageText(other.transform.position, damage, false); // 🔥 Enemy hit
            }
            DestroyProjectile();
        }

        if (shooterTag == "Enemy" && other.CompareTag("Player"))
        {
            PhotonView playerView = other.GetComponent<PhotonView>();

            if (playerView != null)
            {
                playerView.RPC("RPC_TakeDamage", playerView.Owner, damage);
                ShowDamageText(other.transform.position, damage, true); // 🔥 Player hit
            }
            DestroyProjectile();
        }
    }

    void ShowDamageText(Vector3 position, int damage, bool isPlayer)
    {
        if (UIManager.Instance != null && UIManager.Instance.photonView != null)
        {
            string rpcMethod = isPlayer ? "ShowPlayerDamageText" : "ShowEnemyDamageText";

            if (UIManager.Instance.GetType().GetMethod(rpcMethod) != null)
            {
                UIManager.Instance.photonView.RPC(rpcMethod, RpcTarget.All, position, damage);
            }
            else
            {
                Debug.LogError($"❌ {rpcMethod} RPC method is missing in UIManager!");
            }
        }
        else
        {
            Debug.LogError("❌ UIManager instance or PhotonView is missing!");
        }
    }




    void DestroyProjectile()
    {
        if (particleSystem != null)
        {
            particleSystem.transform.parent = null;
            particleSystem.transform.localScale = originalScale;
            particleSystem.Stop();
            Destroy(particleSystem.gameObject, particleSystem.main.duration);
        }

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
