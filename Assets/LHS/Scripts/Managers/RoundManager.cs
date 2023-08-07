using UnityEngine;

public enum Round { ROUND1, ROUND2, END };

public class RoundManager : MonoBehaviour
{
    private int roundIndex = 0;
    public int RoundIndex
    {
        get
        {
            return roundIndex;
        }
        set
        {
            roundIndex = value;
        }
    }

    public Round GetRound()
    {
        return (Round)roundIndex;
    }

    public void SetRound(Round round)
    {
        switch (round)
        {
            case Round.ROUND1:
                RoundOne();
                break;
            case Round.ROUND2:
                RoundTwo();
                break;
            case Round.END:
                EndGame();
                break;
        }
    }

    public void RoundOne()
    {
        
    }

    public void RoundTwo()
    {
        SwitchTeam();
        GameManager.Scene.LoadScene(Scene.GAME);
    }

    public void EndGame()
    {

    }

    public void SwitchTeam()
    {

    }
}
