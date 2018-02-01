using System.Collections.Generic;

public class GameConfigEntry : ICSVDeserializable
{
    private string _strKey;
    private string _strValue;

    public string Key
    {
        get
        {
            return _strKey;
        }
    }

    public string Value
    {
        get
        {
            return _strValue;
        }
    }

    public void CSVDeserialize(Dictionary<string, string[]> data, int index)
    {
        _strKey = data["Key"][index];
        _strValue = data["Value"][index];
    }
}
