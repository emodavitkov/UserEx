namespace UserEx.Services.Data.ReCaptcha
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    public class ReCaptchaService
    {
        private readonly IConfiguration config;

        public ReCaptchaService(IConfiguration config)
        {
            this.config = config;
        }

        public virtual async Task<ReCaptchaRespo> TokenVerify(string token)
        {
            var reCaptchaSecretKey = this.config["ReCaptcha:ApiKey"];

            ReCaptchaData data = new ReCaptchaData
            {
                Response = token,
                Secret = reCaptchaSecretKey,
            };

            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={data.Secret}&response={data.Response}");
            var reCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaRespo>(response);
            return reCaptchaResponse;
        }
    }

    public class ReCaptchaData
    {
        public string Response { get; set; }

        public string Secret { get; set; }
    }

    public class ReCaptchaRespo
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime Challenge_ts { get; set; }

        [JsonProperty("hostname ")]
        public string Hostname { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }
    }
}
