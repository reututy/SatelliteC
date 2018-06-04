using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    public class TRXConfiguration
    {
        public static char[] FromDefClSign = "default".ToCharArray(0,7);
        public static char[] ToDefClSign = "default".ToCharArray(0, 7);
        public static byte[] DefBeacon = Encoding.UTF8.GetBytes("default beacon");
    }
}
