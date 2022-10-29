using System.Numerics;

namespace RSA
{
    public class AlgRSA
    {
        private static Random r = new Random(1);

        private const int start = 30000;
        private const int stop = 40000;

        public int publicKey;
        public int privateKey;
        public int n;

        public AlgRSA()
        {
            int p, q;

            (p, q) = GetRandomPrimeNums();
            
            n = p * q; // модуль

            // Вычисляем функцию Эйлера от числа n.
            // Значение этой функции Fi(n) равно количеству натуральных чисел, не превышающих n и взаимно простых с ним.
            int fi = (p - 1) * (q - 1);

            // Выбираем открытую экспененту (e).
            publicKey = ComputePublicKey(fi);
            
            // Вычисляем секретную экспоненту (d).
            privateKey = ComputePrivateKey(publicKey, fi);
        }

        /// <summary>
        /// Шифрование
        /// </summary>
        /// <param name="m">Message</param>
        /// <returns>c</returns>
        public int Encrypt(int m)
        {
            // ( m ^ publicKey ) mod n
            return (int)BigInteger.ModPow(m, publicKey, n);
        }

        /// <summary>
        /// Расшифровка
        /// </summary>
        /// <param name="c"></param>
        /// <returns>m</returns>
        public int Decrypt(int c)
        {
            // ( c ^ privateKey ) mod n
            return (int)BigInteger.ModPow(c, privateKey, n);
        }

        /// <summary>
        /// Получение p и q
        /// </summary>
        /// <returns>(p, q)</returns>
        static (int, int) GetRandomPrimeNums()
        {
            var primeNums = MathFuncs.GetPrimeNumbers(start, stop);
            int primeLen = primeNums.Count();

            int p_ind = r.Next(primeLen);
            int q_ind = r.Next(primeLen);

            return (primeNums[p_ind], primeNums[q_ind]);
        }

        /// <summary>
        /// Выбирает целое число e, такое что: 1 < e < fi(n) и 
        /// НОД(fi, e) == 1 
        /// </summary>
        /// <param name="fi"></param>
        /// <returns>Взаимно простое с fi число</returns>
        static int ComputePublicKey(int fi)
        {
            int nod = 0;
            int num = -1;
            while (nod != 1)
            {
                num = r.Next(2, fi);
                nod = MathFuncs.GreatestCommonDivisor(fi, num); // нод
            }
            return num;
        }

        /// <summary>
        /// (e * d) mod(fi) = 1
        /// </summary>
        /// <returns>d</returns>
        static int ComputePrivateKey(int e, int fi)
        {
            return MathFuncs.Inverse(e, fi);
        }
    }
}