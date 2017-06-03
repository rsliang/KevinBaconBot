using System.Net.Http;
using System.Web.Configuration;
using System.Threading.Tasks;
using KevinBaconBot.Model;
using Newtonsoft.Json;
using System;

namespace KevinBaconBot.Services
{
    [Serializable]
    public class AzureSearchService
    {
        private static readonly string QueryString = $"https://{WebConfigurationManager.AppSettings["SearchName"]}.search.windows.net/indexes/{WebConfigurationManager.AppSettings["IndexName"]}/docs?api-key={WebConfigurationManager.AppSettings["SearchKey"]}&api-version=2015-02-28&";
        //private static readonly string QueryString = "https://bot-searchdemo.search.windows.net/indexes/musicianindex/docs?api-key=E1EBE99CD8FCB0872C152B422512D7FC&api-version=2016-09-01&";



        public async Task<SearchResult> SearchByName(string name)
        {
            using (var httpClient = new HttpClient())
            {
                string nameQuey = $"{QueryString}search={name}";
                string response = await httpClient.GetStringAsync(nameQuey);
                return JsonConvert.DeserializeObject<SearchResult>(response);
            }
        }

        public async Task<FacetResult> FetchFacets()
        {
            using (var httpClient = new HttpClient())
            {
                string facetQuey = $"{QueryString}facet=Era";
                string response = await httpClient.GetStringAsync(facetQuey);
                return JsonConvert.DeserializeObject<FacetResult>(response);
            }
        }

        public async Task<SearchResult> SearchByEra(string era)
        {
            using (var httpClient = new HttpClient())
            {
                string nameQuey = $"{QueryString}$filter=Era eq '{era}'";
                string response = await httpClient.GetStringAsync(nameQuey);
                return JsonConvert.DeserializeObject<SearchResult>(response);
            }
        }

        public async Task<SearchResult> SearchByMovie(string movie)
        {
            using (var httpClient = new HttpClient())
            {
                string nameQuey = $"{QueryString}$filter=Era eq '{movie}'";
                string response = await httpClient.GetStringAsync(nameQuey);
                return JsonConvert.DeserializeObject<SearchResult>(response);
            }
        }
    }
}