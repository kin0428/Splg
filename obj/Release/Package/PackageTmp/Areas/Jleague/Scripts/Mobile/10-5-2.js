
$(document).ready(function () {
    var firstDayOfMonth = "";
    var lastDayOfMonth = "";
    var urlCommon = "/Jleague/JlgTop/ShowGameInfo";

    $('.prev').click(function (event) {
        var _current_date = $('#h4MonthYear').html();
        var _year = _current_date.slice(0, 4)
        var _month = _current_date.slice(-3).replace("月", "").trim();

        var actualDate = new Date(_year, (_month - 1), 1); // convert to actual date
        var nextMonth = new Date(actualDate.getFullYear(), actualDate.getMonth() - 1, actualDate.getDate());

        var _formatMonthYear = nextMonth.getFullYear() + '年 ' + (nextMonth.getMonth() + 1) + '月';
        $('#h4MonthYear').html(_formatMonthYear);

        firstDayOfMonth = getfirstDayOfMonth(nextMonth.getMonth() + 1, nextMonth.getFullYear());
        lastDayOfMonth = getLastDayOfMonth(nextMonth.getMonth() + 1, nextMonth.getFullYear());

        getGameByWeek(2, firstDayOfMonth, lastDayOfMonth);
        event.preventDefault();

    });

    $('.next').click(function (event) {
        var _current_date = $('#h4MonthYear').html();
        var _year = _current_date.slice(0, 4)
        var _month = _current_date.slice(-3).replace("月", "").trim();

        var actualDate = new Date(_year, (_month - 1), 1); // convert to actual date
        var nextMonth = new Date(actualDate.getFullYear(), actualDate.getMonth() + 1, actualDate.getDate());

        var _formatMonthYear = nextMonth.getFullYear() + '年 ' + (nextMonth.getMonth() + 1) + '月';
        $('#h4MonthYear').html(_formatMonthYear);

        firstDayOfMonth = getfirstDayOfMonth(nextMonth.getMonth()+1, nextMonth.getFullYear());
        lastDayOfMonth = getLastDayOfMonth(nextMonth.getMonth()+1, nextMonth.getFullYear());

        getGameByWeek(2, firstDayOfMonth, lastDayOfMonth);
        event.preventDefault();
    });

    //Load div JlgGameInfoWeek after select year, month, week.
    function getGameByWeek(type, startDate, endDate) {
        var linkUrl = urlCommon + '?type=' + type + '&startDate=' + firstDayOfMonth + '&endDate=' + lastDayOfMonth;
        $('#jlgGameInfoWeek').load(linkUrl);
    }
});


//Get first date of month
function getfirstDayOfMonth(month, year) {

    var firstOfMonth = new Date(year, month, 1);
    var _month = (firstOfMonth.getMonth() + 1) < 10 ? '0' + (firstOfMonth.getMonth()) : (firstOfMonth.getMonth() + 1);
    return firstOfMonth.getFullYear() + '' + _month + '01';
}
//Get last date of month
function getLastDayOfMonth(month, year) {

    var lastDate = new Date(year, month, 0);
    var _month = month < 10 ? '0' + month : month;
    return lastDate.getFullYear() + '' + _month + '' + lastDate.getDate();
}