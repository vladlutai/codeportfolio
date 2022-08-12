using System;
using Tools;

public enum NumberAbbreviationEnum
{
    Default = 0,
    K = 3,
    M = 6,
    B = 9,
    t = 12,
    q = 15,
    Q = 18,
    s = 21,
    S = 24,
    o = 27,
    n = 30,
    d = 33,
    U = 36,
    D = 39,
    T = 42,
    Qt = 45,
    Qd = 48,
    Sd = 51,
    St = 54,
    O = 57,
    N = 60,
    v = 63,
    c = 66
}

public enum OperationEnum
{
    Sum,
    Subtract,
    Lerp
}

public static class AbbreviationUtility
{
    #region Constants
    private const int PowStep = 3;
    private const int MinValue = 1;
    private const int NumberBase = 10;
    private static readonly int MaxPowStepValue = (int)Math.Pow(NumberBase, PowStep);
    #endregion

    public static Score Sum(Score score1, Score score2)
    {
        return Calculate(ref score1, ref score2, OperationEnum.Sum);
    }

    public static Score Subtract(Score score1, Score score2)
    {
        return Calculate(ref score1, ref score2, OperationEnum.Subtract);
    }

    public static Score Lerp(Score score1, Score score2, float interpolation)
    {
        return Calculate(ref score1, ref score2, OperationEnum.Lerp, interpolation);
    }

    public static float ScorePercentage(Score score1, Score score2)
    {
        return CalculatePercent(ref score1, ref score2);
    }

    private static Score Calculate(ref Score score1, ref Score score2, OperationEnum operationEnum, float interpolation = 0)
    {
        Check(ref score1);
        Check(ref score2);
        Score maxScore;
        if (score1.NumberAbbreviationEnum == score2.NumberAbbreviationEnum)
        {
            maxScore = score1.Value > score2.Value ? score1 : score2;
        }
        else
        {
            maxScore = score1.NumberAbbreviationEnum > score2.NumberAbbreviationEnum ? score1 : score2;
        }

        Score minScore = maxScore.Equals(score1) ? score2 : score1;

        NumberAbbreviationEnum numberAbbreviationEnum = maxScore.NumberAbbreviationEnum;

        int powDifference = maxScore.NumberAbbreviationEnum - minScore.NumberAbbreviationEnum;
        decimal value = 0;
        switch (operationEnum)
        {
            case OperationEnum.Sum:
                value = maxScore.Value + minScore.Value / (decimal)Math.Pow(NumberBase, powDifference);
                break;

            case OperationEnum.Subtract:
                value = maxScore.Value - minScore.Value / (decimal)Math.Pow(NumberBase, powDifference);
                break;
            case OperationEnum.Lerp:
                value = (decimal)interpolation * maxScore.Value + ((decimal)(1 - interpolation) * (minScore.Value / (decimal)Math.Pow(NumberBase, powDifference)));
                break;
        }
        Score totalScore = new Score(value, numberAbbreviationEnum);
        Check(ref totalScore);
        totalScore.Value = totalScore.Value.Normalize();
        return totalScore;
    }

    private static float CalculatePercent(ref Score score1, ref Score score2)
    {
        Check(ref score1);
        Check(ref score2);
        Score maxScore;
        if (score1.NumberAbbreviationEnum == score2.NumberAbbreviationEnum)
        {
            maxScore = score1.Value > score2.Value ? score1 : score2;
        }
        else
        {
            maxScore = score1.NumberAbbreviationEnum > score2.NumberAbbreviationEnum ? score1 : score2;         //---- make in one function with cortege 
        }
        Score minScore = maxScore.Equals(score1) ? score2 : score1;
        NumberAbbreviationEnum numberAbbreviationEnum = maxScore.NumberAbbreviationEnum;
        int powDifference = maxScore.NumberAbbreviationEnum - minScore.NumberAbbreviationEnum;
        float value;
        if (score1.NumberAbbreviationEnum >= score2.NumberAbbreviationEnum)
        {
            value = (float)(score2.Value / (decimal)Math.Pow(NumberBase, powDifference) / score1.Value);
        }
        else
            value = (float)(score2.Value / (score1.Value / (decimal)Math.Pow(NumberBase, powDifference)));
        return value;
    }

    private static void Check(ref Score score)
    {
        if (score.Value == 0)
            return;
        if (score.Value >= MaxPowStepValue)
        {
            while (score.Value >= MaxPowStepValue)
            {
                score.Value /= MaxPowStepValue;
                score.NumberAbbreviationEnum += PowStep;
            }
        }
        else if (score.Value < MinValue)
        {
            while (score.Value < MinValue)
            {
                score.Value *= MaxPowStepValue;
                score.NumberAbbreviationEnum -= PowStep;
            }
        }
    }

    public static (decimal, NumberAbbreviationEnum) Check(decimal value, NumberAbbreviationEnum numberAbbreviationEnum)
    {
        if (value == 0)
            return (0, NumberAbbreviationEnum.Default);
        if (value >= MaxPowStepValue)
        {
            while (value >= MaxPowStepValue)
            {
                value /= MaxPowStepValue;
                numberAbbreviationEnum += PowStep;
            }
        }
        else if (value < MinValue)
        {
            while (value < MinValue)
            {
                value *= MaxPowStepValue;
                numberAbbreviationEnum -= PowStep;
            }
        }
        return (value, numberAbbreviationEnum);
    }
}