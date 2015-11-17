$(document).ready(function () {
    var urlCommon = "/User/UserTop";
    var date = new Date();
    var page = $("#Page").val();

    // 読み込み時
    var block = document.getElementsByClassName("block_04_3_l1");
    var active = block[0].getElementsByClassName("active");
    var year = active[0].textContent; // 2xxx
    block = document.getElementsByClassName("j_rank");
    active = block[0].getElementsByClassName("active");
    var month = active[0].textContent; // 3月
    getreport(year, month);

    //Click Year
    $('.block_04_3_l1 li:not(.active) a').live('click', function (event) {
        event.preventDefault();
        $(this).parent().toggleClass("active");
        $(this).parent().siblings().removeClass("active");

        var year;
        year = event.srcElement.innerText;

        var block = document.getElementById("mypage_top_monthes");
        var active = block.getElementsByClassName("active");
        var month = active[0].textContent; // 3月
        getreport(year, month);

    });
    //Click Month
    $('.j_rank a:not(.active)').live('click', function (event) {
        $(this).toggleClass("active");
        $(this).siblings().removeClass("active");

        var month = $(this).text().replace("月", "").trim();

        var block = document.getElementsByClassName("block_04_3_l1");
        var active = block[0].getElementsByClassName("active");
        var year = active[0].textContent; // 2xxx
        getreport(year, month);
        event.preventDefault();
    });

    function getreport(syear, smonth) {

        var other_member_id = $('#usermonthlyreport').data("other_member_id");

        var year = syear.split(' ')[0];
        smonth = smonth.trim();
        var month = smonth.split('月')[0];
        var yearmonth = year + "-" + month;

        var url = '/user/ChangeYearMonth/' + yearmonth + '/' + other_member_id + '/';

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ pyearmonth: yearmonth }),
            async: false,
            success: function (result) {
                var report = result;
                DispGraph('#container_percent_l', '#container_percent_r', report, 1);
                DispGraph('#container_point_l', '#container_point_r', report, 2);
                DispGraph('#container_number_l', '#container_number_r', report, 3);
                //                location.reload();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //                location.reload();
            }
        });
    }

    function DispGraph(container_l, container_r, report, num) {

        //
        $(container_l).empty();

        var i;
        var sport_name = new Array();
        var sports_data = new Array();
        var summery_number = 0;
        var average_total = 0;
        var correctRatioTotal = 0;   // 全体の的中律
        var correctPointTotal = 0;   // 的中ポイント数合計
        var expectNumberTotal = 0;   // 予想数合計
        var gtitle = "その他";
        var msg_unit = "";

        for (i = 0; i < report.length; i++) {
            sport_name[i] = report[i].SportsName;
            switch (num) {
                case 1:
                    //的中率 予想数合計と
                    sports_data[i] = report[i].CorrectPercent;   //スポーツごとの的中率
                    correctRatioTotal += report[i].ExpectNumber * report[i].CorrectPercent; // 的中数合計
                    expectNumberTotal += report[i].ExpectNumber; // 予想数合計
                    break;
                case 2:
                    //的中ポイント数
                    sports_data[i] = report[i].CorrectPoint; //スポーツごとの的中ポイント数
                    correctPointTotal += report[i].CorrectPoint; // 的中ポイント数合計
                    break;
                case 3:
                    //予想数合計
                    sports_data[i] = report[i].ExpectNumber;   //スポーツごとの予想数
                    expectNumberTotal += report[i].ExpectNumber; // 予想数合計
                    m_total = 1;
                    break;
                default:
                    break;
            }
        }

        switch (num) {
            case 1:
                gtitle = "的中率";
                if (expectNumberTotal > 0)
                    summery_number = Math.round(correctRatioTotal / expectNumberTotal).toLocaleString() + "%";
                else
                    summery_number = "―";
                msg_unit = '<br/><center>%</center>';  //PT 
                break;
            case 2:
                gtitle = "的中ポイント数";
                summery_number = correctPointTotal.toLocaleString() + "Pt";
                break;
            case 3:
                gtitle = "予想数";
                summery_number = expectNumberTotal.toLocaleString() + "回";
                break;
            default:
                break;
        }



        // 円の的中率を表示
        var renderer;

        renderer = new Highcharts.Renderer(
            $(container_l)[0],
            180,
            180
        );

        renderer.circle(80, 75, 60).attr({
            fill: '#FBEFE0',
            stroke: '#E67F00',
            'stroke-width': 3
        }).add();

        // 「的中率」 文字表示
        renderer.label('<center>' + gtitle + '</center>', 50, 35)
            .css({
                color: '#E67F00', // 
                fontSize: '18px'
            }).add();

        // 的中率の値をここで設定してください。
        renderer.label(summery_number, 45, 60)
            .css({
                color: '#E67F00', // 
                fontSize: '25px',
                fontFamily: 'Verdana, sans-serif',
                fontWeight: 'bold'
            }).add();

        $(container_r).highcharts({

            // グラフオを表示 縦棒グラフ（％表示付）
            chart: {
                type: "column",
                borderWidth: 0,
                backgroundColor: '#FFFFFF',
                plotBackgroundColor: '#FFFFFF'
            },
            // クレジット非表示
            credits: { enabled: false },

            // データ系列を作成
            // タイトルを指定
            title: { text: "" },
            // x軸のラベルを指定 表示しないためにはnullを指定すること
            xAxis: {
                title: { text: gtitle },
                style: {
                    fontSize: '20px',
                    fontFamily: 'Meiryo UI'
                },
                // グラフの下の文字
                categories: [sport_name[0], sport_name[1], sport_name[2], sport_name[3]]
            },
            // y軸のラベルを指定
            yAxis: {
                title: { text: "" },
                gridLineWidth: 0,
                labels: {
                    enabled: false
                }
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
                    pointWidth: 70
                }
            },

            series: [{
                //棒グラフの値をこのdataへ数値として並べる
                // 「プロ野球」,「MLB」, 「Ｊリーグ」, 「海外サッカー」の順
                // 例：80％→80
                data: [sports_data[0], sports_data[1], sports_data[2], sports_data[3]],
                borderWidth: 3,
                color: '#FBEFE0', //
                borderColor: '#E67F00', //
                dataLabels: {
                    useHTML: true,
                    enabled: true,
                    color: '#E67F00', //
                    //                    format: '{point.y}<br/><font size="1"><center>％</center></font>', // 的中率「％」
                    format: '{point.y}' + msg_unit, // 適切なものを表示 
                    style: {
                        fontSize: '16px',
                        fontFamily: 'Verdana, sans-serif',
                        textShadow: 'none'
                    },
                    inside: {
                        enabled: true
                    }
                }
            }]
        });
    };

});
function GetRelevantYearMonth(memberId, num) {
    var block = document.getElementsByClassName("block_04_3_l1");
    var active = block[0].getElementsByClassName("active");
    var year = active[0].textContent; // 2xxx
    block = document.getElementsByClassName("j_rank");
    active = block[0].getElementsByClassName("active");
    var month = active[0].textContent; // 3月
    year = year.trim();
    month = month.replace("月", "").trim();
    var relevantYearMonth = year + (parseInt(month) < 10 ? "0" : "") + month;

    var link = "/user_article/new/6/" + memberId + "/" + relevantYearMonth + "/" + num;
    document.location.href = "/user_article/new/6/" + memberId + "/" + relevantYearMonth + "/" + num;
}