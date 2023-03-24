using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PlacowkaOswiatowaQuiz.Helpers.Filters
{
    public class UserLoggedInAttribute : TypeFilterAttribute
	{
		public UserLoggedInAttribute() : base(typeof(UserLoggedInFilter))
		{
		}
	}

    public class UserLoggedInFilter : IAuthorizationFilter
    {
        private readonly string _message = "Odmowa dostępu. Proszę się zalogować.";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool allowAnonymous = context
                .ActionDescriptor
                .EndpointMetadata
                .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

            if (    !allowAnonymous
                &&  !context.HttpContext.Session.Keys.Contains(Constants.QuizUserKey))
            {
                string returnUrl = context.HttpContext.Request.Path.Value ?? "/";
                context.HttpContext.Session.SetString(Constants.ErrorMessageKey, _message);
                context.HttpContext.Session.SetString(Constants.ReturnUrlKey, returnUrl);

                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        controller = "Home",
                        action = "Index"
                    }));
            }
        }
    }
}

