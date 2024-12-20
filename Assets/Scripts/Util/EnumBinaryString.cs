using System;
using System.Collections.Generic;

[Serializable]
public class EnumBinaryString<T> where T : Enum
{
    public string BinaryString;
    public List<string> EnumValueNames;

    public EnumBinaryString(string binaryString)
    {
        EnumValueNames = new List<string>();
        BinaryString = binaryString;
        Array values = Enum.GetValues(typeof(T));
        foreach (int i in values)
        {
            string name = Enum.GetName(typeof(T), i);
            EnumValueNames.Add(name);
        }
    }

    public bool BinaryStringContainsEnum(T enumValue)
    {
        return BinaryString[Convert.ToInt32(enumValue)] == '1';
    }
}