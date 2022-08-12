using System;

namespace Tools
{
    [Serializable]
    public struct Score
    {
        public decimal Value;
        public NumberAbbreviationEnum NumberAbbreviationEnum;

        public Score(decimal value, NumberAbbreviationEnum numberAbbreviationEnum)
        {
            (decimal, NumberAbbreviationEnum) checkValue = AbbreviationUtility.Check(value, numberAbbreviationEnum);
            Value = checkValue.Item1;
            NumberAbbreviationEnum = checkValue.Item2;
        }

        public static Score Lerp(Score score1, Score score2, float interpolation)
        {
            return AbbreviationUtility.Lerp(score1, score2, interpolation);
        }

        public static Score operator +(Score score1, Score score2)
        {
            return AbbreviationUtility.Sum(score1, score2);
        }

        public static Score operator -(Score score1, Score score2)
        {
            return AbbreviationUtility.Subtract(score1, score2);
        }

        public static Score operator *(Score score, int multiplier)
        {
            return new Score(score.Value * multiplier, score.NumberAbbreviationEnum);
        }
        public static Score operator *(Score score, float multiplier)
        {
            return new Score(score.Value * (decimal)multiplier, score.NumberAbbreviationEnum);
        }

        public static float operator /(Score score1, Score score2)
        {
            return AbbreviationUtility.ScorePercentage(score1, score2);
        }

        public static Score operator /(Score score1, int divider)
        {
            return new Score(score1.Value / divider, score1.NumberAbbreviationEnum);
        }

        public static Score operator *(int multiplier, Score score)
        {
            return new Score(score.Value * multiplier, score.NumberAbbreviationEnum);
        }

        public static bool operator <=(Score score, Score score1)
        {
            if (score.NumberAbbreviationEnum == score1.NumberAbbreviationEnum)
            {
                return score.Value <= score1.Value;
            }
            return score.NumberAbbreviationEnum <= score1.NumberAbbreviationEnum;
        }

        public static bool operator >=(Score score, Score score1)
        {
            if (score.NumberAbbreviationEnum == score1.NumberAbbreviationEnum)
            {
                return score.Value >= score1.Value;
            }
            return score.NumberAbbreviationEnum >= score1.NumberAbbreviationEnum;
        }

        public static bool operator <(Score score, Score score1)
        {
            if (score.NumberAbbreviationEnum == score1.NumberAbbreviationEnum)
            {
                return score.Value < score1.Value;
            }
            return score.NumberAbbreviationEnum < score1.NumberAbbreviationEnum;
        }

        public static bool operator >(Score score, Score score1)
        {
            if (score.NumberAbbreviationEnum == score1.NumberAbbreviationEnum)
            {
                return score.Value > score1.Value;
            }
            return score.NumberAbbreviationEnum > score1.NumberAbbreviationEnum;
        }
        public static bool operator ==(Score score, int score1)
        {
            if (score.Value == score1)
                return true;
            else
                return false;
        }
        public static bool operator !=(Score score, int score1)
        {
            if (score.Value != score1)
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            string numberValue = (NumberAbbreviationEnum >= 0) ? (NumberAbbreviationEnum == NumberAbbreviationEnum.Default ? Math.Round(Value, 0) : Math.Round(Value, 2)).ToString() : "1";
            string value = numberValue + (NumberAbbreviationEnum != NumberAbbreviationEnum.Default && NumberAbbreviationEnum > 0
                ? NumberAbbreviationEnum.ToString()
                : "");
            return value;
        }

        public static Score Parse(string parseValue)
        {
            if (decimal.TryParse(parseValue, out decimal scoreValue))
            {
                return new Score(scoreValue, NumberAbbreviationEnum.Default);
            }
            string abbreviation = String.Empty;
            foreach (char symbol in parseValue)
            {
                if (char.IsLetter(symbol))
                {
                    abbreviation += symbol;
                }
            }
            string digitValue = parseValue.Replace(abbreviation, "");
            return new Score(decimal.Parse(digitValue),
                (NumberAbbreviationEnum)Enum.Parse(typeof(NumberAbbreviationEnum), abbreviation));
        }
    }
}