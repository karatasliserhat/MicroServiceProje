// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Course.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]{

             new ApiResource("resource_catalog"){Scopes={ "catalog_scope_fullpermission" } },
             new ApiResource("resource_photos"){Scopes={ "photos_scope_fullpermission" } },
             new ApiResource("resource_basket"){Scopes={ "basket_scope_fullpermission" } },
             new ApiResource("resource_discount"){Scopes={"discount_fullpermission"}},
             new ApiResource("resource_order"){Scopes={"order_fullpermission"}},
             new ApiResource("resource_payment"){Scopes={"payment_fullpermission"}},
             new ApiResource("resource_gateway"){Scopes={"gateway_fullpermission"}},
             new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource{ Name="roles", Description="Kullanıcı Rolleri", DisplayName="Roles", UserClaims=new []{"role"}}
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_scope_fullpermission","Catalog Api Full Access"),
                new ApiScope("photos_scope_fullpermission","Photo Stock Api Full Access"),
                new ApiScope("basket_scope_fullpermission","Basket Api Full Access"),
                new ApiScope("discount_fullpermission","Discount Api Full Access"),
                new ApiScope("order_fullpermission","Order Api Full Access"),
                new ApiScope("payment_fullpermission","Payment Api Full Access"),
                new ApiScope("gateway_fullpermission","Gateway Api Full Access"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName="Asp Net Core Web",
                    ClientId="WebClientApp",
                    ClientSecrets ={new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={ "catalog_scope_fullpermission", "photos_scope_fullpermission", "gateway_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
                },
                new Client
                {
                    ClientName="Asp Net Core Web",
                    ClientId="WebClientResourceWebApp",
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowOfflineAccess=true,
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    AccessTokenLifetime=1*60*60,
                    RefreshTokenExpiration= TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                    RefreshTokenUsage= TokenUsage.ReUse,
                    AllowedScopes={"basket_scope_fullpermission", "order_fullpermission",  "gateway_fullpermission", IdentityServerConstants.StandardScopes.Email,IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile,IdentityServerConstants.LocalApi.ScopeName,IdentityServerConstants.StandardScopes.OfflineAccess,"roles"}
                },
                new Client
                {
                    ClientName="Token Exchange Validator",
                    ClientId="TokenExchangeValidator",
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedGrantTypes={"urn:ietf:params:oauth:grant-type:token-exchange"},
                    AllowedScopes={IdentityServerConstants.StandardScopes.OpenId, "payment_fullpermission", "discount_fullpermission" }
                }

            };
    }
}