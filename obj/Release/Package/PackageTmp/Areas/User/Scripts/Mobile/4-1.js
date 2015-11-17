var userTopDate = new Date();
var npbBar;
var jlgBar;
var mlbBar;
var npbPercent;
var jlgPercent;
var mlbPercent;

var inSidePos;
var outSidePos;


$(document).ready(function () {
	
     npbBar = $("#npb_correct_percentage_bar");
     jlgBar = $("#jlg_correct_percentage_bar");
     mlbBar = $("#mlb_correct_percentage_bar");
     npbPercent = $("#npb_correct_percentage_bar_text");
     jlgPercent = $("#jlg_correct_percentage_bar_text");
     mlbPercent = $("#mlb_correct_percentage_bar_text");

     inSidePos = 0;
     outSidePos = npbPercent.position().left + 100;

    getreport(0);

    //Click Month
    $('#prev_month').click(function (event) {
        event.preventDefault();
        getreport(-1);
    });
    $('#next_month').click(function (event) {
        event.preventDefault();
        getreport(1);
    });

    function getreport(monthDifference) {

        var otherMemberId = $("#usermonthlyreport").data("other_member_id");

        userTopDate.setMonth(userTopDate.getMonth() + monthDifference);
        var yearmonth = userTopDate.getFullYear() + "-" + (userTopDate.getMonth() + 1);
        $("#user_selected_month").html(userTopDate.getFullYear() + '年' + (userTopDate.getMonth() + 1) + '月');
        $("#usertopreportmonth").html(userTopDate.getMonth() + 1);

        var url = '/user/ChangeYearMonth/' + yearmonth + '/' + otherMemberId + '/';

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
                //DispGraph('#container_point_l', '#container_point_r', report, 2);
                //DispGraph('#container_number_l', '#container_number_r', report, 3);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    }

    function DispGraph(container_l, container_r, report, num) {


        var i;
        var sport_name = new Array();
        var sports_data = new Array();
        var summery_number = 0;
        var average_total = 0;
        var correctRatioTotal = 0;   // 全体の的中律
        var correctPointTotal = 0;   // 的中ポイント数合計
        var expectNumberTotal = 0;   // 予想数合計

        sports_data[0] = 0;
        sports_data[1] = 0;
        sports_data[2] = 0;

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
                    summery_number = Math.round(correctRatioTotal / expectNumberTotal).toLocaleString();
                else
                    summery_number = 0;

                sports_data[0] = (sports_data[0] == undefined) ? 0 : sports_data[0];
                sports_data[1] = (sports_data[1] == undefined) ? 0 : sports_data[1];
                sports_data[2] = (sports_data[2] == undefined) ? 0 : sports_data[2];

                $("#total_correct_percentage_month").html(userTopDate.getMonth() + 1);
                $("#total_correct_percentage").html(summery_number);

                var npb_bar_width = sports_data[0] == 0 ? 0 : sports_data[0];
                var jlg_bar_width = sports_data[1] == 0 ? 0 : sports_data[1];
                var mlb_bar_width = sports_data[2] == 0 ? 0 : sports_data[2];

                npbBar.attr("style", "width: " + npb_bar_width + "%;");
                jlgBar.attr("style", "width: " + jlg_bar_width + "%;");
                mlbBar.attr("style", "width: " + mlb_bar_width + "%;");

                npbPercent.html('<span class="num fs30">' + sports_data[0] + '</span>%');
                jlgPercent.html('<span class="num fs30">' + sports_data[1] + '</span>%');
                mlbPercent.html('<span class="num fs30">' + sports_data[2] + '</span>%');

                //0%のとき

                if (sports_data[0] == 0) {
                    npbPercent.position(outSidePos);
                    npbPercent.addClass("gray");
                } else {
                    npbPercent.removeClass("gray");
                }
                if (sports_data[1] == 0) {
                    jlgPercent.position(outSidePos);
                    jlgPercent.addClass("gray");
                } else {
                    jlgPercent.removeClass("gray");
                }
                if (sports_data[2] == 0) {
                    mlbPercent.position(outSidePos);
                    mlbPercent.addClass("gray");
                } else {
                    mlbPercent.removeClass("gray");
                }

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



    };

});