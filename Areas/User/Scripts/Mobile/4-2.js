var userTopDate = new Date();

$(document).ready(function () {
    var urlCommon = "/User/UserExpectedList";
    var date = new Date();
    var page = $("#Page").val();
    //    var teamID = $("#TeamID").val();

    getpoint(0);

    $('#prev_month').click(function (event) {
        event.preventDefault();
        getpoint(-1);
    });
    $('#next_month').click(function (event) {
        event.preventDefault();
        getpoint(1);
    });

    function getpoint(monthDifference) {

        userTopDate.setMonth(userTopDate.getMonth() + monthDifference);
        var year = userTopDate.getFullYear();
        var month = userTopDate.getMonth() + 1;
        var yearmonth = year + "-" + month;

        var dt = new Date();
        var this_year = dt.getFullYear();
        var this_month = dt.getMonth() + 1;

        $("#user_selected_month").text(year + "年 " + month + "月");

        var otherMemberId = $("#other_member_id").data("other_member_id");
        var user_id = otherMemberId;
        var url = '/user/expectedlist/ChangeYearMonth/' + yearmonth + '/' + otherMemberId + '/';

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ pyearmonth: yearmonth , user_id : user_id}),
            async: false,
            success: function (result) {

                // 試合リスト表示
                var gif = result.Games;
                var glists = $("#GameLists");
                var glist = '<h3 class="my-title">予想リスト</h3>';
                var targetMonth = tmonth;
                if (gif.length > 0) {
                    targetMonth = gif[0].TargetMonth;
                }
                    var tmonth = targetMonth;
                    for(j = 0; j < gif.length; j++)
                    {
                    var game_betwin = gif[j].BetWin;
                    var game_date = gif[j].Date;
                    var game_time = gif[j].Time;
                    var game_name = gif[j].GameName;
                    var sports_id = gif[j].SportsID;
                    var expected_teamname = gif[j].TeamNameS;
                    var game_comment = gif[j].CommentS;
                    var game_status = gif[j].GameStatus;
                    var game_point = gif[j].Points;//.ToString("#,##0");
                    var bed_point = gif[j].Points;
                    var game_url = gif[j].Url;
                    var article_url = gif[j].UrlArticle;
                    var betSelectID = gif[j].BetSelectID;
                    var game_id = gif[j].GameID;
                    var game_odds = gif[j].Odds;
                    var oddsid = (parseInt(game_odds)).toString();
                    var hometeam = gif[j].HomeTeamS;
                    var visitorteam = gif[j].VisitorTeamS;
                    var homescore = gif[j].HomeScore;
                    var visitorscore = gif[j].VisitorScore;
                    var winner = "";
                    // game_status
                    // １・・試合2週間以上前 ベット不可、２/３・・試合前 ベット可、４/５・・試合開始5分前～試合開始　ベット不可
                    // ６/７・・試合中　ベット不可、８/９・・試合終了後、１０・・試合中止
                    if (betSelectID == 1) {
                        winner = "ホームの勝ち";
                    }
                    else if (betSelectID == 2) {
                        if (sports_id == sports_id) {
                            winner = "アウェイの勝ち";
                        }
                        else {
                            winner = "ビジターの勝ち";
                        }
                    }
                    else if (betSelectID == 3){
                        winner = "引き分け";
                    }
                    glist = glist + '<div class="panel-time">';
                    switch (game_status)      //4/5試合5分前～試合開始 6/7;試合中 
                    {
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                            glist = glist + '<div class="bg01"><p class="orange">予想締切</p></div>';
                            break;
                        default:
                            glist = glist + '<div class="bg02"><p>' + game_date + '<span>' + game_time + '</span></p></div>';
                            break;
                    }
                    glist = glist + '</div>' +
                    '<div class="panel-group blue">' +
                    '<div class="clear">' +
                    '<div class="panel-col1 fs15">' + hometeam + '</div>' +
                    '<div class="panel-col4">';
                    switch (game_status)      // 1;試合中 2:終了
                    {
                        case 6:
                        case 7:
                            glist = glist + '<span class="col01 fs36">' + homescore + '</span>' +
                            '<span class="col02 fs30">-</span>' +
                            '<span class="col01 fs36">' + visitorscore + '</span>';
                            break;
                        case 8:
                        case 9:
                            glist = glist + '<p>' +
                            '試合結果' +
                            '<br>' +
                            '<strong class="fs24">' + game_time + '</strong>' +
                            '</p>';
                            break;
                        case 10:
                            glist = glist + '<p>試合中止</p>';
                            break;
                        default:
                            glist = glist + '<p>' +
                            '試合開始' +
                            '<br>' +
                            '<strong class="fs24">' + game_time + '</strong>' +
                            '</p>';
                            break;
                    }
                    glist = glist + '</div>' +
                    '<div class="panel-col1 fs15">' + visitorteam + '</div>' +
                    '</div>';

                    var odds = game_odds.toFixed(1);

                    // １・・試合2週間以上前 ベット不可、２/３・・試合前 ベット可、４/５・・試合開始5分前～試合開始　ベット不可
                    // ６/７・・試合中　ベット不可、８/９・・試合終了後、１０・・試合中止

                    if ((game_status == 8 || game_status == 9) && game_betwin)   //WIN         // 1;試合中 2:終了
                    {

                        glist = glist + '<dl class="my-board1">' +
                        '<dt class="num fs15 orange">WIN</dt>' +
                        '<dd>' +
                        '<p class="my-col01">' + winner + '</p>' +
                        '<p class="my-col02">x<span class="fs24">' + odds + '</span></p>' +
                        '<p class="my-col03">' + game_point + ' pt</p>' +
                        '</dd>' +
                        '</dl>';
                    }
                    else if ((game_status == 8 || game_status == 9) && !game_betwin )   //LOSE         // 1;試合中 2:終了
                    {
                        glist = glist + '<dl class="my-board1 my-board2">' +
                        '<dt class="num fs15 gray">LOSE</dt>' +
                        '<dd>' +
                        '<p class="my-col01">' +winner + '</p>' +
                        '<p class="my-col02">x<span class="fs24">' + odds + '</span></p>' +
                        '<p class="my-col03">' +game_point + ' pt</p>' +
                        '</dd>' +
                        '</dl>';
                    }
                    else
                    {
                        glist = glist + '<div class="my-board">' +
                        '<p class="my-col01">' + winner + '</p>' +
                        '<p class="my-col02">x<span class="fs24">' + odds + '</span></p>';

                        if (game_status == 8 || game_status == 9)   // １・・ベット不可、２/３・・ベット可、８/９・・試合終了後、４/５/６/７/１０・・その他
                        {
                            glist = glist + '<p class="my-col03">' + game_point + ' pt</p>';
                        }
                        else if (game_status == 2 || game_status == 3) {
                            glist = glist + '<p class="my-col03">' + game_point + ' pt</p>';
                        }
                        else {
                            glist = glist + '<p class="my-col03"> </p>';
                        }
                        glist = glist + '</div>';
                    }
                    if ((game_status == 6 || game_status == 7))         // 1;試合中 2:終了
                    {
                        glist = glist + '<p class="my-link1"><a href="' + game_url + '">詳細を見る</a></p>';
                    }
                    else
                    {
                        glist = glist + '<p class="my-link2" id="ExpectCommentGameID-' + sports_id + '-' + game_id + '">' + game_comment + '</p>' +
                        '<p class="my-link3"><a href="/user_article/new/5/' + sports_id + '/' + game_id + '">この予想に関する記事を作成する</a></p>';
                        // 他ユーザなので、入力不可
                        //if (game_status == 1)   // １・・予想、２・・キャンセル、３・・中止、４・・結果確定
                        //{
                        //    glist = glist + '<p class="my-link4"><a id="ExpectCancelGameID-' + sports_id + '-' + game_id + '-' + oddsid + '" href="#">予想を削除する</a></p>';
                        //}
                    }
                    glist = glist + '</div>';
                }
                glists.html(glist);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //                location.reload();
            }
        });
    }
});