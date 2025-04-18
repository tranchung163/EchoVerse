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
using UnityEngine.Events;
using dotenv.net;
using dotenv.net.Utilities;

public class OpenAIController : MonoBehaviour
{
    public TMP_Text textField;
    public TMP_InputField inputField;
    private OpenAIAPI api;
    private List<ChatMessage> messages;
    public TypeWriter typeWriter;
    public UnityEvent OnFinished;
    bool StillTalking = false;

    private bool noreponde = false;

    void Start()
    {
        // Pass it to OpenAIAPI
        api = new OpenAIAPI("");

        StartConversation();
    }

    private void StartConversation()
    {
        messages = new List<ChatMessage>
        {
            new ChatMessage(ChatMessageRole.System, "Alright, so you are an aliean flower. That acts like a travel guide, and you are gonna sugguest places to go to and you have to follow a spacy theme every time you speak, like it has to do with space, also keep it short like around 10 words")
        };
        inputField.text = "";
    }

    public async void GetResponse()
    {
        if(typeWriter.StillTalking) return;

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
        typeWriter.SentText(responseMessage.Content);

        StillTalking = false;
        
        OnFinished.Invoke();
    }
}
