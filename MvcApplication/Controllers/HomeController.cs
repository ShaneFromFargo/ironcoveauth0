using Auth0.ManagementApi;
using PizzaAuth.Models;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PizzaAuth.Controllers
{
    public class HomeController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            PizzaAuthUser model = new PizzaAuthUser();
            //Check if user is authenticated. If not return view
            if(!User.Identity.IsAuthenticated)
            {
                model.IsAuthenticated = false;
                return View(model);
            }
            model.IsAuthenticated = true;
            var claimList = ClaimsPrincipal.Current.Claims;
            string auth0UserId = null;

            //Walk through each claim, find the items we need to populate the model
            foreach (var claim in claimList)
            {
                string claimKey = claim.Type;
                string claimVal = claim.Value;
                if (claim.Type == "name")
                {
                    model.Name = claimVal;
                    ViewBag.Name = claimVal;
                }
                if (claim.Type == "user_id")
                {
                    auth0UserId = claimVal;
                }
                if (claim.Type == "gender")
                {
                    model.Gender = claim.Value;
                }
            }

            if (auth0UserId != null)
            {
                try
                {
                    //Grab the auth0 managment token and call the managment api
                    string auth0ManagmentToken = ConfigurationManager.AppSettings["auth0:ManagmentToken"];
                    var client = new ManagementApiClient(auth0ManagmentToken, ConfigurationManager.AppSettings["auth0:Domain"]);
                    var auth0User = await client.Users.GetAsync(auth0UserId);

                    //Get the rest of the data not populated in the claims
                    if (auth0User != null)
                    {
                        ViewBag.PictureUrl = auth0User.Picture;
                        model.EmailVerified = auth0User.EmailVerified;
                        
                        //Loop through each metadata to find city and country
                        foreach (var metadata in auth0User.UserMetadata)
                        {
                            if (metadata.Path == "country")
                            {
                                ViewBag.Country = metadata.Value;
                            }
                            if (metadata.Path == "city")
                            {
                                ViewBag.City = metadata.Value;
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    string error = e.Message;
                    //Handle error here. For some reason the Auth0 SDK throws an exception if the user does not exist. It should simply return null, to avoid the try catch.
                }

            }
            return View(model);
        }
    }
}