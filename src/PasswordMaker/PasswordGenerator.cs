using System.Collections.Generic;
using System.Linq;

namespace PasswordMaker;

public partial class PasswordGenerator
{
    private PasswordMakerOptions passwordMakerOptions;

    public PasswordGenerator(PasswordMakerOptions options)
    {
        passwordMakerOptions = options;
    }

    public PasswordGenerator()
    {
        passwordMakerOptions = new PasswordMakerOptions();
    }

    public string GeneratePassword()
    {
        return GeneratePassword(passwordMakerOptions);
    }

    public static string GeneratePassword(PasswordMakerOptions options)
    {
        List<char> charSet = options.GetCharacterSet();

        if (options.AllowMultiplesOfSameCharacter)
        {
            char[] passwordChars = new char[options.Length];
            for (int i = 0; i < options.Length; i++)
            {
                int index = CryptoRandom.NextInt(0, charSet.Count - 1);
                passwordChars[i] = charSet[index];
            }
            return new string(passwordChars);
        }
        else
        {
            charSet.Shuffle();
            if (charSet.Count <= options.Length)
                return new string(charSet.ToArray());
            return new string(charSet.Take(options.Length).ToArray());
        }
    }
}