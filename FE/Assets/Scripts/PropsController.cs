using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsController : MonoBehaviour
{
    enum PropType { None, Glass, Hat }

    [SerializeField] private Transform[] props;
    [SerializeField] private PropType currentPropType = PropType.None;
    [SerializeField] private PropType newPropType;
    [SerializeField] private Transform currentProp;
    [SerializeField] private float despawn_time = 0.2f;
    [SerializeField] private float spawn_time = 0.5f;

    public void PropsSelection(int id)
    {
        if(id == 4)
        {
            DanceAnimation();
            return; 
        }
        newPropType = GetPropTypeById(id);
        if (newPropType == currentPropType && currentPropType != PropType.None)
        {
            DespawnProp(currentProp); 
        }
        currentPropType = newPropType; 
        SpawnProp(props[id]); 
    }

    private void DanceAnimation()
    {
        GetComponent<Animator>().SetTrigger("Dance"); 
    }

    private void DespawnProp(Transform currentProp)
    {
        currentProp.gameObject.SetActive(false);
    }

    private void SpawnProp(Transform prop)
    {
        prop.gameObject.SetActive(true);
        currentProp = prop; 

    }

    private PropType GetPropTypeById(int id)
    {
        if (id == 0 || id == 1)
        {
            return PropType.Glass;
        }
        if (id == 2 || id == 3)
        {
            return PropType.Hat;
        }
        return PropType.None;
    }


}
