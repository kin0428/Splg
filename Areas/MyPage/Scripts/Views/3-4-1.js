$(document).ready(function () {

    // 読み込み時
    var tmp = $(".mytitle").attr("id");
    var group_id = tmp.split('-')[0];
    var year = tmp.split('-')[1];
    var month = tmp.split('-')[2];
    reloadCorrect(group_id, year, month);

    // グループから抜ける
    $(".leavegroup").click(function (event) {
        var result = confirm("グループを抜けますか？")
        if (result == true) leaveGroup();
    });

    function leaveGroup() {

        var groupid;
        var eventTarget = event.target || event.srcElement;
        groupid = String(eventTarget.id);
        var url = '/mypage/MyPageGroupDetails/LeaveGroup/';

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ groupid: groupid }),
            async: false,
            success: function (result) {
                location = '/mypage/MyPageGroupList/'
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                location.reload();
            }
        });
    }

    //Click Month Ranking
    $('#SelectMonthRanking a:not(.active)').live('click', function (event) {
        $(this).toggleClass("active");
        $(this).siblings().removeClass("active");

        var year_month = $(this).attr("id");
        var group_id = year_month.split('-')[0];
        var year = year_month.split('-')[1];
        var month = year_month.split('-')[2];
        reloadRanking(group_id, year, month);
        event.preventDefault();
    });
    function reloadRanking(group_id, year, month) {

        var url = '/mypage/MyPageGroupDetails/JsonGetRanking/';

        var post_data = {
            group_id: parseInt(group_id),
            year: parseInt(year),
            month: parseInt(month)
        };
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(post_data),
            async: false,
            success: function (result) {

                var plists = $("#PointRanking");
                var plist = "";
                for (i = 0; i < result.length; i++) {
                    var nickNames = result[i].Nickname;
                    var member_image = result[i].ProfileImg;
                    var member_payofpoints = result[i].FormattedDisplayPoints;
                    var member_expect = result[i].FormattedLastExpectedPointDate == null ? '' : '最終予想 ' + result[i].FormattedLastExpectedPointDate;
                    var member_id = result[i].MemberId;
                    var member_url = result[i].userURL;
                    plist = plist +
                        '<div class="sub_list_row">' +
                            '<h4>' +
                            '<a href="' + member_url + '">' +
                                '<img class="circle resimg" src=' + member_image + ' alt="" /></a>' +
                                '<span class="fs14 blue"><a href="' + member_url + '">' + nickNames + '  </a></span>' +
                                '<span class="fs14">' + member_payofpoints + ' pt  </span>' +
                                '<span class="fs10 gray2"> ' + member_expect + '</span>' +
                            '</h4>' +
                        '</div>';
                }
                plists.html(plist);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                location.reload();
            }
        });
    }

    //Click Month Correct
    $('#SelectMonthCorrect a:not(.active)').live('click', function (event) {
        $(this).toggleClass("active");
        $(this).siblings().removeClass("active");

        var year_month = $(this).attr("id");
        var group_id = year_month.split('-')[0];
        var year = year_month.split('-')[1];
        var month = year_month.split('-')[2];
        reloadCorrect(group_id, year, month);
        event.preventDefault();
    });
    function reloadCorrect(group_id, year, month) {

        var url = '/mypage/MyPageGroupDetails/JsonGetCorrect/';

        var post_data = {
            group_id: parseInt(group_id),
            year: parseInt(year),
            month: parseInt(month)
        };
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(post_data),
            async: false,
            success: function (result) {
                var correct = result;
                if (correct != null)
                {
                    DispGraph(correct);
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                location.reload();
            }
        });
    }

    function DispGraph(correct) {

        var correctPercents = [];
        var iconHtmls = [];

        for (i = 0; i < correct.length; i++) {
            correctPercents[i] = correct[i].CorrectPercent;
            var memberId = correct[i].MemberId;
            var nickName = correct[i].Nickname;
            var ProfileImg = correct[i].ProfileImg;
            var top = (40 + 7) * i + 14;
            var iconHtml = '<div id="iconOnBar' + i + '" style="margin-right: 5px; height: 25px; width:25px; position: absolute;  top: ' + top + 'px;  left: 0px;z-index:90' + '">' +
                '<a class="" href="/user/' + memberId + '/"><img class="resimg circle" src="' + ProfileImg + '" alt="' + nickName + '"></a></div>';
            iconHtmls[i] = iconHtml;
        }

        $('#container').highcharts({

            // グラフを表示 横棒グラフ（％表示付）
            chart: {
                type: "bar",
                height: (40 + 7) * correct.length,
                spacing:[0,0,0,0],
                borderWidth: 0,
                backgroundColor: '#FFFFFF',
                plotBackgroundColor: '#FFFFFF',
            },
            // クレジット非表示
            credits: { enabled: false },

            // データ系列を作成
            // タイトルを指定
            title: { text: "" },
            // x軸のラベルを指定 表示しないためにはnullを指定すること
            xAxis: {
                gridLineWidth: 0,
                title: { text: "" },
                gridLineWidth: 0,
                labels: {
                    enabled: false
                }
            },
            // y軸のラベルを指定
            yAxis: {
                gridLineWidth: 0,
                title: { text: "" },
                gridLineWidth: 0,
                labels: {
                    enabled: false
                },
                min: 0,
                max: 100
            },

            //グラフにマウスオーバーすると出てくるポップアップの表示設定 表示無し
            tooltip: {
                enabled: false
            },

            //凡例の設定 表示無し
            legend: { enabled: false },

            // 棒グラフの幅
            plotOptions: {
                series: {
                    pointWidth: 40
                }
            },


            series: [{
                // 棒グラフの値をこのdataへ数値として並べる
                // 的中率を並べてください
                // 例：80％→80
                data: correctPercents,
                borderWidth: 3,
                color: '#FBEFE0', // 
                borderColor: '#E67F00', // 
                dataLabels: {
                    useHTML: true,
                    enabled: true,
                    align: 'right', // 
                    //                                           color: '#E67F00', // 文字の色現在は無指定
                    format: '<span style="margin-left: 30px;font-style: italic">{point.y}％</span>', // 的中率「％」
                    style: {
                        fontSize: '16px',
                        fontFamily: 'Verdana, sans-serif',
                        textShadow: 'none'
                    },
                    inside: { enabled: true }
                }
            }]

        }

        );

        for (i = 0; i < correct.length; i++) {
            var iconHtml = iconHtmls[i];
            $("#container").append(iconHtml);
        }

    }


    // 試合情報
    var urlCommon = "detail/ShowGameInfo";
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
        var tmp = $(".mytitle").attr("id");
        var group_id = tmp.split('-')[0];
        var year = tmp.split('-')[1];
        var month = tmp.split('-')[2];
        //Format date to string: yyyyMMdd
        var sDate = getDateFormatCompare(date);
        //Update for data-gamedate
        $("#Page").attr('data-gamedate', sDate);
        var linkUrl;

        linkUrl = urlCommon + '?type=' + type + '&gameDate=' + sDate + '&groupID=' + group_id + '&sports_id=1';
        $('#npbGameInfo').load(linkUrl);

        linkUrl = urlCommon + '?type=' + type + '&gameDate=' + sDate + '&groupID=' + group_id + '&sports_id=3';
        $('#mlbGameInfo').load(linkUrl);

        linkUrl = urlCommon + '?type=' + type + '&gameDate=' + sDate + '&groupID=' + group_id + '&sports_id=2';
        $('#jlgGameInfo').load(linkUrl);

        //Set text when gameDate change.
        $.ajax({
            type: "POST",
            url: '/mypage/MyPageGroupDetails/FormatGameDate',
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