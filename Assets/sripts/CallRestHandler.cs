using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class CallRestHandler : MonoBehaviour
{

    string server = "https://api.beingai.io/gossip";
    string mimic = "/mimic";
    string chat = "/chat";
    string file = "/file";
    string url = "";
    public static float audioLength = 0;
    public static string animeBefore;
    public static string animeAfter;
    public static bool isChat = true;
    public static AudioClip audioTrack;
    public delegate void AudioLoaded();
    public static event AudioLoaded OnAudioLoaded;

    public delegate void FileUploadedLoaded(bool sucess);
    public static event FileUploadedLoaded OnFileUploadedLoaded;


    public void callChat(string data, bool audio_response)
    {
        url = server + chat;
        JsonCallModel jsonCallModel = new JsonCallModel();
        jsonCallModel.text = data;
        jsonCallModel.audio_response = audio_response;
        string json = JsonUtility.ToJson(jsonCallModel);
        StartCoroutine(loadChatResult(url, json, audio_response));
    }

    public void callfile()
    {
        url = server + file;
        StartCoroutine(loadTextFile(url));
       
    }

    public void callMimic(string data)
    {
        //Debug.Log("callMimic: " + data);
        url = server + mimic;
        JsonModel jsonMimic = new JsonModel();
        jsonMimic.text = data;
        string json = JsonUtility.ToJson(jsonMimic);

        StartCoroutine(loadMimicResult(url, json));
    }

    IEnumerator loadTextFile(string url)
    {
        yield return null;
        byte[] bytes = File.ReadAllBytes(Utils.fileName);
        string encodedText = Convert.ToBase64String(bytes);
        JsonFileModel jsonFileModel = new JsonFileModel();
        jsonFileModel.base_64_encoded_file = encodedText;
        jsonFileModel.file_name = Path.GetFileName(Utils.fileName);
        
        jsonFileModel.file_type = Path.GetExtension(Utils.fileName); //Utils.fileType;//"pdf";
        string json = JsonUtility.ToJson(jsonFileModel);
        Debug.Log("callfile: " + json);
        StartCoroutine(loadfileResult(url, json));
    }

    IEnumerator loadMimicResult(string url,string json)
    {
        //Debug.Log("loadMimicResult: "+json);
        //WWWForm form = new WWWForm();
        //form.AddField("text", json);

        UnityWebRequest www = UnityWebRequest.Post(url, json);
        UploadHandler customUploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        customUploadHandler.contentType = "application/json";

        www.uploadHandler = customUploadHandler;
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("User-id", PlayerPrefs.GetInt("id",111).ToString());
        www.SetRequestHeader("Accept", "audio/mpeg3");
        www.SetRequestHeader("AUTHORIZATION", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJwb2MiLCJyb2xlIjoiWmVlQXBwIiwiZG9tYWluIjoiYmVpbmdhaSIsInRpbWUiOjE2MDgzNjY3MTI3NDEsImV4cCI6MTYwODM2Njc0OSwiaWF0IjoxNjA4MzY2NzEzfQ.Zeq1jlY-2ZsL9VGCaS9mObXU52MdB5_p7Y0KJmNqm2Y");
        //Debug.Log("loadMimicResult: " + www.isDone);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        //Debug.Log("loadMimicResult after send: " + www.isDone);
        //Debug.Log("TAG:"+ PlayerPrefs.GetInt("id", 111).ToString() + "loadMimicResult: " + www.error);
        if (www.error != null)
        {
            Debug.Log("Error is " + www.error);
        }
        else
        {
            //Debug.Log(" loadMimicResult Not error");
            // Show results as text
            //Debug.Log(www.downloadHandler.text);
            //var uwr = UnityWebRequestMultimedia.GetAudioClip("http://myserver.com/mysound.ogg", AudioType.MPEG)
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
            string fullPath = Path.Combine(Application.persistentDataPath , "Test.mpeg3");//Application.persistentDataPath
            string path = "file://" + fullPath;
            File.WriteAllBytes(fullPath, results);
            
            //if (System.IO.File.Exists(fullPath))
            //{
            //    //do stuff
            //    //Debug.LogError("fie exists");
            //}
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(GetAudioClip(path));
            //float[] f = ConvertByteToFloat(results);
            //AudioClip audioClip = AudioClip.Create("testSound", f.Length, 1, 44100, false, false);
            //audioClip.SetData(f, 0);
            //AudioManeger.Instance.PlayAudio(audioClip);
        }
    }


    IEnumerator loadfileResult(string url, string json)
    {
        //////Debug.Log("loadChatResult: " + json+ " url: " + url);
        //WWWForm form = new WWWForm();
        //form.AddField("text", json);

        UnityWebRequest www = UnityWebRequest.Post(url, json);
        UploadHandler customUploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        customUploadHandler.contentType = "application/json";

        www.uploadHandler = customUploadHandler;
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("User-id", PlayerPrefs.GetInt("id", 111).ToString());
        //www.SetRequestHeader("Accept", "application/json");//"audio/mpeg3"  "application/json" audio/mpeg3
        www.SetRequestHeader("AUTHORIZATION", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJwb2MiLCJyb2xlIjoiWmVlQXBwIiwiZG9tYWluIjoiYmVpbmdhaSIsInRpbWUiOjE2MDgzNjY3MTI3NDEsImV4cCI6MTYwODM2Njc0OSwiaWF0IjoxNjA4MzY2NzEzfQ.Zeq1jlY-2ZsL9VGCaS9mObXU52MdB5_p7Y0KJmNqm2Y");
        ////Debug.Log("loadChatResult: " + www.isDone);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        ////Debug.Log("loadChatResult after send: " + www.isDone);
        ////Debug.Log("TAG:" + PlayerPrefs.GetInt("id", 111).ToString() + "loadMimicResult: " + www.error);
        if (www.error != null)
        {
            Debug.Log("loadfileResult Error is " + www.error);
            if (OnFileUploadedLoaded != null)
                OnFileUploadedLoaded(false);
        }
        else
        {

            Debug.Log("loadfileResult" + www.error + "  responseCode: " + www.responseCode);
            if (OnFileUploadedLoaded != null)
                OnFileUploadedLoaded(true);
            ////Debug.Log(" loadChatResult Not error");
            // Show results as text
            ////Debug.Log(www.downloadHandler.text);
            //var uwr = UnityWebRequestMultimedia.GetAudioClip("http://myserver.com/mysound.ogg", AudioType.MPEG)
            // Or retrieve results as binary data

                //JsonAudioResponce jsonAudio = new JsonAudioResponce();
                //jsonAudio = JsonUtility.FromJson<JsonAudioResponce>(www.downloadHandler.text);

                ////byte[] results = www.downloadHandler.data;//System.Text.Encoding.UTF8.GetBytes(jsonAudio.speech);//www.downloadHandler.data;
                ////animeBefore = jsonAudio.before;
                ////string enc = Convert.ToBase64String(results);
                //byte[] data = Convert.FromBase64String(jsonAudio.speech);
                ////////Debug.LogError("loadChatResult : animeBefore" + (jsonAudio.before == null) + " animeAfter: " + (jsonAudio.after == null));
                //if (jsonAudio.before == null)
                //    animeBefore = "";
                //else
                //    animeBefore = jsonAudio.before;
                //if (jsonAudio.after == null)
                //    animeAfter = "";
                //else
                //    animeAfter = jsonAudio.after;
                //string fullPath = Path.Combine(Application.persistentDataPath, "Test.mpeg3");//Application.persistentDataPath
                //string path = "file://" + fullPath;
                //File.WriteAllBytes(fullPath, data);
                ////if (System.IO.File.Exists(fullPath))
                ////{
                ////    //do stuff
                ////    ////Debug.LogError("fie exists");
                ////}
                //yield return new WaitForSeconds(0.1f);
                //StartCoroutine(GetAudioClip(path));



                //float[] f = ConvertByteToFloat(results);
                //AudioClip audioClip = AudioClip.Create("testSound", f.Length, 1, 44100, false, false);
                //audioClip.SetData(f, 0);
                //AudioManeger.Instance.PlayAudio(audioClip);
        }
    }

    // get audio with anime

    IEnumerator loadChatResult(string url, string json, bool audio_response)
    {
        //////Debug.Log("loadChatResult: " + json+ " url: " + url);
        //WWWForm form = new WWWForm();
        //form.AddField("text", json);

        UnityWebRequest www = UnityWebRequest.Post(url, json);
        UploadHandler customUploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        customUploadHandler.contentType = "application/json";

        www.uploadHandler = customUploadHandler;
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("User-id", PlayerPrefs.GetInt("id", 111).ToString());
        www.SetRequestHeader("Accept", "application/json");//"audio/mpeg3"  "application/json" audio/mpeg3
        www.SetRequestHeader("AUTHORIZATION", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJwb2MiLCJyb2xlIjoiWmVlQXBwIiwiZG9tYWluIjoiYmVpbmdhaSIsInRpbWUiOjE2MDgzNjY3MTI3NDEsImV4cCI6MTYwODM2Njc0OSwiaWF0IjoxNjA4MzY2NzEzfQ.Zeq1jlY-2ZsL9VGCaS9mObXU52MdB5_p7Y0KJmNqm2Y");
        ////Debug.Log("loadChatResult: " + www.isDone);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        ////Debug.Log("loadChatResult after send: " + www.isDone);
        ////Debug.Log("TAG:" + PlayerPrefs.GetInt("id", 111).ToString() + "loadMimicResult: " + www.error);
        if (www.error != null)
        {
            ////Debug.Log("Error is " + www.error);
        }
        else
        {
            ////Debug.Log(" loadChatResult Not error");
            // Show results as text
            ////Debug.Log(www.downloadHandler.text);
            //var uwr = UnityWebRequestMultimedia.GetAudioClip("http://myserver.com/mysound.ogg", AudioType.MPEG)
            // Or retrieve results as binary data
            if (audio_response)
            {
                JsonAudioResponce jsonAudio = new JsonAudioResponce();
                jsonAudio = JsonUtility.FromJson<JsonAudioResponce>(www.downloadHandler.text);
               Debug.LogError("jsonAudio: " + jsonAudio.text.ToLower());
                if (Utils.isMimicCalled)
                {
                    Debug.LogError("isMimicCalled: " + Utils.mimicCalledtext);
                    Utils.isMimicCalled = false;
                    if (!jsonAudio.text.ToLower().Equals(Utils.mimicCalledtext))
                    {
                        yield break;
                    }
                    else
                    {
                        if (Utils.checkjokeemotioncall)
                        {
                            Utils.checkafterjoke = false;
                        }
                        else
                        {
                            Utils.nwutralafterjoke = false;
                        }
                    }
                }

                //byte[] results = www.downloadHandler.data;//System.Text.Encoding.UTF8.GetBytes(jsonAudio.speech);//www.downloadHandler.data;
                //animeBefore = jsonAudio.before;
                //string enc = Convert.ToBase64String(results);

                Debug.LogError("jsondata: " + jsonAudio.text.ToLower());
                byte[] data = Convert.FromBase64String(jsonAudio.speech);
                //////Debug.LogError("loadChatResult : animeBefore" + (jsonAudio.before == null) + " animeAfter: " + (jsonAudio.after == null));
                if (jsonAudio.before == null)
                    animeBefore = "";
                else
                    animeBefore = jsonAudio.before;
                if (jsonAudio.after == null)
                    animeAfter = "";
                else
                    animeAfter = jsonAudio.after;

                if (jsonAudio.text.ToLower().Contains("you like it"))
                {
                    Utils.checkafterjoke = true;
                    //Utils.nwutralafterjoke = true;
                }
                string fullPath = Path.Combine(Application.persistentDataPath, "Test.mpeg3");//Application.persistentDataPath
                string path = "file://" + fullPath;
                File.WriteAllBytes(fullPath, data);
                //if (System.IO.File.Exists(fullPath))
                //{
                //    //do stuff
                //    ////Debug.LogError("fie exists");
                //}
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(GetAudioClip(path));
                //float[] f = ConvertByteToFloat(results);
                //AudioClip audioClip = AudioClip.Create("testSound", f.Length, 1, 44100, false, false);
                //audioClip.SetData(f, 0);
                //AudioManeger.Instance.PlayAudio(audioClip);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                ChatModel myObject = JsonUtility.FromJson<ChatModel>(www.downloadHandler.text);
                ChatControllerZee.instance.MessageFromServer(myObject.text);
            }

        }
    }

    IEnumerator GetAudioClip(string audioPath)
    {
        //using (var uwr = UnityWebRequestMultimedia.GetAudioClip(audioPath, AudioType.MPEG))
        //{
        //    yield return uwr.SendWebRequest();
        //    if (uwr.isNetworkError || uwr.isHttpError)
        //    {
        //        ////Debug.LogError("GetAudioClip: "+ uwr.error);
        //        yield break;
        //    }

        //    AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
        //    // use audio clip
        //    AudioManeger.Instance.PlayAudio(clip);
        //}
        //////Debug.LogError("fie GetAudioClip" + audioPath);
        WWW request = new WWW(audioPath);
        while (!request.isDone)
        {
            ////Debug.Log("request progrss " + request.progress);
            //audio_status.text = audio_status.text + "request progrss " + request.progress;
            yield return new WaitForEndOfFrame();
        }
        if (request.error == null)
        {
            audioTrack = request.GetAudioClip(false, false, AudioType.MPEG);
            ////Debug.Log("Failed to Load!"+ audioTrack.loadState);
            while (audioTrack.loadState == AudioDataLoadState.Loading)
            {
                // Wait until loading completed
                yield return new WaitForEndOfFrame();
            }
            if (audioTrack.loadState != AudioDataLoadState.Loaded)
            {
                // Fail to load
                ////Debug.Log("Failed to Load!");
                yield break;
            }
            audioLength = audioTrack.length;
            //AudioManeger.Instance.PlayAudio(audioTrack);
            if (OnAudioLoaded != null)
                OnAudioLoaded();
        }
    }

    private float[] ConvertByteToFloat(byte[] array)
    {
        float[] floatArr = new float[array.Length / 4];
        for (int i = 0; i < floatArr.Length; i++)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(array, i * 4, 4);
            //floatArr[i] = BitConverter.ToSingle(array, i * 4);
            floatArr[i] = BitConverter.ToSingle(array, i * 4) / 0x80000000;
        }
        return floatArr;
    }
}
