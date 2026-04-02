using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem healingEffect;
    [SerializeField] private ParticleSystem damageEffect;
    private Animator animator;

    private void Start()
    {
        PlayerAttribute.OnPlayerDamageTaken += PlayDamageEffect;
        PlayerAttribute.OnPlayerHealed += PlayHealingEffect;
        PlayerAttribute.OnPlayerAteFood += PlayEatingAnimation;
        animator = GetComponentInChildren<Animator>();
    }
    public void PlayHealingEffect()
    {
        if (healingEffect != null)
        {
            healingEffect.Play();
        }
    }

    public void PlayEatingAnimation()
    {
        animator.SetTrigger("ChompTrigger");
    }

    public void PlayDamageEffect()
    {
        if (damageEffect != null)
        {
            damageEffect.Play();
        }
    }



}
