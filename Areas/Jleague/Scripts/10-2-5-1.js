$(document).ready(function () {
    var urlCommon = "/Jleague/JlgTop/ShowGameInfo";
    var date = new Date();
    var page = $("#Page").val();
    var teamID = $("#TeamID").val();   

    //Click next to get gameinfo in next date.
    $(".board_next").click(function (event) {
        date.setDate(date.getDate() + 1);
        var _nextDate = date;
        getGameInfo(urlCommon, page, _nextDate);
    });

    //Click previous to get game info in previous date.
    $(".board_prev").click(function (event) {
        date.setDate(date.getDate() - 1);
        var _prevDate = date;
        getGameInfo(urlCommon, page, _prevDate);
    });

    function getGameInfo(urlCommon, type, date) {
        //Format date to string: yyyyMMdd
        var sDate = getDateFormatCompare(date);
        //Update for data-gamedate
        $("#Page").attr('data-gamedate', sDate);
        var linkUrl;
        if (type == 1)
        {
            linkUrl = urlCommon + '?type=' + type + '&gameDate=' + sDate;
        }
        else if(type == 3)
        {
            linkUrl = urlCommon + '?type=' + type + '&gameDate=' + sDate + '&teamID=' + teamID;
        }       
        $('#jlgGameInfo').load(linkUrl);

        //Set text when gameDate change.
        $.ajax({
            type: "POST",
            url: '/Jleague/JlgTop/FormatGameDate',
            contentType: "application/json; charset=utf-8",
            dataType: "json",          
            data: JSON.stringify({ date: sDate }),
            async: false,
            success: function (result) {
                if (result != null) {
                    $("#gameDate").text(result);
                }
            },
        });      
    }

    function getDateFormatCompare(d) {
        var _month = d.getMonth() < 10 ? '0' + (d.getMonth() + 1) : (d.getMonth() + 1);
        var _date = d.getDate() < 10 ? '0' + d.getDate() : d.getDate();
        return d.getFullYear() + '' + _month + '' + _date;
    }

});