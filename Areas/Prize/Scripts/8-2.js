/// Get first day of week
function getFirstDayOfWeek(y, m, w) {
    var d = new Date(y, --m);
    d.setDate(--w * 7 - d.getDay() + 1);
    return d;
}

function getWeekDates(y, m, w) {
    var d = getFirstDayOfWeek(y, m, w)
    var week = [new Date(d)];
    var i = 6;
    while (i--) {
        week.push(new Date(d.setDate(d.getDate() + 1)));
    }
    return week;
}
function getNumberWeeksOfTheMonth(year, month) {
    var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
    var firstDate = new Date(year, month, 6);
    var secondDate = new Date(year, month+1, 5);

    var diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));
    return Math.round(diffDays/7);
}
function getMondaysOfMonth() {
    var d = new Date();
    month = d.getMonth(),
    mondays = [];

    d.setDate(1);

    // Get the first Monday in the month
    while (d.getDay() !== 1) {
        d.setDate(d.getDate() + 1);
    }

    // Get all the other Mondays in the month
    while (d.getMonth() === month) {
        mondays.push(new Date(d.getTime()));
        d.setDate(d.getDate() + 7);
    }
    return mondays;
}

function countWeeksInMonth(year, month) {
    var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
    var firstDate = new Date(year + "/" + month + "/" + 1);
    
    //get next month
    if (month == 12) {
        var nextMonth = 1;
        var nextYear = parseInt(year) + 1;
    } else {
        var nextMonth = parseInt(month) + 1;
        var nextYear = year;
    }
    var secondDate = new Date(nextYear + "/" + nextMonth + "/" + 1);
    
    var diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));
    var firstDateCount = firstDate.getDay();

    if (firstDateCount == 1) {
        var dayCount = diffDays;
    } else {
        var dayCount = diffDays - (7 - firstDateCount + 1);
    }
    
    if (dayCount > 28) {
        return 5;
    } else {
        return 4;
    }
}

function getNumberWeekOfMounth() {
    var date = new Date();
    var d1 = date.getDate();
    var d2 = date.getDay();
    return weekOfMonth = Math.ceil((d1 - 1 - d2) / 7);
}

function getDateFormatCompare(d) {
    var _month = d.getMonth() < 10 ? '0' + (d.getMonth() + 1) : (d.getMonth() + 1);
    var _date = d.getDate() < 10 ? '0' + d.getDate() : d.getDate();
    return d.getFullYear() + '' + _month + '' + _date;
}

var gameIDs = [];
var gameIDsFistLast = [];
var date = new Date();
var currentYear = date.getFullYear();
var currentMonth = date.getMonth();
$(document).ready(function () {
    var urlCommon = "/Npb/NpbTop/ShowGameInfo";
    var dt = new Date();
    var year = dt.getFullYear(); // read the current year   

    //Month click
    $('.j_rank a:not(.active)').live('click', function (event) {
        $(this).toggleClass("active");
        $(this).siblings().removeClass("active");

        $('.block_04_3_l3').empty();
        var _month = $(this).text().replace("月", "");
        _month = _month.replace("/", "").trim();
        var countWeek = countWeeksInMonth(year, _month);
        
        for (var i = 1; i <= countWeek; i++) {
            var active = "";
            if (i == 1) {
               active = "active";
            }
            $('.block_04_3_l3').append('<li class="' + active + '"><a href="javascript:void(0);">第<span class="fs18">' + i + '</span>週</a></li>');

        }
        firstDateWeek = getDayOfWeek(year, _month,1, 1);
        lastDateWeek = getDayOfWeek(year, _month, 1, 7);
        getGameByWeek(2, firstDateWeek, lastDateWeek);
        event.preventDefault();
    });

    //Week click
    $('.block_04_3_l3 li:not(.active) a').live('click', function (event) {

        $(this).parent().toggleClass("active");
        $(this).parent().siblings().removeClass("active");
        var _week = $(this).parent().text().replace("第", "").replace("週", "");
        var _month = $(this).parent().parent().siblings().find(".active").text().replace("月  /", "").trim();
        firstDateWeek = getDayOfWeek(year, _month, _week, 1);
        lastDateWeek = getDayOfWeek(year, _month, _week, 7);
        getGameByWeek(2, firstDateWeek, lastDateWeek);
        event.preventDefault();
    });

    //Load div npbGameInfoWeek after select year, month, week.
    function getGameByWeek(type, startDate, endDate) {
        var linkUrl = urlCommon + '?type=' + type + '&startDate=' + startDate + '&endDate=' + endDate;
        $('#npbGameInfoWeek').load(linkUrl);
    }

    /**
    * year: int
    * month: int
    * week: int
    * day: int
    */
    function getDayOfWeek(year, month, week, day) {
        var date = new Date(year + "/" + month + "/1");
        var firstDay = date.getDay();

        if (firstDay == 1) {
            day += 7 * (week - 1);
        } else {
            day += (7 - firstDay + 1) + 7 * (week - 1);
        }

        result = new Date(year + "/" + month + "/" + day);
        var Y = parseInt(result.getFullYear());
        var m = parseInt(result.getMonth()) + 1;
        var d = parseInt(result.getDate());
        m = m < 10 ? '0' + m : m;
        d = d < 10 ? '0' + d : d;

        return (Y + m + d);
    }
  
});