using UnityEngine;
using Gemini;
using System.Threading.Tasks;

public class Communication : MonoBehaviour
{
    // Lock and Synchronization
    private bool lproj;
    // Gemini API
    private GeminiTextRequest geminiTextRequest;

     // Global
    private string llmOutput;

    void Start()
    {
        lproj = true;
        llmOutput = "";
        geminiTextRequest = new GeminiTextRequest();
    }

    /**
    <summary>
    You can pass as many strings as you want, they can be images, videos, cloud files, etc. as long as you chang the config in GeminiTextRequest.cs
    The first input must be text input.
    </summary>
    */
    void Run()
    {
        // Specify your PNG file path
        string filePath = "path/to/your/image.png";

        // Convert PNG to Base64 string
        string base64String = ConvertToBase64(filePath);
        
        yield return StartCoroutine(CommunicationWithGemini("Looks at my images!", base64String));
        Debug.Log($"Response is: {llmOutput}");
    }

    string ConvertToBase64(string filePath)
    {
        // Read the file into a byte array
        byte[] imageBytes = File.ReadAllBytes(filePath);

        // Convert the byte array to a Base64 string
        string base64String = Convert.ToBase64String(imageBytes);

        return base64String;
    }

    IEnumerator CommunicationWithGemini(params string[] strings)
    {
        yield return new WaitUntil(() => lproj);
        lproj = false;

        // Start the async operation in a way that doesn't block the Unity main thread
        // Directly using GenerateContent within Task.Run
        Task<GeminiTextResponse> llmTask = Task.Run(async () => await geminiTextRequest.SendMsg(strings));

        // Wait until the async task has completed
        while (!llmTask.IsCompleted)
        {
            yield return null;
        }

        GeminiTextResponse response = llmTask.Result; // Access the Result property here
        string tempOut = response.candidates[0].content.parts[0].text;
        llmOutput = tempOut;

        // Release lock
        lproj = true;
    }
}
