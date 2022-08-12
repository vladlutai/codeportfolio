using UnityEngine;

namespace Tools
{
    public static class InfoAbbreviationUtility
    {
        public enum InfoAbbreviationEnum
        {
            Kb = 1,
            Mb,
            Gb,
            Tb,
            Pb
        }

        public static (float value, InfoAbbreviationEnum abbreviation) ConvertByteTo(float initValue,
            InfoAbbreviationEnum outAbbreviationEnum)
        {
            initValue /= Mathf.Pow(10, 3 * (int) outAbbreviationEnum);
            return (initValue, outAbbreviationEnum);
        }
    }
}