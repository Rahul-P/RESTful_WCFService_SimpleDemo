
namespace TweetClient.Transports
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(Namespace = 
        "http://schemas.datacontract.org/2004/07/TweetBL")]
    public class Tweet
    {
       
        public int Id { get; set; }
       
        public string PostedBy { get; set; }
     
        public string Text { get; set; }
    }
}
