﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace EShpper.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        //public static IEnumerable<ApiScope> ApiScopes =>
        //    new ApiScope[]
        //    {
        //        new ApiScope("scope1"),
        //        new ApiScope("scope2"),
        //    };

        public static IEnumerable<ApiScope> ApiScopes =>
           new ApiScope[]
           {
                //new ApiScope("catalogueapi"),
                new ApiScope("basketapi"),
                new ApiScope("catalogueapi.read"),
                new ApiScope("catalogueapi.write"),
                new ApiScope("eshoppinggateway")
           };

        public static IEnumerable<ApiResource> ApiResources =>
          new ApiResource[]
          {
                //List of Microservices can go here.
                // new ApiResource("Catalogues", "Catalogue.API")
                //{
                //    Scopes = {"catalogueapi"}
                //},
                new ApiResource("Catalogue", "Catalogue.API")
                {
                    Scopes = {"catalogueapi.read", "catalogueapi.write"}
                },
                new ApiResource("Basket", "Basket.API")
                {
                    Scopes = {"basketapi"}
                },
                new ApiResource("EShoppingGateway", "EShopping Gateway")
                {
                    Scopes = {"eshoppinggateway", "basketapi"}
                },
                new ApiResource("eshoppingAngular", "EShopping Angular")
                {
                    Scopes = {"eshoppinggateway", "catalogueapi.read", "catalogueapi.write", "basketapi", "catalogueapi.read"}
                }
          };
        public static IEnumerable<Client> Clients =>
               new Client[]
               {
                // new Client
                //{
                //    ClientName = "Catalogue API Client",
                //    ClientId = "CatalogueApiClients",
                //    ClientSecrets = {new Secret("5c6eb3b4-61a7-4668-ac57-2b4591ec26d2".Sha256())},
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    AllowedScopes = {"catalogueapi"}
                //},
                //m2m flow 
                new Client
                {
                    ClientName = "Catalogue API Client",
                    ClientId = "CatalogueApiClient",
                    ClientSecrets = {new Secret("5c6eb3b4-61a7-4668-ac57-2b4591ec26d2".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"catalogueapi.read", "catalogueapi.write"}
                },
                new Client
                {
                    ClientName = "Basket API Client",
                    ClientId = "BasketApiClient",
                    ClientSecrets = {new Secret("5c6ec4c5-61a7-4668-ac57-2b4591ec26d2".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"basketapi"}
                },

                new Client
                {
                    ClientName = "EShopping Gateway Client",
                    ClientId = "EShoppingGatewayClient",
                    ClientSecrets = {new Secret("5c7fd5c5-61a7-4668-ac57-2b4591ec26d2".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"eshoppinggateway", "basketapi"}
                },
                new Client
                {
                    ClientName = "Angular-Client",
                    ClientId = "angular-client",
                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = new List<string>
                        {
                            "http://localhost:4200/signin-callback",
                            "http://localhost:4200/assets/silent-callback.html",
                            "https://localhost:9009/signin-oidc"
                        },
                    RequirePkce = true,
                    AllowAccessTokensViaBrowser = true,
                    Enabled = true,
                    UpdateAccessTokenClaimsOnRefresh = true,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "eshoppinggateway"
                    },
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    RequireClientSecret = false,
                    AllowRememberConsent = false,
                    //PostLogoutRedirectUris = new List<string> {"http://localhost:4200/signout-callback"},
                    RequireConsent = false,
                    AccessTokenLifetime = 3600,
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:4200/signout-callback",
                        "https://localhost:9009/signout-callback-oidc"
                    },
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("5c6eb3b4-61a7-4668-ac57-2b4591ec26d2".Sha256())
                    }
                }
               };
    };
}
