using COVIDVaccineIND.Database;
using COVIDVaccineIND.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace COVIDVaccineIND.Services
{
    public class CoWinService
    {
        public CoWinService(int cacheDurationDays = 1)
        {
            this.cacheDurationDays = cacheDurationDays;
        }

        private readonly int cacheDurationDays;

        public async Task<CoWinResponseModel> GetVaccineSlots(string pincode, DateTime fromDate, DateTime toDate)
        {
            List<Session> sessions = new List<Session>();
            
            for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                CoWinResponseModel dateResponse = await GetVaccineSlots(pincode, date);
                
                if (dateResponse != null && dateResponse.sessions.Length > 0)
                {
                    sessions.AddRange(dateResponse.sessions);
                }
            }

            if (sessions.Count > 0)
            {
                CoWinResponseModel coWinResponse = new CoWinResponseModel();
                coWinResponse.sessions = sessions.ToArray();
                return coWinResponse;
            }

            return null;
        }
        public async Task<CoWinResponseModel> GetVaccineSlots(string pincode, DateTime date)
        {
            string requestUri = GetRequestURI(pincode, date.ToString("dd-MM-yyyy"));

            #region Try from cache

            CoWinResponseModel cachedResponse = await GetCachedResponse(requestUri);

            if (cachedResponse != null)
            {
                return cachedResponse;
            }

            #endregion

            #region Try from api

            using (HttpClient client = new HttpClient())
            {
                var responseTask = client.GetAsync(requestUri);

                try
                {
                    responseTask.Wait(-1);
                }
                catch (AggregateException)
                {

                }
                catch (Exception)
                {

                }

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    var uploadResponse = readTask.Result;

                    await CacheResponse(requestUri, uploadResponse);

                    CoWinResponseModel response = JsonConvert.DeserializeObject<CoWinResponseModel>(uploadResponse);

                    return response;
                }
            }

            #endregion

            return null;
        }

        private string GetRequestURI(string pincode, string date)
        {
            return $"https://cdn-api.co-vin.in/api/v2/appointment/sessions/public/findByPin?pincode={pincode}&date={date}";
        }

        private async Task<CoWinResponseModel> GetCachedResponse(string requestUri)
        {
            DBConnect dBConnect = DBConnect.GetDBConnect();

            APIResponseCache aPICachedResponse = (await dBConnect.GetAPIResponses()).Where(x => string.Compare(requestUri, x.RequestURI, true) == 0).FirstOrDefault();

            if (aPICachedResponse != null && aPICachedResponse.ExpiryDate > DateTime.Now)
            {
                CoWinResponseModel response = JsonConvert.DeserializeObject<CoWinResponseModel>(aPICachedResponse.Response);

                return response;
            }

            return null;
        }

        private async Task CacheResponse(string requestUri, string response)
        {
            DBConnect dBConnect = DBConnect.GetDBConnect();

            APIResponseCache aPICachedResponse = (await dBConnect.GetAPIResponses()).Where(x => string.Compare(requestUri, x.RequestURI, true) == 0).FirstOrDefault();

            if (aPICachedResponse != null)
            {
                aPICachedResponse.Response = response;
                aPICachedResponse.ModifiedDate = DateTime.Now;
                aPICachedResponse.ExpiryDate = DateTime.Now.AddDays(cacheDurationDays);

                await dBConnect.UpdateRecord(aPICachedResponse);
            }
            else
            {
                APIResponseCache aPIResponse = new APIResponseCache()
                {
                    CreatedDate = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddDays(cacheDurationDays),
                    RequestURI = requestUri,
                    Response = response
                };

                await dBConnect.InsertRecord(aPIResponse);
            }
        }
    }
}
