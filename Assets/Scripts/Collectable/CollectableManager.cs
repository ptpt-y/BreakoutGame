using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    #region Singleton
    private static CollectableManager _instance;
    public static CollectableManager Instance => _instance;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public List<Collectable> AvaliableBuffs;
    public List<Collectable> AvaliablDebuffs;
    [Range(0f,1f)]
    public float BuffsChance;
    [Range(0f,1f)]
    public float DebuffsChance;
}
