using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem healingEffect;
    [SerializeField] private ParticleSystem damageEffect;

    private void Start()
    {
        PlayerAttribute.OnPlayerDamageTaken += PlayDamageEffect;
        PlayerAttribute.OnPlayerHealed += PlayHealingEffect;
    }
    public void PlayHealingEffect()
    {
        if (healingEffect != null)
        {
            healingEffect.Play();
        }
    }

    public void PlayDamageEffect()
    {
        if (damageEffect != null)
        {
            damageEffect.Play();
        }
    }



}
