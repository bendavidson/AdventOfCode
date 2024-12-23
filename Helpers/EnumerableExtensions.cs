using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Helpers
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach (var parent in source)
            {
                yield return parent;

                var children = selector(parent);
                foreach (var child in SelectRecursive(children, selector))
                    yield return child;
            }
        }

        public static IEnumerable<T> SelectLastChild<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach(var parent in source)
            {
                if (selector(parent).Any())
                {
                    var children = selector(parent);

                    yield return (T)children.SelectLastChild(selector);

                }
                else
                    yield return parent;
            }
        }
    }
}
