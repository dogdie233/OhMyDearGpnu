using System.Collections;

namespace OhMyDearGpnu.Api.Modules
{
    [Flags]
    public enum OddEvenFlag : byte
    {
        None = 0,
        Odd = 1,
        Even = 2,
        Both = 3
    }
    
    public struct NumberRange : IEnumerable<int>, IEquatable<NumberRange>, IComparable<int>
    {
        public readonly int start;  // include
        public readonly int end;  // exclude
        public readonly OddEvenFlag oddEvenFlag;
        
        // flag odd -> 0 
        // flag even -> 1
        // flag both -> 2
        public int RealStart => start + ((((int)oddEvenFlag - 1) ^ (start & 1)) == 0 ? 1 : 0);
        
        #region Constructors
        /// <summary>
        /// Create a range to describe a period
        /// Start must be less than or equal to end
        /// </summary>
        /// <param name="start">Include</param>
        /// <param name="end">Exclude</param>
        /// <param name="oddEvenFlag">odd even flag</param>
        public NumberRange(int start, int end, OddEvenFlag oddEvenFlag = OddEvenFlag.Both)
        {
            if (start > end)
                throw new ArgumentException("Start must be less than or equal to end.");
            this.start = start;
            this.end = end;
            this.oddEvenFlag = oddEvenFlag;
        }

        public static NumberRange Parse(string str)
            => Parse(str.AsSpan());

        public static NumberRange Parse(ReadOnlySpan<char> str)
        {
            str = str.Trim();  // trim spaces
            var connectionIndex = str[1..].IndexOf('-') + 1;  // skip first character maybe mean negative sign
            var bracketIndex = str.IndexOf('(');
            var flags = OddEvenFlag.Both;
            if (bracketIndex != -1)
            {
                if (bracketIndex < connectionIndex)
                    throw new ArgumentException($"Invalid range format. {str}");
                flags = str[bracketIndex + 1] switch
                {
                    '单' => OddEvenFlag.Odd,
                    '双' => OddEvenFlag.Even,
                    var c => throw new ArgumentException($"Invalid flag: {c}")
                };
                str = str[..bracketIndex];
            }
            if (connectionIndex == 0)
            {
                var value = int.Parse(str);
                return new NumberRange(value, value + 1, flags);
            }

            var left = int.Parse(str[..connectionIndex]);
            var right = int.Parse(str[(connectionIndex + 1)..]);
            if (left > right)
                (left, right) = (right, left);
            return new NumberRange(left, right + 1, flags);
        }
        #endregion

        public bool IsInRange(int value) => oddEvenFlag != OddEvenFlag.None && value >= RealStart && value < end &&
                                            (oddEvenFlag == OddEvenFlag.Both || ((value - RealStart) & 1) == 0);

        #region Equatable
        public override bool Equals(object? obj) => obj is NumberRange range && Equals(range);
        public bool Equals(NumberRange other) => start == other.start && end == other.end;

        public override int GetHashCode() => HashCode.Combine(start, end);

        public static bool operator ==(NumberRange left, NumberRange right) => left.Equals(right);
        public static bool operator !=(NumberRange left, NumberRange right) => !(left == right);
        #endregion

        #region Comparable
        public int CompareTo(int other)
        {
            if (other >= end)
                return -1;
            if (other < start)
                return 1;
            return 0;
        }

        public static bool operator >(int left, NumberRange right) => left >= right.end;
        public static bool operator >(NumberRange left, int right) => left.start > right;
        public static bool operator <(int left, NumberRange right) => left < right.start;
        public static bool operator <(NumberRange left, int right) => left.end <= right;
        #endregion

        #region Enumerable

        public IEnumerator<int> GetEnumerator()
            => oddEvenFlag == OddEvenFlag.None
                ? new Enumerator(0, 0, 1)
                : new Enumerator(RealStart, end, 3 - ((int)oddEvenFlag & 1) - ((int)oddEvenFlag >> 1));

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<int>
        {
            private int current;
            private readonly int startAt;
            private readonly int endAt;
            private readonly int step;

            public int Current => current;
            object IEnumerator.Current => current;

            public Enumerator(int startAt, int endAt, int step)
            {
                this.startAt = startAt;
                this.endAt = endAt;
                this.step = step;
                Reset();
            }

            public bool MoveNext()
            {
                current += step;
                return current < endAt;
            }
            
            public void Reset() => current = startAt - step;
            public void Dispose() { }
        }
        #endregion
    }
}
