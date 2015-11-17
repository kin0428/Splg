using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Splg.Core.Constant;

namespace Splg.Areas.Prize.Models.ViewModel
{
    public class RallyViewModel
    {
        public long RallyId { get; set; }

        public string RallyName { get; set; }
        
        public string ImageUrl { get; set; }
        
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsDateAndDayOfWeekWithJapaneseSeparator)]   
        public DateTime EntryStartDate { get; set; }

        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsDateAndDayOfWeekWithJapaneseSeparator)]   
        public DateTime? EntryEndDate { get; set; }

        public string TargetYearMonth 
        {
            get 
            {
                if (EntryEndDate == null)
                {
                    return "999912";
                }
                else
                {
                    return EntryEndDate.Value.ToString("yyyyMM");
                }
            }
        }
    }
}