using System;

[Serializable]
public struct Message{
    public string header;
    public string description;

    public Message(string header, string description){
        this.header = header;
        this.description = description;
    }
}