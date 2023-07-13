using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] lifeSprites;
    private void Awake() {
        Brick.OnBrickDestruction += OnBrickDestruction;
        BricksManager.OnLevelLoaded += OnLevelLoaded;
        GameManager.OnLifeLost += OnLifeLost;
    }
    private void Start() {
        // Brick.OnBrickDestruction += OnBrickDestruction;
        // BricksManager.OnLevelLoaded += OnLevelLoaded;
        // GameManager.Instance.OnLifeLost += OnLifeLost;
    }
    private void OnLevelLoaded(){

    }
    private void OnBrickDestruction(Brick obj){
        
    }
    private void OnLifeLost(int remainingLives){
        lifeSprites[remainingLives].SetActive(false);
    }
    private void OnDisable() {
        Brick.OnBrickDestruction -= OnBrickDestruction;
        BricksManager.OnLevelLoaded -= OnLevelLoaded;
        GameManager.OnLifeLost -= OnLifeLost;
    }
}
