using UnityEngine;

public class MagicAttack : IAttack
{
    private float attackRange = 15f;
    private int defaultDamage = 15;
    private float lastAttackTime = 0f;
    private float attackCooldown = 1f;
    private LineRenderer lineRenderer;

    public MagicAttack()
    {
        GameObject lineObj = new GameObject("MagicAttackRay");
        lineRenderer = lineObj.AddComponent<LineRenderer>();

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.cyan;
        lineRenderer.enabled = false;
    }

    public void ExecuteAttack(Transform attackerTransform, int damage = -1)
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        int finalDamage = (damage != -1) ? damage : defaultDamage;

        Vector3 attackStart = attackerTransform.position + Vector3.up * 0.5f;
        Vector3 direction = attackerTransform.forward;
        Vector3 attackEnd = attackStart + direction * attackRange;

        Debug.DrawLine(attackStart, attackEnd, Color.blue, 1f);
        Debug.Log($"ÀÒÀÊÓÞ Magic Attack Ray: Start={attackStart}, End={attackEnd}");

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, attackStart);
        lineRenderer.SetPosition(1, attackEnd);

        attackerTransform.GetComponent<MonoBehaviour>().StartCoroutine(HideRay());

        if (Physics.Raycast(attackStart, direction, out RaycastHit hit, attackRange))
        {
            Debug.Log($"Hit: {hit.collider.name}");
            if (hit.collider.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(finalDamage, DamageType.MAGICAL);
            }
            else if (hit.collider.TryGetComponent(out PlayerController player))
            {
                player.ReceiveDamage(finalDamage);
            }
        }
        else
        {
            Debug.Log("No hit detected");
        }
    }

    private System.Collections.IEnumerator HideRay()
    {
        yield return new WaitForSeconds(0.2f);
        lineRenderer.enabled = false;
    }

    public float GetAttackRange()
    {
        return attackRange;
    }
}
