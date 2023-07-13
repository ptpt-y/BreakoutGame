using System.Collections.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    private SpriteRenderer sr;
    public int Hitpoints = 1;
    public ParticleSystem DestroyEffect;
    public static event Action<Brick> OnBrickDestruction;
    private void Awake()
    {
        this.sr = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
        Ball ball = coll.gameObject.GetComponent<Ball>();
        ApplyCollisionLogic(ball);
    }

    private void ApplyCollisionLogic(Ball ball)
    {
        AudioManager.Instance.HitAudioPlay();
        this.Hitpoints--;
        if (this.Hitpoints <= 0)
        {
            BricksManager.Instance.RemainingBricks.Remove(this);
            OnBrickDestruction?.Invoke(this);
            CheckBrickSpawn();
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {
            this.sr.sprite = BricksManager.Instance.Sprites[this.Hitpoints - 1];
        }
    }
    private void CheckBrickSpawn(){
        float buffSpawnChance = UnityEngine.Random.Range(0f,1f);
        float deBuffSpawnChance = UnityEngine.Random.Range(0f,1f);
        bool alreadySpawned = false;
        if(buffSpawnChance <= CollectableManager.Instance.BuffsChance){
            alreadySpawned = true;
            Collectable newBuff = this.SpawnCollectable(true);
        }
        if(deBuffSpawnChance <= CollectableManager.Instance.DebuffsChance && !alreadySpawned){
            alreadySpawned = true;
            Collectable newBuff = this.SpawnCollectable(false);
        }
    }
    private Collectable SpawnCollectable(bool isBuff){
        List<Collectable> collection;
        if(isBuff){
            collection = CollectableManager.Instance.AvaliableBuffs;
        }else{
            collection = CollectableManager.Instance.AvaliablDebuffs;
        }
        int buffIndex = UnityEngine.Random.Range(0,collection.Count);
        Collectable prefab = collection[buffIndex];
        Collectable newCollectable = Instantiate(prefab,this.transform.position,Quaternion.identity) as Collectable;
        return newCollectable;
    }

    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.02f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }
    public void Init(Transform containerTransform, Sprite sprite, Color color, int hitpoints)
    {
        this.transform.SetParent(containerTransform);
        this.sr.sprite = sprite;
        this.sr.color = color;
        this.Hitpoints = hitpoints;
    }

}
