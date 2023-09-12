using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebItNow
{
    public class clsStrings
    {
        public clsStrings()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static string SafeSqlLikeClauseLiteral(string prmSQLString)
        {
            string s = prmSQLString;
            s = s.Replace("'", "''");
            s = s.Replace("[", "[[]");
            s = s.Replace("%", "[%]");
            s = s.Replace("_", "[_]");
            return (s);
        }
        public static string fnYESNO(bool prmValue)
        {
            if (prmValue)
            {
                return ("SI");
            }
            else
            {
                return ("NO");
            }
        }
        public static string fnErrorMessage(string prmMessage)
        {
            return ("<span style=\"color:Red;\">" +
                    "<img src = \"images/icons16/error.png\" height=\"16\" width=\"16\" alt=\"Error\" />&nbsp;" +
                    prmMessage + "</span>");
        }
        public static string fnInfoMessage(string prmMessage)
        {
            return ("<span style=\"color:Blue;\">" +
                    "<img src = \"images/icons16/information.png\" height=\"16\" width=\"16\" alt=\"Información\" />&nbsp;" +
                    prmMessage + "</span>");
        }
        public static string fnNormalMessage(string prmMessage)
        {
            return ("<span style=\"vertical-align:center\">" +
                    "<img src = \"images/icons16/information.png\" height=\"16\" width=\"16\" alt=\"Información\" />&nbsp;" +
                    prmMessage + "</span>");
        }
    }
}