using Auth0.ManagementApi;
using PizzaAuth.Models;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PizzaAuth.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            PizzaAuthUser model = new PizzaAuthUser();
            //Immediatly check if user is authenticated. If not return view
            if(!User.Identity.IsAuthenticated)
            {
                model.IsAuthenticated = false;
                return View(model);
            }
            var claimList = ClaimsPrincipal.Current.Claims;
            string auth0Token = null;
            string auth0UserId = null;
            foreach (var claim in claimList)
            {
                string claimVal = claim.Value;
                if (claim.Type == "access_token")
                {
                    auth0Token = claimVal;
                }
                if (claim.Type == "user_id")
                {
                    auth0UserId = claimVal;
                }
            }

            if (auth0UserId != null)
            {
                try
                {
                    var client = new ManagementApiClient("eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik16Z3dRa1U1TUVWRlFrVTBRMFE1UWtRek5UYzRPRUl4UkRnMFFUQkdPVFl3TkRNNU1qWTJPQSJ9.eyJpc3MiOiJodHRwczovL2lyb25jb3ZlLmF1dGgwLmNvbS8iLCJzdWIiOiJDNEhoSnpuc3R6MjAxTXlnY1JIQmtURFBqaE5MQjczakBjbGllbnRzIiwiYXVkIjoiaHR0cHM6Ly9pcm9uY292ZS5hdXRoMC5jb20vYXBpL3YyLyIsImlhdCI6MTUyMjA3OTExNywiZXhwIjoxMDE1MjIwNzkxMTYsImF6cCI6IkM0SGhKem5zdHoyMDFNeWdjUkhCa1REUGpoTkxCNzNqIiwic2NvcGUiOiJyZWFkOmNsaWVudF9ncmFudHMgY3JlYXRlOmNsaWVudF9ncmFudHMgZGVsZXRlOmNsaWVudF9ncmFudHMgdXBkYXRlOmNsaWVudF9ncmFudHMgcmVhZDp1c2VycyB1cGRhdGU6dXNlcnMgZGVsZXRlOnVzZXJzIGNyZWF0ZTp1c2VycyByZWFkOnVzZXJzX2FwcF9tZXRhZGF0YSB1cGRhdGU6dXNlcnNfYXBwX21ldGFkYXRhIGRlbGV0ZTp1c2Vyc19hcHBfbWV0YWRhdGEgY3JlYXRlOnVzZXJzX2FwcF9tZXRhZGF0YSBjcmVhdGU6dXNlcl90aWNrZXRzIHJlYWQ6Y2xpZW50cyB1cGRhdGU6Y2xpZW50cyBkZWxldGU6Y2xpZW50cyBjcmVhdGU6Y2xpZW50cyByZWFkOmNsaWVudF9rZXlzIHVwZGF0ZTpjbGllbnRfa2V5cyBkZWxldGU6Y2xpZW50X2tleXMgY3JlYXRlOmNsaWVudF9rZXlzIHJlYWQ6Y29ubmVjdGlvbnMgdXBkYXRlOmNvbm5lY3Rpb25zIGRlbGV0ZTpjb25uZWN0aW9ucyBjcmVhdGU6Y29ubmVjdGlvbnMgcmVhZDpyZXNvdXJjZV9zZXJ2ZXJzIHVwZGF0ZTpyZXNvdXJjZV9zZXJ2ZXJzIGRlbGV0ZTpyZXNvdXJjZV9zZXJ2ZXJzIGNyZWF0ZTpyZXNvdXJjZV9zZXJ2ZXJzIHJlYWQ6ZGV2aWNlX2NyZWRlbnRpYWxzIHVwZGF0ZTpkZXZpY2VfY3JlZGVudGlhbHMgZGVsZXRlOmRldmljZV9jcmVkZW50aWFscyBjcmVhdGU6ZGV2aWNlX2NyZWRlbnRpYWxzIHJlYWQ6cnVsZXMgdXBkYXRlOnJ1bGVzIGRlbGV0ZTpydWxlcyBjcmVhdGU6cnVsZXMgcmVhZDpydWxlc19jb25maWdzIHVwZGF0ZTpydWxlc19jb25maWdzIGRlbGV0ZTpydWxlc19jb25maWdzIHJlYWQ6ZW1haWxfcHJvdmlkZXIgdXBkYXRlOmVtYWlsX3Byb3ZpZGVyIGRlbGV0ZTplbWFpbF9wcm92aWRlciBjcmVhdGU6ZW1haWxfcHJvdmlkZXIgYmxhY2tsaXN0OnRva2VucyByZWFkOnN0YXRzIHJlYWQ6dGVuYW50X3NldHRpbmdzIHVwZGF0ZTp0ZW5hbnRfc2V0dGluZ3MgcmVhZDpsb2dzIHJlYWQ6c2hpZWxkcyBjcmVhdGU6c2hpZWxkcyBkZWxldGU6c2hpZWxkcyB1cGRhdGU6dHJpZ2dlcnMgcmVhZDp0cmlnZ2VycyByZWFkOmdyYW50cyBkZWxldGU6Z3JhbnRzIHJlYWQ6Z3VhcmRpYW5fZmFjdG9ycyB1cGRhdGU6Z3VhcmRpYW5fZmFjdG9ycyByZWFkOmd1YXJkaWFuX2Vucm9sbG1lbnRzIGRlbGV0ZTpndWFyZGlhbl9lbnJvbGxtZW50cyBjcmVhdGU6Z3VhcmRpYW5fZW5yb2xsbWVudF90aWNrZXRzIHJlYWQ6dXNlcl9pZHBfdG9rZW5zIGNyZWF0ZTpwYXNzd29yZHNfY2hlY2tpbmdfam9iIGRlbGV0ZTpwYXNzd29yZHNfY2hlY2tpbmdfam9iIHJlYWQ6Y3VzdG9tX2RvbWFpbnMgZGVsZXRlOmN1c3RvbV9kb21haW5zIGNyZWF0ZTpjdXN0b21fZG9tYWlucyByZWFkOmVtYWlsX3RlbXBsYXRlcyBjcmVhdGU6ZW1haWxfdGVtcGxhdGVzIHVwZGF0ZTplbWFpbF90ZW1wbGF0ZXMiLCJndHkiOiJjbGllbnQtY3JlZGVudGlhbHMifQ.D1DzZIrkYkKOcfeo5EYJVckGJvsbjVdLtKL4KXu8ijoQtnH125cYgwxJ5K_5ib33Kc6TpD5GGIpbcGxGCYRiXPsK0UkEC-H2I-yC5tQ1tc0wjueL_jH4_0QdveO-Fio6oLjw7QABQB63Utc10Gl_VLkNzbiknvbS-iiWXfXNEvzcQQABLsaLYpS7_3KzggLcS24aGidwiDD7VPSOafi2f6XnoYGrTjM9SGsbODK_KLQRP39RPcUx1NKsey9Vhpk7cRHtkG9KF_zl2Ej_UoxN1GKPp3nVdN8wmt1w_eoGXN0zHjbJL18KDUjPjVuiYN_q6dJ8KX_PWGM1sTxsG1NW-A", ConfigurationManager.AppSettings["auth0:Domain"]);
                    var auth0User = await client.Users.GetAsync(auth0UserId);
                    if (model != null)
                    {
                        model.ImageUrl = auth0User.Picture;
                        model.EmailVerified = auth0User.EmailVerified;
                        foreach (var metadata in auth0User.UserMetadata)
                        {
                            if (metadata.Path == "country")
                            {
                                model.Country = metadata.Value;
                            }
                            if (metadata.Path == "city")
                            {
                                model.City = metadata.Value;
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