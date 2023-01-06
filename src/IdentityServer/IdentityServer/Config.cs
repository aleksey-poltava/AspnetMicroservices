using System;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using static System.Net.WebRequestMethods;

namespace IdentityServer
{
	public class Config
	{
        public static IEnumerable<Client> Clients =>
         new Client[]
         {
             new Client {
                 ClientId = "catalogClient",
                 AllowedGrantTypes = GrantTypes.ClientCredentials,
                 ClientSecrets =
                     {
                        new Secret("secret".Sha256())
                     },
                        AllowedScopes = { "catalogAPI" }
             },
             new Client {
                 ClientId = "shopping_web",
                 ClientName = "Shopping Web App",
                 AllowedGrantTypes = GrantTypes.Code,
                 AllowRememberConsent = false,
                 RedirectUris = new List<string>()
                 {
                    "https://localhost:5003/signin-oidc" //this is client app port
                 },
                 PostLogoutRedirectUris = new List<string>()
                 {
                    "https://localhost:5003/signout-callback-oidc"
                 },
                 ClientSecrets = new List<Secret>
                 {
                    new Secret("secret".Sha256())
                 },
                 AllowedScopes = new List<string>
                 {
                     IdentityServerConstants.StandardScopes.OpenId,
                     IdentityServerConstants.StandardScopes.Profile
                 }
 }
         };

        public static IEnumerable<ApiScope> ApiScopes =>
             new ApiScope[]
             {
                new ApiScope("catalogAPI", "Catalog API"),
                new ApiScope("orderAPI", "Order API"),
                new ApiScope("basketAPI", "Basket API")
             };

        public static IEnumerable<ApiResource> ApiResources =>
         new ApiResource[]
         {
         };

        public static IEnumerable<IdentityResource> IdentityResources =>
         new IdentityResource[]
         {
         };

        public static List<TestUser> TestUsers =>
         new List<TestUser>
         {
         };

    }
}

