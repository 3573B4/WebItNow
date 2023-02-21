using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebItNow
{
    public class modGeneral
    {
        public bool validarCaracter(string nomFile)
        {
            char[] a = { '{', '}', ',', '/', '[', ']', '`', '^', '~', '+', '*', '´', '¨', '¿', '¡' };
            string sA = new string(a);

            for (int i = 0; i < nomFile.Length; i++)
            {
                for (int j = 0; j < a.Length; j++)
                {
                    if (nomFile[i] == a[j])
                    {

                        return false;
                        //break;
                    }
                }
            }
            return true;
        }
    }
}