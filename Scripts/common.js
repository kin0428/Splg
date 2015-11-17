
//スポログフロントアプリ
//-----------------------
var SpologApp = new Object();

// 所持ポイントの更新メッセージング
//------------------------------------
SpologApp.POSESSION_CHANGE_SOURCE_TYPES = new Object();
SpologApp.POSESSION_CHANGE_SOURCE_TYPES.PREDICTION = 1;
SpologApp.POSESSION_CHANGE_SOURCE_TYPES.PRIZE = 2;

SpologApp.possessionPointChangeHandlers = {};
SpologApp.onPosessionPointChanged =  function (source) {
	   
    var handlers = SpologApp.possessionPointChangeHandlers;
    if (handlers == null || handlers.length == 0)
        return;
    
    for (i = 0; i < handlers.length; i++) {
        handlers[i](source);
    }
}


//予想が変更されたとき、右カラムの所持ポイントと予想を更新
function onExpectationChanged() {

    //Load div pointPrediction
    var linkUrl = "/RightContent/ShowExpectedPoints";
    $('#show_expected_points').load(linkUrl);

}

//所持ポイント数が変更されたとき、ほかのコンポーネントに通知
function onPosessionChanged() {

    SpologApp.onPosessionPointChanged(SpologApp.POSESSION_CHANGE_SOURCE_TYPES.PREDICTION);

}


