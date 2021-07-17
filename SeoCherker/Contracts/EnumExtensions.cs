using System;

namespace OlegChibikov.SympliInterview.SeoChecker.Contracts
{
    public static class EnumExtensions
    {
        public static string? GetName<T>(this T value)
            where T : Enum =>
            Enum.GetName(typeof(T), value);
    }
}
