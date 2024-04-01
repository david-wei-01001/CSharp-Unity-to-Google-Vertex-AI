using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Google.Apis.Auth.OAuth2;
using System.Net.Http.Headers;

namespace Gemini
{

    public class AIHttpClient
    {
        #region MyRegion
        protected static HttpClient httpClient;
        protected static string PROJECT_ID = "Your Project ID";

        // for other types of model, please look at:
        // https://cloud.google.com/vertex-ai/generative-ai/docs/learn/models
        protected static string MODEL_ID = "gemini-1.0-pro-vision";
        protected static string QUERY = "generateContent";
        #endregion

        static AIHttpClient()
        {
            httpClient = new HttpClient();
        }

        /** 
        <summary>
        generate credential tokens to connect to Google Cloud Vertex AI
        </summary>
        */
        private async Task<string> GetAccessTokenAsync()
        {
            GoogleCredential credential = await GoogleCredential.GetApplicationDefaultAsync();
            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(new[] { "https://www.googleapis.com/auth/cloud-platform" });
            }
            var token = await ((ITokenAccess)credential).GetAccessTokenForRequestAsync();
            return token;
        }

        /** 
        <summary>
        Send query to Google Cloud Vertex AI via network and save the reply
        </summary>
        <param name="data"> The data to send. </param>
        <param name="url"> The url of the specific LLM model to communicate with. </param>
        */
        public async Task<ResponseData> PostAsync<RequestData, ResponseData>(RequestData data, string url) where RequestData : class, new() where ResponseData : class, new()
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                string accessToken = await GetAccessTokenAsync();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                string responseBody = string.Empty;


                responseBody = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(responseBody))
                {
                    ResponseData responseData = JsonConvert.DeserializeObject<ResponseData>(responseBody);

                    return SetResponse(responseData ?? null, true, "");
                }

                return SetResponse<ResponseData>(null, false, "网络请求失败！");

            }
            catch (Exception ex)
            {
                return SetResponse<ResponseData>(null, false, ex.Message);
            }
        }
        
        /** 
        <summary>
        Record reply sent over Network
        </summary>
        */
        private ResponseData SetResponse<ResponseData>(ResponseData responseData, bool success, string errormsg) where ResponseData : class, new()
        {
            if (responseData == null)
            {
                responseData = new ResponseData();
                success = false;
            }

            var properties = responseData.GetType().GetProperties();

            foreach (var item in properties)
            {
                if (!item.CanRead || !item.CanWrite) continue;

                if (item.Name == "Success")
                {
                    item.SetValue(responseData, success);
                }
                if (item.Name == "ErrorMsg")
                {
                    item.SetValue(responseData, errormsg);
                }
            }

            return responseData;
        }

    }
}
