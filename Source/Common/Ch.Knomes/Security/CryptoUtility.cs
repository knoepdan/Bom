using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Ch.Knomes.Security
{
    public static class CryptoUtility
    {

        private static readonly Random RandomInstance = new Random();

        public static byte[] GetRandomBytes(int nofBytes)
        {
            byte[] random = new byte[nofBytes];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
            }
            return random;
        }

        public static string GetPseudoRandomString(int length)
        {
            lock (RandomInstance)
            {
                var result = GetPseudoRandomString(RandomInstance, length);
                return result;
            }

        }

        public static string GetPseudoRandomString(Random rand, int length)
        {
            if (rand == null)
            {
                throw new ArgumentNullException(nameof(rand));
            }
            string result = "";
            for (int i = 1; i <= length; i++)
            {
                Boolean isAcceptedChar = false;
                int rndNum = 0;
                while (!isAcceptedChar)
                {
                    rndNum = rand.Next(48, 123);
                    // ASCII codes 48 = 0 and 57 = 9 therefore 0 to 9 are ok
                    // 65 = A & 90 = Z therefore A to Z upper case are ok
                    // 97 = a & 122 = z therefore a to z lower case are ok
                    isAcceptedChar = !(rndNum < 48 | rndNum > 57 & rndNum < 65 | rndNum > 90 & rndNum < 97 | rndNum > 122);
                }
                result += Convert.ToChar(rndNum);
            }
            return result;
        }


        /*

        public static string GetRandomString(int length)
        {
          -> this works but is extremly slow
            string result = "";
            using (var rng = RandomNumberGenerator.Create()) // not using System.Random as it is not completly random. Using random it would be:  //  rndNum = Random.Next(48, 123); 
            {
                for (int i = 1; i <= length; i++)
                {
                    Boolean isAcceptedChar = false;
                    int rndNum = 0;
                    while (!isAcceptedChar)
                    {
                        byte[] randomInt = new byte[4];
                        rng.GetBytes(randomInt);
                        rndNum = BitConverter.ToInt32(randomInt, 0);
                        // ASCII codes 48 = 0 and 57 = 9 therefore 0 to 9 are ok
                        // 65 = A & 90 = Z therefore A to Z upper case are ok
                        // 97 = a & 122 = z therefore a to z lower case are ok
                        isAcceptedChar = !(rndNum < 48 | rndNum > 57 & rndNum < 65 | rndNum > 90 & rndNum < 97 | rndNum > 122);
                    }
                    result += Convert.ToChar(rndNum);
                }
            }
            return result;
        }

        */

       
    }
}
