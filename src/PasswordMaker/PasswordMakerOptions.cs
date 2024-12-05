using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace PasswordMaker;

public class PasswordMakerOptions : INotifyPropertyChanged
{
    private string name;
    public string Name
    {
        get { return name; }
        set
        {
            if (name != value)
            {
                name = value;
                NotifyPropertyChanged();
            }
        }
    }

    public ObservableCollection<CharacterGroupTypes> IncludeCharacterGroupTypes { get; set; }

    private string includeSpecificCharacters;
    public string IncludeSpecificCharacters
    {
        get { return includeSpecificCharacters; }
        set
        {
            if (includeSpecificCharacters != value)
            {
                includeSpecificCharacters = value;
                NotifyPropertyChanged();
            }
        }
    }

    private string excludeSpecificCharacters;
    public string ExcludeSpecificCharacters
    {
        get { return excludeSpecificCharacters; }
        set
        {
            if (excludeSpecificCharacters != value)
            {
                excludeSpecificCharacters = value;
                NotifyPropertyChanged();
            }
        }
    }

    private bool excludeLookAlikeCharacters;
    public bool ExcludeLookAlikeCharacters
    {
        get { return excludeLookAlikeCharacters; }
        set
        {
            if (excludeLookAlikeCharacters != value)
            {
                excludeLookAlikeCharacters = value;
                NotifyPropertyChanged();
            }
        }
    }

    private bool allowMultiplesOfSameCharacter;
    public bool AllowMultiplesOfSameCharacter
    {
        get { return allowMultiplesOfSameCharacter; }
        set
        {
            if (allowMultiplesOfSameCharacter != value)
            {
                allowMultiplesOfSameCharacter = value;
                NotifyPropertyChanged();
            }
        }
    }

    private int length;
    public int Length
    {
        get { return length; }
        set
        {
            if (value < 1) value = 1;
            if (length != value)
            {
                length = value;
                NotifyPropertyChanged();
            }
        }
    }

    public const string LookAlikeCharacters = "O0l1I|";

    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public PasswordMakerOptions()
    {
        IncludeCharacterGroupTypes = [];
        IncludeSpecificCharacters = string.Empty;
        ExcludeSpecificCharacters = string.Empty;
        ExcludeLookAlikeCharacters = false;
        AllowMultiplesOfSameCharacter = true;
        Length = 30;
        Name = "new options";
    }

    public static PasswordMakerOptions NewOptionsDefault()
    {
        PasswordMakerOptions pmo = new()
        {
            Name = "Default"
        };
        pmo.AddCharacterGroupType(CharacterGroupTypes.Upper);
        pmo.AddCharacterGroupType(CharacterGroupTypes.Lower);
        pmo.AddCharacterGroupType(CharacterGroupTypes.Number);
        pmo.AddCharacterGroupType(CharacterGroupTypes.Math);
        pmo.AddCharacterGroupType(CharacterGroupTypes.Special);
        return pmo;
    }

    public static PasswordMakerOptions NewOptionsAlphanumeric()
    {
        PasswordMakerOptions pmo = new()
        {
            Name = "Alphanumeric"
        };
        pmo.AddCharacterGroupType(CharacterGroupTypes.Upper);
        pmo.AddCharacterGroupType(CharacterGroupTypes.Lower);
        pmo.AddCharacterGroupType(CharacterGroupTypes.Number);
        return pmo;
    }

    public static PasswordMakerOptions NewOptionsNumbers()
    {
        PasswordMakerOptions pmo = new()
        {
            Name = "Numbers"
        };
        pmo.AddCharacterGroupType(CharacterGroupTypes.Number);
        return pmo;
    }

    public PasswordMakerOptions(ObservableCollection<CharacterGroupTypes> includeCharacterGroupTypes,
        string includeSpecificCharacters, string excludeSpecificCharacters,
        bool excludeLookAlikeCharacters, bool allowMultiplesOfSameCharacter, int length)
    {
        // Use explicit options passed in
        IncludeCharacterGroupTypes = includeCharacterGroupTypes;
        IncludeSpecificCharacters = includeSpecificCharacters;
        ExcludeSpecificCharacters = excludeSpecificCharacters;
        ExcludeLookAlikeCharacters = excludeLookAlikeCharacters;
        AllowMultiplesOfSameCharacter = allowMultiplesOfSameCharacter;
        Length = length;
    }

