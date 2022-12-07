// ReSharper disable StringLiteralTypo

namespace TestProject1.Helpers.Tests;

public class CommunicationDeviceTests
{
    [TestCase("m")]
    [TestCase("mj")]
    [TestCase("mjq")]
    public void Should_not_identify_a_start_of_packet_until_4_characters_are_seen(string input)
    {
        Assert.That(Communicator.IsStartOfPacketString(input.ToCharArray()), Is.False);
    }

    [TestCase("m")]
    [TestCase("mj")]
    [TestCase("mjq")]
    [TestCase("abcdefghijklm")]
    public void Should_not_identify_a_start_of_message_until_14_characters_are_seen(string input)
    {
        Assert.That(Communicator.IsStartOfMessageString(input.ToCharArray()), Is.False);
    }

    [Test]
    public void Should_not_report_a_start_of_packet_if_4_characters_are_not_unique()
    {
        var input = "mjqj";
        Assert.That(Communicator.IsStartOfPacketString(input.ToCharArray()), Is.False);
    }
    
    [TestCase("abcdefghijklma")]
    [TestCase("abcdefghijklmb")]
    [TestCase("abcdefghijklmc")]
    public void Should_not_identify_a_start_of_message_if_14_characters_are_not_unique(string input)
    {
        Assert.That(Communicator.IsStartOfMessageString(input.ToCharArray()), Is.False);
    }
    
    [TestCase("abcdefghijklmn")]
    public void Should_identify_a_start_of_message_if_14_characters_are_unique(string input)
    {
        Assert.That(Communicator.IsStartOfMessageString(input.ToCharArray()), Is.True);
    }
    
    [Test]
    public void Should_report_a_start_of_packet_if_4_characters_are_unique()
    {
        var input = "jpqm";
        Assert.That(Communicator.IsStartOfPacketString(input.ToCharArray()), Is.True);
    }
    
    [Test]
    public void Should_report_a_start_of_packet_if_last_4_characters_are_unique()
    {
        var input = "mjqjpqm";
        Assert.That(Communicator.IsStartOfPacketString(input.ToCharArray()), Is.True);
    }
    
    [TestCase("mjqjpqm",7)]
    [TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb",7)]
    [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz",5)]
    [TestCase("nppdvjthqldpwncqszvftbrmjlhg",6)]
    [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg",10)]
    [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw",11)]
    public void Should_identify_the_index_where_start_of_packet_identified(string input, int expectedStartOfPacket)
    {
        Assert.That(Communicator.StartOfPacket(input), Is.EqualTo(expectedStartOfPacket));
    }

    [TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb",19)]
    [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz",23)]
    [TestCase("nppdvjthqldpwncqszvftbrmjlhg",23)]
    [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg",29)]
    [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw",26)]
    public void Should_identify_the_index_where_start_of_message_identified(string input,int expected)
    {
        Assert.That(Communicator.StartOfMessage(input), Is.EqualTo(expected));
    }
}