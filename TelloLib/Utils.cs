using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelloLib
{
    internal class Utils
    {
        public static void AssertValue(int value, int min, int max, string errorMsg)
        {
            if (value < min || value > max)
            {
                throw new Exception(errorMsg);
            }
        }
    }
}
