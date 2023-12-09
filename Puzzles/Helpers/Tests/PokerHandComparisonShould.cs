namespace TestProject1.Helpers.Tests;

public class PokerHandComparisonShould
{
    [Test]
    public void Identify_hands_with_a_high_card()
    {
        var hand = new Tuple<string, string>("23475","");
        Assert.That(PokerHandComparison.HandStrength(hand),Is.EqualTo(HandStrength.HighCard));
    }

    [TestCase("22345")]
    [TestCase("25356")]
    [TestCase("35243")]
    public void Pair(string cards){
        var hand = new Tuple<string, string>(cards,"");
        Assert.That(PokerHandComparison.HandStrength(hand),Is.EqualTo(HandStrength.Pair));
    }
    
    [TestCase("22335")]
    [TestCase("25323")]
    [TestCase("35223")]
    public void Two_pair(string cards)
    {
        var hand = new Tuple<string, string>(cards,"");
        Assert.That(PokerHandComparison.HandStrength(hand),Is.EqualTo(HandStrength.TwoPair));
    }

    [TestCase("22245")]
    [TestCase("45222")]
    [TestCase("42225")]
    public void Three_of_a_kind(string cards)
    {
        var hand = new Tuple<string, string>(cards,"");
        Assert.That(PokerHandComparison.HandStrength(hand),Is.EqualTo(HandStrength.ThreeOfAKind));
    }
    
    [TestCase("22333")]
    [TestCase("33322")]
    public void Full_house(string fullHouse)
    {
        var hand = new Tuple<string, string>(fullHouse,"");
        Assert.That(PokerHandComparison.HandStrength(hand),Is.EqualTo(HandStrength.FullHouse));
    }
    
    [TestCase("22225")]
    [TestCase("52222")]
    public void Four_of_a_kind(string fourOfAKind)
    {
        var hand = new Tuple<string, string>(fourOfAKind,"");
        Assert.That(PokerHandComparison.HandStrength(hand),Is.EqualTo(HandStrength.FourOfAKind));
    }
    
    [Test]
    public void Five_of_a_kind()
    {
        var hand = new Tuple<string, string>("22222","");
        Assert.That(PokerHandComparison.HandStrength(hand),Is.EqualTo(HandStrength.FiveOfAKind));
    }
}