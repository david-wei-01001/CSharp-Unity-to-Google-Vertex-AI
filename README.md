# C# and Unity to Google Vertex AI
Provide code to connect form C# to Google Vertex AI that can be run within Unity

# Descriptions
Google Gemini is not available in some countries, and a connection to Google Vertex AI is required to bypass any legal restrictions\
Google also provides in its Vertex AI API a sample code to connect to Vertex AI in C#:
https://cloud.google.com/vertex-ai/generative-ai/docs/start/quickstarts/quickstart-multimodal

However, this code may not work in Unity due to licensing issues.\
The connection provided in this repo completely bypasses any licensing issues inside Unity to guarantee a connection

## Codes
- GeminiTestRequest.cs, GeminiTextResponse.cs AIHttpClients.cs are Gemini Driving codes
- HowToUseInCSharp.cs, HowToUseInUnity.cs are illustration codes

## Plugins
- You may need to add some plugins to your project. They are all in the Plugins folder
- In the case of Unity, go to your project, and drop the Plugins folder in the Asset folder
- If you already have a Plugins folder, merge the contents

## How to Pass In Different Kinds of Data

- Go to file **GeminiTextRequest.cs**, you may discover that **SendMsg** Can take as many strings as possible:
```cs
  public async Task<GeminiTextResponse> SendMsg(params string[] strings)
```
- Each string it takes is a kind of data encoded as string. But the first string must be a text prompt, and all other string can be string encoded data like base64string of an image, etc.
- The **Part** class has to specify this kind of data before you pass in.
      - For example, **string** can take string data, **InlineData** can take image on the device as a base64string, and **FileData** can take a path to a file on Google Cloud, as long as you provide the **mimeType** of the kind of data correctly, eg. "image/png"", ""image/jpeg", ...
      - ```cs
        public class Part
          {
             public string text { get; set; }
             public InlineData inlineData { get; set; }
             public FileData fileData { get; set; }
          }
        public class InlineData
          {
              public string mimeType { get; set; }
              public string data { get; set; } // Base64-encoded data
          }
        public class FileData
          {
              public string mimeType { get; set; }
              public string uri { get; set; } // path to files on Google Cloud
          }
        ```
- If you want to pass in other types of data, please consult [Part API](https://cloud.google.com/dotnet/docs/reference/Google.Cloud.AIPlatform.V1/latest/Google.Cloud.AIPlatform.V1.Part) for al lkind of data acceptable
- Then add a line to the Part class
  ```cs
  public YourData yourData {get; set; }
  ```
  Notice the second "your" is lower cased. The second name must follow camelCase
  
- Then create your own class of data:
  ```cs
  public class YourData
  {
        // your content
  }
  ```
- Finally, here is how to pass data to Gemini in GeminiTextRequest.cs. Remember, the first input must be text prompt
```cs
FullRequest fullRequest = new FullRequest();
List<Part> partsList = new List<Part>(); // Create a list to hold Part objects
Content content = new Content { role = "user"};

for (int i = 0; i < strings.Length; i++)
{
    Part part = new Part();

    // First input must be text prompt
    if (i == 0){
        part.text = strings[0];

        // Add the created Part object to the parts list
        partsList.Add(part);

    }
    else 
    {
       part.yourData = new YourData
            {
                // populate your content with string[i]
            };
        // Add the created Part object to the parts list
        partsList.Add(part);
    }
}
Part[] partsArray = partsList.ToArray();
content.parts = partsArray;
fullRequest.contents = content;
```

# Setup Requirements
- Create a Google Cloud account at https://cloud.google.com/vertex-ai?hl=en 
- Go to AIHttpClients.cs/MyRegion
- Change the PROJECT_ID to your project ID
- Modify MODEL_ID and QUERY based on your needs (please look at AIHttpClients.cs)

# Acknowledgements And Modifications
- This repo is extracted from my LLM-Robotics Project. The link is provided below:\
      https://github.com/david-wei-01001/LLM-Robotics
- The Network connection with Gemini is based on the following repo with significant changes:\
    https://github.com/jackcodewu/GeminiAI.Net

- Changes:
    1. The original repo will connect to Google Gemini, but Gemini is not available in many countries, and people have to connect with Google Vertex AI. This program rewrites the network connection part to auto-generate credentials and connect to Google Vertex AI
    2. The original repo only allows passing in one part, a text-based prompt. This program has rewritten the input logic so it can take as many parts as wanted and also able to take in images as input
    3. Please look at GeminiTestRequest.cs for details of how to pass in other forms of data (eg. files on the cloud, videos, functions, etc.)
    4. Provide all necessary Plugins to complete the connection
