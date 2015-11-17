using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Splg.Models.Game.ViewModel
{
    public class ExpectedGameSchedulesViewModel
    {
        [DisplayFormat(DataFormatString = "{0:M月d日(ddd)}")]
        public DateTime ScheduledDate { get; set; }

        public int GameCount 
        {
            get
            {
                 return ExpectedGameSchedules.Count();
            }
        }

        public List<ExpectedGameSchedule> ExpectedGameSchedules { get; set; }
    }
}