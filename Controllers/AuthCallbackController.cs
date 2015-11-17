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
 * Class		: AuthCallbackController
 * Developer	: Nguyen Ho Long Hai
 * 
 */
#endregion

#region Using directives
using Splg.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace Splg.Controllers
{
    public class AuthCallbackController :Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        protected override Google.Apis.Auth.OAuth2.Mvc.FlowMetadata FlowData
        {
            get { return new AppAuthFlowMetadata(); }
        }
    }
}
