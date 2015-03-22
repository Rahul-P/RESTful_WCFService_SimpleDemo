
namespace Tweet.WCFService.RESTful
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Web;
    using TweetBL;

    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements
        (RequirementsMode = 
        AspNetCompatibilityRequirementsMode.Allowed)]
    public class TweetService
    {         
        private TweetBL.ManageTweet _businessLayerTweetService;

        public TweetService()
        {
            _businessLayerTweetService = new TweetBL.ManageTweet();
        }
     
        [WebGet(UriTemplate = "/GetTweets")]
        public IList<Tweet> GetTweets()
        {
            return _businessLayerTweetService.GetTweets();
        }

        [WebGet(UriTemplate = "/Tweet/{tweetId}")]
        public Tweet GetTweetByID(string tweetId)
        {
            int tweetIdParsedToInt;
            Int32.TryParse(tweetId, out tweetIdParsedToInt);

            return _businessLayerTweetService.GetTweetById(tweetIdParsedToInt);
        }

        [WebInvoke(UriTemplate = "/Tweets")]
        public void CreateTweet(Tweet newTweet)
        {
            _businessLayerTweetService.CreateTweet(newTweet);
        }

        [WebInvoke(Method = "PUT", UriTemplate = "/Tweet")]
        public void UpdateTweet(Tweet updateTweet)
        {
            _businessLayerTweetService.UpdateTweet(updateTweet);
        }

        [WebInvoke(Method = "DELETE", UriTemplate = "/Tweet/{deleteTweetId}")]
        public void DeleteTweet(string deleteTweetId)
        {
            int deleteTweetIdParsedToInt;
            Int32.TryParse(deleteTweetId, out deleteTweetIdParsedToInt);

            _businessLayerTweetService.DeleteTweet(deleteTweetIdParsedToInt);
        }
    }
}
