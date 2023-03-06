using System;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace PlacowkaOswiatowaQuiz.Helpers
{
	public class HttpClientMiddleware : DelegatingHandler
	{
        private readonly IHttpContextAccessor _accessor;

        public HttpClientMiddleware(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var httpContext = _accessor.HttpContext;

            var quizUser = httpContext?.Session.GetString("quiz-user");

            if (!string.IsNullOrEmpty(quizUser))
                request.Headers.Add("Cookie", quizUser);

            return base.SendAsync(request, cancellationToken);
        }
    }
}

