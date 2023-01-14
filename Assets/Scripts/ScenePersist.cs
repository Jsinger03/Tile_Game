using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    void Awake()
    {
        int numScenePersists = FindObjectsOfType<ScenePersist>().Length;
        if (numScenePersists > 1)
        {
            Destroy(gameObject);//if we have more than one, destroy the newly created one and keep the old one
        }
        else
        {
            DontDestroyOnLoad(gameObject);//if there is no existing game(Object) dont destroy it
        }
    }

    public void ScenePersistReset()
    {
        Debug.Log("destroying");
        Destroy(gameObject);
    }
}
