using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSystem : BaseSystem
{
    private Filter[] filters;

    public struct Filter
    {
        public Filter(
            int id,
            GameObject go,
            InputComponent ic
            )
        {
            this.id = id;

            gameObject = go;
            inputComponent = ic;
        }

        public int id;

        public GameObject gameObject;
        public InputComponent inputComponent;
    }

    public override void Initialize(Transform[] objects)
    {
        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            InputComponent ic = objects[i].GetComponent<InputComponent>();

            if (ic)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, ic));
            }
        }

        filters = tmpFilters.ToArray();
    }

    public override void SetupInputComponent(InputComponent inputComponent)
    {
        player.healthComponent.OnPlayerTakeDamage += PlayTakeDamageEffect;
        //player.healthComponent.OnPlayerRestoreDamage += RestoreHeart;
        player.animationComponent.OnPlayGatherAnimation += PlayGatherEffect;
    }

    public override void Tick(float deltaTime)
    {
        
    }

    private void PlayTakeDamageEffect()
    {
        player.vfxComponent.damageEffectSystem.Play();    
    }

    private void PlayGatherEffect()
    {
        player.vfxComponent.gatherEffect.Play();
    }

}
