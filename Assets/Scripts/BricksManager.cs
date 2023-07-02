using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BricksManager : MonoBehaviour
{
    #region Singleton
    private static BricksManager _instance;
    public static BricksManager Instance => _instance;
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

    private int maxRows = 15;
    private int maxCols = 15;

    private GameObject bricksContainer;
    private float initialBrickSpawnPositionX = -7.58f;
    private float initialBrickSpawnPositionY = 3.25f;
    private float shiftAmountX = 0.365f * 3f;
    private float shiftAmountY = 0.365f * 1.5f;

    public Brick brickPrefab;
    public Sprite[] Sprites;
    public Color[] BrickColors;
    public List<Brick> RemainingBricks { get; set; }
    public List<int[,]> LevelsData { get; set; }
    public int InitialBricksCount { get; set; }
    public int CurrentLevel;
    private void Start()
    {
        this.bricksContainer = new GameObject("BricksContainer");
        this.LevelsData = this.LoadLevelsData();
        this.GenerateBricks();
    }
    public void LoadLevel(int level)
    {
        this.CurrentLevel = level;
        this.ClearRemainingBricks();
        this.GenerateBricks();
    }
    public void LoadNextLevel()
    {
        this.CurrentLevel++;
        if (this.CurrentLevel >= this.LevelsData.Count)
        {
            GameManager.Instance.ShowVictoryScreen();
        }
        else
        {
            this.LoadLevel(this.CurrentLevel);
        }
    }
    private void ClearRemainingBricks()
    {
        foreach (Brick brick in this.RemainingBricks)
        {
            Destroy(brick.gameObject);
        }
    }
    private void GenerateBricks()
    {
        this.RemainingBricks = new List<Brick>();
        int[,] currentLevelData = this.LevelsData[this.CurrentLevel];
        float currentSpawnX = initialBrickSpawnPositionX;
        float currentSpawnY = initialBrickSpawnPositionY;
        float zShift = 0;
        for (int row = 0; row < this.maxRows; row++)
        {
            for (int col = 0; col < this.maxCols; col++)
            {
                int brickType = currentLevelData[row, col];
                if (brickType > 0)
                {
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(bricksContainer.transform, this.Sprites[brickType - 1], this.BrickColors[brickType], brickType);

                    this.RemainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }
                currentSpawnX += shiftAmountX;
                if (col + 1 == this.maxCols)
                {
                    currentSpawnX = initialBrickSpawnPositionX;
                }
            }
            currentSpawnY -= shiftAmountY;
        }
        this.InitialBricksCount = this.RemainingBricks.Count;
    }
    private List<int[,]> LoadLevelsData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;
        string[] rows = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxCols];
        int currentRow = 0;

        for (int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];
            if (line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < bricks.Length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }
                currentRow++;
            }
            else
            {
                currentRow = 0;
                levelsData.Add(currentLevel);
                currentLevel = new int[maxRows, maxCols];
            }
        }
        return levelsData;
    }
}
