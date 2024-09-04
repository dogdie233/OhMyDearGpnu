using OhMyDearGpnu.Api.Modules;

namespace OhMyDearGpnu.Api.Test;

public class DiscreteNumberRangeTests
{
    [SetUp]
    public void Init()
    {
    }

    [Test]
    public void ParseAllSingle()
    {
        var str = "-2,-1,0,1,2,3,5,4,6,2,1000,9,10";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [-2, -1, 0, 1, 2, 3, 4, 5, 6, 9, 10, 1000];
        CollectionAssert.AreEqual(expect, actual);
    }

    [Test]
    public void ParseEvil()
    {
        var str = "-1--1,-1-1,1-1,-1,1";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [-1, 0, 1];
        CollectionAssert.AreEqual(expect, actual);
    }

    [Test]
    public void ParseSameNumber()
    {
        var str = "10-10";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [10];
        CollectionAssert.AreEqual(expect, actual);
    }

    [Test]
    public void ParseNegativeSame()
    {
        var str = "-10--10";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [-10];
        CollectionAssert.AreEqual(expect, actual);
    }
    
    [Test]
    public void ParseNegativeToPositive()
    {
        var str = "-10-10";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [-10, -9, -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        CollectionAssert.AreEqual(expect, actual);
    }
    
    [Test]
    public void ParsePositiveToNegative()
    {
        var str = "10--10";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [-10, -9, -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        CollectionAssert.AreEqual(expect, actual);
    }

    [Test]
    public void ParseGreater()
    {
        var str = "10-20";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];
        CollectionAssert.AreEqual(expect, actual);
    }

    [Test]
    public void ParseGreaterButNegative()
    {
        var str = "-20--10";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [-20, -19, -18, -17, -16, -15, -14, -13, -12, -11, -10];
        CollectionAssert.AreEqual(expect, actual);
    }
    
    [Test]
    public void ParseLess()
    {
        var str = "20-10";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];
        CollectionAssert.AreEqual(expect, actual);
    }
    
    [Test]
    public void ParseLessButNegative()
    {
        var str = "-10--20";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [-20, -19, -18, -17, -16, -15, -14, -13, -12, -11, -10];
        CollectionAssert.AreEqual(expect, actual);
    }
    
    [Test]
    public void ParseEmpty()
    {
        var str = "";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [];
        CollectionAssert.AreEqual(expect, actual);
    }

    [Test]
    public void ParseNull()
    {
        string? str = null;
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [];
        CollectionAssert.AreEqual(expect, actual);
    }

    [Test]
    public void ParseFullOverlapRange()
    {
        var str = "1-4,2-3";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [1, 2, 3, 4];
        CollectionAssert.AreEqual(expect, actual);
    }

    [Test]
    public void ParseFullOverlapRange2()
    {
        var str = "2-3,1-4";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [1, 2, 3, 4];
        CollectionAssert.AreEqual(expect, actual);
    }

    [Test]
    public void ParsePartialOverlapRange()
    {
        var str = "1-3,2-4";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [1, 2, 3, 4];
        CollectionAssert.AreEqual(expect, actual);
    }
    
    [Test]
    public void ParsePartialOverlapRange2()
    {
        var str = "2-4,1-3";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [1, 2, 3, 4];
        CollectionAssert.AreEqual(expect, actual);
    }
    
    [Test]
    public void ParseGapRange()
    {
        var str = "1-2,4-5";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [1, 2, 4, 5];
        CollectionAssert.AreEqual(expect, actual);
    }
    
    [Test]
    public void ParseGapRange2()
    {
        var str = "4-5,1-2";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [1, 2, 4, 5];
        CollectionAssert.AreEqual(expect, actual);
    }

    [Test]
    public void ParseGapRangeAndSingle()
    {
        var str = "1-2,4,5-6";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [1, 2, 4, 5, 6];
        CollectionAssert.AreEqual(expect, actual);
    }

    [Test]
    public void ParseGapRangeAndSingle2()
    {
        var str = "1-2,5-6,4";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [1, 2, 4, 5, 6];
        CollectionAssert.AreEqual(expect, actual);
    }

    [Test]
    public void ParseNormalUsuallyData()
    {
        var str = "2-17(双),18,19";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [2, 4, 6, 8, 10, 12, 14, 16, 18, 19];
        CollectionAssert.AreEqual(expect, actual);
    }

    [Test]
    public void ParseNormalUsuallyData2()
    {
        var str = "2-17(单),18,19";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [3, 5, 7, 9, 11, 13, 15, 17, 18, 19];
        CollectionAssert.AreEqual(expect, actual);
    }

    [Test]
    public void ParseEvil2()
    {
        var str = "1-4(双),3-7,8-10(单)";
        var actual = DiscreteNumberRange.Parse(str);
        int[] expect = [2, 3, 4, 5, 6, 7, 9];
        CollectionAssert.AreEqual(expect, actual);
    }
}