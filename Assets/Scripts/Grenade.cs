using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour, IDamageDealer
{
    float explosionTime = 2;
    float countdown;
    bool exploded = false;
    float throwForce;
    Rigidbody rb;
    private GameObject player;

    int damage;

    int IDamageDealer.damage
    {
        get { return damage; }
        set { }
    }

    public DamageType damageType
    {
        get => throw new System.NotImplementedException();
        set => throw new System.NotImplementedException();
    }

    public DamageSource damageSource
    {
        get => DamageSource.Neutral;
        set { }
    }

    void Start()
    {
        countdown = explosionTime;
        player = GameObject.FindGameObjectWithTag("Player");
        throwForce = calcThrowForce();
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce + transform.up * throwForce / 1.5f);
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0 && !exploded)
        {
            GameObject expl = (GameObject) Resources.Load(Const.Grenade.GrenadeExplosion,
                typeof(GameObject));
            SoundManagerRework.Instance.PlayEffectOneShot(Resources.Load(Const.SFX.Explosion) as AudioClip);
            Instantiate(expl, gameObject.transform.position, Quaternion.identity);
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 1.5f);
            foreach (var hitCollider in hitColliders)
            {
                IDamageable damageItem = hitCollider.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
                if (damageItem != null)
                {
                    damageItem.TakeDamage(this);
                }
            }

            exploded = true;
            Destroy(this.gameObject);
        }
    }

    private float calcThrowForce()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Camera.main.transform.position.y - transform.position.y));
        float distance = Vector3.Distance(player.transform.position, mousePos);

        return distance * 2;
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
    }
}