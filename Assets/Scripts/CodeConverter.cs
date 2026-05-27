using System;
using System.Text;

public static class CodeConverter {

    private const string Charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private static readonly int BaseValue = Charset.Length;

    public static string toString(ulong lobbyId) {
        if (lobbyId == 0)
            return Charset[0].ToString();

        StringBuilder result = new StringBuilder();

        while (lobbyId > 0) {
            int remainder = (int)(lobbyId % (ulong)BaseValue);
            result.Insert(0, Charset[remainder]);
            lobbyId /= (ulong)BaseValue;
        }

        return result.ToString();
    }


    public static ulong toUlong(string code) {
        ulong result = 0;

        foreach (char c in code) {
            int value = Charset.IndexOf(c);

            if (value < 0)
                throw new ArgumentException($"Invalid character: {c}");

            result = result * (ulong)BaseValue + (ulong)value;
        }

        return result;
    }
}

