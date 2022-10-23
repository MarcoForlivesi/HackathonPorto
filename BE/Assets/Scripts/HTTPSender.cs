using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPSender : MonoBehaviour
{
    [SerializeField] private string Ip;
    [SerializeField] private string TCPPort;
    [SerializeField] private string endpoint;

    private TMP_Text errorText;
    private Transform errorPopup;

    
    public void SendData(RewardRecognitionData rewardRecognitionData)
    {
        StartCoroutine(UploadTcp_Coroutine(rewardRecognitionData));
    }

    // Update is called once per frame
    private IEnumerator UploadTcp_Coroutine(RewardRecognitionData rewardRecognitionData)
    {
        string url = string.Format("http://{0}:{1}{2}", Ip, TCPPort, endpoint);

        string requestBody = JsonUtility.ToJson(rewardRecognitionData);

        var req = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(requestBody);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Error Url:{url} error: {req.error}");
            //errorText.text = $"Error Url:{url} error: {req.error}";
            //errorPopup.gameObject.SetActive(true);
        }
        else
        {
            string responseBody = req.downloadHandler.text;
            //Debug.Log($"Send to: {url} message: {xchangeState} response: {responseBody}");
        }
    }
}
