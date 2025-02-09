using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Fpl.Client.Clients
{
    public class FplHttpHandler : HttpClientHandler
    {
        private readonly CookieFetcher _cookieFetcher;

        public FplHttpHandler(CookieFetcher cookieFetcher)
        {
            _cookieFetcher = cookieFetcher;
          
            
            AutomaticDecompression = DecompressionMethods.GZip;
            SslProtocols = System.Security.Authentication.SslProtocols.Tls12;   
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var sessionCookie = await _cookieFetcher.GetSessionCookie();
            request.Headers.Add("Cookie", sessionCookie);


            return await base.SendAsync(request, cancellationToken);
        }

      
    }
}