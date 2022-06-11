using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PourLiquidReceiver : MonoBehaviour
{

    [SerializeField] private AlchemyRecipe alchemyRecipe;
    public UnityEvent OnLiquidParticleAdded = null;
    private void OnParticleCollision(GameObject other)
    {
        // other is particleSystem
        if (other.TryGetComponent<SpillFluid>(out SpillFluid sf))
        {
            ObjectProperties op = other.transform.parent.parent.parent.GetComponent<ObjectProperties>();
            if(op.gameObject.CompareTag("CustomPotion") || op.gameObject.CompareTag("EmptyPotion"))
            {
                return;
            }
            else if(alchemyRecipe)
            {
                if(OnLiquidParticleAdded != null)
                {
                    OnLiquidParticleAdded.Invoke();
                }
                alchemyRecipe.AddIngredient(op);
            }
        }
        else {
            return;
        }
    }
}
