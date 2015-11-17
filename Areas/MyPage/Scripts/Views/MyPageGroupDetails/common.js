//
// MyPageGroupDetailのcommon.js
//
$(document).ready(function () {
    // 予想する click ------
    $(document).on('click', '.bbtn:not(.disable)', function (event) {
        event.preventDefault();
        var memberID;
        winH = $("body").height();
        winW = $("body").width();
        $.ajax({
            type: "GET",
            url: '/Npb/NpbTop/DefineLoginOrNot',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            async: false,
            success: function (result) {
                if (result != null) {
                    memberID = result;
                }
            },
        });

        //User not login : show popup login.
        if (memberID == "") {
            $('#overlay').css('height', winH);
            $('#overlay').css('width', winW);
            $("#overlay").show(300);
        }
            //User login : insert data to db.
        else {
            var odd = $(this).attr('data-odd');
            if (odd == "0") {
                $('#warningPopup').css('height', winH);
                $('#warningPopup').css('width', winW);
                $("#textPopup").text("Can not bet");
                $("#warningPopup").show(300);
            }
            else {
                //Get attribute for item click.
                var id = $(this).attr('data-id');
                var type = $(this).attr('data-type');
                var expectTarget = $(this).attr('data-expectTarget');
                var team = $(this).attr('data-team');
                var gameDate = $(this).attr('data-gameDate');
                var isChange;
                //Start prediction.
                $.ajax({
                    type: "POST",
                    url: '/Npb/NpbTop/StartPrediction',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ expectTargetID: expectTarget, gameID: id, betSelectID: type, memberID: memberID, teamID: team, gameDate: gameDate }),
                    async: false,
                    success: function (result) {
                        if (result == true) {
                            isChange = result;
                        }
                    },
                });
                if (isChange == true) {
                    //Change color for button and title.
                    $(this).addClass("change");
                    $(this).addClass("hasbet");
                    $(this).siblings('p').addClass("green");
                    $(this).siblings('p').addClass("green");
                    $(this).parent().siblings().find('.bbtn').addClass("disable");
                    $(this).parent().siblings().find('.bnum').addClass("gray");

                    //Load div pointPrediction
                    var linkUrl = "/RightContent/ShowExpectedPoints";
                    $('#show_expected_points').load(linkUrl);

                    //Continue change style of the same gameID.
                    var button = $('.' + id).children('.evt_bl01_2_2').find('.type' + type).find('.bbtn');
                    button.addClass("change");
                    button.siblings('p').addClass("green");
                    button.siblings('p').addClass("green");
                    button.parent().siblings().find('.bbtn').addClass("disable");
                    button.parent().siblings().find('.bnum').addClass("gray");
                }
            }
        }
    });

    $('.pointBet').autoNumeric('init', {
        vMax: '10000',
        vMin: '0',
        nBracket: null,
        mDec: null,
        wEmpty: 'zero',
    });

    $(document).on('keypress', '.pointBet', function (event) {
        var code = event.keyCode || event.which;
        if (code == 13) { //Enter keycode  
            $(".pointBet").blur();
        }
    });

    $(document).on('blur', '.pointBet', function () {
        debugger;
        var memberID = $("#pointPrediction").attr('data-member');
        //Test status of game, if status change.
        //Reload point.
        var newStatus = 0;
        var sportID = $(this).attr('data-sport');
        var id = $(this).attr('data-gameid');
        $.ajax({
            type: "POST",
            url: '/Npb/NpbTop/GetStatusByGameID',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ sportID: sportID, gameID: id, memberID: memberID }),
            cache: false,
            async: false,
            success: function (status) {
                if (status != null) {
                    newStatus = status;
                }
            },
        });
        if ((sportID == 1 && newStatus >= 1) || ((sportID == 2 || sportID == 3) && newStatus >= 4)) {
            //Show popup alert : game ongoing, not change point bet.
            $('#warningPopup').css('height', winH);
            $('#warningPopup').css('width', winW);
            $("#textPopup").text("締切り時刻を過ぎたため変更できません。");
            $("#warningPopup").show(300);

            //Load div pointPrediction
            var linkUrl = "/RightContent/ShowExpectedPoints";
            $('#show_expected_points').load(linkUrl);
        }
        else {
            var newPoint = parseInt($(this).val().split(',').join('').trim()) * 100;

            var oldPoint = $(this).attr('data-old');
            //Get total point in week
            var date = parseInt($(this).attr('data-date'));
            var totalPoint = parseInt($('#Point_' + date).text());
            var newTotal;
            winH = $("body").height();
            winW = $("body").width();

            if (newPoint == 0) {
                //Alert and set value to old value.
                $('#warningPopup').css('height', winH);
                $('#warningPopup').css('width', winW);
                $("#textPopup").text("予想には100ポイント以上必要です。");
                $("#warningPopup").show(300);
                $(this).val(oldPoint / 100);
            }
            else if (newPoint > 1000000) {
                $('#warningPopup').css('height', winH);
                $('#warningPopup').css('width', winW);
                $("#textPopup").text("ひとつの予想にかけられる上限は100万ポイントまでです。");
                $("#warningPopup").show(300);
                $(this).val(oldPoint / 100);
            }
            else {
                if (newPoint > totalPoint) {
                    //alert("所持ポイント以上は入力できません。");
                    $('#warningPopup').css('height', winH);
                    $('#warningPopup').css('width', winW);
                    $("#textPopup").text("所持ポイント以上は入力できません。");
                    $("#warningPopup").show(300);
                    $(this).val(oldPoint / 100);
                }
                else if (newPoint == oldPoint) {
                    //Not change.
                }
                else {
                    var remainPoint = totalPoint - newPoint;
                    if (remainPoint >= 0) {
                        var point = $(this).attr('data-point');
                        var expectPoint = $(this).attr('data-expectpoint');
                        //Continue update to db
                        $.ajax({
                            type: "POST",
                            url: '/Npb/NpbRightContent/UpdatePrediction',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ expectPointID: expectPoint, pointID: point, newPoint: newPoint, oldPoint: oldPoint }),
                            async: false,
                            success: function (result) {
                                if (result != null) {
                                    newTotal = result;
                                }
                            },
                        });
                    }
                    else {
                        //Don't enough point, set value to old value.                
                        $(this).val(oldPoint);
                    }
                    if (newTotal != null) {
                        //Update data-old
                        var tmp = parseInt($(this).attr('value')) * 100;
                        $(this).attr('data-old', tmp);
                        //Update totalpoint
                        $('#Point_' + date).text(newTotal);
                        //Update point if win
                        var odd = parseFloat($(this).attr('data-odd')).toFixed(2);
                        var tmp2 = tmp * odd;
                        $(this).parent().parent().siblings().children().text(tmp2);
                    }
                }
            }
        }

    });
    //取り消す click   
    $(document).on("click", ".side_b1_li", function (e) {
        e.preventDefault();
        var point = $(this).attr('data-point');
        var expectPoint = $(this).attr('data-expectpoint');
        var date = parseInt($(this).attr('data-date'));
        var oldPoint = $(this).siblings('.tmp').children().val();
        var deletedPoint = parseInt(oldPoint) * 100;
        var newTotal;
        //Cancel prediction
        $.ajax({
            type: "POST",
            url: '/Npb/NpbRightContent/CancelPrediction',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ expectPointID: expectPoint, pointID: point, deletedPoint: deletedPoint }),
            async: false,
            success: function (result) {
                if (result != null) {
                    newTotal = result;
                }
            },
        });
        if (newTotal != null) {
            //Remove div in html
            var currentId = $(this).parent().parent(".side_b2").attr('id');
            $('#' + currentId).empty();
            //Update total point in html.
            $('#Point_' + date).text(newTotal);
            debugger;
            //Get current url to define page for reload.
            var sportID = $(this).attr('data-sport');
            var url = window.location.href;
            var urlCommon;
            var pageGameInfo;
            var pageGameLeague;
            var pageGameInfoWeek;
            var pageGameInfoNextDate;
            var urlGetNextDate;
            var pageHome;
            var pTop2Home;
            var page4Common;
            var pageMoreGame;
            //Load pages in npb
            if (sportID == "1") {
                urlCommon = "detail/ShowGameInfo"; //"/Npb/NpbTop/ShowGameInfo";
                pageGameInfo = "npbGameInfo";
                pageGameLeague = "npbGameLeague";
                pageGameInfoWeek = "npbGameInfoWeek";
                pageGameInfoNextDate = "npbGameInfoNextDate";
                urlGetNextDate = "/Npb/NpbRightContent/GetNextDate";
                pageHome = "Page5";
                pTop2Home = "pTopNpb";
                page4Common = "Page4Npb";
                pageMoreGame = "npbMoreGame";
            }
                //Load pages in jleague
            else if (sportID == "2") {
                //Home Page : have 3 type sport.
                urlCommon = "detail/ShowGameInfo"; //"/Jleague/JlgTop/ShowGameInfo";
                pageGameInfo = "jlgGameInfo";
                pageGameLeague = "jlgGameLeague";
                pageGameInfoWeek = "jlgGameInfoWeek";
                pageGameInfoNextDate = "jlgGameInfoNextDate";
                urlGetNextDate = "/Jleague/JlgRightContent/GetNextDate";
                pageHome = "Page7";
                pageName = "jlgGameInfo";
                pTop2Home = "pTopJlg";
                page4Common = "Page4Jlg";
                pageMoreGame = "jlgMoreGame";
            }
                //Load pages in mlb
            else if (sportID == "3") {
                urlCommon = "detail/ShowGameInfo"; //"/Mlb/MlbTop/ShowGameInfo";
                pageGameInfo = "mlbGameInfo";
                pageGameLeague = "mlbGameLeague";
                pageGameInfoWeek = "mlbGameInfoWeek";
                pageGameInfoNextDate = "mlbGameInfoNextDate";
                urlGetNextDate = "/Mlb/MlbRightContent/GetNextDate";
                pageHome = "Page6";
                pTop2Home = "pTopMlb";
                page4Common = "Page4Mlb";
                pageMoreGame = "mlbMoreGame";
            }
            else {
                //Another sport.  
            }
            //Continue update div : npbGameInfo.
            var type = $("#Page").val();
            var gameID = $(this).attr('data-gameid').substring(0, 8);
            var flag = false;
            var linkUrl;
            switch (type) {
                case "1":
                    {
                        var gDate = $("#Page").attr('data-gamedate');
                        //Deleted in page that have gameID
                        if (sportID != 1) {
                            flag = true;
                            //Load if page 8-1, 9-1 have the same gameID.
                            var tmp = $(".mytitle").attr("id");
                            var group_id = tmp.split('-')[0];
                            var year = tmp.split('-')[1];
                            var month = tmp.split('-')[2];
                            linkUrl = urlCommon + '?type=' + type + '&gameDate=' + gDate + '&group_id=' + group_id + 'sports_id=' + sportID;
                        }
                        else {
                            if (gameID == gDate) {
                                flag = true;
                                //Load if page 8-1, 9-1 have the same gameID.
                                linkUrl = urlCommon + '?type=' + type + '&gameDate=' + gDate;
                            }
                        }
                        break;
                    }
                case "3":
                    {
                        var gDate = $("#Page").attr('data-gamedate');
                        if (sportID != 1) {
                            flag = true;
                            var teamID = $("#Page").attr('data-teamid');
                            //Load if page 8-5-1, 9-5-1 have the same gameID.
                            linkUrl = urlCommon + '?type=' + type + '&gameDate=' + gDate + '&teamID=' + teamID;
                        }
                        else {
                            if (gameID == gDate) {
                                flag = true;
                                var teamID = $("#Page").attr('data-teamid');
                                //Load if page 8-5-1, 9-5-1 have the same gameID.
                                linkUrl = urlCommon + '?type=' + type + '&gameDate=' + gDate + '&teamID=' + teamID;
                            }
                        }
                        break;
                    }
                case "4":
                    {
                        var gID = $("#Page").attr('data-gameid');
                        if (sportID != 1) {
                            flag = true;
                            //Load if page 8-6, 9-6 (Top) have the same gameID.
                            linkUrl = urlCommon + '?type=' + type + '&gameID=' + gID;
                        }
                        else {
                            if (gID == $(this).attr('data-gameid')) {
                                flag = true;
                                //Load if page 8-6, 9-6 (Top) have the same gameID.
                                linkUrl = urlCommon + '?type=' + type + '&gameID=' + gID;
                            }
                        }
                        break;
                    }
                default:
                    //Do nothing.
                    break;
            }
            if (flag) {
                $('#' + pageGameInfo).load(linkUrl);
            }
            //Continue update div npbGameInfoWeek if have. (8-2 Week)
            var type2 = $("#Page2").val();
            if (type2 == "2") {
                var sDate = $("#Page2").attr('data-startdate');
                var eDate = $("#Page2").attr('data-enddate');
                if (gameID >= sDate && gameID <= eDate) {
                    var linkUrl = urlCommon + '?type=' + type2 + '&startDate=' + sDate + '&endDate=' + eDate;
                    $('#' + pageGameInfoWeek).load(linkUrl);
                }
            }
            //Continue update div npbGameInfoNextDate if have (8-6 Bottom)
            var type3 = $("#Page3").val();
            if (type3 == "5") {
                var gDate = $("#Page3").attr('data-gamedate');
                var nextDate;
                $.ajax({
                    type: "POST",
                    url: urlGetNextDate,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ gameDate: gDate }),
                    async: false,
                    success: function (result) {
                        if (result != null) {
                            nextDate = result;
                        }
                    },
                });
                if (sportID != 1) {
                    linkUrl = urlCommon + '?type=' + type3 + '&gameDate=' + gDate;
                    $('#' + pageGameInfoNextDate).load(linkUrl);
                }
                else {
                    if (nextDate == gameID) {
                        linkUrl = urlCommon + '?type=' + type3 + '&gameDate=' + gDate;
                        $('#' + pageGameInfoNextDate).load(linkUrl);

                    }
                }
            }
            //Homepage Top 2
            var homeType = $('#' + pTop2Home).val();
            if (homeType == "6") {
                var lstGame = $('#' + pTop2Home).attr('data-listgame');
                var arrGames = lstGame.split(',');
                var gID = $(this).attr('data-gameid');
                $.each(arrGames, function (index, value) {
                    if (gID == value)
                        flag = true;
                });
                if (sportID == 2) {
                    var league = $('#' + pTop2Home).attr('data-league');
                    linkUrl = urlCommon + '?type=' + homeType + '&lstGameID=' + lstGame + '&leagueType=' + league;
                }
                else {
                    linkUrl = urlCommon + '?type=' + homeType + '&lstGameID=' + lstGame;
                }

                if (flag) {
                    $('#' + pageGameInfo).load(linkUrl);
                }
            }
            //Continue update div more_game if have (Home Page)
            var type4 = $('#' + page4Common).val();
            if (type4 == "6") {
                var isLoad = false;
                var lstGame = $('#' + page4Common).attr('data-listgame');
                var arrGames = lstGame.split(',');
                var gID = $(this).attr('data-gameid');
                $.each(arrGames, function (index, value) {
                    if (gID == value)
                        isLoad = true;
                });
                if (isLoad) {
                    if (sportID == 2) {
                        var league = $('#' + page4Common).attr('data-league');
                        linkUrl = urlCommon + '?type=' + type4 + '&lstGameID=' + lstGame + '&leagueType=' + league;
                    }
                    else {
                        linkUrl = urlCommon + '?type=' + type4 + '&lstGameID=' + lstGame;
                    }
                    $('#' + pageMoreGame).load(linkUrl);
                }
            }
            //Continue update div GameLeague if have (Home Page)
            var type5 = $('#' + pageHome).val();
            if (type5 == "4") {
                var gID = $('#' + pageHome).attr('data-gameid');
                if (gID == $(this).attr('data-gameid')) {
                    if (sportID == "2") {
                        linkUrl = urlCommon + '?type=' + type5 + '&gameID=' + gID + '&leagueType=' + league + '&link=1';
                    }
                    else {
                        linkUrl = urlCommon + '?type=' + type5 + '&gameID=' + gID + '&link=1';
                    }
                    $('#' + pageGameLeague).load(linkUrl);
                }
            }


        }
    });

    //Close popup warning if click button X.
    $("#closePopup").click(function () {
        $("#warningPopup").hide(300);
    });

    $(document).on('mouseover', '.bbtn', function () {
        $(this).addClass("change");
        $(this).siblings('p').addClass("green");
        $(this).siblings('p').addClass("green");
    });

    $(document).on('mouseout', '.bbtn', function () {
        if ($(this).hasClass("hasbet")) {
            // do nothing
        } else {
            $(this).removeClass("change");
            $(this).siblings('p').removeClass("green");
            $(this).siblings('p').removeClass("green");
        }
    });

    // submenu 
    $(".menu01 a").click(function (event) {
        $(this).parent().toggleClass("active");
        $("#menu01").toggle();
    });
    // Member 
    $(".menu02").click(function (event) {
        $("#menu02").toggle();
    });

    // pop up 
    winH = $("body").height();
    winW = $("body").width();
    $(".log_link_required").live("click", function () {
        $('#overlay').css('height', winH);
        $('#overlay').css('width', winW);
        $("#overlay").show(300);
    });
    $("#close").click(function () {
        $("#overlay").hide(300);
    });
    // submenu tab
    $("#section04").show();
    $(".tab-menu a").click(function (event) {
        event.preventDefault();
        $(this).parent().addClass("current");
        $(this).parent().siblings().removeClass("current");
        var tab = $(this).attr("href");
        $(".tab-section").not(tab).hide();
        $(tab).show();
    });
    // accordian
    $(".subcontent").hide();
    $(".board_list li a").click(function (event) {
        event.preventDefault();
        $(this).parent().toggleClass("active");
        $(this).siblings('.subcontent').toggle();
    });

    // datetime click------
    //$('.block_04_3_l1 li:not(.active) a').live('click', function (event) {
    //    event.preventDefault();
    //    $(this).parent().toggleClass("active");
    //    $(this).parent().siblings().removeClass("active");
    //});
    //$('.block_04_3_l2 li:not(.active) a').live('click', function (event) {
    //    event.preventDefault();
    //    $(this).parent().toggleClass("active");
    //    $(this).parent().siblings().removeClass("active");
    //});
    //$('.block_04_3_l3 li:not(.active) a').live('click', function (event) {
    //    event.preventDefault();
    //    $(this).parent().toggleClass("active");
    //    $(this).parent().siblings().removeClass("active");
    //});
    // yakyu team click -----------
    $('.yak_team li:not(.active) a').live('click', function (event) {
        event.preventDefault();
        $(this).parent().toggleClass("active");
        $(this).parent().siblings().removeClass("active");
    });
    // Jtabs -------
    $("#tab1").css('display', 'block');
    $('.jtabs li:not(.active) a').live('click', function (event) {
        event.preventDefault();
        $(this).parent().toggleClass("active");
        $(this).parent().siblings().removeClass("active");
        var tab = $(this).attr("href");
        $(".jflag").not(tab).css('display', 'none');
        $(tab).css('display', 'block');
    });
    $("#a01").show();
    $('.feed_tab a:not(.active)').live('click', function (event) {
        event.preventDefault();
        $(this).toggleClass("active");
        $(this).siblings().removeClass("active");
        var tab = $(this).attr("href");
        $(".jflag").not(tab).hide(300);
        $(tab).show(300);
    });
    // --- scroll to content
    $('.anchor').on('click', function (event) {
        event.preventDefault();
        var target = "#" + $(this).data('target');
        $('html, body').animate({
            scrollTop: $(target).offset().top
        }, 500);
    });

    // --- scroll to content
    $('.anchor1').on('click', function (event) {
        event.preventDefault();
        var target = "#" + $(this).data('target');
        var offposition = $(target).offset().top - 70;
        $('html, body').animate({
            scrollTop: offposition
        }, 500);
    });

    // ---8-5-4 tab selection// --- faq -list
    $("#feedtab1").css('display', 'block');
    $('.feedtab-list a').click(function (event) {
        event.preventDefault();
        $(this).addClass("active");
        $(this).siblings().removeClass("active");
        var tab = $(this).attr("href");
        $(".feedtab-contain").not(tab).css('display', 'none');
        $(tab).css('display', 'block');
    });
    // ---8-5-8 tab selection
    $('.feed-name li a').click(function (event) {
        event.preventDefault();
        $(this).parent().addClass("active");
        $(this).parent().siblings().removeClass("active");
        var tab = $(this).attr("href");
        $(".feedtab-contain").not(tab).css('display', 'none');
        $(tab).css('display', 'block');
    });

    // --- scroll to content
    $('.anchor1').on('click', function (event) {

        var target = "#" + $(this).data('target');
        var offposition = $(target).offset().top - 70;
        $('html, body').animate({
            scrollTop: offposition
        }, 500);
        event.preventDefault();
    });

    // --- faq -list
    $(".faq_list dd").hide();
    $(".faq_list dt").click(function (event) {
        event.preventDefault();
        $(this).toggleClass("active");
        $(this).siblings('dd').toggle();
    });
});


