using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    Animator anim;

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
    [SerializeField]
    GameObject dagger;

    [SerializeField]
    LayerMask meleeLayerMask;

    public bool IsMelee { get; set; } = false;

    private void Awake()
    {
        meleeAttackTimer = meleeAttackCooldown + 0.01f;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Melee();
    }

    void Melee()
    {
        meleeAttackTimer += Time.deltaTime;
        if (meleeAttackTimer >= meleeAttackCooldown)
        {
            dagger.SetActive(false);
            IsMelee = false;
            meleeAttackTimer = 0;
        }
        else if (Input.GetKeyDown(meleeButton) && !IsMelee)
        {
            IsMelee = true;
            dagger.SetActive(true);
            anim.SetTrigger("Melee");
            var hits = Physics.OverlapSphere(meleeDetectionPoint.position, meleeAttackRadius, meleeLayerMask, QueryTriggerInteraction.Ignore);
            var set = new HashSet<Enemy>();
            foreach (var hit in hits)
            {
                if (hit.transform != null && hit.transform.TryGetComponent<Enemy>(out Enemy enemy) && !set.Contains(enemy))
                {
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
