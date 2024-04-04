using System;
using System.IO;
using Gemini;
using System.Threading.Tasks;


class Communication
{
   /**
    <summary>
    You can pass as many strings as you want, they can be images, videos, cloud files, etc. as long as you change the config in GeminiTextRequest.cs
    The first input must be text input.
    </summary>
    */
   static void Main(string[] args)
    {
      // Specify your PNG file path
     string filePath = "path/to/your/image.png";

     // Convert PNG to Base64 string
     string base64String = ConvertToBase64(filePath);
      
     string llmOutput = CommunicationWithGemini("Looks at my images!", base64String);
     Console.WriteLine($"Response is: {llmOutput}");
    }

   static string ConvertToBase64(string filePath)
    {
        // Read the file into a byte array
        byte[] imageBytes = File.ReadAllBytes(filePath);

        // Convert the byte array to a Base64 string
        string base64String = Convert.ToBase64String(imageBytes);

        return base64String;
    }
   
    static string CommunicationWithGemini(params string[] strings)
    {
        // Start the async operation in a way that doesn't block the  main thread
        // Directly using GenerateContent within Task.Run
        Task<GeminiTextResponse> llmTask = Task.Run(async () => await geminiTextRequest.SendMsg(strings));

        // Wait until the async task has completed
        while (!llmTask.IsCompleted);

        GeminiTextResponse response = llmTask.Result; // Access the Result property here
        string llmOutput = response.candidates[0].content.parts[0].text;
        return llmOutput;
    }
}