//
//----------------------------------------------------------------
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
            cache : false,
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

                //Get total point
                var possesionPoint;
                $.ajax({
                    type: "GET",
                    url: "/Npb/NpbTop/GetPossesionPoint",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    async: false,
                    success: function (point) {
                        if (point != null) {
                            possesionPoint = point;
                        }
                    }
                });

                if (possesionPoint < 100) {
                    $('#warningPopup').css('height', winH);
                    $('#warningPopup').css('width', winW);
                    $("#textPopup").text("予想には100ポイント以上必要です。");
                    $("#warningPopup").show(300);
                    return;
                }

                var isBet = $(this).hasClass('hasbet');
                if (!isBet)
                {
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
                        $(this).find('span').text('予想中');
                        $(this).parent().siblings().find('.bbtn').addClass("disable");
                        $(this).parent().siblings().find('.bnum').addClass("gray1");

                        //予想が変更されたとき、右カラムの所持ポイントと予想を更新
                        onExpectationChanged();

                        //所持ポイントをほかのコンポーネントに通知
                        onPosessionChanged();

                        //Continue change style of the same gameID.
                        var button = $('.' + id).children('.evt_bl01_2_2').find('.type' + type).find('.bbtn');
                        button.addClass("change");
                        button.siblings('p').addClass("green");
                        button.siblings('p').addClass("green");
                        button.parent().siblings().find('.bbtn').addClass("disable");
                        button.parent().siblings().find('.bnum').addClass("gray1");
                    } else {
                        alert("処理に失敗しました。");
                    }
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
        aSep: ''
    });

    $(document).on('keypress', '.pointBet', function (event) {
        var code = event.keyCode || event.which;
        if (code == 13) { //Enter keycode  
            $(this).blur();
        }
    });

    $(document).on('blur', '.pointBet', function () {       
        updatePoint(this);        
    });

    function formatNumber(num) {
        return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
    }

    function updatePoint(textbox)
    {
        var memberID = $("#pointPrediction").attr('data-member');
        //Test status of game, if status change.
        //Reload point.

        var newStatus = 0;
        var sportID = $(textbox).attr('data-sport');
        var id = $(textbox).attr('data-gameid');
        $.ajax({
            type: "POST",
            url: '/Npb/NpbTop/GetStatusByGameID',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ sportID: sportID, gameID: id, memberID : memberID }),
            cache: false,
            async: false,
            success: function (status) {
                if (status != null) {
                    newStatus = status;
                }
            },
        });
        if ((sportID == 1 && newStatus >= 1) || ((sportID == 2 || sportID == 3) && newStatus >= 4))
        {
            //Show popup alert : game ongoing, not change point bet.
            $('#warningPopup').css('height', winH);
            $('#warningPopup').css('width', winW);
            $("#textPopup").text("締切り時刻を過ぎたため変更できません。");
            $("#warningPopup").show(300);

            //予想が変更されたとき、右カラムの所持ポイントと予想を更新
            onExpectationChanged();
        }
        else
        {
            var newPoint = parseInt($(textbox).val().split(',').join('').trim()) * 100;
            var oldPoint = $(textbox).attr('data-old');

            //Get total point in week
            var date = parseInt($(textbox).attr('data-date'));
            var totalPoint = parseInt($('#PossessionPoint').text().replace(',', ''));
            var actualTotalPoint = 0;
            var newTotal;
            winH = $("body").height();
            winW = $("body").width();
            debugger;
            if (newPoint == 0) {
                //Alert and set value to old value.
                $('#warningPopup').css('height', winH);
                $('#warningPopup').css('width', winW);
                $("#textPopup").text("予想には100ポイント以上必要です。");
                $("#warningPopup").show(300);
                $(textbox).val(oldPoint / 100);
            }
            else if (newPoint > 1000000) {
                $('#warningPopup').css('height', winH);
                $('#warningPopup').css('width', winW);
                $("#textPopup").text("ひとつの予想にかけられる上限は100万ポイントまでです。");
                $("#warningPopup").show(300);
                $(textbox).val(oldPoint / 100);
            }
            else {
                actualTotalPoint = totalPoint + parseInt(oldPoint);
                if (newPoint > actualTotalPoint) {
                    $('#warningPopup').css('height', winH);
                    $('#warningPopup').css('width', winW);
                    $("#textPopup").text("所持ポイント以上は入力できません。");
                    $("#warningPopup").show(300);
                    $(textbox).val(oldPoint / 100);
                }
                else if (newPoint == oldPoint) {
                    //Not change.
                }
                else {
                    var remainPoint = actualTotalPoint - newPoint;
                    if (remainPoint >= 0) {
                        var pointId = $(textbox).attr('data-point');
                        var expectPoint = $(textbox).attr('data-expectpoint');
                        //Continue update to db
                        $.ajax({
                            type: "POST",
                            url: '/Npb/NpbRightContent/UpdatePrediction',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ expectPointID: expectPoint, pointID: pointId, newPoint: newPoint, oldPoint: oldPoint }),
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
                        $(textbox).val(oldPoint / 100);
                    }
                    if (newTotal != null) {

                        //所持ポイントをほかのコンポーネントに通知
                        onPosessionChanged();

                        //Update data-old
                        var tmp = parseInt($(textbox).attr('value')) * 100;
                        $(textbox).attr('data-old', tmp);
                        var newTotalFormatted = formatNumber(newTotal[0]);
                        //Update totalpoint
                        $('#PossessionPoint').text(newTotalFormatted);
                        //Update point if win
                        var odd = parseFloat($(textbox).attr('data-odd')).toFixed(1);
                        var tmp2 = formatNumber(Math.round(tmp * odd));
                        $('#TotalPoint_' + id).text(tmp2);
                        //$(textbox).parent().parent().siblings().children().text(tmp2);
                    }
                }
            }
        }
    }

    //取り消す click   
    $(document).on("click", ".side_b1_li", function (e) {
        e.preventDefault();
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

            //予想が変更されたとき、右カラムの所持ポイントと予想を更新
            onExpectationChanged();
        }
        else {
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
                //所持ポイントをほかのコンポーネントに通知
                onPosessionChanged();

                //Remove div in html
                var currentId = $(this).parent().parent(".side_b2").attr('id');
                $('#' + currentId).empty();
                //Update total point in html.
                var newTotalFormatted = formatNumber(newTotal);
                $('#Point_' + date).text(newTotalFormatted);               
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
                    urlCommon = "/Npb/NpbTop/ShowGameInfo";
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
                    urlCommon = "/Jleague/JlgTop/ShowGameInfo";
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
                    urlCommon = "/Mlb/MlbTop/ShowGameInfo";
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
                                //Jリーグの節等の対応
                                var isJlgscheduleresult = $("#Page").attr('data-jlgscheduleresult');

                                if (isJlgscheduleresult) {
                                    flag = true;
                                    var occasionNo = $("#Page").attr('data-occasion');
                                    var round = $("#Page").attr('data-round');
                                    var leagueType = $("#Page").attr('data-leaguetype');

                                    linkUrl = urlCommon + '?type=' + 2 + '&occasionNo=' + occasionNo + '&round=' + round + '&leagueType=' + leagueType;
                                } else {
                                    flag = true;
                                    //Load if page 8-1, 9-1 have the same gameID.
                                    linkUrl = urlCommon + '?type=' + type + '&gameDate=' + gDate;
                                }
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


                    //予想が変更されたとき、右カラムの所持ポイントと予想を更新
                    onExpectationChanged();

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
	$('.feedtab-list a').click(function(event) {
		event.preventDefault();
		$(this).addClass("active");
		$(this).siblings().removeClass("active");
        var tab = $(this).attr("href");
        $(".feedtab-contain").not(tab).css('display', 'none');
        $(tab).css('display', 'block');
	});
	// ---8-5-8 tab selection
	$('.feed-name li a').click(function(event) {
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

    //Popup
	$(".outside_overclay").css('height', winH);
	$(".outside_overclay").css('width', winW);
	$(".popclose").click(function (event) {	   
	    event.preventDefault();
	    closePopupTutorial();
	});	
});

function closePopupTutorial() {   
    $(".outside_overclay").hide();
    $(".add_popup").hide();
}




