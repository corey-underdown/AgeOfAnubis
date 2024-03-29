using UnityEngine;
using System.Collections;

public enum WeaponSwing {LIGHT, MEDIUM, HEAVY}
[System.Serializable]
public class Weapon : MonoBehaviour 
{
    public int m_itemID;
    public int m_level;
    public string m_name;
    public int m_goldCost;
	public Attack m_attack;
	private PolygonCollider2D m_col;
    public GameObject m_trail;

	public WeaponSwing m_swingType;

	private bool m_isAttacking = false;

    void Awake()
    {
		m_col = gameObject.GetComponent<PolygonCollider2D>();
        m_col.enabled = false;
    }

	void Update()
	{
        if (transform.parent.parent.parent.GetComponent<Player>().isInAttackAnimation())
        {
            m_isAttacking = true;
        }
        else
        {
            m_isAttacking = false;
        }

        if(transform.parent.parent.parent.GetComponent<Player>().isAttacking())
        {
            m_col.enabled = true;
			if(m_trail != null)
				m_trail.SetActive(true);
        }
        else
        {
            m_col.enabled = false;
			if(m_trail != null)
				m_trail.SetActive(false);
        }
	}

    public void Attack(Animator anim)
    {
        if (!m_isAttacking)
		{
			PlaySwingSFX();
			anim.SetInteger("AttackType", (int)m_swingType);
            anim.SetTrigger("Attack");
		} 
    }

	void OnTriggerEnter2D(Collider2D col)
	{
		Damageable damagable = col.GetComponent<Damageable>();

		if (damagable != null)
		{
			damagable.OnTakeDamage(m_attack.GetDamage(transform));


			PlaySFX();

			Enemy en = col.gameObject.GetComponent<Enemy>();
			if (en != null)
			{


			}
		}
	}

    public void ApplyWeaponData(WeaponData data)
    {
        m_level = data.level;
        m_itemID = data.itemID;
        m_name = data.name;
        m_attack.m_attackStrength = data.attackStrength;
        m_attack.m_knockbackForce = data.knockback;
        m_attack.m_effectType = data.effectType;
        m_attack.m_effectStrength = data.effectStrength;
        m_attack.m_effectDuration = data.effectDuration;
        m_goldCost = data.goldCost;
    }

	void PlaySFX()
	{
		switch (m_swingType)
		{
			case WeaponSwing.LIGHT:
			case WeaponSwing.MEDIUM:
				AudioManager.Inst.PlaySFX(AudioManager.Inst.a_eny_takeDamage);
				break;
			case WeaponSwing.HEAVY:
				print("Heavy");
				AudioManager.Inst.PlaySFX(AudioManager.Inst.a_thump);
				AudioManager.Inst.PlaySFX(AudioManager.Inst.a_eny_takeDamage);
				break;
		}
	}

	void PlaySwingSFX()
	{
		switch (m_swingType)
		{
			case WeaponSwing.LIGHT:
				AudioManager.Inst.PlaySFX(AudioManager.Inst.a_dagger_swing);
				break;
			case WeaponSwing.MEDIUM:
				//AudioManager.Inst.PlaySFX(AudioManager.Inst.a_giveDamage); TODO
				AudioManager.Inst.PlaySFX(AudioManager.Inst.a_cut);
				break;
			case WeaponSwing.HEAVY:
				AudioManager.Inst.PlaySFX(AudioManager.Inst.a_axe_swing);
				break;
		}
	}
}
