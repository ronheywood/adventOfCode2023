namespace TestProject1.Helpers.Tests;

public class PokerHandComparisonShould
{
    [Test]
    public void Identify_hands_with_a_high_card()
    {
        var hand = new Tuple<string, string>("23475", "");
        Assert.That(PokerHandComparison.HandStrength(hand), Is.EqualTo(HandStrength.HighCard));
    }

    [TestCase("22345")]
    [TestCase("25356")]
    [TestCase("35243")]
    public void Pair(string cards)
    {
        var hand = new Tuple<string, string>(cards, "");
        Assert.That(PokerHandComparison.HandStrength(hand), Is.EqualTo(HandStrength.Pair));
    }

    [TestCase("22335")]
    [TestCase("25323")]
    [TestCase("35223")]
    public void Two_pair(string cards)
    {
        var hand = new Tuple<string, string>(cards, "");
        Assert.That(PokerHandComparison.HandStrength(hand), Is.EqualTo(HandStrength.TwoPair));
    }

    [TestCase("22245")]
    [TestCase("45222")]
    [TestCase("42225")]
    public void Three_of_a_kind(string cards)
    {
        var hand = new Tuple<string, string>(cards, "");
        Assert.That(PokerHandComparison.HandStrength(hand), Is.EqualTo(HandStrength.ThreeOfAKind));
    }

    [TestCase("22333")]
    [TestCase("33322")]
    public void Full_house(string fullHouse)
    {
        var hand = new Tuple<string, string>(fullHouse, "");
        Assert.That(PokerHandComparison.HandStrength(hand), Is.EqualTo(HandStrength.FullHouse));
    }

    [TestCase("22225")]
    [TestCase("52222")]
    public void Four_of_a_kind(string fourOfAKind)
    {
        var hand = new Tuple<string, string>(fourOfAKind, "");
        Assert.That(PokerHandComparison.HandStrength(hand), Is.EqualTo(HandStrength.FourOfAKind));
    }

    [Test]
    public void Five_of_a_kind()
    {
        var hand = new Tuple<string, string>("22222", "");
        Assert.That(PokerHandComparison.HandStrength(hand), Is.EqualTo(HandStrength.FiveOfAKind));
    }

    [TestCase("J2346", "62346", HandStrength.Pair)]
    [TestCase("JKKK2", "KKKK2", HandStrength.FourOfAKind)]
    [TestCase("22JJJ", "22222", HandStrength.FiveOfAKind)]
    [TestCase("JJJJJ", "JJJJJ", HandStrength.FiveOfAKind,
        Description = "Jokers are Jacks for the purpose of a tie break")]
    [TestCase("KKKK2", "KKKK2", HandStrength.FourOfAKind)]
    [TestCase("J2222", "22222", HandStrength.FiveOfAKind)]
    [TestCase("JJJJ2", "22222", HandStrength.FiveOfAKind)]
    [TestCase("JJJJA", "AAAAA", HandStrength.FiveOfAKind)]
    [TestCase("JJJ2A", "AAA2A", HandStrength.FourOfAKind)]
    [TestCase("JJ22A", "2222A", HandStrength.FourOfAKind)]
    [TestCase("J222A", "2222A", HandStrength.FourOfAKind)]
    [TestCase("J22J4", "22224", HandStrength.FourOfAKind)]
    [TestCase("J22AA", "A22AA", HandStrength.FullHouse)]
    [TestCase("2233J", "22333", HandStrength.FullHouse)]
    [TestCase("J2234", "22234", HandStrength.ThreeOfAKind)]
    [TestCase("2234J", "22342", HandStrength.ThreeOfAKind)]
    [TestCase("22JJ1", "22221", HandStrength.FourOfAKind)]
    [TestCase("JJ577", "77577", HandStrength.FourOfAKind)]
    
    
    public void Jokers_are_wild(string cards, string expectedCards, HandStrength expectedHandStrength)
    {
        var hand = new Tuple<string, string>(cards, "");
        var wildCardHand = WildCardPoker.SelectWildCard(hand);
        Assert.Multiple(() =>
        {
            Assert.That(wildCardHand.Item1, Is.EqualTo(expectedCards));
            Assert.That(PokerHandComparison.HandStrength(wildCardHand), Is.EqualTo(expectedHandStrength));
        });
    }
}