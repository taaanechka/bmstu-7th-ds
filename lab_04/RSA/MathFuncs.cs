using System;

namespace RSA
{
    public class MathFuncs
    {
        /// НОД двух чисел (Евклид)
        public static int GreatestCommonDivisor(int a, int b)
        {
            int temp;
            while (b != 0)
            {
                temp = a;
                a = b;
                b = temp % b;
            }
            return a;
        }

        // Решето Эратосфена
        // на выходе простые числа до n включительно
        static List<int> EratosthenesSieve(int n)
        {
            var flags = new List<bool>();
            for (int i = 0; i < n + 1; i++)
            {
                flags.Add(false);
            }

            int len = (int)Math.Sqrt(n);

            for (int i = 2; i <= len; i++)
            {
                if (!flags[i])
                    for (int j = 2; i*j <= n; j++)
                    {
                        flags[i*j] = true;
                    }
            }

            var nums = new List<int>();

            for (int i = 2; i < n + 1; i++)
            {
                if (!flags[i])
                    nums.Add(i);
            }

            return nums;
        }

        // Получение простых числе в диапазоне(start, stop)
        public static List<int> GetPrimeNumbers(int start, int stop)
        {
            var nums = EratosthenesSieve(stop);
            int len = nums.Count();

            var res = new List<int>();

            int i  = 0;
            while (nums[i++] < start) ;

            for (int j = i; j < len; j++)
            {
                res.Add(nums[j]);
            }

            return nums;
        }

        /// Расширенный алгоритм Евклида (соотношение Безу)
        /// Находит такое t, что
        /// (a * t) mod n == 1
        public static int Inverse(int a, int n)
        {
            int t = 0, r = n; 
            int newt = 1, newr = a;

            while (newr != 0)
            {
                var quotient = r / newr; // коефф.
                var tmpt = newt;
                newt = t - quotient * newt;
                t = tmpt;

                var tmpr = newr;
                newr = r - quotient * newr;
                r = tmpr;
            }

            if (r > 1)
                return -1;
            if (t < 0)
                t = t + n;

            return t;
        }
    }
}