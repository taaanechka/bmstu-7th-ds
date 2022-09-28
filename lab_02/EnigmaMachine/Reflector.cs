using System;
using System.Collections.Generic;

namespace EnigmaMachine
{
    public class Reflector: Device
    {
        public Reflector()
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            List<int> idxList = new List<int>();
            int idx, k = 0;

            for (int i = 0; i < bytesNum; i++)
            {
                connArr[i] = -1;
                idxList.Add(i);
            }

            while (idxList.Count > 0)
            {
                idx = rnd.Next(0, idxList.Count);

                for (; connArr[k] != -1; k++) ;

                int val = idxList[idx];
                connArr[k] = val;
                connArr[val] = k;

                idxList.RemoveAt(idx);

                int idx_k = idxList.IndexOf(k);
                if (idx_k >= 0)
                {
                    idxList.RemoveAt(idx_k);
                }

                k++;
            }
        }
    }
}