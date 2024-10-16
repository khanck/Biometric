﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILogger = Serilog.ILogger;
using TCC.Payment.Integration.Config;
using TCC.Payment.Integration.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web;
using TCC.Payment.Integration.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Unicode;


namespace TCC.Payment.Integration.Biometric
{
    public class AlpetaServer: IAlpetaServer, IDisposable
    {
        private readonly AlpetaConfiguration _alpetaConfiguration;
        private readonly ILogger _logger;
        private IEnumerable<string> cookies=new List<string>();
        public AlpetaServer(IOptions<AlpetaConfiguration> alpetaConfiguration, ILogger logger)
        {
            _alpetaConfiguration = alpetaConfiguration.Value;
            _logger = logger;

            ////Login();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public async Task<AlpetaConfiguration> Login()
        {

            AlpetaConfiguration? result = new AlpetaConfiguration();

            Task.Run(async () =>
            {
                try
                {            
                    string uri = String.Format(_alpetaConfiguration.Endpoint + "{0}", "login");

                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //client.DefaultRequestHeaders.Authorization = GetAuthenticationHeader(Configurations);
                    client.Timeout = TimeSpan.FromSeconds(_alpetaConfiguration.ApiReqTimeout);

                    //HttpClient client = new Authentication().GetHttpClient(_Configurations);

                    //AuthorizeDto.action = _Configurations.Action;
                    string inputJson = JsonConvert.SerializeObject(new User());
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                  
                    //CookieOptions langCookie = new CookieOptions();
                    //langCookie.Expires = DateTime.Now.AddYears(1);

                    using (HttpResponseMessage response = await client.PostAsync(uri, inputContent))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var str=response.Content.ReadAsStringAsync();
                            cookies = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;  

                            _logger.Information(" AlpetaServer Login: Success   ID :{0}"/*, result.transId*/);
                        }
                        else
                        {
                            //result.ResultCode = (int)response.StatusCode;
                            //result.message = (await response.Content.ReadAsStringAsync());

                            _logger.Error(" AlpetaServer Login: Error :  ID:{0} {1}"/*, result.transId, JsonConvert.SerializeObject(result)*/);

                        }
                    }

                }
                catch (Exception ex)
                {
                    //result.ResultCode = 500;
                    //result.message = ex.Message;

                    _logger.Error("AlpetaServer Login :Internal error {0}", ex);
                }

            }).Wait();
            return result;


        }



