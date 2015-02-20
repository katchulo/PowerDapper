using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acto.API.Tests.Util
{
    public static class General
    {

        public static string RemovePoints(string item)
        {
            char nullChar = '\0';
            return item
                .Replace('.', nullChar)
                .Replace('-', nullChar)
                .Replace(',', nullChar)
                .Replace('/', nullChar)
                .Replace(':', nullChar);
        }

    }
}
