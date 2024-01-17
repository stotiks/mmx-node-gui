using Mmx.Gui.Win.Common.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common.Node
{
    public static class NodeApi
    {

        public static readonly Uri baseUri = new Uri("http://127.0.0.1:11380");
        public static readonly Uri guiUri = new Uri(baseUri, "/gui/");

        public static readonly Uri apiUri = new Uri(baseUri, "/api/");
        public static readonly Uri wapiUri = new Uri(baseUri, "/wapi/");

        public static readonly Uri nodeExitUri = new Uri(baseUri, "/wapi/node/exit");

        public static readonly Uri harvesterRemPlotDirUri = new Uri(baseUri, "/api/harvester/rem_plot_dir");
        public static readonly Uri harvesterAddPlotDirUri = new Uri(baseUri, "/api/harvester/add_plot_dir");
        public static readonly Uri harvesterReloadUri = new Uri(baseUri, "api/harvester/reload");

        public static readonly Uri dummyUri = new Uri(baseUri, "/dummyUri/");
        public static readonly string XApiTokenName = "x-api-token";

        private static readonly Uri sessionUri = new Uri(baseUri, "/server/session");


        private static readonly HttpClient httpClient = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        private static readonly string logoutJS = GetResource("logout.js");
        private static readonly string waitStartJS = GetResource("waitStart.js");

        private static readonly string loadingHtmlTemplate = GetResource("loading.html");

        private static readonly string jsString = "//javascript";

        private static string _xApiToken;
        public static string loadingHtml => loadingHtmlTemplate;
        public static string logoutHtml => loadingHtml.Replace(jsString, logoutJS);

        public static string waitStartHtml => loadingHtml.Replace(jsString, waitStartJS);

        static NodeApi()
        {
            XApiToken = GetRandomHexNumber(64);
        }

        public static bool IsRunning
        {
            get
            {
                var task = Task.Run(CheckRunning);
                task.Wait();
                return task.Result;
            }
        }

        public static string XApiToken
        {
            get => _xApiToken;

            private set
            {
                _xApiToken = value;
                OnXApiTokenChanged();
            }
        }

        private static void OnXApiTokenChanged()
        {
            httpClient.DefaultRequestHeaders.Remove(XApiTokenName);
            httpClient.DefaultRequestHeaders.Add(XApiTokenName, _xApiToken);
        }

        public static async Task AddPlotDirTask(string dirName)
        {
            dynamic data = new JObject();
            data.path = dirName;

            var myContent = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            _ = await httpClient.PostAsync(harvesterAddPlotDirUri, byteContent);
            _ = await ReloadHarvester();
        }

        public static Task<HttpResponseMessage> ReloadHarvester()
        {
            return httpClient.GetAsync(harvesterReloadUri);
        }

        public static async Task RemovePlotDirTask(string dirName)
        {
            dynamic data = new JObject();
            data.path = dirName;

            var myContent = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            _ = await httpClient.PostAsync(harvesterRemPlotDirUri, byteContent);
            _ = await ReloadHarvester();
        }

        private static async Task<bool> CheckRunning()
        {
            try
            {
                _ = await httpClient.GetAsync(sessionUri);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Node is not running");
            }

            return false;
        }

        private static string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            Random random = new Random();
            random.NextBytes(buffer);
            string result = string.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }

        public static void InitXToken()
        {
            string json = "{}";
            try
            {
                json = File.ReadAllText(NodeHelpers.httpServerConfigPath);
            }
            catch
            {
                Console.WriteLine(@"config not found");
            }

            JObject httpServerConfig = JsonConvert.DeserializeObject<JObject>(json);
            var tokenMap = httpServerConfig["token_map"] as JObject;
            JProperty firstAdminToken = null;

            if (tokenMap != null)
            {
                foreach (JProperty prop in tokenMap.Properties())
                {
                    if (prop.Value.ToString() == "ADMIN")
                    {
                        firstAdminToken = prop;
                        break;
                    }
                }
            }

            if (IsRunning)
            {
                if (firstAdminToken != null)
                {
                    XApiToken = firstAdminToken.Name;
                }
                else
                {
                    //throw new Exception("Can not get XApiToken");
                }
            }
            else
            {
                var property = new JProperty(XApiToken, "ADMIN");

                if (firstAdminToken == null)
                {
                    if (tokenMap == null)
                    {
                        var jObject = new JObject { property };
                        httpServerConfig["token_map"] = jObject;
                    }
                    else
                    {
                        tokenMap.Add(property);
                    }

                }
                else
                {
                    firstAdminToken.Replace(property);
                }

                json = JsonConvert.SerializeObject(httpServerConfig, Formatting.Indented);
                File.WriteAllText(NodeHelpers.httpServerConfigPath, json);
            }
        }

        private static string GetResource(string resName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(resName));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        internal static Task<HttpResponseMessage> Exit()
        {
            return httpClient.GetAsync(nodeExitUri);
        }
    }
}