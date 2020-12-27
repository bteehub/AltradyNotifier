using System;
using System.Collections.Specialized;
using System.Net;

namespace AltradyNotifier
{
    public class Pushover
    {
        private readonly string _baseUrl;

        private readonly string _user;
        private readonly string _token;

        public Pushover(string user, string token)
        {
            _baseUrl = "https://api.pushover.net/1/messages.json";

            _user = user.Trim();
            _token = token.Trim();
        }

        public void SendMessage((string title, string message) titleMessage) => SendMessage(titleMessage.title, titleMessage.message);
        
        public void SendMessage(string title, string message)
        {
            title = title.Substring(0, Math.Min(250, title.Length));
            message = message.Substring(0, Math.Min(1024 - title.Length, message.Length));

            var parameters = new NameValueCollection
            {
                { "token", _token },
                { "user", _user },
                { "title",  title},
                { "message", message}
            };

            using (var client = new WebClient())
            {
                client.UploadValues(_baseUrl, parameters);
            }
        }
    }
}
