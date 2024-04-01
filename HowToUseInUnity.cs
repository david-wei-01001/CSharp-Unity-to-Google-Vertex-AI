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

    void Run()
    {
        yield return StartCoroutine(CommunicationWithGemini('YOUR INPUT SEQUENCES'));
        Debug.Log($"Response is: {llmOutput}");
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
