var mypageTopDate = new Date();

$(document).ready(function () {


    $('.bxslider').bxSlider({
        slideWidth: 35,
        slideMargin: 5,
        minSlides: 1,
        maxSlides: 7,
        moveSlides: 1,
    });

    var urlCommon = "/MyPage/MyPageGroupDetails";
    var date = new Date();
    var page = $("#Page").val();

    // 読み込み時
    var tmp = $(".my-title").attr("id");
    var group_id = tmp.split('-')[0];
    var year = tmp.split('-')[1];
    var month = tmp.split('-')[2];
    reloadRanking(group_id, 0);

    $('#prev_month').click(function (event) {
        event.preventDefault();
        var tmp = $(".my-title").attr("id");
        var group_id = tmp.split('-')[0];
        var year = tmp.split('-')[1];
        var month = tmp.split('-')[2];
        reloadRanking(group_id, -1);
    });
    $('#next_month').click(function (event) {
        event.preventDefault();
        var tmp = $(".my-title").attr("id");
        var group_id = tmp.split('-')[0];
        var year = tmp.split('-')[1];
        var month = tmp.split('-')[2];
        reloadRanking(group_id, 1);
    });

    function reloadRanking(group_id, monthDifference) {

        mypageTopDate.setMonth(mypageTopDate.getMonth() + monthDifference);
        $("#mypagetopreportmonth").html(mypageTopDate.getFullYear() + '年' + (mypageTopDate.getMonth()+1) + '月'); 

        var url = '/mypage/MyPageGroupDetails/JsonGetRanking/';

        var year = mypageTopDate.getFullYear();
        var month = mypageTopDate.getMonth() + 1;   // getMonth():0-11
        var post_data = {
            group_id: parseInt(group_id),
            year: year,
            month: month
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
                var orange = "orange";
                var plist = "";
                    for (i = 0; i < result.length; i++) {
                    var member_name = result[i].Nickname;
                    var member_image = result[i].ProfileImg;
                    var member_payofpoints = result[i].FormattedDisplayPoints;
                    var member_id = result[i].MemberId;
                    var member_url = result[i].userURL;
                    plist = plist +
                    '<li>' +
                        '<p class="num fs24 ' + orange + '">' + (i+1) + '</p>' +
                        '<div><a href=' + member_url + '><img class="circle" src=' + member_image + ' alt="" /></a></div>' +
                        '<dl>' +
                            '<dt><a href=' + member_url + '>' + member_name + '</a></dt>' +
                            '<dd><span class="fs15">' + member_payofpoints + '</span> pt</dd>' +
                        '</dl>' +
                    '</li>';
                    orange = "";
                    }
                plists.html(plist);

                reloadCorrect(group_id, year, month, result)
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                location.reload();
            }
        });
    }

    function reloadCorrect(group_id, year, month, result) {

        var correct = result;
        if (correct != null) {
            DispGraph(correct);
        }
    }

    function DispGraph(correct) {

        var percent_data = [];
        var member_name = [];

        for (i = 0; i < correct.length; i++) {
            percent_data[i] = correct[i].CorrectPercent;
            member_name[i] = correct[i].Nickname;
        }
        for (i = 0; i < percent_data.length; i++) {
            var bpb_bar_width = percent_data[i] == 0 ? 5 : percent_data[0];

            $("#correct_percentage_bar_text_" + (i + 1).toString()).html(percent_data[i]);
            //$("#correct_percentage_bar_" + (i + 1).toString()).width(bpb_bar_width + '%');
            $("#correct_percentage_bar_" + (i + 1).toString()).attr("style","width: " + bpb_bar_width + "%;");
        }

        //$('#container').highcharts({

        //    // グラフを表示 横棒グラフ（％表示付）
        //    chart: {
        //        type: "bar",
        //        borderWidth: 0,
        //        backgroundColor: '#FFFFFF',
        //        plotBackgroundColor: '#FFFFFF',
        //    },
        //    // クレジット非表示
        //    credits: { enabled: false },

        //    // データ系列を作成
        //    // タイトルを指定
        //    title: { text: "" },
        //    // x軸のラベルを指定 表示しないためにはnullを指定すること
        //    xAxis: {
        //        title: { text: "" },
        //        gridLineWidth: 0,
        //        labels: {
        //            enabled: false
        //        }
        //    },
        //    // y軸のラベルを指定
        //    yAxis: {
        //        title: { text: "" },
        //        gridLineWidth: 0,
        //        labels: {
        //            enabled: false
        //        }
        //    },

        //    //グラフにマウスオーバーすると出てくるポップアップの表示設定 表示無し
        //    tooltip: {
        //        enabled: false
        //    },

        //    //凡例の設定 表示無し
        //    legend: { enabled: false },

        //    // 棒グラフの幅
        //    plotOptions: {
        //        series: {
        //            pointWidth: 40
        //        }
        //    },


        //    series: [{
        //        // 棒グラフの値をこのdataへ数値として並べる
        //        // 的中率を並べてください
        //        // 例：80％→80
        //        data: percent_data,
        //        borderWidth: 3,
        //        color: '#FBEFE0', // 
        //        borderColor: '#E67F00', // 
        //        dataLabels: {
        //            useHTML: true,
        //            enabled: true,
        //            align: 'right', // 
        //            //                                           color: '#E67F00', // 文字の色現在は無指定
        //            format: '<span style="font-style: italic">{point.y}％</span>', // 的中率「％」
        //            style: {
        //                fontSize: '16px',
        //                fontFamily: 'Verdana, sans-serif',
        //                textShadow: 'none'
        //            },
        //            inside: { enabled: true }
        //        }
        //    }]

        //});
    }

        // 試合情報
    var urlCommon = "mypage/MyPageGroupDetails/ShowGameInfo";
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
        if (type == 1) {
            linkUrl = urlCommon + '?type=' + type + '&gameDate=' + sDate + '&groupID=' + group_id;
        }
        else if (type == 3) {
            linkUrl = urlCommon + '?type=' + type + '&gameDate=' + sDate + '&teamID=' + teamID;
        }
        $('#npbGameInfo').load(linkUrl);

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