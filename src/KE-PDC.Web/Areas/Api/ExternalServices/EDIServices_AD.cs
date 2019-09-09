using KE_PDC.ViewModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KE_PDC.Services
{
    public class EDIServices_AD
    {
    }
    public interface IEDIServicesAD
    {
        string LoginAD(string User, string password);
        
    }

    public class WebApi : IEDIServicesAD
    {

        IConfiguration _configuration;

        public WebApi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string LoginAD(string User, string Password)
        {
            string s = @"fe\k83SCgK8(2v$W";
            string m = _configuration.GetConnectionString(Areas.Api.ExternalServices.Utils.StaticName.SSOMethod);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_configuration.GetConnectionString(Areas.Api.ExternalServices.Utils.StaticName.apiUrl));
            client.DefaultRequestHeaders.Add("app_id", "PDC");
            client.DefaultRequestHeaders.Add("app_key", s);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            ReqAd LoginAd = new ReqAd();
            LoginAd.User = User;
            LoginAd.Password = Password;

             var myContent = JsonConvert.SerializeObject(LoginAd);
            var stringContent = new StringContent(myContent, Encoding.UTF8, "application/json");
            // List data response.
            HttpResponseMessage response = client.PostAsync(_configuration.GetConnectionString(Areas.Api.ExternalServices.Utils.StaticName.SSOMethod), stringContent).Result;
            // List data response.
            string responseString = response.Content.ReadAsStringAsync().Result;
            return (responseString);


        }
    }
}
