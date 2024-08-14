using System.Collections;

namespace OhMyDearGpnu.Api.Modules
{
    public struct NumberRange : IEnumerable<int>, IEquatable<NumberRange>, IComparable<int>
    {
        public readonly int start;  // include
        public readonly int end;  // exclude

        #region Constructors
        /// <summary>
        /// Create a range to describe a period
        /// </summary>
        /// <param name="start">Include</param>
        /// <param name="end">Exclude</param>
        public NumberRange(int start, int end)
        {
            this.start = start;
            this.end = end;
        }

        public static NumberRange Parse(string str)
            => Parse(str.AsSpan());

        public static NumberRange Parse(ReadOnlySpan<char> str)
        {
            var connectionIndex = str.IndexOf('-');
            if (connectionIndex == -1)
                return new NumberRange(int.Parse(str), int.Parse(str) + 1);
            return new NumberRange(int.Parse(str.Slice(0, connectionIndex)), int.Parse(str.Slice(connectionIndex + 1, str.Length - connectionIndex - 1)));
        }
        #endregion

        public bool IsInRange(int value) => value >= start && value < end;

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
        public IEnumerator<int> GetEnumerator() => new NumberEnumerator(start, end);
        IEnumerator IEnumerable.GetEnumerator() => new NumberEnumerator(start, end);

        public struct NumberEnumerator : IEnumerator<int>
        {
            private int current;
            private readonly int startAt;
            private readonly int endAt;

            public int Current => current;
            object IEnumerator.Current => current;

            public NumberEnumerator(int startAt, int endAt)
            {
                current = startAt - 1;
                this.endAt = endAt;
            }

            public bool MoveNext() => ++current < endAt;
            public void Reset() => current = startAt - 1;
            public void Dispose() { }
        }
        #endregion
    }
}
