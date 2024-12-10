using BOs;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.IdentityModel.Tokens;

namespace Services.Implement
{
    /// <summary>
    /// This class is request and response data
    /// </summary>
    public class ChatContent
    {
        public class Part
        {
            public class Blob
            {
                public string mimeType { get; set; }
                /// <summary>
                /// Base64 encoded string
                /// </summary>
                public string data { get; set; }
            }

            public class FileData
            {
                public string mimeType { get; set; }
                public string fileUri { get; set; }
            }

            public string text { get; set; }
            public FileData fileData { get; set; }
            public Blob inlineData { get; set; }
        }

        public string? role { get; set; }
        public List<Part> parts { get; set; }
    }

    public class Candidate
    {
        /// <summary>  
        /// Enumeration representing the reasons for finishing a model's response. 
        /// </summary>  
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum FinishReason
        {
            FINISH_REASON_UNSPECIFIED,
            STOP,
            MAX_TOKENS,
            SAFETY,
            RECITATION,
            LANGUAGE,
            OTHER,
            BLOCKLIST,
            PROHIBITED_CONTENT,
            SPII,
            MALFORMED_FUNCTION_CALL
        }

        public ChatContent content { get; set; }
        public FinishReason finishReason { get; set; }
    }

    public class GeminiRequest
    {
        public List<ChatContent> contents { get; set; }
    }

    public class GeminiResponse
    {
        public List<Candidate> candidates { get; set; }
        //public List<object> promptFeedback { get; set; }
        //public List<object> usageMetadata { get; set; }
    }

    public class ErrorRespsonse
    {
        public class Error
        {
            public string code { get; set; }
            public string message { get; set; }
            public string status { get; set; }
        }

        public Error error { get; set; }
    }

    /// <summary>
    /// This class is used to interact with the ChatBot API (currently Gemini)
    /// </summary>
    public class ChatBotCore
    {
        private HttpClient _client = new HttpClient();

        /// <summary>
        /// History message of a conversation
        /// </summary>
        public List<Message> History { get; set; } = new List<Message>();
        public string Model { set; get; } = "gemini-1.5-flash";
        public string ApiKey { set; get; } = "AIzaSyB78Sqk6AGXDBGSXvNaLX5UQdtBe02Amvs";

        public ChatBotCore()
        {
            ApiKey ??= Environment.GetEnvironmentVariable("GEMINI_API_KEY", EnvironmentVariableTarget.User);
            Console.WriteLine("API Key: " + ApiKey);
        }

        /// <summary>
        /// Send a message to the ChatBot API
        /// </summary>
        /// <param name="newMessange">newMessage will be used as a new prompt that user enter</param>
        /// <returns>A string reprensent model response to user's prompt</returns>
        /// <exception cref="ArgumentNullException">API Key or model is missing</exception>
        /// <exception cref="Exception">Gemini send back error</exception>
        public async Task<string> SendMessageAsync(string newMessange)
        {
            if (ApiKey == null)
            {
                throw new ArgumentNullException("API Key is not set");
            }

            if (Model == null)
            {
                throw new ArgumentNullException("Model is not set");
            }

            // Add history to the request 
            List<ChatContent> _chatContents = new List<ChatContent>();

            foreach (var message in History)
            {
                if (message.UserResquest != null)
                {
                    _chatContents.Add(new ChatContent
                    {
                        role = "user",
                        parts = new List<ChatContent.Part> { new ChatContent.Part { text = message.UserResquest } }
                    });
                }
                if (message.ModelResponse != null)
                {
                    _chatContents.Add(new ChatContent
                    {
                        role = "model",
                        parts = new List<ChatContent.Part>([new ChatContent.Part { text = message.ModelResponse }])
                    });
                }
            }

            // Add new message to the request
            _chatContents.Add(new ChatContent
            {
                role = "user",
                parts = [new ChatContent.Part { text = newMessange }]
            });

            Uri uri = new Uri($"https://generativelanguage.googleapis.com/v1beta/models/{Model}:generateContent?key={ApiKey}");
            var content = new StringContent(JsonSerializer.Serialize(new GeminiRequest { contents = _chatContents }), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(uri, content);
            Console.WriteLine("AI Http status: " + response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                string errorRespsonse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("err: " + errorRespsonse);

                throw new Exception(errorRespsonse);
            }

            var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
            Console.WriteLine("Finish Reason: " + geminiResponse.candidates[0].finishReason);

            var modelResponse = geminiResponse.candidates[0].content.parts[0].text;

            // ModelResponse will exist at this point, save it (also add conversationID of the old message (if exist) to the new one)
            History.Add(new Message
            {
                ConservationId = History.IsNullOrEmpty() ? 0 : History.First().ConservationId,
                UserResquest = newMessange,
                ModelResponse = modelResponse
            });

            return modelResponse;
        }


        /// <summary>
        /// Send a message to the ChatBot API
        /// </summary>
        /// <param name="newMessange"> <paramref name="newMessange"/>.UserRequest will be used as a new prompt that user enter</param>
        /// <returns>Origin <paramref name="newMessange"/> that added modelResponse into it</returns>
        /// <exception cref="ArgumentNullException">API Key or model is missing</exception>
        /// <exception cref="Exception">Gemini send back error</exception>
        public async Task<Message> SendMessageAsync(Message newMessange)
        {
            if (ApiKey == null)
            {
                throw new ArgumentNullException("API Key is not set");
            }

            if (Model == null)
            {
                throw new ArgumentNullException("Model is not set");
            }

            // Add history to the request 
            List<ChatContent> _chatContents = new List<ChatContent>();

            foreach (var message in History)
            {
                if (message.UserResquest != null)
                {
                    _chatContents.Add(new ChatContent
                    {
                        role = "user",
                        parts = new List<ChatContent.Part> { new ChatContent.Part { text = message.UserResquest } }
                    });
                }
                if (message.ModelResponse != null)
                {
                    _chatContents.Add(new ChatContent
                    {
                        role = "model",
                        parts = new List<ChatContent.Part>([new ChatContent.Part { text = message.ModelResponse }])
                    });
                }
            }

            // Add new message to the request
            _chatContents.Add(new ChatContent
            {
                role = "user",
                parts = [new ChatContent.Part { text = newMessange.UserResquest }]
            });

            Uri uri = new Uri($"https://generativelanguage.googleapis.com/v1beta/models/{Model}:generateContent?key={ApiKey}");
            var content = new StringContent(JsonSerializer.Serialize(new GeminiRequest { contents = _chatContents }), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(uri, content);
            Console.WriteLine("AI Http status: " + response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                string errorRespsonse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("err: " + errorRespsonse);

                throw new Exception(errorRespsonse);
            }

            var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
            Console.WriteLine("Finish Reason: " + geminiResponse.candidates[0].finishReason);

            var modelResponse = geminiResponse.candidates[0].content.parts[0].text;

            // ModelResponse will exist at this point, save it (also add conversationID of the old message (if exist) to the new one)
            newMessange.ConservationId = History.IsNullOrEmpty() ? 0 : History.First().ConservationId;
            newMessange.ModelResponse = modelResponse;
            History.Add(newMessange);

            return newMessange;
        }
    }
}
