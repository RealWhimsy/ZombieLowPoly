using UnityEngine;

public class StaticDifficultyManager : MonoBehaviour
{
    void Start()
    {
        int difficulty = Mathf.RoundToInt(Random.Range(Const.Difficulties.MinDifficultyIndex, Const.Difficulties.MaxDifficultyIndex));
        Difficulty.SetDifficultyToIndex(difficulty);
    }
}
