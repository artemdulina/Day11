using System.Collections.Generic;

namespace CustomCollectionsTests
{
    public class CompareByValue : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (x > y)
                return -1;
            if (x < y)
                return 1;
            return 0;
        }
    }

    public class CompareStrings : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x.Length > y.Length)
                return 1;
            if (x.Length < y.Length)
                return -1;
            return 0;
        }
    }

    //Критерий сравнения точек?
    public class ComparePoints : IComparer<Point>
    {
        public int Compare(Point x, Point y)
        {
            if (x.X > y.X)
                return 1;
            if (x.X < y.X)
                return -1;
            return 0;
        }
    }
}
