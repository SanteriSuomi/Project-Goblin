using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField]
    KeyCode meleeButton = KeyCode.Mouse1;
    [SerializeField]
    Transform meleeDetectionPoint;
    [SerializeField]
    float damage = 24.9f;
    [SerializeField]
    float meleeAttackCooldown = 1.5f;
    float meleeAttackTimer;
    [SerializeField]
    float meleeAttackRadius = 0.5f;

    private void Awake()
    {
        meleeAttackTimer = meleeAttackCooldown + 0.01f;
    }

    private void Update()
    {
        meleeAttackTimer += Time.deltaTime;
        if (Input.GetKey(meleeButton) && meleeAttackTimer >= meleeAttackCooldown)
        {
            meleeAttackTimer = 0;
            var hits = Physics.SphereCastAll(meleeDetectionPoint.position, meleeAttackRadius, transform.forward, 1);
            var set = new HashSet<Enemy>();
            foreach (var hit in hits)
            {
                if (hit.transform != null && hit.transform.TryGetComponent<Enemy>(out Enemy enemy) && !set.Contains(enemy))
                {
                    Debug.Log("melee hit");
                    set.Add(enemy);
                    enemy.ModifyHealth(-damage);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(meleeDetectionPoint.position, meleeAttackRadius);
    }
}
