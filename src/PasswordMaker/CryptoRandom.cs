using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PasswordMaker;

public static class CryptoRandom
{
    private static readonly Lazy<RandomNumberGenerator> rng = new(() => RandomNumberGenerator.Create());

    public static byte[] NextBytes(int length)
    {
        byte[] bytes = new byte[length];
        rng.Value.GetBytes(bytes);
        return bytes;
    }

    public static int NextInt()
    {
        return BitConverter.ToInt32(NextBytes(4), 0);
    }

    public static int NextInt(int minValue, int maxValue)
    {
        if (minValue > maxValue)
            return maxValue + (int)Math.Floor(NextFraction() * (double)(minValue - maxValue + 1));
        else
            return minValue + (int)Math.Floor(NextFraction() * (double)(maxValue - minValue + 1));
    }

    public static UInt32 NextUInt()
    {
        return BitConverter.ToUInt32(NextBytes(4), 0);
    }

    public static UInt64 NextUInt64()
    {
        return BitConverter.ToUInt64(NextBytes(8), 0);
    }

    public static double NextDouble()
    {
        return BitConverter.ToDouble(NextBytes(8), 0);
    }

    public static double NextFraction()
    {
        UInt64 i = NextUInt64();
        // Largest fraction (positive number less than 1.0) that can be represented by a double is 0.99999999999999989
        // The UInt64 value corresponding to this fraction is UInt64.MaxValue - 1024 = 18446744073709550591
        // So, if random UInt64 is greater than this, just return the largest double fraction (0.99999999999999989)
        if (i > 18446744073709550591) return 0.99999999999999989;
        return (double)i / (double)UInt64.MaxValue;
    }

    public static string NextPrintableString(int length)
    {
        byte[] bytes = new byte[length];
        for (int i = 0; i < length; i++)
        {
            bytes[i] = (byte)NextInt(33, 126);  // Printable ASCII taken as characters with decimal values 33 through 126
        }
        return Encoding.UTF8.GetString(bytes);
    }

    public static string NextAsciiString(int length)
    {
        return Encoding.UTF8.GetString(NextBytes(length));
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        // Shuffle items in a list using the Fisher–Yates algorithm
        // Equivalent to randomly choosing items from a hat, one at a time
        int ub = list.Count - 1;
        for (int i = 0; i < ub; i++)
        {
            list.Swap(i, NextInt(i, ub));
        }
    }

    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        T item = list[i];
        list[i] = list[j];
        list[j] = item;
    }

    private const string defaultPasswordCharacterSet = "23456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
    public static string GenerateTemporaryPassword(int length)
    {
        // Alphanumeric but without look-alike characters O, 0, l, 1, I
        // Use case is for temporary passwords that a user might have to type
        return NextStringFromCharacters(defaultPasswordCharacterSet, length);
    }

    private const string alphanumericCharacterSet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public static string NextAlphaNumericString(int length)
    {
        return NextStringFromCharacters(alphanumericCharacterSet, length);
    }

    public static string NextStringFromCharacters(string characterSet, int length)
    {
        ReadOnlySpan<char> characterSpan = characterSet.AsSpan();
        char[] passwordChars = new char[length];
        for (int i = 0; i < length; i++)
        {
            int index = NextInt(0, characterSpan.Length - 1);
            passwordChars[i] = characterSpan[index];
        }
        return new string(passwordChars);
    }
}