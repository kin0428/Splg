﻿$(document).ready(function () {
    var urlCommon = "/Jleague/JlgTop/ShowGameInfo";
    var date = new Date();
    var page = $("#Page").val();
    var teamID = $("#TeamID").val();   
    var gdate = $("#Page").attr('data-gameDate')
    // ボタンの使用可、不可の切り替え
    changeNextPrevStat(gdate);

    //Click next to get gameinfo in next date.
    $(".next").click(function (event) {
        if (!$(".next").hasClass('disable')) {
            event.preventDefault();
        // 日付取得
        var gameDate = $("#Page").attr('data-gameDate');
        // 基準日設定
        gameDate = parseInt(gameDate) + 1;
        // GameKindId取得
        // URL取得
        var url = location.href;
        var gameKindId = 0;
        // leagueIdのセット
        // J1の場合：1
        if (url.indexOf('/j1') > 0) {
            gameKindId = 2
            // J2の場合:2
        } else if (url.indexOf('/j2') > 0) {
            gameKindId = 6
            // ナビスコカップの場合：3
        } else if (url.indexOf('/jleaguecup') > 0) {
            gameKindId = 4
        }
        $.ajax({
            type: "POST",
            url: '/Jleague/JlgTop/GetGameDate',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ dateInput: gameDate,gameKindId: gameKindId,dateType: 1 }),
            async: false,
            success: function (result) {
                if (result != null) {
                    $("#Page").attr('data-gameDate', result);
                }
            },
        });
        gdate = $("#Page").attr('data-gameDate')
        gmDate = new Date(gdate.substring(0, 4), gdate.substring(4, 6), gdate.substring(6, 8));
        changeNextPrevStat(gdate);
        var _nextDate = gmDate;
        getGameInfo(urlCommon, page, _nextDate);
        }
    });

    //Click previous to get game info in previous date.
    $(".prev").click(function (event) {
        if (!$(".prev").hasClass('disable')) {
            event.preventDefault();
            // 日付取得
            var gameDate = $("#Page").attr('data-gameDate');
            // 基準日設定
            gameDate = parseInt(gameDate) - 1;
            // GameKindId取得
            // URL取得
            var url = location.href;
            var gameKindId = 0;
            // leagueIdのセット
            // J1の場合：1
            if (url.indexOf('/j1') > 0) {
                gameKindId = 2
                // J2の場合:2
            } else if (url.indexOf('/j2') > 0) {
                gameKindId = 6
                // ナビスコカップの場合：3
            } else if (url.indexOf('/jleaguecup') > 0) {
                gameKindId = 4
            }
            $.ajax({
                type: "POST",
                url: '/Jleague/JlgTop/GetGameDate',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ dateInput: gameDate, gameKindId: gameKindId, dateType: 2 }),
                async: false,
                success: function (result) {
                    if (result != null) {
                        $("#Page").attr('data-gameDate', result);
                    }
                },
            });
            gdate = $("#Page").attr('data-gameDate')
            gmDate = new Date(gdate.substring(0, 4), gdate.substring(4, 6), gdate.substring(6, 8));
            changeNextPrevStat(gdate);
            var _prevDate = gmDate;
            getGameInfo(urlCommon, page, _prevDate);
        }
    });

    function getGameInfo(urlCommon, type, date) {
        // 初期値（JリーグTOPの場合：0）
        var leagueType = 0;

        // URL取得
        var url = location.href;

        // leagueIdのセット
        // J1の場合：1
        if (url.indexOf('/j1') > 0) {
            leagueType = 1
            // J2の場合:2
        } else if (url.indexOf('/j2') > 0) {
            leagueType = 2
            // ナビスコカップの場合：3
        } else if (url.indexOf('/jleaguecup') > 0) {
            leagueType = 3
        }

        var sDate = $("#Page").attr('data-gameDate');
        var linkUrl;
        if (type == 1)
        {
            linkUrl = urlCommon + '?type=' + type + '&gameDate=' + sDate + '&leagueType=' + leagueType;
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
        // 日付によってクリック可能かどうかを変更
    function changeNextPrevStat(date) {
        var firstDate = $("#Page").attr('data-firstgamedate');
        var lastDate = $("#Page").attr('data-lastgamedate');
        // 最初の日付だった場合は、前の試合は使用不可に。
        if (date <= firstDate) {
            $(".prev").addClass('disable');
        } else if (date >= lastDate) {
            $(".next").addClass('disable');
        } else {
            if ($(".next").hasClass('disable')) {
                $(".next").removeClass('disable');
            }
            if ($(".prev").hasClass('disable')) {
                $(".prev").removeClass('disable');
            }
        }
    }


});