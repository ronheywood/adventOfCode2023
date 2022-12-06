using NUnit.Framework.Constraints;

namespace TestProject1.Helpers;


public enum RockPaperScissorsGameChoice
{
    None = 0,
    Rock = 1,
    Paper = 2,
    Scissors = 3,
}

public class RockPaperScissors
{
    private static Dictionary<string, RockPaperScissorsGameChoice> _playerDictionary = new Dictionary<string, RockPaperScissorsGameChoice>()
    {
        { "A", RockPaperScissorsGameChoice.Rock },
        { "B", RockPaperScissorsGameChoice.Paper },
        { "C", RockPaperScissorsGameChoice.Scissors },
        { "X", RockPaperScissorsGameChoice.Rock },
        { "Y", RockPaperScissorsGameChoice.Paper },
        { "Z", RockPaperScissorsGameChoice.Scissors },
    };

    public static int Result(RockPaperScissorsGameChoice playerA, RockPaperScissorsGameChoice playerB)
    {
        if (playerA == playerB) return 3;
        switch (playerA)
        {
            case RockPaperScissorsGameChoice.Paper when playerB == RockPaperScissorsGameChoice.Scissors:
            case RockPaperScissorsGameChoice.Scissors when playerB == RockPaperScissorsGameChoice.Rock:
            case RockPaperScissorsGameChoice.Rock when playerB == RockPaperScissorsGameChoice.Paper:
                return 6;
            case RockPaperScissorsGameChoice.None:
            default:
                return 0;
        }
    }

    public static int Score(RockPaperScissorsGameChoice playerA, RockPaperScissorsGameChoice playerB)
    {
        return Result(playerA, playerB) + (int) playerB;
    }

    public static Tuple<RockPaperScissorsGameChoice,RockPaperScissorsGameChoice> PlayerChoices(string input)
    {
        var inputArray = input.Split(' ');
        return new Tuple<RockPaperScissorsGameChoice, RockPaperScissorsGameChoice>(_playerDictionary[inputArray[0]], _playerDictionary[inputArray[1]]);
    }

    public static int GameResult(IEnumerable<string> input, bool rigTheGame = false)
    {
        var total = 0;
        foreach (var s in input)
        {
            var round = (rigTheGame) ? FixResult(s) : s;
            var (player1,player2) = PlayerChoices(round);
            total += Score(player1, player2);
        }

        return total;
    }

    public static string FixResult(string input)
    {
        var winner = new Dictionary<string, string> { {"A", "B"},{"B" , "C"}, {"C" , "A"} };
        var lose = new Dictionary<string, string> { {"A", "C"},{"B" , "A"}, {"C" , "B"} };
        
        var strings = input.Split(' ');
        var playerA = strings[0];
        
        //Draw
        if(strings[1] == "Y") return $"{playerA} {playerA}";
        
        var desiredOutcome = (strings[1] == "Z") ? winner : lose;
        var choiceToGetDesiredOutcome = desiredOutcome[playerA];
        return $"{playerA} {choiceToGetDesiredOutcome}";
    }
}