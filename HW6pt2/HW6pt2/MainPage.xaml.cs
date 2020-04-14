using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace HW6pt2
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            isConnected();
        }
        public bool isConnected()
        {
            var status = Connectivity.NetworkAccess;
            if(status == NetworkAccess.Internet)//checks status of internet
            { 
                return true; 
            }
            else
            {
                DisplayAlert("Hey!", "You have no internet. Find a network to connect to!", "OK");
                return false;
            }
        }
       
        public async void GetWord(string WordInput)
        {
            
            //what we build and send to the server
            HttpClient client = new HttpClient();
            var uri = new Uri("https://owlbot.info/api/v2/dictionary/" + WordInput);

            List<Dictionary> wordData = new List<Dictionary>();//creating the list
            var request = new HttpRequestMessage(); //initializing request message

            request.Headers.Add("Application", "application / json");//initializing header and passing api token
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", "5485b52a4e68b42e14fa6cdad1c84a140ad64d5c");//my api token
            var response = await client.GetAsync(uri);
            
            //await is used so that we dont execute the next lines of code in the function until after we get a response from the server  
            //If the http status code is 200 it means OK(successful) in the response, then read the content in the response
            if (response.IsSuccessStatusCode)
            {

                var jsonContent = await response.Content.ReadAsStringAsync();
                wordData = JsonConvert.DeserializeObject<List<Dictionary>>(jsonContent);
            }
            else
            {
                Debug.WriteLine("An error occured while loading data");
            }

            WordView.ItemsSource = wordData;
        }

        private void InputBox_Completed(object sender, EventArgs e)
        {
            
            string WordInput = InputBox.Text;
            if (string.IsNullOrWhiteSpace(WordInput)) //checks for valid input, doesn't allow null or white space
            {
                DisplayAlert("404: Word not found", "This page is empty. Try searching for a different word", "OK");
            }
            else if (isConnected())
            {
                WordInput = WordInput.ToLower();
                GetWord(WordInput);
            }
        }
    }
}