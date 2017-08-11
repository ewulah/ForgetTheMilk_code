using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForgetTheMilk
{
    public class StringUtils
    {
        public string MakeUpper(string s)
        {
            return s.ToUpper();
        }

        public string AppendNumberToString(string s, int num)
        {
            return s + num;
        }
    }
}