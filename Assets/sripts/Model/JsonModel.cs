using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class JsonModel 
{
    public string text;
}


public class JsonCallModel
{
    public string text;
    public bool audio_response;
}

public class JsonAudioResponce
{
    public string text;
    public string speech;
    public string after;
    public string during;
    public string before;
}

public class JsonFileModel
{
    public string base_64_encoded_file;
    public string file_name;
    public string file_type;
}