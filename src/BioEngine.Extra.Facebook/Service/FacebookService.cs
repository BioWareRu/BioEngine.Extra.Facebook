using System;
using System.Threading.Tasks;
using Flurl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Flurl.Http;

namespace BioEngine.Extra.Facebook.Service
{
    public class FacebookService
    {
        private readonly ILogger<FacebookService> _logger;

        public FacebookService(ILogger<FacebookService> logger)
        {
            _logger = logger;
        }

        public async Task<string> PostLink(Uri link, FacebookServiceConfiguration config)
        {
            _logger.LogDebug("Post new link to facebook");
            var response =
                await $"{config.ApiURL}/{config.PageId}/feed"
                    .SetQueryParam("link", link.ToString())
                    .SetQueryParam("access_token", config.AccessToken)
                    .WithTimeout(60)
                    .PostUrlEncodedAsync(new { });
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error while sending facebook request");
                throw new Exception($"Bad facebook response: {data}");
            }

            var postReponse = JsonConvert.DeserializeObject<FacebookNewPostResponse>(data);
            _logger.LogDebug("Link posted to facebook");
            return postReponse.Id;
        }

        public async Task<bool> DeletePost(string postId, FacebookServiceConfiguration config)
        {
            _logger.LogDebug("Delete post from facebook");
            var response =
                await $"{config.ApiURL}/{postId}"
                    .SetQueryParam("access_token", config.AccessToken)
                    .DeleteAsync();
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error while sending facebook request");
                throw new Exception($"Bad facebook response: {data}");
            }

            var postReponse = JsonConvert.DeserializeObject<FacebookDeleteResponse>(data);
            _logger.LogDebug($"Post deleted from facebook: {(postReponse.Success ? "Yes" : "No")}");
            return postReponse.Success;
        }

        public class FacebookNewPostResponse
        {
            [JsonProperty("id")] public string Id { get; set; }
        }

        public class FacebookDeleteResponse
        {
            [JsonProperty("success")] public bool Success { get; set; }
        }
    }
}