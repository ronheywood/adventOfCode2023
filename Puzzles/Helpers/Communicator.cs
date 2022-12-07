using NUnit.Framework.Constraints;

namespace TestProject1.Helpers;

public static class Communicator
{
    public static bool IsStartOfPacketString(IEnumerable<char> charArray)
    {
        var lastFourCharacters = charArray.TakeLast(4).ToArray();
        return lastFourCharacters.Length >= 4 && CharactersAreUnique(lastFourCharacters);
    }

    public static int StartOfPacket(string input)
    {
        var cursorPosition = 0;
        while (true)
        {
            var startOfPacket = cursorPosition + 1;
            var check = input.ToCharArray().Take(startOfPacket);
            if (IsStartOfPacketString(check)) return startOfPacket;
            cursorPosition = startOfPacket;
        }
    }

    public static int StartOfMessage(string input)
    {
        var cursorPosition = 0;
        while (true)
        {
            var startOfPacket = cursorPosition + 1;
            var check = input.ToCharArray().Take(startOfPacket);
            if (IsStartOfMessageString(check)) return startOfPacket;
            cursorPosition = startOfPacket;
        }
    }

    public static bool IsStartOfMessageString(IEnumerable<char> charArray)
    {
        var fourteenCharacters = charArray.TakeLast(14).ToArray();
        return fourteenCharacters.Length >= 14 && CharactersAreUnique(fourteenCharacters);
    }

    private static bool CharactersAreUnique(char[] characters)
    {
        return characters.All(x => characters.Count(y => y == x) == 1);
    }
}