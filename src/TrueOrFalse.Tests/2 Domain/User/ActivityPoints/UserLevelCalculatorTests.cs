using NUnit.Framework;

[TestFixture]
public class UserLevelCalculatorTests
{

    //[Datapoint]
    //public double[] ArrayDouble1 = { 1.2, 3.4 };

    [Test]
    public void Should_calculate_level()
    {

        Assert.That(UserLevelCalculator.GetLevel(1), Is.EqualTo(0));
        Assert.That(UserLevelCalculator.GetLevel(65), Is.EqualTo(1));
        Assert.That(UserLevelCalculator.GetLevel(1999), Is.EqualTo(4));
        Assert.That(UserLevelCalculator.GetLevel(2000), Is.EqualTo(5));
        Assert.That(UserLevelCalculator.GetLevel(4000), Is.EqualTo(6));
        Assert.That(UserLevelCalculator.GetLevel(7463), Is.EqualTo(6));
        Assert.That(UserLevelCalculator.GetLevel(7464), Is.EqualTo(7));
        Assert.That(UserLevelCalculator.GetLevel(16126), Is.EqualTo(7));
        Assert.That(UserLevelCalculator.GetLevel(16127), Is.EqualTo(8));
        Assert.That(UserLevelCalculator.GetLevel(27857), Is.EqualTo(8));
        Assert.That(UserLevelCalculator.GetLevel(27858), Is.EqualTo(9));
        Assert.That(UserLevelCalculator.GetLevel(1624950), Is.EqualTo(39));
        Assert.That(UserLevelCalculator.GetLevel(1716955), Is.EqualTo(39));
        Assert.That(UserLevelCalculator.GetLevel(11219485), Is.EqualTo(99));
        Assert.That(UserLevelCalculator.GetLevel(11447346), Is.EqualTo(99));
        Assert.That(UserLevelCalculator.GetLevel(11447347), Is.EqualTo(100));
        Assert.That(UserLevelCalculator.GetLevel(12859989), Is.EqualTo(100));

    }
}
