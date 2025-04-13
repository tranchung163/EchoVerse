using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using dotenv.net;
using dotenv.net.Utilities;

public class OpenAIController : MonoBehaviour
{
    public TMP_Text textField;
    public TMP_InputField inputField;
    private OpenAIAPI api;
    private List<ChatMessage> messages;
    bool StillTalking = false;

    private bool noreponde = false;

    void Start()
    {

        string relativePath = "../Key.env";
        string fullPath = Path.GetFullPath(relativePath);

        Debug.Log($"[ENV] Attempting to load: {fullPath}");

        if (!File.Exists(fullPath))
        {
            Debug.LogError($"[ENV] File does NOT exist at: {fullPath}");
            return;
        }

        var options = new DotEnvOptions(envFilePaths: new[] { "../Key.env" });
        DotEnv.Load(options);

        string apiKey = Environment.GetEnvironmentVariable("OpenAI");

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API key not found. Make sure it's defined in your key.env file.");
            return;
        }

        // Pass it to OpenAIAPI
        api = new OpenAIAPI(apiKey);

        StartConversation();
    }

    private void StartConversation()
    {
        messages = new List<ChatMessage>
        {
            new ChatMessage(ChatMessageRole.System, "You a cute lil aliean")
        };
        inputField.text = "";
    }

    public async void GetResponse()
    {
        if (inputField.text.Length < 1 && StillTalking)
        {
            return;
        }

        StillTalking = true;

        // Fill the user message from the input field
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = inputField.text;
        if (userMessage.Content.Length > 100)
        {
            // Limit messages to 100 characters
            userMessage.Content = userMessage.Content.Substring(0, 100);
        }
        Debug.Log(string.Format("{0}: {1}", userMessage.rawRole, userMessage.Content));

        // Add the message to the list
        messages.Add(userMessage);

        // Clear the input field
        inputField.text = "";

        // Send the entire chat to OpenAI to get the next message
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.9,
            MaxTokens = 50,
            Messages = messages
        });

        // Get the response message
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        Debug.Log(string.Format("{0}: {1}", responseMessage.rawRole, responseMessage.Content));

        // Add the response to the list of messages
        messages.Add(responseMessage);

        // Update the text field with the response
        textField.text = responseMessage.Content;

        StillTalking = false;
    }
}
