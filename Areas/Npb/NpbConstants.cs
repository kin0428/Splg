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
 * Namespace	: Splg.Areas.Npb.Controllers
 * Class		: NpbConstants
 * Developer	: Nguyen Minh Kiet
 * Updated date : 2015-03-05
 */
#endregion

#region Using directives
using System.Web.Mvc;
#endregion

namespace Splg.Areas.Npb
{
    public class NpbConstants 
    {
        public enum GameAssortment
        {
          
            /// <summary>
            /// GameAssortment = 1, 
            /// Japan : セ・リーグ, English : Central League.       
            /// </summary>
            Se = 1,

            /// <summary>
            /// GameAssortment = 2, 
            /// Japan : パ・リーグ, English : Pacific League.               
            /// </summary>  
            Pa = 2          
        }

        public enum TakeTop
        {

            /// <summary>
            /// TakeTop = 5               
            /// </summary>
            Five = 5,

            /// <summary>
            /// TakeTop = 10             
            /// </summary>  
            Ten = 10
        }

        public enum TypeID 
        {

            /// <summary>
            /// TypeOne = 1      
            /// Get TeamInfoPitchingInfos
            /// 物理名称	NpbTeamInfoPitcher																												
            /// Html: 8-5-4
            /// </summary>
            TypeOne = 1,

            /// <summary>
            /// TypeTwo = 2   
            /// Get CatcherFielderInfos
            /// 物理名称	NpbTeamInfoCatcherFielder	
            /// Html: 8-5-5
            /// </summary>  
            TypeTwo = 2,
            /// <summary>
            /// TypeThree = 3 
            /// Get DirectorStaffInfos
            /// 物理名称	NpbTeamInfoDirectorStaff											
            /// Html: 8-5-6
            /// </summary>  
            TypeThree = 3,
        }

        public enum TeamInfoMenu
        {
            /// <summary>
            /// TabActive = 1      
            /// Active menu NpbTeamInfoTeamTop
            /// 物理名称	NpbTeamInfoTeamTop																												
            /// Html: 8-5-1
            /// </summary>
            TabActive_1 = 1,

            /// <summary>
            /// TabActive = 2      
            /// Active menu NpbTeamInfDailyResult
            /// 物理名称	NpbTeamInfDailyResult																												
            /// Html: 8-5-2
            /// </summary>
            TabActive_2 = 2,

            /// <summary>
            /// TabActive = 3     
            /// Active menu NpbTeamInfoConfrontationResult
            /// 物理名称	NpbTeamInfoConfrontationResult																												
            /// Html: 8-5-3
            /// </summary>
            TabActive_3 = 3,

            /// <summary>
            /// TabActive = 4      
            /// Active menu TeamInfoPitchingInfos
            /// 物理名称	NpbTeamInfoPitcher																												
            /// Html: 8-5-4
            /// </summary>
            TabActive_4 = 4,         

            /// <summary>
            /// TabActive = 5   
            /// Get CatcherFielderInfos
            /// 物理名称	NpbTeamInfoCatcherFielder	
            /// Html: 8-5-5
            /// </summary>  
            TabActive_5 = 5,

            /// <summary>
            /// TypeThree = 6
            /// Get DirectorStaffInfos
            /// 物理名称	NpbTeamInfoDirectorStaff											
            /// Html: 8-5-6
            /// </summary>  
            TabActive_6 = 6,

            /// <summary>
            /// TypeThree = 7
            /// Get NpbTeamInfoNews
            /// 物理名称	NpbTeamInfoNews											
            /// Html: 8-5-7
            /// </summary>  
            TabActive_7 = 7,
        }
    }
}