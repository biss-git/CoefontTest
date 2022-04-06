using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoefontApi.v1
{
    /// <summary>
    /// Coefont にアクセスする用の Client
    /// AccessKey と ClientSecret を設定して利用してください。
    /// </summary>
    public class CoefontClient : IDisposable
    {
        private readonly HttpClient client = new HttpClient();

        public static readonly string contentType = "application/json";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="accessKey">アクセスキー</param>
        /// <param name="clientSecret">シークレットキー</param>
        public CoefontClient(string accessKey = null, string clientSecret = null)
        {
            this.AccessKey = accessKey;
            this.ClientSecret = clientSecret;
        }

        /// <summary>
        /// アクセスキー
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// シークレットキー
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Coefont API の URL
        /// </summary>
        public string BaseUrl { get; set; } = "https://api.coefont.cloud/v1";

        private async Task<string> GetString(string requestUrl)
        {
            ValidateKey();

            using var message = MakeRequestMessage(String.Empty, AccessKey, ClientSecret, HttpMethod.Get, requestUrl);
            using var response = await client.SendAsync(message);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            throw new HttpRequestException($"Status Code : {(int)response.StatusCode} {response.StatusCode}");
        }

        /// <summary>
        /// 辞書情報を取得する
        /// </summary>
        /// <returns>Json</returns>
        public async Task<string> GetDictString()
        {
            return await GetString($"{BaseUrl}/dict");
        }

        /// <summary>
        /// 辞書情報を取得する
        /// </summary>
        /// <returns>辞書情報</returns>
        public async Task<DictItem[]> GetDict()
        {
            var json = await GetDictString();
            return JsonSerializer.Deserialize<DictItem[]>(json);
        }

        /// <summary>
        /// 辞書を追加する
        /// </summary>
        /// <param name="dictItems">辞書情報</param>
        public async Task PostDict(DictItem[] dictItems)
        {
            ValidateKey();

            if (dictItems == null || dictItems.Length == 0)
            {
                throw new ArgumentNullException(nameof(dictItems));
            }

            foreach (var dict in dictItems)
            {
                var v = dict.PostValidation();
                if (v != null)
                {
                    throw new ArgumentException($"dictItems[{Array.IndexOf(dictItems, dict)}] : {v}");
                }
            }

            using var message = MakeRequestMessage(
                body_json: JsonSerializer.Serialize(dictItems),
                accessKey: AccessKey,
                clientSecret: ClientSecret,
                method: HttpMethod.Post,
                requestUrl: $"{BaseUrl}/dict");
            using var response = await client.SendAsync(message);
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            throw new HttpRequestException($"Status Code : {(int)response.StatusCode} {response.StatusCode}");
        }

        /// <summary>
        /// 辞書情報を削除する
        /// </summary>
        /// <param name="dictItems">辞書情報</param>
        public async Task DeleteDict(DictItem[] dictItems)
        {
            ValidateKey();

            if (dictItems == null || dictItems.Length == 0)
            {
                throw new ArgumentNullException(nameof(dictItems));
            }

            foreach (var dict in dictItems)
            {
                var v = dict.DeleteValidation();
                if (v != null)
                {
                    throw new ArgumentException($"dictItems[{Array.IndexOf(dictItems, dict)}] : {v}");
                }
            }

            using var message = MakeRequestMessage(
                body_json: JsonSerializer.Serialize(dictItems),
                accessKey: AccessKey,
                clientSecret: ClientSecret,
                method: HttpMethod.Delete,
                requestUrl: $"{BaseUrl}/dict");
            using var response = await client.SendAsync(message);
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            throw new HttpRequestException($"Status Code : {(int)response.StatusCode} {response.StatusCode}");
        }

        /// <summary>
        /// プロの音声一覧取得
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetProString()
        {
            return await GetString($"{BaseUrl}/coefonts/pro");
        }

        /// <summary>
        /// プロの音声一覧取得
        /// </summary>
        /// <returns></returns>
        public async Task<Coefonts> GetPro()
        {
            var json = await GetProString();
            return JsonSerializer.Deserialize<Coefonts>(json);
        }

        /// <summary>
        /// エンタープライズの音声一覧取得
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetEnterpriseString()
        {
            return await GetString($"{BaseUrl}/coefonts/enterprise");
        }

        /// <summary>
        /// エンタープライズの音声一覧取得
        /// </summary>
        /// <returns></returns>
        public async Task<Coefonts> GetEnterprise()
        {
            var json = await GetEnterpriseString();
            return JsonSerializer.Deserialize<Coefonts>(json);
        }

        public async Task<TextResponse> PostText2Speeach(Text text)
        {
            ValidateKey();

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            {
                var v = text.PostValidation();
                if (v != null)
                {
                    throw new ArgumentException($"{v}");
                }
            }

            var jsonText = JsonSerializer.Serialize(text);

            var t = JsonSerializer.Deserialize<Text>(jsonText);

            using var message = MakeRequestMessage(
                body_json: jsonText,
                accessKey: AccessKey,
                clientSecret: ClientSecret,
                method: HttpMethod.Post,
                requestUrl: $"{BaseUrl}/text2speech");
            using var response = await client.SendAsync(message);
            if (response.IsSuccessStatusCode)
            {
                var result = new TextResponse()
                {
                    RequestText = text,
                    RequestTextString = jsonText,
                    Url = response.RequestMessage.RequestUri.ToString(),
                    Wave = await response.Content.ReadAsByteArrayAsync(),
                    ExpirationDate = DateTime.Now.AddDays(7),
                };
                return result;
            }
            throw new HttpRequestException($"Status Code : {(int)response.StatusCode} {response.StatusCode}");
        }

        /// <summary>
        /// AccessKey と ClientSecret が null じゃないことを確認するだけ
        /// </summary>
        public void ValidateKey()
        {
            if (string.IsNullOrWhiteSpace(AccessKey))
            {
                throw new ArgumentNullException(nameof(AccessKey));
            }

            if (string.IsNullOrWhiteSpace(ClientSecret))
            {
                throw new ArgumentNullException(nameof(ClientSecret));
            }
        }

        public void Dispose()
        {
            client.Dispose();
        }

        /// <summary>
        /// Unix時間の文字列を取得
        /// </summary>
        /// <returns></returns>
        public static string GenerateUnixTime()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
        }

        /// <summary>
        /// Coefont ようにハッシュ化とかしてる
        /// </summary>
        /// <param name="body_json"></param>
        /// <param name="accessKey"></param>
        /// <param name="clientSecret"></param>
        /// <param name="method"></param>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public static HttpRequestMessage MakeRequestMessage(string body_json, string accessKey, string clientSecret, HttpMethod method = null, string requestUrl = null)
        {
            var unixTime = GenerateUnixTime();

            var signature = string.Empty;

            // シークレットキーのByte情報を取得
            var hmac_key = Encoding.UTF8.GetBytes(clientSecret);

            // UNIX時間(UTC) + 本文のJSONが、ハッシュ化対象となります。
            var hmac_body = Encoding.UTF8.GetBytes(unixTime + body_json);

            using (var hmac = new HMACSHA256(hmac_key))
            {
                //ハッシュ化したデータをHEX化します。                
                signature = BitConverter.ToString(hmac.ComputeHash(hmac_body, 0, hmac_body.Length));

                //HEXから「ー」を削除します。
                signature = signature.Replace("-", "").ToLower();
            }

            var request = new HttpRequestMessage(method, requestUrl)
            {
                Content = new StringContent(body_json, Encoding.UTF8, contentType)
            };
            request.Headers.Add("X-Coefont-Date", unixTime);
            request.Headers.Add("X-Coefont-Content", signature);
            request.Headers.Add("Authorization", accessKey);

            return request;
        }
    }
}
