#region (c) 2015 Prime Labo - All rights reserved
/*                                      COPYRIGHT NOTICE
 * -------------------------------------------------------------------------------------
 * All materials (including but not limited to source code, compiled assemblies, images,
 * resources, etc.) are copyrighted to Prime Labo. No usage is allowed unless permitted 
 * by written consent. You may not use, reverse-engineer these materials under any
 * circumstances.
 * 
 *                                    PROJECT DESCRIPTION
 * -------------------------------------------------------------------------------------
 * Namespace	: Splg.Controllers
 * Class		: AppAuthFlowMetadata
 * Developer	: Nguyen Ho Long Hai
 * 
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Models;
using Google.Apis.Plus.v1;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.Drive.v2;
#endregion

namespace Splg.Controllers
{
    public class AppAuthFlowMetadata : FlowMetadata
    {
        private static readonly IAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = ComCommon.GetAccessKey(Constants.GOOGLE),
                ClientSecret = ComCommon.GetSecretKey(Constants.GOOGLE)
            },
            Scopes = new[] {
                PlusService.Scope.PlusLogin,  
                PlusService.Scope.UserinfoEmail,
                PlusService.Scope.UserinfoProfile
            },
            DataStore = GetFileDataStore()

        });

         public static FileDataStore GetFileDataStore()
        {
            var path = HttpContext.Current.Server.MapPath("~/App_Data/Drive.Api.Auth.Store");
            FileDataStore fileDataStore=new FileDataStore(path);
            fileDataStore.ClearAsync();
            return fileDataStore;
        }

        public override string GetUserId(Controller controller)
        {
            return "splg";
        }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }
    }
}