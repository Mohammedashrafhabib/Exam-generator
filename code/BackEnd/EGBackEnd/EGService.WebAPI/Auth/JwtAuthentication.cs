#region Using ...
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using EGService.Common.Enums;
using EGService.Core.IRepositories;
using EGService.Entity.Entities;
using Framework.Common.Enums;
using EGService.Core.Common;
#endregion

/*


 */
namespace EGService.WebAPI.Auth
{
	public class JwtAuthentication : Attribute, IActionFilter
	{
		#region Data Members
		
		private IUsersRepositoryAsync _usersRepository;
		IServiceProvider _serviceProvider;
		private IJwtService _jwtService;
		#endregion


		#region Constructors

		

		/// <summary>
		/// Validates Authentication only.
		/// </summary>
		public JwtAuthentication()
		{

		}
		#endregion

		private bool ValidateToken(string token, out string id)
		{
			id = null;

			var simplePrinciple = _jwtService.GetPrincipal(token);
			if (simplePrinciple == null)
				return false;
			var identity = simplePrinciple.Identity as ClaimsIdentity;

			if (identity == null)
				return false;

			if (!identity.IsAuthenticated)
				return false;

			var usernameClaim = identity.FindFirst(ClaimTypes.Name);
			id = usernameClaim?.Value;

			if (string.IsNullOrEmpty(id))
				return false;

            var userId = Convert.ToInt64(identity.FindFirst(ClaimTypes.Name).Value);
            this._usersRepository = _serviceProvider.GetService(typeof(IUsersRepositoryAsync)) as IUsersRepositoryAsync;
            var user =  this._usersRepository.GetAsync(a => a.Id == userId && a.IsActive == true && a.IsDeleted != true);
			if (user.Result.Count== 0)
				return false;
			// You can implement more validation to check whether username exists in your DB or not or something else. 



			return true;
		}
		protected IPrincipal AuthJwtToken(string token)
		{
			string id;

			if (ValidateToken(token, out id))
			{
				//to get more information from DB in order to build local identity based on username 
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, id)
                    // you can add more claims if needed like Roles ( Admin or normal user or something else)
                };


				string authorization = Boolean.FalseString;
				long userId = long.Parse(id);

					authorization = bool.TrueString;
				

				var identity = new ClaimsIdentity(claims, "Jwt");
				IPrincipal user = new ClaimsPrincipal(identity);
				identity.AddClaim(new Claim(ClaimTypes.AuthorizationDecision, authorization));

				return user;
			}

			return null;
		}


		public void OnActionExecuted(ActionExecutedContext context)
		{

		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
		//	return;

			try
			{
				_serviceProvider = context.HttpContext.RequestServices;

				_jwtService = _serviceProvider.GetService(typeof(IJwtService)) as IJwtService;

				var request = context.HttpContext.Request;
				var response = context.HttpContext.Response;
				var authorization = request.Headers["Authorization"];
				// checking request header value having required scheme "Bearer" or not.
				if (string.IsNullOrEmpty(authorization) || authorization.ToString().StartsWith("Bearer") == false)
				{
					context.Result = new StatusCodeResult(ApplicationConstants.AuthFailureCode);

					return;
				}
				// Getting Token value from header values.
				var token = authorization.ToString().Split(" ")[1];
				var principal = AuthJwtToken(token);
				if (principal == null)
				{
					context.Result = new StatusCodeResult(ApplicationConstants.AuthFailureCode);
				}
				else
				{
					var identity = principal.Identity as ClaimsIdentity;
					var isAuthorization = bool.Parse(identity.FindFirst(ClaimTypes.AuthorizationDecision).Value);

					if (isAuthorization == false)
					{
						context.Result = new StatusCodeResult(ApplicationConstants.AuthFailureCode);
					}
					else
					{

						//context.Principal = principal
					}
				}
			}
			catch (Exception)
			{

			}
		}


		

		#region Properties
		public string Realm { get; set; }
		public bool AllowMultiple => false;
		public string AuthenticationSchemes { get; set; }
		public string Policy { get; set; }
		public string Roles { get; set; }
		#endregion
	}
}
