namespace Assets.Sources.Utils
{
    public class DigitUtils
    {
        public static string CutDigit(uint digit)
        {
            if (digit >= 1000 && digit < 1000000)
                return ((float)digit / 1000).ToString("0.00") + "K";
            else if (digit >= 1000000)
                return ((float)digit / 1000000).ToString("0.00") + "M";
            else
                return digit.ToString();
        }
    }
}
