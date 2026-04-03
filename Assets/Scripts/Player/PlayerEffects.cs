using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem healingEffect;
    [SerializeField] private ParticleSystem damageEffect;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        PlayerAttribute.OnPlayerDamageTaken += PlayDamageEffect;
        PlayerAttribute.OnPlayerHealed += PlayHealingEffect;
        PlayerAttribute.OnPlayerAteFood += PlayEatingAnimation;
        if (animator == null) animator = GetComponentInChildren<Animator>();

    }
    [Button("PlayHealingEffect")]

    public void PlayHealingEffect()
    {
        if (healingEffect != null)
        {
            healingEffect.Play();
        }
    }
    [Button("PlayEatingAnimation")]
    public void PlayEatingAnimation()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();

        animator.SetTrigger("ChompTrigger");
    }
    [Button("PlayDamageEffect")]
    public void PlayDamageEffect()
    {
        if (damageEffect != null)
        {
            damageEffect.Play();
        }
    }
    private void OnDestroy()
    {
        PlayerAttribute.OnPlayerDamageTaken -= PlayDamageEffect;
        PlayerAttribute.OnPlayerHealed -= PlayHealingEffect;
        PlayerAttribute.OnPlayerAteFood -= PlayEatingAnimation;
    }



}
