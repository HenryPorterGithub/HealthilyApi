using System.Net.Http.Headers;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json.Linq;

namespace HealthilyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthilyController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        private static readonly string APIKey = "5RPqHl6FfPaIvpH1xBqGJ8G1UeEdDw0P9dAd00Y5";
        private static readonly string APIToken = "KYvhiVBtr7ezhR3TycUkA6a5DKFy0mvz.9FIn85KBH9rtMeusujylFpTvfqY0vssk";
        private static readonly string Email = "henrymacarthurporter@gmail.com";
        private static readonly string Name = "Henry Porter";
        private static readonly string Id = "1";

        private static readonly string locale = "en-gb";
        private string accessToken { get; set; } = "";

        public HealthilyController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            HttpClient client = new HttpClient();
            string jsonPayload = $"{{\"id\":\"{Id}\",\"name\":\"{Name}\",\"email\":\"{Email}\",\"email_verified\":true}}";

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://portal.your.md/v4/login"),
                Headers =
            {
                { "accept", "*/*" },
                { "Authorization", APIToken },
                { "x-api-key", APIKey },
            },
                Content = new StringContent(jsonPayload)
                {
                    Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
                }
            };

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                JObject responseObject = JObject.Parse(responseContent);
                accessToken = responseObject["access_token"].ToString();
                StoreAccessToken(accessToken);
                return Ok();
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            string accessToken = GetAccessToken();

            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest("Access token is not available."); // Return bad request if access token is not available
            }

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://portal.your.md/v4/logout"),
                Headers =
            {
                { "accept", "*/*" },
                { "authorization", $"Bearer {accessToken}" },
            },
            };

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                accessToken = ""; // Clear access token upon successful logout
                return Ok(); // Return success
            }
            else
            {
                return StatusCode((int)response.StatusCode); // Return error status code
            }
        }

        public class SymptomCheckRequest
        {
            public string Type { get; set; } = "entry";
            public string Name { get; set; }
            public string Gender { get; set; }
            public int YearOfBirth { get; set; }
            public string InitialSymptom { get; set; }
            public bool Other { get; set; }
        }


        //#region First Query
        //var requestContent = new
        //{
        //    type = "entry",
        //    name = "Tim",
        //    gender = "male",
        //    year_of_birth = 1978,
        //    initial_symptom = "shaking and coughing",
        //    other = false
        //};

        //var payload = new
        //{
        //    answer = requestContent
        //};
        //#endregion

        //#region Second Query
        //var answerContent = new
        //{
        //    type = "generic",
        //    input = new
        //    {
        //        include = new[] { "assessment_C0010200", "clarify_CM001658" },
        //        exclude = new string[] { }
        //    }
        //};

        //// Construct the payload for the response
        // var payload2 = new
        //{
        //    answer = answerContent,
        //    conversation = new
        //    {
        //        id = "87c13a19-6c55-479d-a061-3dd9b2a5b6be" // Replace with actual conversation ID
        //    }
        //};
        //#endregion

        //#region Third Query
        //// Structure for responding to the question
        //var answerContent3 = new
        //{
        //    type = "generic",
        //    input = new
        //    {
        //        id = "continue_assessment" // Choose to continue with assessment
        //    }
        //};

        //// Construct the payload for the response
        //var payload3 = new
        //{
        //    answer = answerContent,
        //    conversation = new
        //    {
        //        id = "87c13a19-6c55-479d-a061-3dd9b2a5b6be" // Replace with the actual conversation ID
        //    }
        //};
        //#endregion

        //#region Fourth Query
        //// Structure for responding to the clarification question
        //var answerContent4 = new
        //{
        //    type = "generic",
        //    input = new
        //    {
        //        id = "A010102" // Choose the option that best describes the symptom (e.g., Tremor or trembling)
        //    }
        //};

        //// Construct the payload for the response
        //var payload4 = new
        //{
        //    answer = answerContent,
        //    conversation = new
        //    {
        //        id = "87c13a19-6c55-479d-a061-3dd9b2a5b6be" // Replace with the actual conversation ID
        //    }
        //};
        //#endregion


        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] JObject requestBody)
        {
            try
            {
                string accessToken = GetAccessToken();

                // Deserialize the JSON object to extract necessary information
                dynamic requestData = requestBody;

                // Extract the message content from the request
                string messageContent = requestData.message;
                string chatId = requestData.conversation.id;

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://portal.your.md/v4/chat"),
                    Headers =
            {
                { "accept", "*/*" },
                { "authorization", $"Bearer {accessToken}" },
                { "x-api-key", APIKey },
            },
                    Content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json")
                };

                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return Ok(responseBody); // Return success
                }
                else
                {
                    return StatusCode((int)response.StatusCode); // Return error status code
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions
                Console.WriteLine(ex);
                return StatusCode(500); // Return internal server error status code
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string text)
        {
            string accessToken = GetAccessToken();

            HttpClient client = new HttpClient();
            string jsonPayload = $"{{\"text\":\"{text}\"}}";

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://portal.your.md/v4/search/symptoms"),
                Headers =
                {
                    { "accept", "*/*" },
                    { "authorization", $"Bearer {accessToken}" },
                    { "x-api-key", APIKey },
                },
                Content = new StringContent(jsonPayload)
                {
                    Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                }
            };

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                accessToken = ""; // Clear access token upon successful logout
                return Ok(); // Return success
            }
            else
            {
                return StatusCode((int)response.StatusCode); // Return error status code
            }
        }

        [HttpGet("content")]
        public async Task<IActionResult> Content(string skip, string limit)
        {
            string accessToken = GetAccessToken();

            HttpClient client = new HttpClient();
            string jsonPayload = $"{{\"locale\":\"{locale}\",\"skip\":\"{skip}\",\"limit\":\"{limit}\"}}";

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://portal.your.md/v4/content/articles"),
                Headers =
                {
                    { "accept", "*/*" },
                    { "authorization", $"Bearer {accessToken}" },
                    { "x-api-key", APIKey },
                },
                Content = new StringContent(jsonPayload)
                {
                    Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                }
            };

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                accessToken = ""; // Clear access token upon successful logout
                return Ok(); // Return success
            }
            else
            {
                return StatusCode((int)response.StatusCode); // Return error status code
            }
        }

        #region helpers
        private void StoreAccessToken(string accessToken)
        {
            _memoryCache.Set("AccessToken", accessToken, TimeSpan.FromMinutes(30)); // Cache the token for 30 minutes
        }

        private string GetAccessToken()
        {
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                return accessToken;
            }
            return null;
        }
        #endregion
    }
}
