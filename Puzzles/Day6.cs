using TestProject1.Helpers;

namespace TestProject1;

public class Day6
{
    [TestCase(1896)]
    public void Should_identify_start_of_packet_from_characters_from_puzzle_input(int correctAnswer)
    {
        var input = PuzzleInput.GetFile("day6.txt");
        Assert.That(Communicator.StartOfPacket(input.First()), Is.EqualTo(correctAnswer));
    }
    
    [TestCase(3452)]
    public void Should_identify_start_of_message_from_characters_from_puzzle_input(int correctAnswer)
    {
        var input = PuzzleInput.GetFile("day6.txt");
        Assert.That(Communicator.StartOfMessage(input.First()), Is.EqualTo(correctAnswer));
    }
}