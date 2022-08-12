public static class DecimalExtensions
{
    private const decimal DecimalNormalizeCoef = 1.000000000000000000000000000000000m;
    public static decimal Normalize(this decimal value)
    {
        return value / DecimalNormalizeCoef;
    }
}