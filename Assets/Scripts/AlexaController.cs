using Amazon;
using Amazon.DynamoDBv2.Model;
using AmazonsAlexa.Unity.AlexaCommunicationModule;
using System.Collections.Generic;
using UnityEngine;

public class AlexaController : MonoBehaviour
{
    //Step 5: Add variables below
    public string publishKey;
    public string subscribeKey;
    public string channel;
    public string tableName;
    public string identityPoolId;
    public string AWSRegion = RegionEndpoint.USEast1.SystemName;
    public bool debug = false;
    public GameObject lightCube;
    public GameObject camera;

    private Dictionary<string, AttributeValue> attributes;
    private AmazonAlexaManager alexaManager;

    public SceneController sceneControl;
    public MainUIControl mainUICtrl;
    public MainControl mainCtrl;

    // Use this for initialization
    void Start()
    {
        //Step 6: Add Games SDK for Alexa initialization
        UnityInitializer.AttachToGameObject(gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        alexaManager = new AmazonAlexaManager(publishKey, subscribeKey, channel, tableName, identityPoolId, AWSRegion, this.gameObject, OnAlexaMessage, null, debug); //Initialize the Alexa Manager
    }

    public void ConfirmSetup(GetSessionAttributesEventData eventData)
    {
        //Step 7: Notify the skill that setup has completed by updating the skills persistant attributes (in DynamoDB)
        attributes = eventData.Values;
        attributes["SETUP_STATE"] = new AttributeValue { S = "COMPLETED" }; //Set SETUP_STATE attribute to a string, COMPLETED
        alexaManager.SetSessionAttributes(attributes, SetAttributesCallback);
    }

    void Update()
    {
        //Step 8: Add Keyboard event listener (For "gameplay")
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed");
            HandleSpacePress();
        }
    }

    

    public void HandleSpacePress()
    {
        //Step 9: Handle the keyboard input
        if (!PlayerPrefs.HasKey("alexaUserDynamoKey")) //If the AlexaUserId has not been recieved from Alexa (If the user has not opened the skill)
        {
            Debug.LogError("'alexaUserDynamoKey' not found in PlayerPrefs. We must establish connection from Alexa to set this. Please open the skill to set the 'AlexaUserId' PlayerPref.");
        }
        else
        {
            alexaManager.GetSessionAttributes((result) =>
            {
                if (result.IsError)
                    Debug.LogError(result.Exception.Message);
                UpdateLight("Color", "blue", result);
            });
        }
    }

    //Callback for when a message is recieved
    public void OnAlexaMessage(HandleMessageEventData eventData)
    {
        //Step 10: Listen for new messages from the Alexa skill
        Debug.Log("OnAlexaMessage");

        Dictionary<string, object> message = eventData.Message;

        //Get Session Attributes with in-line defined callback
        switch (message["type"] as string)
        {
            case "AlexaUserId":
                Debug.Log("AlexaUserId: " + message["message"]);
                alexaManager.alexaUserDynamoKey = message["message"] as string;
                break;
        }

        alexaManager.GetSessionAttributes((result) =>
        {
            if (result.IsError)
                Debug.LogError(eventData.Exception.Message);

            switch (message["type"] as string)
            {
                case "AlexaUserId":
                    ConfirmSetup(result);
                    break;
                case "Color":
                    Debug.Log("Requested Light Color: " + message["message"]);
                    UpdateLight(message["type"] as string, message["message"] as string, result);
                    break;
                case "State":
                    Debug.Log("Requested Light State: " + message["message"]);
                    UpdateLight(message["type"] as string, message["message"] as string, result);
                    break;
                case "GetObject":
                    Debug.Log("Requested object direction: " + message["message"]);
                    GetObjectInDirection(message["type"] as string, message["message"] as string);
                    break;

                case "SceneInformation":
                    Debug.Log("Requested information of actual scene" + message["message"]);
                    // scene method info()
                    GetSceneInfo(mainCtrl.onsightLoc);
                    break;
                case "Continue":
                    Debug.Log("continue request" + message["message"]);
                    int activeID = mainUICtrl.activeID;
                    
                    switch (activeID)
                    {
                        case 3:
                            mainUICtrl.Fade(activeID, activeID + 1);
                            mainUICtrl.ShowYorLocations();
                            break;
                    }
                    break;

                case "GoToLocation":
                    Debug.Log("go to location" + message["message"]);
                    mainCtrl.onsightLoc.SelectLocation();
                    mainCtrl.infoBtn.SetActive(false);
                    mainCtrl.backBtn.SetActive(false);
                    break;

                   
                default:
                    break;
            }
        });
    }

