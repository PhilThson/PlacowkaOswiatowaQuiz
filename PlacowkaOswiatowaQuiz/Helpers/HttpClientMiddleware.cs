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
            //request.Headers.Add("user", "LoggedInUser");

            var quizUser = httpContext.Session.GetString("quiz-user");
            if (!string.IsNullOrEmpty(quizUser))
                request.Headers.Add("Cookie", quizUser);

            return base.SendAsync(request, cancellationToken);
        }

        //KOMENTARZ:
        //Pomimo dodania tej klasy w kontenerze DI jako Transient,
        //to jest ona tworzona jednokrotnie
        //wywołuje się tylko 'nowa' metoda

        //Generalnie middleware trwa tak długo jak HttpMessageHandler
        //middleware jest doklejony do MessageHandlera, a nie do HttpClienta

        //A MessageHandler - w istocie przekazuje zapytania generowane przez
        //klienta http, ale jest osobnym bytem,
        //czas jego trwania domyślnie to ok 2 min.
        //można zmienić poprzez .SetHandlerLifetime(..);
    }
}

