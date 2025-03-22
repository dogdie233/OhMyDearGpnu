using System.Collections;

using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.Modules;

public struct DiscreteNumberRange : IEnumerable<int>
{
    private readonly NumberRange[] ranges;
    
    public DiscreteNumberRange()
    {
        this.ranges = [];
    }
    
    private DiscreteNumberRange(NumberRange[] ranges)
    {
        this.ranges = ranges;
    }

    public static DiscreteNumberRange Parse(string? str)
        => Parse(str.AsSpan());
    
    public static DiscreteNumberRange Parse(ReadOnlySpan<char> str)
    {
        if (str.Length == 0)
            return new DiscreteNumberRange();
        var userData = (idx: 0, ranges: new NumberRange[str.Count(',') + 1]);
        str.Split(',', SplitAction, ref userData);
        if (userData.idx != userData.ranges.Length)
            Array.Resize(ref userData.ranges, userData.idx);
        return Create(userData.ranges, false);
        
        static void SplitAction(ReadOnlySpan<char> span, scoped ref (int idx, NumberRange[] ranges) data)
        {
            data.ranges[data.idx++] = NumberRange.Parse(span);
        }
    }

    public static DiscreteNumberRange Create(NumberRange[] ranges, bool cloneArray = true)
    {
        NumberRange[] sortedRanges;
        if (cloneArray)
        {
            sortedRanges = new NumberRange[ranges.Length];
            Array.Copy(ranges, sortedRanges, ranges.Length);
        }
        else
        {
            sortedRanges = ranges;
        }
        Array.Sort(sortedRanges, (a, b) => a.start.CompareTo(b.start));
        
        var newRanges = new List<NumberRange>();
        var currentRange = sortedRanges[0];
        for (var i = 1; i < sortedRanges.Length; i++)
        {
            if (sortedRanges[i].oddEvenFlag == OddEvenFlag.None)
                continue;

            if (currentRange.end == sortedRanges[i].RealStart &&
                currentRange.oddEvenFlag == sortedRanges[i].oddEvenFlag)
            {
                currentRange = new NumberRange(currentRange.start, sortedRanges[i].end, currentRange.oddEvenFlag);
                continue;
            }
            if (currentRange.end <= sortedRanges[i].RealStart)
            {
                AddRange(currentRange);
                currentRange = sortedRanges[i];
                continue;
            }
            
            if (currentRange.oddEvenFlag == sortedRanges[i].oddEvenFlag)
            {
                currentRange = new NumberRange(currentRange.start, Math.Max(currentRange.end, sortedRanges[i].end), currentRange.oddEvenFlag);
                continue;
            }

            if (sortedRanges[i].end < currentRange.end)
            {
                if (currentRange.oddEvenFlag == OddEvenFlag.Both)
                    continue;
                AddRange(new NumberRange(currentRange.start, sortedRanges[i].RealStart, currentRange.oddEvenFlag));
                AddRange(new NumberRange(sortedRanges[i].RealStart, sortedRanges[i].end, OddEvenFlag.Both));
                currentRange = new NumberRange(sortedRanges[i].end, currentRange.end, currentRange.oddEvenFlag);
                continue;
            }

            if (sortedRanges[i].oddEvenFlag == OddEvenFlag.Both &&
                currentRange.RealStart == sortedRanges[i].RealStart)
            {
                currentRange = sortedRanges[i];
                continue;
            }
            AddRange(new NumberRange(currentRange.start, sortedRanges[i].RealStart, currentRange.oddEvenFlag));
            AddRange(new NumberRange(sortedRanges[i].RealStart, currentRange.end, OddEvenFlag.Both));
            currentRange = new NumberRange(currentRange.end, sortedRanges[i].end, sortedRanges[i].oddEvenFlag);
        }
        AddRange(currentRange);
        return new DiscreteNumberRange(newRanges.ToArray());
        
        void AddRange(in NumberRange range)
        {
            if (range.oddEvenFlag == OddEvenFlag.None || range.RealStart >= range.end)
                return;
            newRanges.Add(range);
        }
    }

    public static DiscreteNumberRange CreateEmpty()
        => new([]);

    public bool IsInRange(int number)
        => ranges.Any(range => range.IsInRange(number));
    
    public IEnumerator<int> GetEnumerator()
        => new Enumerator(ranges);

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    
    public struct Enumerator : IEnumerator<int>
    {
        private readonly NumberRange[] ranges = [];
        private NumberRange.Enumerator internalEnumerator;
        private int index = -1;

        public Enumerator(NumberRange[] ranges)
        {
            this.ranges = ranges;
            index = -1;
        }

        public int Current => internalEnumerator.Current;

        object? IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (internalEnumerator.MoveNext())
                return true;

            do
            {
                if (++index >= ranges.Length)
                    return false;
                internalEnumerator = (NumberRange.Enumerator)ranges[index].GetEnumerator();
            } while (!internalEnumerator.MoveNext());
            
            return true;
        }

        public void Reset()
        {
            index = -1;
        }
    }
}