        public async Task<BiometricAuthentication> VerifyUserBiometric(string userId)
        {

            BiometricAuthentication? result = new BiometricAuthentication();

            Task.Run(async () =>
            {
                try
                {
                    string uri = String.Format(_alpetaConfiguration.Endpoint + "authLogs?startTime={0}&endTime={1}&searchCategory=user_id&searchKeyword={2}&offset=0&limit=1", DateTime.Now.AddSeconds(-10).ToString("yyyy-MM-dd"), DateTime.Now.Date.ToString("yyyy-MM-dd"), userId);

                    //string uri = String.Format(_alpetaConfiguration.Endpoint + "authLogs?startTime={0}&endTime={1}&searchCategory=user_id&searchKeyword={2}&offset=0&limit=1", DateTime.Now.AddSeconds(-10).ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss"), userId);
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //client.DefaultRequestHeaders.Authorization = GetAuthenticationHeader(Configurations);
                    client.Timeout = TimeSpan.FromSeconds(_alpetaConfiguration.ApiReqTimeout);

                    //HttpClient client = new Authentication().GetHttpClient(_Configurations);
                    client.DefaultRequestHeaders.Add("Cookie", cookies);

                    //AuthorizeDto.action = _Configurations.Action;
                    string inputJson = JsonConvert.SerializeObject(_alpetaConfiguration);
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");


                    using (HttpResponseMessage response = await client.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var str = response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<BiometricAuthentication>(await response.Content.ReadAsStringAsync());                      
                           

                            _logger.Information("AlpetaServer VerifyUserBiometric: Success   ID :{0}"/*, result.transId*/);
                        }
                        else
                        {
                            //result.ResultCode = (int)response.StatusCode;
                            //result.message = (await response.Content.ReadAsStringAsync());

                            _logger.Error(" Error in AlpetaServer VerifyUserBiometric:  ID:{0} {1}"/*, result.transId, JsonConvert.SerializeObject(result)*/);

                        }
                    }

                }
                catch (Exception ex)
                {
                    //result.ResultCode = 500;
                    //result.message = ex.Message;

                    _logger.Error("AlpetaServer VerifyUserBiometric:Internal error {0}", ex);
                }

            }).Wait();
            return result;


        }
        public async Task<BiometricAuthentication> GetCurrentUserBiometric()
        {

            BiometricAuthentication? result = new BiometricAuthentication();

            Task.Run(async () =>
            {
                try
                {
                    string uri = String.Format(_alpetaConfiguration.Endpoint + "authLogs?startTime={0}&endTime={1}&offset=0&limit=1", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));

                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //client.DefaultRequestHeaders.Authorization = GetAuthenticationHeader(Configurations);
                    client.Timeout = TimeSpan.FromSeconds(_alpetaConfiguration.ApiReqTimeout);

                    //HttpClient client = new Authentication().GetHttpClient(_Configurations);
                    client.DefaultRequestHeaders.Add("Cookie", cookies);

                    //AuthorizeDto.action = _Configurations.Action;
                    string inputJson = JsonConvert.SerializeObject(_alpetaConfiguration);
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");


                    using (HttpResponseMessage response = await client.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var str = response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<BiometricAuthentication>(await response.Content.ReadAsStringAsync());
                           
                            if(result.AuthLogList!=null)
                            result.AuthLogList= result.AuthLogList.FindAll(o=>(o.EventTime>=DateTime.Now.AddSeconds(-_alpetaConfiguration.VerificationTimeout) && o.EventTime <= DateTime.Now) &o.AuthResult==0 );  //for live success Biometric Verification  VerificationTimeout  will be in seconnds 

                            _logger.Information(" Authorize: Success   ID :{0}"/*, result.transId*/);
                        }
                        else
                        {
                            //result.ResultCode = (int)response.StatusCode;
                            //result.message = (await response.Content.ReadAsStringAsync());

                            _logger.Error(" Error in AlpetaServer GetCurrentUserBiometric:  ID:{0} {1}"/*, result.transId, JsonConvert.SerializeObject(result)*/);

                        }
                    }

                }
                catch (Exception ex)
                {
                    //result.ResultCode = 500;
                    //result.message = ex.Message;

                    _logger.Error("AlpetaServer GetCurrentUserBiometric:Internal error {0}", ex);
                }

            }).Wait();
            return result;


        }
        public async Task<BiometricAuthentication> GetCurrentUserBiometric(string userID)
        {

            BiometricAuthentication? result = new BiometricAuthentication();

            Task.Run(async () =>
            {
                try
                {
                    string uri = String.Format(_alpetaConfiguration.Endpoint + "authLogs?startTime={0}&endTime={1}&searchCategory=user_id&searchKeyword={2}&offset=0&limit=1", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"),userID);

                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //client.DefaultRequestHeaders.Authorization = GetAuthenticationHeader(Configurations);
                    client.Timeout = TimeSpan.FromSeconds(_alpetaConfiguration.ApiReqTimeout);

                    //HttpClient client = new Authentication().GetHttpClient(_Configurations);
                    client.DefaultRequestHeaders.Add("Cookie", cookies);

                    //AuthorizeDto.action = _Configurations.Action;
                    string inputJson = JsonConvert.SerializeObject(_alpetaConfiguration);
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");


                    using (HttpResponseMessage response = await client.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var str = response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<BiometricAuthentication>(await response.Content.ReadAsStringAsync());

                            if (result.AuthLogList != null)
                                result.AuthLogList = result.AuthLogList.FindAll(o => (o.EventTime >= DateTime.Now.AddSeconds(-_alpetaConfiguration.VerificationTimeout) && o.EventTime <= DateTime.Now) & o.AuthResult == 0 );  //for live success Biometric Verification  VerificationTimeout  will be in seconnds 

                            _logger.Information(" Authorize: Success   ID :{0}"/*, result.transId*/);
                        }
                        else
                        {
                            //result.ResultCode = (int)response.StatusCode;
                            //result.message = (await response.Content.ReadAsStringAsync());

                            _logger.Error(" Error in GetCurrentUserBiometric:  ID:{0} {1}"/*, result.transId, JsonConvert.SerializeObject(result)*/);

                        }
                    }

                }
                catch (Exception ex)
                {
                    //result.ResultCode = 500;
                    //result.message = ex.Message;

                    _logger.Error("AlpetaServer GetCurrentUserBiometric:Internal error {0}", ex);
                }

            }).Wait();
            return result;


        }

        public async Task<CreateUserResult> CreateUser(CreateUserRequestDTO userId)
        {

            CreateUserResult? result = new CreateUserResult();


            try
            {
                string uri = String.Format(_alpetaConfiguration.Endpoint + "users?UserID={0}", userId.UserInfo.ID);

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                var body = JsonConvert.SerializeObject(userId);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //client.DefaultRequestHeaders.Authorization = GetAuthenticationHeader(Configurations);
                client.Timeout = TimeSpan.FromSeconds(_alpetaConfiguration.ApiReqTimeout);

                //HttpClient client = new Authentication().GetHttpClient(_Configurations);
                client.DefaultRequestHeaders.Add("Cookie", cookies);

                Console.WriteLine(content);
                using (HttpResponseMessage response = await client.PostAsync(uri, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        result = JsonConvert.DeserializeObject<CreateUserResult>(await response.Content.ReadAsStringAsync());

                        _logger.Information(" Create User in apleta server : response :{0}", result);
                    }
                    else
                    {
                        _logger.Error(" Error in AlpetaServer CreateUser:  ID:{0} {1}"/*, result.transId, JsonConvert.SerializeObject(result)*/);

                    }
                }

            }
            catch (Exception ex)
            {
                //result.ResultCode = 500;
                //result.message = ex.Message;

                _logger.Error("AlpetaServer CreateUser:Internal error {0}", ex);
            }


            return result;


        }

        public async Task<AlpetaConfiguration> UpdateUserPicture(UpdateUserPictureReqDTO updateUserPictureReqDTO)
        {
            AlpetaConfiguration? result = new AlpetaConfiguration();


            try
            {
                string uri = String.Format(_alpetaConfiguration.Endpoint + "users/{0}/picture", updateUserPictureReqDTO.UserId);

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                var body = JsonConvert.SerializeObject(new { ImageType = updateUserPictureReqDTO.ImageType, Picture = updateUserPictureReqDTO.Picture});
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                client.Timeout = TimeSpan.FromSeconds(_alpetaConfiguration.ApiReqTimeout);

                //HttpClient client = new Authentication().GetHttpClient(_Configurations);
                client.DefaultRequestHeaders.Add("Cookie", cookies);

                Console.WriteLine(content);
                using (HttpResponseMessage response = await client.PutAsync(uri, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var str = response.Content.ReadAsStringAsync();
                        _logger.Information(" UpdateUserPicture :   response :{0}", str);
                    }
                    else
                    {
                        _logger.Error(" Error in AlpetaServer UpdateUserPicture:  ID:{0} {1}"/*, result.transId, JsonConvert.SerializeObject(result)*/);

                    }
                }

            }
            catch (Exception ex)
            {
                //result.ResultCode = 500;
                //result.message = ex.Message;

                _logger.Error("AlpetaServer UpdateUserPicture:Internal error {0}", ex);
            }


            return result;
        }

        public async Task<Response> SaveUserToTerminal(SaveUserToTerminalDto saveUserToTerminalDto)
        {
            Response? result = new Response();


            try
            {
                string uri = String.Format(_alpetaConfiguration.Endpoint + "terminals/{0}/users/{1}", saveUserToTerminalDto.TerminalId, saveUserToTerminalDto.UserId);

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                var body = JsonConvert.SerializeObject(new { saveUserToTerminalDto .DownloadInfo});
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                client.Timeout = TimeSpan.FromSeconds(_alpetaConfiguration.ApiReqTimeout);

                //HttpClient client = new Authentication().GetHttpClient(_Configurations);
                client.DefaultRequestHeaders.Add("Cookie", cookies);

                Console.WriteLine(content);
                using (HttpResponseMessage response = await client.PostAsync(uri, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        result = JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync());
                        _logger.Information(" SaveUserToTerminal:   response :{0}", result);
                    }
                    else
                    {
                        _logger.Error(" Error in SaveUserToTerminal:  ID:{0} {1}"/*, result.transId, JsonConvert.SerializeObject(result)*/);

                    }
                }

            }
            catch (Exception ex)
            {
                //result.ResultCode = 500;
                //result.message = ex.Message;

                _logger.Error("AlpetaServer SaveUserToTerminal:Internal error {0}", ex);
            }


            return result;
        }


        public async Task<BiometricAuthentication> GetVerificationDetails(Int64 index)
        {

            BiometricAuthentication? result = new BiometricAuthentication();

            Task.Run(async () =>
            {
                try
                {
                    string uri = String.Format(_alpetaConfiguration.Endpoint + "authLogs/{0}", index);

                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //client.DefaultRequestHeaders.Authorization = GetAuthenticationHeader(Configurations);
                    client.Timeout = TimeSpan.FromSeconds(_alpetaConfiguration.ApiReqTimeout);

                    //HttpClient client = new Authentication().GetHttpClient(_Configurations);
                    client.DefaultRequestHeaders.Add("Cookie", cookies);

                    //AuthorizeDto.action = _Configurations.Action;
                    string inputJson = JsonConvert.SerializeObject(_alpetaConfiguration);
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");


                    using (HttpResponseMessage response = await client.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var str = response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<BiometricAuthentication>(await response.Content.ReadAsStringAsync());


                            _logger.Information(" Authorize: Success   ID :{0}"/*, result.transId*/);
                        }
                        else
                        {
                            //result.ResultCode = (int)response.StatusCode;
                            //result.message = (await response.Content.ReadAsStringAsync());

                            _logger.Error(" Error in AlpetaServer GetVerificationDetails:  ID:{0} {1}"/*, result.transId, JsonConvert.SerializeObject(result)*/);

                        }
                    }

                }
                catch (Exception ex)
                {
                    //result.ResultCode = 500;
                    //result.message = ex.Message;

                    _logger.Error("AlpetaServer GetVerificationDetails:Internal error {0}", ex);
                }

            }).Wait();
            return result;


        }

    }
}