    void GetSceneInfo(Location loc)
    {
        Dictionary<string, string> messageToAlexa = new Dictionary<string, string>();
        string information = loc.name + " " + loc.description + " ";
        messageToAlexa.Add("SceneInfo", information) ;
        alexaManager.SendToAlexaSkill(messageToAlexa, OnMessageSent);
    }

    

    private void GetObjectInDirection(string type, string message)
    {
        //Step 11: Get the object in a specific direction (Note: For this demo, there is only one object, the cube)
        RaycastHit hit;
        Dictionary<string, string> messageToAlexa = new Dictionary<string, string>();
        Vector3 forward = camera.transform.forward * 10;
        Vector3 backward = camera.transform.forward * -10;
        Vector3 right = camera.transform.right * 10;
        Vector3 left = camera.transform.right * -10;
        Vector3 up = camera.transform.up * 10;
        Vector3 down = camera.transform.up * -10;

        Vector3 direction = forward;

        switch (message)
        {
            case "forward":
                direction = forward;
                break;
            case "backward":
                direction = backward;
                break;
            case "right":
                direction = right;
                break;
            case "left":
                direction = left;
                break;
            case "up":
                direction = up;
                break;
            case "down":
                direction = down;
                break;
        }

        messageToAlexa.Add("object", "nothing");

        if (Physics.Raycast(camera.transform.position, direction, out hit, (float)15.0))
        {
            if (hit.rigidbody)
            {
                messageToAlexa.Remove("object");
                messageToAlexa.Add("object", hit.rigidbody.name);
            }
        }

        alexaManager.SendToAlexaSkill(messageToAlexa, OnMessageSent);
    }

    public void UpdateLight(string type, string value, GetSessionAttributesEventData eventData)
    {
        //Step 12: Update the light based on the incoming message, then save the state of the light through the skill's session attributes
        attributes = eventData.Values;
        if (type == "Color")
        {
            attributes["color"] = new AttributeValue { S = value }; //Set color attribute to a string value
        }
        else if (type == "State")
        {
            attributes["state"] = new AttributeValue { S = value }; //Set state attribute to a string value
        }

        switch (value)
        {
            case "white":
                lightCube.GetComponent<Renderer>().material.color = Color.white;
                break;
            case "red":
                lightCube.GetComponent<Renderer>().material.color = Color.red;
                break;
            case "green":
                lightCube.GetComponent<Renderer>().material.color = Color.green;
                break;
            case "yellow":
                lightCube.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case "blue":
                lightCube.GetComponent<Renderer>().material.color = Color.blue;
                break;
            case "on":
                lightCube.GetComponent<Renderer>().enabled = true;
                break;
            case "off":
                lightCube.GetComponent<Renderer>().enabled = false;
                break;
        }
        alexaManager.SetSessionAttributes(attributes, SetAttributesCallback);  //Save Attributes for Alexa to use
    }

    public void SetAttributesCallback(SetSessionAttributesEventData eventData)
    {
        //Step 13: Callback for when session attributes have been updated
        Debug.Log("OnSetAttributes");
        if (eventData.IsError)
            Debug.LogError(eventData.Exception.Message);
    }


    public void OnMessageSent(MessageSentEventData eventData)
    {
        //Step 14: Callback for when a message is sent
        Debug.Log("OnMessageSent");
        if (eventData.IsError)
            Debug.LogError(eventData.Exception.Message);
    }
}
