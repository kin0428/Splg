using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;

namespace Splg.Areas.Jleague.Models.ViewModel
{
    public class JlgPersonInfoViewModel
    {
        //public PlayerInfoRE PersonInfo { get; set; }
        //public string PositionName { get; set; }
        //public int TeamID { get; set; }
        //public string TeamName { get; set; }
        //public string TeamIcon { get; set; }

        public PlayerInfoDI PersonInfoDI { get; set; }
        public PlayerInfoDA PersonInfoDA { get; set; }
        private string imagePath;
        public string ImagePath
        {
            get
            {
                string result = Constants.IMG_DEFAULT_PROFILE;
                if (!String.IsNullOrEmpty(imagePath))
                {
                    if (!imagePath.StartsWith("/") && !imagePath.StartsWith("~"))
                        imagePath = "/" + imagePath;

                    return imagePath;
                }

                return result;
            }
            set { imagePath = value; }
        }
    }
}