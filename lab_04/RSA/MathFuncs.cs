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

        // функция проверяет - простое ли число n
        public static bool IsNumberPrimePrecise(int num)
        {
            if (num > 1)
            {
                for (int i = 2; i < num; i++)
                    if (num % i == 0)
                        return false;
                return true;
            }

            return false;
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