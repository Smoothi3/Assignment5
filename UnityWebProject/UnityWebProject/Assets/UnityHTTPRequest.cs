using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using TMPro;
using UnityEngine.UI;

//using UnityEngine.UIElements;

public class UnityHTTPRequest : MonoBehaviour
{
    public string nameData;
    public TMP_InputField screenNameInput;
    public TMP_InputField firstNameInput;
    public TMP_InputField lastNameInput;
    public TMP_InputField dateInput;
    public TMP_InputField scoreInput;
    public TMP_InputField singleSearchInput;
    public InputField singleSearchBox;
    public InputField multiSearchBox;

    public TMP_InputField editScreenName;
    public TMP_InputField editFirstName;
    public TMP_InputField editLastName;
    public TMP_InputField editDate;
    public TMP_InputField editScore;
    [SerializeField]

    string postData;

    [Serializable]
    public class MyData
    {
        //Change Data for whatever you need
        public string myScreenName;
        public string myFirstName;
        public string myLastName;
        public string myDate;
        public float myScore;
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(MakeWebRequest());
    }

    IEnumerator MakeWebRequest()
    {
        //GET Request Example
        var getRequest = new UnityWebRequest($"http://localhost:3000/unity?name={nameData}");
        getRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return getRequest.SendWebRequest();
        Debug.Log(getRequest.result);



        //CreateSingleObjectFromData(getRequest);
        //CreateObjectsFromArray(jsonData);
    }
    IEnumerator PostRequest(string sendData)
    {
        var request = new UnityWebRequest("http://localhost:3000/unityPost", "POST");
        byte[] bodyData = Encoding.UTF8.GetBytes(sendData);
        request.uploadHandler = new UploadHandlerRaw(bodyData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

    }

    IEnumerator MakeWebRequestSingle()
    {
        //GET Request Example with Query
        string searchName = singleSearchInput.text;
        var getRequest = new UnityWebRequest($"http://localhost:3000/unityGetOne?myScreenName={searchName}");
        getRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return getRequest.SendWebRequest();
        Debug.Log(getRequest.result);

        switch (getRequest.result)
        {
            case UnityWebRequest.Result.Success:
                if(getRequest.downloadHandler.text == "null")
                {
                    singleSearchBox.text = "Name not found, try again";
                } else
                {
                    Debug.Log(getRequest.downloadHandler.text);
                    singleSearchBox.text = getRequest.downloadHandler.text;
                }
                break;
        }



        //CreateSingleObjectFromData(getRequest);
        //CreateObjectsFromArray(jsonData);
    }

    IEnumerator MakeWebRequestMultiple()
    {
        //GET Request Example with Query
        var getRequest = new UnityWebRequest($"http://localhost:3000/unity");
        getRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return getRequest.SendWebRequest();
        Debug.Log(getRequest.result);
        

        switch (getRequest.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(getRequest.downloadHandler.text);
                multiSearchBox.text = getRequest.downloadHandler.text;
                break;
        }



        //CreateSingleObjectFromData(getRequest);
        //CreateObjectsFromArray(jsonData);
    }

    IEnumerator MakeDeleteRequest()
    {
        //Check if entry exists
        string searchName = editScreenName.text;

        var searchRequest = new UnityWebRequest($"http://localhost:3000/unityGetOne?myScreenName={searchName}");
        searchRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return searchRequest.SendWebRequest();

        switch (searchRequest.result)
        {
            case UnityWebRequest.Result.Success:
                if (searchRequest.downloadHandler.text == "null")
                {
                    Debug.Log(singleSearchBox.text = "Name not found, try again");
                }
                else
                {
                    Debug.Log("Deleting " + searchName);
                    var getRequest = new UnityWebRequest($"http://localhost:3000/unityDeleteEntry?myScreenName={searchName}", "POST");
                    getRequest.downloadHandler = new DownloadHandlerBuffer();
                    yield return getRequest.SendWebRequest();
                }
                break;
        }
        
    }

    IEnumerator MakeUpdateRequest(string sendData)
    {
        //Check if entry exists
        string searchName = editScreenName.text;

        var searchRequest = new UnityWebRequest($"http://localhost:3000/unityGetOne?myScreenName={searchName}");
        searchRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return searchRequest.SendWebRequest();

        switch (searchRequest.result)
        {
            case UnityWebRequest.Result.Success:
                if(searchRequest.downloadHandler.text == "null")
                {
                    Debug.Log(singleSearchBox.text = "Name not found, try again");
                } else
                {
                    Debug.Log("Updating " + searchName);
                    var putRequest = new UnityWebRequest("http://localhost:3000/unityUpdate", "PUT");
                    byte[] bodyData = Encoding.UTF8.GetBytes(sendData);
                    putRequest.uploadHandler = new UploadHandlerRaw(bodyData);
                    putRequest.downloadHandler = new DownloadHandlerBuffer();
                    putRequest.SetRequestHeader("Content-Type", "application/json");
                    yield return putRequest.SendWebRequest();
                }
                break;
        } 
    }

    private static void CreateSingleObjectFromData(UnityWebRequest getRequest)
    {
        var deserializedJson = JsonUtility.FromJson<MyData>(getRequest.downloadHandler.text);
        Debug.Log(deserializedJson.myScreenName);
    }

    private void CreateObjectsFromArray(string jsonData)
    {
        string jsonString = fixJson(jsonData);
        MyData[] objData = JsonHelper.FromJson<MyData>(jsonString);
        //create data object array here
    }

    public void GetMultipleData()
    {
        StartCoroutine(MakeWebRequestMultiple());
    }

    public void GetSingleData()
    {
        StartCoroutine(MakeWebRequestSingle());
    }

    public void DeleteEntry()
    {
        StartCoroutine(MakeDeleteRequest());
    }

    public void SendData()
    {
        MyData sendData = new MyData();
        sendData.myScreenName = screenNameInput.text;
        sendData.myFirstName = firstNameInput.text;
        sendData.myLastName = lastNameInput.text;
        sendData.myDate = dateInput.text;
        sendData.myScore = int.Parse(scoreInput.text);
        var postData = JsonUtility.ToJson(sendData);
        Debug.Log(postData);
        StartCoroutine(PostRequest(postData));

    }

    public void UpdateData()
    {
        MyData sendData = new MyData();
        sendData.myScreenName = editScreenName.text;
        sendData.myFirstName = editFirstName.text;
        sendData.myLastName = editLastName.text;
        sendData.myDate = editDate.text;
        sendData.myScore = int.Parse(editScore.text);
        var postData = JsonUtility.ToJson(sendData);
        Debug.Log(postData);
        StartCoroutine(MakeUpdateRequest(postData));
    }

    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            Debug.Log(wrapper.Items);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}