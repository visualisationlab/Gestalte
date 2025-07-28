using System;

[Serializable]
public class RobotAgentResponse
{
    public string Lua;

    public static string Format()
    {
        return @"
{
""Lua"": ""this:MethodName()""
}
";
    }
}