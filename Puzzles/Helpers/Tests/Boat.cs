namespace TestProject1.Helpers.Tests;

public class Boat
{
    private int _speed;
    private long _distance = 0;
    public long Distance => _distance;

    public int Speed => _speed;

    public int Time { get; set; }

    public void Charge(int chargeTime)
    {
        if (chargeTime >= Time)
        {
            _speed = 0;
            _distance = 0;
            return;
        }
        _speed = chargeTime;
        _distance = CalculateDistanceForCharge(chargeTime);
    }

    private long CalculateDistanceForCharge(int chargeTime)
    {
        return chargeTime * (Time - chargeTime);
    }

    public IEnumerable<Tuple<long,long>> ChargeTimes()
    {
        var list = new List<Tuple<long, long>>();
        for (var i = 0; i < Time-1; i++)
        {
            list.Add(new Tuple<long, long>(i+1,CalculateDistanceForCharge(i+1)) );
        }
        return list;
    }
}