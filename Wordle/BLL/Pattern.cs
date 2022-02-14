using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.BLL
{
    public enum Pattern
    {
        Incorrect = 0,
        Misplaced = 1,
        Correct = 2,
    }

    public class ListEqualityComparer<Pattern> : IEqualityComparer<List<Pattern>>
    {
        public bool Equals(List<Pattern> lhs, List<Pattern> rhs)
        {
            return lhs.SequenceEqual(rhs);
        }

        public int GetHashCode(List<Pattern> list)
        {
            unchecked
            {
                return list.Aggregate(23, (current, item) => current * 31 + (item == null ? 0 : item.GetHashCode()));
            }
        }
    }
}
