# CSharp-Unity-to-Google-Vertex-AI
Provide code to connect form C# to Google Vertex AI that can be run within Unity

# Descriptions
Google Gemini is not available in some countries, and a connection to Google Vertex AI is required to bypass any legal restrictions\
Google also provides in its Vertex AI API a sample code to connect to Vertex AI in C#:
https://cloud.google.com/vertex-ai/generative-ai/docs/start/quickstarts/quickstart-multimodal

However, this code may not work in Unity due to licensing issues.\
The connection provided in this repo completely bypasses any licensing issues inside Unity to guarantee a connection

## Code
- GeminiTestRequest.cs, GeminiTextResponse.cs AIHttpClients.cs are Gemini Driving code
- HowToUseInCSharp.cs, HowToUseInUnity.cs are illustration code

# Setup Requirements
- Create a Google Cloud account
- Go to AIHttpClients.cs/MyRegion
- Change the PROJECT_ID to your project ID
- Modify MODEL_ID and QUERY based on your needs (please look at AIHttpClients.cs)

# Acknowledgements And Modifications
- This repo is extracted from my LLM-Robotics Project. The link is provided below:
- The Network connection with Gemini is based on the following repo with significant changes:
    https://github.com/jackcodewu/GeminiAI.Net

- Changes:
    1. The repo will connect to Google Gemini, but Gemini is not available in many countries, and people have to connect with Google Vertex AI. We rewrite the network connection port to auto-generate credentials and connect to Google Vertex AI
    2. The repo only allows passing in one part, a text-based prompt. We have rewritten the input logic so it can take as many parts as wanted and also able to take in images as input
    3. Please look at GeminiTestRequest.cs for details of how to pass in other forms of data (eg. files on the cloud, videos, functions, etc.)
