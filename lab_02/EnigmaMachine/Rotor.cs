namespace EnigmaMachine
{
    public class Rotor: Device
    {
        public int rotate()
        {
            int tmp = connArr[0];

            for (int i = 0; i < bytesNum; i++)
            {
                connArr[i] = connArr[(i + 1) % bytesNum];
            }
            
            connArr[bytesNum - 1] = tmp;

            rotNum++;
            rotNum %= bytesNum;

            return rotNum;
        }
    }
}