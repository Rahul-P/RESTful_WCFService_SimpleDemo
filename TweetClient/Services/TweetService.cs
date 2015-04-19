
namespace TweetClient.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using TweetClient.Transports;
    using System.Runtime.Serialization.Json;
    using System.Xml.Serialization;

    public class TweetService
    {
        public TweetService()
        {
        }

        public IList<Tweet> GetTweets()
        {
            // Making a call to our GetTweets Endpoint by employing a 
            // WebClient!
            var client = new WebClient();

            // Requesting JSON from the Web Server - because we have 
            //deserializer for JSON in place
            client.Headers.Add("Accept", "application/json");

            // Using WebClient we can download a file, a string etc.. 
            // we will downlaod String because we will be receving 
            // the data from our TweetService.svc in XML format or JSON format
            // ...all we have to do than is create a deserializer to create 
            // a object from that received XML/JSON data! .. sweet

            // to download a string we need to give the web address of our 
            // service endpoint below I have specified the local address 
            // and the correct endpoint to access the required information
            var result = client.DownloadString
                ("http://localhost:47354/TweetService.svc/GetTweets");            

            // Now we will employ the JSON desiralizer that comes with .NET 
            // to deserialize and create a Tweet object for us from the response 
            // received from TweetService.svc
            var serializer = new DataContractJsonSerializer(typeof(List<Tweet>));

            List<Tweet> resultObject;
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(result)))
            {
                resultObject = (List<Tweet>)serializer.ReadObject(stream);
            }

            return resultObject;
        }

        public Tweet GetTweetByID(int tweetId)
        {
            // Making a call to our Tweet Endpoint by employing a 
            // WebClient!
            var client = new WebClient();

            // No Need to request for XML as Web-Server returns XML by default.
            var result = client.DownloadString
                ("http://localhost:47354/TweetService.svc/Tweet/" + tweetId);

            var serializer = new XmlSerializer(typeof(Tweet));

            Tweet resultObject;
            using (var stream = new 
                MemoryStream(Encoding.ASCII.GetBytes(result)))
            {
                resultObject = (Tweet)serializer.Deserialize(stream);
            }
            return resultObject;
        }

        public void CreateTweet(Tweet tweetRecord)
        {
            // the enpoint is: /Tweets
            SendDataToServer(
                "http://localhost:47354/TweetService.svc/Tweets",
                "POST", tweetRecord);
        }        

        public void UpdateTweet(Tweet tweet)
        {
            SendDataToServer(
                "http://localhost:47354/TweetService.svc/Tweet",
                "PUT", tweet);
        }

        public void DeleteTweet(int tweetId)
        {
            SendDataToServer(
                "http://localhost:47354/TweetService.svc/Tweet/" 
                + tweetId, "DELETE",
                new DeleteTweet { DeleteTweetId = tweetId });
        }

        private T SendDataToServer<T>(string endpoint, string method, T tweet)
        {
            // Endpoit - is the URI we will be dealing with
            var request = (HttpWebRequest)HttpWebRequest.Create(endpoint);

            // Indicate that we are dealing with JSON object
            // to the Web Server
            request.Accept = "application/json";
            request.ContentType = "application/json";

            // HTTP Verb Method Type: Get/Put/Post
            request.Method = method;

            // Working with Request and Response Streams directly           
            var serializer = new DataContractJsonSerializer(typeof(T));

            // Stream that represents the data that will be sent to the Server
            // This will open a Stream connection (Network Stream that
            // we can write too) to the Server 
            var requestStream = request.GetRequestStream();

            // Write the Request Stream
            // Serialize and write a .net object (in memory: tweet) to JSON 
            serializer.WriteObject(requestStream, tweet);

            // Close the request stream
            requestStream.Close();

            var response = request.GetResponse();
            if (response.ContentLength == 0)
            {
                response.Close();
                return default(T);
            }

            // Deserialize from JSON object back to a .net object
            var responseStream = response.GetResponseStream();
            var responseObject = (T)serializer.ReadObject(responseStream);

            responseStream.Close();

            return responseObject;
        }
    }
}





