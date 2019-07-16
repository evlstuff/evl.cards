public static class UnityTips
{
    public static bool IsNullOrEmpty<T>(this T[] t)
    {
        return t == null || t.Length < 1;
    }

    public static bool IsNullOrEmpty(this string str) {
        return string.IsNullOrEmpty(str);
    }

    public static bool IsNullOrEmpty<T>(this T t)
    {
        return t == null;
    }
}
