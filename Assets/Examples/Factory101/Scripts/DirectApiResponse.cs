using System;
using System.Collections.Generic;

[Serializable]
public class APIResponse
{
    public string id;
    public string @object;
    public long created;
    public string model;
    public List<Choice> choices;
    public Usage usage;
}

[Serializable]
public class Choice
{
    public int index;
    public Message message;
    public string finish_reason;
}

[Serializable]
public class Message
{
    public string role;
    public string content;
}

[Serializable]
public class Usage
{
    public int prompt_tokens;
    public int completion_tokens;
    public int total_tokens;
}