    public PasswordMakerOptions(string characterSet, bool allowMultiplesOfSameCharacter, int length)
    {
        // Use only specific characters passed in
        IncludeCharacterGroupTypes = [];
        IncludeSpecificCharacters = characterSet;
        ExcludeSpecificCharacters = string.Empty;
        ExcludeLookAlikeCharacters = false;
        AllowMultiplesOfSameCharacter = allowMultiplesOfSameCharacter;
        Length = length;
    }

    public List<char> GetCharacterSet(bool shuffle = true)
    {
        HashSet<char> charSet = [];
        // Groups
        foreach (CharacterGroupTypes cgt in IncludeCharacterGroupTypes)
        {
            PasswordMakerCharacterGroup g = PasswordMakerCharacterGroups.CharacterGroupDefinitions.FindGroupByType(cgt);
            if (g is not null) AppendCharacters(charSet, g.Characters);
        }

        // Explicitly specified characters
        AppendCharacters(charSet, IncludeSpecificCharacters);

        // Remove excluded
        RemoveCharacters(charSet, ExcludeSpecificCharacters);

        // Remove look-alike
        if (ExcludeLookAlikeCharacters)
        {
            RemoveCharacters(charSet, LookAlikeCharacters);
        }

        List<char> charList = charSet.ToList();
        if (shuffle) charList.Shuffle();

        return charList;
    }

    public int GetCharacterSetLength()
    {
        return GetCharacterSet(false).Count;
    }

    private void AppendCharacters(HashSet<char> chars, string s)
    {
        if (string.IsNullOrEmpty(s)) return;
        foreach (char c in s)
            chars.Add(c);
    }

    private void RemoveCharacters(HashSet<char> chars, string s)
    {
        if (string.IsNullOrEmpty(s)) return;
        foreach (char c in s)
            chars.Remove(c);
    }

    [JsonIgnore]
    public bool IncludeUpper
    {
        get { return IncludeCharacterGroupTypes.Contains(CharacterGroupTypes.Upper); }
        set { SetCharacterGroupType(CharacterGroupTypes.Upper, value); }
    }

    [JsonIgnore]
    public bool IncludeLower
    {
        get { return IncludeCharacterGroupTypes.Contains(CharacterGroupTypes.Lower); }
        set { SetCharacterGroupType(CharacterGroupTypes.Lower, value); }
    }

    [JsonIgnore]
    public bool IncludeNumber
    {
        get { return IncludeCharacterGroupTypes.Contains(CharacterGroupTypes.Number); }
        set { SetCharacterGroupType(CharacterGroupTypes.Number, value); }
    }

    [JsonIgnore]
    public bool IncludeMath
    {
        get { return IncludeCharacterGroupTypes.Contains(CharacterGroupTypes.Math); }
        set { SetCharacterGroupType(CharacterGroupTypes.Math, value); }
    }

    [JsonIgnore]
    public bool IncludePunctuation
    {
        get { return IncludeCharacterGroupTypes.Contains(CharacterGroupTypes.Punctuation); }
        set { SetCharacterGroupType(CharacterGroupTypes.Punctuation, value); }
    }

    [JsonIgnore]
    public bool IncludeBrackets
    {
        get { return IncludeCharacterGroupTypes.Contains(CharacterGroupTypes.Brackets); }
        set { SetCharacterGroupType(CharacterGroupTypes.Brackets, value); }
    }

    [JsonIgnore]
    public bool IncludeQuotes
    {
        get { return IncludeCharacterGroupTypes.Contains(CharacterGroupTypes.Quotes); }
        set { SetCharacterGroupType(CharacterGroupTypes.Quotes, value); }
    }

    [JsonIgnore]
    public bool IncludeSpecial
    {
        get { return IncludeCharacterGroupTypes.Contains(CharacterGroupTypes.Special); }
        set { SetCharacterGroupType(CharacterGroupTypes.Special, value); }
    }

    private void SetCharacterGroupType(CharacterGroupTypes characterGroupType, bool included)
    {
        if (included) AddCharacterGroupType(characterGroupType);
        else RemoveCharacterGroupType(characterGroupType);
    }

    private void AddCharacterGroupType(CharacterGroupTypes characterGroupType)
    {
        if (IncludeCharacterGroupTypes.Contains(characterGroupType)) return;
        IncludeCharacterGroupTypes.Add(characterGroupType);
        NotifyPropertyChanged(characterGroupType.ToString());
    }

    private void RemoveCharacterGroupType(CharacterGroupTypes characterGroupType)
    {
        if (!IncludeCharacterGroupTypes.Contains(characterGroupType)) return;
        IncludeCharacterGroupTypes.Remove(characterGroupType);
        NotifyPropertyChanged(characterGroupType.ToString());
    }
}