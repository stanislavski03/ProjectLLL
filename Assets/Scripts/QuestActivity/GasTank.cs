using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasTank : EInteractable
{
    public  override void Interact()
    {
        QuestManager.Instance.GasTankGathered();
        Destroy(gameObject);
    }


}
