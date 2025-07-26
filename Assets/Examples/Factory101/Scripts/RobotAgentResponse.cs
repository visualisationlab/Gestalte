using System;

[Serializable]
public class RobotAgentResponse
{
    public string Lua;
    public string GameObjectGUID;

    public static string Format()
    {
        return @"
{
""Lua"": ""this:MethodName()"",
""GameObjectGUID"": ""abcd-efgh-ijklmnop-etc""
}
";
    }
}