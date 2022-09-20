using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static GitTest.RepoList;

namespace GitTest
{
    public class Test
    {
        string token = ""; //임시 삭제..
        string today = DateTime.Now.ToString(@"dd'/'MM'/'yyyy");
        private bool SetRepositoryList(string json)
        {
            var jsonConvert = JsonConvert.DeserializeObject<List<Root>>(json);
            // 리스트로 던져야함

            var httpWebRequest = WebRequest.CreateHttp("https://api.github.com/repos/"+jsonConvert[0].full_name+"/commits");
            httpWebRequest.Headers.Add("Authorization","Token:" + token);
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.97 Safari/537.36";
            var httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
            var stream = httpWebResponse.GetResponseStream();
            var streamReader = new StreamReader(stream);
            var readToEnd = streamReader.ReadToEnd();
            stream.Close();
            httpWebResponse.Close();

            var result = JsonConvert.DeserializeObject<List<Root2>>(readToEnd);
            var toString = result.First<Root2>().commit.author.date.ToString();
            if (today.Equals(toString.Substring(0,10).Trim())) 
            {
                return true;
            }
            return false;
        }
        private string GetRepositoryList()
        {
            
            HttpWebRequest httpWebRequest = WebRequest.CreateHttp("https://api.github.com/users/" + "kklove1015" + "/repos");
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.97 Safari/537.36";
            httpWebRequest.Headers.Add("Authorization", "Token:" + token);

            var httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
            var stream = httpWebResponse.GetResponseStream();
            var streamReader = new StreamReader(stream);
            var readToEnd = streamReader.ReadToEnd();
            stream.Close();
            httpWebResponse.Close();
            return readToEnd;
            //return JsonConvert.DeserializeObject(readToEnd);
        }
        public void OnTest()
        {
            var getRepositoryList = GetRepositoryList();
            var setRepositoryList = SetRepositoryList(getRepositoryList);
            if (!setRepositoryList) 
            {
                Console.WriteLine("커밋안됨");
            }
        }
    }
}
