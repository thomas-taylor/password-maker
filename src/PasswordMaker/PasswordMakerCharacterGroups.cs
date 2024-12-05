using System.Collections.Generic;
using System.Linq;

namespace PasswordMaker;

public enum CharacterGroupTypes
{
    Upper = 1,
    Lower = 2,
    Number = 3,
    Math = 4,
    Punctuation = 5,
    Brackets = 6,
    Quotes = 7,
    Special = 8
}

public class PasswordMakerCharacterGroup
{
    public CharacterGroupTypes CharacterGroupType;
    public string GroupName;
    public string Characters;
    public string Display;

    public PasswordMakerCharacterGroup(string groupName, CharacterGroupTypes characterGroupType, string characters, string display)
    {
        GroupName = groupName;
        CharacterGroupType = characterGroupType;
        Characters = characters;
        Display = display;
    }
}

public class PasswordMakerCharacterGroups : List<PasswordMakerCharacterGroup>
{
    private static PasswordMakerCharacterGroups characterGroupDefinitions;
    public static PasswordMakerCharacterGroups CharacterGroupDefinitions
    {
        get
        {
            if (characterGroupDefinitions is null) InitCharacterGroupDefinitions();
            return characterGroupDefinitions;
        }
    }

    private static void InitCharacterGroupDefinitions()
    {
        characterGroupDefinitions = [];
        characterGroupDefinitions.Add(new PasswordMakerCharacterGroup("Upper case", CharacterGroupTypes.Upper, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABC..."));
        characterGroupDefinitions.Add(new PasswordMakerCharacterGroup("Lower case", CharacterGroupTypes.Lower, "abcdefghijklmnopqrstuvwxyz", "abc..."));
        characterGroupDefinitions.Add(new PasswordMakerCharacterGroup("Numbers", CharacterGroupTypes.Number, "0123456789", "0123..."));
        characterGroupDefinitions.Add(new PasswordMakerCharacterGroup("Math", CharacterGroupTypes.Math, "+-*/<=>", "+ - * / < = >"));
        characterGroupDefinitions.Add(new PasswordMakerCharacterGroup("Punctuation", CharacterGroupTypes.Punctuation, "!,.:;?", "! , .  : ; ?"));
        characterGroupDefinitions.Add(new PasswordMakerCharacterGroup("Brackets", CharacterGroupTypes.Brackets, "()[]{}", "( ) [ ] { }"));
        characterGroupDefinitions.Add(new PasswordMakerCharacterGroup("Quotes", CharacterGroupTypes.Quotes, "\"'`", "\" ' `"));
        characterGroupDefinitions.Add(new PasswordMakerCharacterGroup("Special", CharacterGroupTypes.Special, "#$%&@\\^_|~", "# $ % & @ \\ ^ _ | ~"));
    }

    public PasswordMakerCharacterGroup FindGroupByType(CharacterGroupTypes type)
    {
        return this.Where(g => g.CharacterGroupType == type).FirstOrDefault();
    }

    public bool ContainsGroupType(CharacterGroupTypes characterGroupType)
    {
        return FindGroupByType(characterGroupType) is not null;
    }
}