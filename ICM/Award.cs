using System;
using System.Collections.Generic;
using System.Text;

namespace ICM
{
    public class Award : IComparable
    {
        public Award()
        {
            wins = new List<Double>(6);
        }
        public string name;
        public List<double> wins;
        public int playercount;

        public override string ToString()
        {
            return name;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return name.CompareTo(obj);
        }

        #endregion
    }
}
