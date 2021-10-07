using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMulti : MonoBehaviour
{
    [SerializeField] private float fireRate;
    [SerializeField] private float retargetRate;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private WeaponPrefabMulti weaponMulti;
    private float nextTimeToFire = 0.0f;
    private List<Transform> targets = new List<Transform>();
    void Start()
    {
        weaponMulti.SetDamage(damage);
        InvokeRepeating("UpdateTargets", 1f, 1.0f / retargetRate);

    }

    private void UpdateTargets()
    {
        targets = new List<Transform>();
        foreach (var enemyCollider in Physics.OverlapSphere(transform.position, range, enemyLayerMask, QueryTriggerInteraction.UseGlobal))
        {
            var enemy = enemyCollider.GetComponent<Enemy>();
            if (!enemy) enemy = enemyCollider.transform.parent.GetComponent<Enemy>();
            if (enemy)
            {
                targets.Add(enemy.transform);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (targets.Count == 0) return;

        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            weaponMulti.Shoot(targets);
        }
    }
}
