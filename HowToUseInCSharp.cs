using System;
using Gemini;
using System.Threading.Tasks;


class Communication
{
   static void Main(string[] args)
    {
        string llmOutput = CommunicationWithGemini("YOUR INPUT STRING");
        Console.WriteLine($"Response is: {llmOutput}");
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