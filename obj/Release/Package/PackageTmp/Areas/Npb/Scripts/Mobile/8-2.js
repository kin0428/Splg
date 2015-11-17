
$(document).ready(function () {
    var firstDayOfMonth = "";
    // var lastDayOfMonth = "";
    var urlCommon = "/Npb/NpbTop/ShowGameInfo";
    var strYearMonth = "";
    var year = "";
    var month = "";

    //======Bind data dropdownlist day when page load========//
    var d = new Date();
    var month = d.getMonth();
    var year = d.getFullYear();
    var days = getDaysInMonth(month, year)
    fillMonthtoDropDown(days);

    //======End Bind data dropdownlist day when page load====//

    //===prev month click===// 
    $('.prev').click(function (event) {

        strYearMonth = $('#h4MonthYear').html();
        year = strYearMonth.slice(0, 4)
        month = strYearMonth.slice(-3).replace("月", "").trim();

        var actualDate = new Date(year, (month - 1), 1); // convert to actual date
        var nextMonth = new Date(actualDate.getFullYear(), actualDate.getMonth() - 1, actualDate.getDate());

        var _formatMonthYear = nextMonth.getFullYear() + '年 ' + (nextMonth.getMonth() + 1) + '月';
        $('#h4MonthYear').html(_formatMonthYear);

        var _days = getDaysInMonth(nextMonth.getMonth(), nextMonth.getFullYear());
        fillMonthtoDropDown(_days);

        firstDayOfMonth = getfirstDayOfMonth(nextMonth.getMonth() + 1, nextMonth.getFullYear());
        getGameByWeek(2, firstDayOfMonth, firstDayOfMonth);
       
        event.preventDefault();
    });

    //===prev month click===// 
    $('.next').click(function (event) {

        strYearMonth = $('#h4MonthYear').html();
        year = strYearMonth.slice(0, 4)
        month = strYearMonth.slice(-3).replace("月", "").trim();

        var actualDate = new Date(year, (month - 1), 1); // convert to actual date
        var nextMonth = new Date(actualDate.getFullYear(), actualDate.getMonth() + 1, actualDate.getDate());

        var _formatMonthYear = nextMonth.getFullYear() + '年 ' + (nextMonth.getMonth() + 1) + '月';
        $('#h4MonthYear').html(_formatMonthYear);

        var _days = getDaysInMonth(nextMonth.getMonth(), nextMonth.getFullYear());
        fillMonthtoDropDown(_days);

        firstDayOfMonth = getfirstDayOfMonth(nextMonth.getMonth() + 1, nextMonth.getFullYear());
        getGameByWeek(2, firstDayOfMonth, firstDayOfMonth);
      
        event.preventDefault();
    });

    //dropdownlist date change
    $('#ddl_day').change(function (event) {

        strYearMonth = $('#h4MonthYear').html();
        year = strYearMonth.slice(0, 4)
        month = strYearMonth.slice(-3).replace("月", "").trim();
        month = (month < 10 ? '0' + month : month);
        var _value = $(this).val();
        if (_value == parseInt(_value, 10)) {

            firstDayOfMonth = year + '' + month + '' + _value;
            getGameByWeek(2, firstDayOfMonth, firstDayOfMonth);
        }

        event.preventDefault();
    });


    //Load div npbGameInfoWeek after select year, month, week.
    function getGameByWeek(type, startDate, endDate) {
        var linkUrl = urlCommon + '?type=' + type + '&startDate=' + firstDayOfMonth + '&endDate=' + firstDayOfMonth;
        $('#npbGameInfoWeek').load(linkUrl);
    }
});


//Get first date of month
function getfirstDayOfMonth(month, year) {

    var date = new Date(year, month, 1);
    var _month = (date.getMonth() + 1) < 10 ? '0' + (date.getMonth()) : (date.getMonth() + 1);
    return date.getFullYear() + '' + _month + '01';
}

//Get last date of month
//function getLastDayOfMonth(month, year) {

//    var lastDate = new Date(year, month, 0);
//    var _month = month < 10 ? '0' + month : month;
//    return lastDate.getFullYear() + '' + _month + '' + lastDate.getDate();
//}

// get all day in month
function getDaysInMonth(month, year) {
    var date = new Date(year, month, 1);
    var days = [];
    while (date.getMonth() === month) {
        days.push(new Date(date));
        date.setDate(date.getDate() + 1);
    }
    return days;
}
// fill data to dropdownlist day
function fillMonthtoDropDown(days) {
    //get id dropdownlist
    var sel = $("#ddl_day");

    //remove option in dropdownlist
    $('#ddl_day option[value!="0"]').remove();
    for (var i = 0; i <= days.length; i++) {
        var date = new Date(days[i]);
        if (!isNaN(date)) {
            var opt = document.createElement('option');
            opt.innerHTML = date.getDate() + '日';
            opt.value = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
            sel.append(opt);
            //check current month
            var today = new Date();
            if (today.getMonth() == date.getMonth()) {
                var dd = today.getDate() < 10 ? '0' + today.getDate() : today.getDate();
                sel.val(dd);
            }
            else {

                sel.prop("selectedIndex", 0);
            }
        }
    }
}