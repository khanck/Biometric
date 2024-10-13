using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Integration.Config;
using TCC.Payment.Integration.Interfaces;
using TCC.Payment.Integration.Models;
using TCC.Payment.Integration.Models.Innovatrics;
using ILogger = Serilog.ILogger;

namespace TCC.Payment.Integration.Biometric
{
    public class InnovatricsAbis : IInnovatricsAbis, IDisposable
    {

        private readonly InnovatricsConfiguration _innovatricsConfiguration;
        private readonly ILogger _logger;        
        public InnovatricsAbis(IOptions<InnovatricsConfiguration> configuration, ILogger logger)
        {
            _innovatricsConfiguration = configuration.Value;
            _logger = logger;


        }
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public async Task<AbisResponse> EnrollPerson(AbisEnrollUser person)
        {

            AbisResponse? result = new AbisResponse();

            Task.Run(async () =>
            {
                try
                {
                    string uri = String.Format(_innovatricsConfiguration.Endpoint + "applicants/{0}/enroll", person.externalId);

                    // Bypass  SSL error 
                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };                   
                    var client = new HttpClient(clientHandler);
                    ///END 

                    //HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //client.DefaultRequestHeaders.Authorization = GetAuthenticationHeader(Configurations);
                    client.Timeout = TimeSpan.FromSeconds(_innovatricsConfiguration.ApiReqTimeout);

                    //HttpClient client = new Authentication().GetHttpClient(_Configurations);

                    //AuthorizeDto.action = _Configurations.Action;
                    string inputJson = JsonConvert.SerializeObject(person);
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");                  

                    using (HttpResponseMessage response = await client.PutAsync(uri, inputContent))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            //var str = response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<AbisResponse>(await response.Content.ReadAsStringAsync());
                            result.IsSuccess=true;
                            _logger.Information(" InnovatricsAbis EnrollPerson: Success   ID :{0}"/*, result.transId*/);
                        }
                        else
                        {
                            //result.ResultCode = (int)response.StatusCode;
                            var errors = JsonConvert.DeserializeObject<AbisError>(await response.Content.ReadAsStringAsync());

                            _logger.Error(" InnovatricsAbis EnrollPerson: Error :  ID:{0} {1}"/*, result.transId, JsonConvert.SerializeObject(result)*/);

                        }
                    }

                }
                catch (Exception ex)
                {
                    //result.ResultCode = 500;
                    //result.message = ex.Message;

                    _logger.Error("InnovatricsAbis EnrollPerson :Internal error {0}", ex);
                }

            }).Wait();
            return result;


        }


        public async Task<List<AbisResponse>> IdentifyByFace(Identification request)
        {

            List<AbisResponse> ? result = new List<AbisResponse>();

            Task.Run(async () =>
            {
                try
                {
                    string uri = String.Format(_innovatricsConfiguration.Endpoint + "identify/images");

                    // Bypass  SSL error 
                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                    var client = new HttpClient(clientHandler);
                    ///END 

                    //HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //client.DefaultRequestHeaders.Authorization = GetAuthenticationHeader(Configurations);
                    client.Timeout = TimeSpan.FromSeconds(_innovatricsConfiguration.ApiReqTimeout);

                    //HttpClient client = new Authentication().GetHttpClient(_Configurations);

                    //AuthorizeDto.action = _Configurations.Action;
                    request.identificationParameters.threshold=_innovatricsConfiguration.Threshold;

                    string inputJson = JsonConvert.SerializeObject(request);
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await client.PostAsync(uri, inputContent))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var str = response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<List<AbisResponse>>(await response.Content.ReadAsStringAsync());
                            _logger.Information(" InnovatricsAbis IdentifyByFace: Success   ID :{0}"/*, result.transId*/);
                        }
                        else
                        {
                            //result.ResultCode = (int)response.StatusCode;
                            var error = JsonConvert.DeserializeObject<List<AbisError>>(await response.Content.ReadAsStringAsync());

                            _logger.Error(" InnovatricsAbis IdentifyByFace: Error :  ID:{0} {1}"/*, result.transId, JsonConvert.SerializeObject(result)*/);

                        }
                    }

                }
                catch (Exception ex)
                {
                    //result.ResultCode = 500;
                    //result.message = ex.Message;

                    _logger.Error("InnovatricsAbis IdentifyByFace :Internal error {0}", ex);
                }

            }).Wait();
            return result;


        }

        //hard delete
        public async Task<List<AbisResponse>> DeletePerson(Guid externalId)  
        {

            List<AbisResponse>? result = new List<AbisResponse>();

            Task.Run(async () =>
            {
                try
                {
                    string uri = String.Format(_innovatricsConfiguration.Endpoint + "applicants/{0}",externalId);

                    // Bypass  SSL error 
                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                    var client = new HttpClient(clientHandler);
                    ///END 

                    //HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //client.DefaultRequestHeaders.Authorization = GetAuthenticationHeader(Configurations);
                    client.Timeout = TimeSpan.FromSeconds(_innovatricsConfiguration.ApiReqTimeout);

                    //HttpClient client = new Authentication().GetHttpClient(_Configurations);

                    //AuthorizeDto.action = _Configurations.Action;
                    
                    using (HttpResponseMessage response = await client.DeleteAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var str = response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<List<AbisResponse>>(await response.Content.ReadAsStringAsync());
                            _logger.Information(" InnovatricsAbis DeletePerson: Success   ID :{0}"/*, result.transId*/);
                        }
                        else
                        {
                            //result.ResultCode = (int)response.StatusCode;
                            var error = JsonConvert.DeserializeObject<List<AbisError>>(await response.Content.ReadAsStringAsync());

                            _logger.Error(" InnovatricsAbis DeletePerson: Error :  ID:{0} {1}"/*, result.transId, JsonConvert.SerializeObject(result)*/);

                        }
                    }

                }
                catch (Exception ex)
                {
                    //result.ResultCode = 500;
                    //result.message = ex.Message;

                    _logger.Error("InnovatricsAbis DeletePerson :Internal error {0}", ex);
                }

            }).Wait();
            return result;


        }
    }
}
