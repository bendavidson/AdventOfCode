using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Helpers
{
    internal class GreaterThan : IEqualityComparer<int>
    {
        public bool Equals(int a, int b)
        {
            if (a > b)
                return true;
            else
                return false;
        }

        public int GetHashCode(int obj)
        {
            return obj;
        }
    }
    internal class LessThan : IEqualityComparer<int>
    {
        public bool Equals(int a, int b)
        {
            if (a < b)
                return true;
            else
                return false;
        }

        public int GetHashCode(int obj)
        {
            return obj;
        }
    }
}
