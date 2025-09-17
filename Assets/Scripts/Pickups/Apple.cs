using UnityEngine;

public class Apple : Pickup
{
    LevelGenerator levelGenerator;
    ScoreManager scoreManager;
    [SerializeField] float changeMovespeedAmount = 2f;
    [SerializeField] int appleScore = 50;

    public void Init(LevelGenerator levelGenerator, ScoreManager scoreManager)
    {
        this.levelGenerator = levelGenerator;
        this.scoreManager = scoreManager;

    }
    protected override void OnPickup()
    {
        levelGenerator.ChangeChunkMoveSpeed(changeMovespeedAmount);
        scoreManager.IncreaseScore(appleScore);
    }
}
