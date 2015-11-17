$(document).ready(function () {
    var urlCommon = "/User/UserExpectedList";
    var date = new Date();
    var page = $("#Page").val();
    //    var teamID = $("#TeamID").val();

    // 読み込み時
    var block = document.getElementsByClassName("block_04_3_l1");
    var active = block[0].getElementsByClassName("active");
    var year = active[0].textContent; // 2xxx
    block = document.getElementsByClassName("j_rank");
    active = block[0].getElementsByClassName("active");
    var month = active[0].textContent; // 3月
    getpoint(year, month);

    //Click Year
    $('.block_04_3_l1 li:not(.active) a').live('click', function (event) {
        event.preventDefault();
        $(this).parent().toggleClass("active");
        $(this).parent().siblings().removeClass("active");

        var year;
        year = event.srcElement.innerText;

        var block = document.getElementsByClassName("j_rank");
        var active = block[0].getElementsByClassName("active");
        var month = active[0].textContent; // 3月
        getpoint(year, month);

    });
    //Click Month
    $('.j_rank a:not(.active)').live('click', function (event) {
        event.preventDefault();
        $(this).toggleClass("active");
        $(this).siblings().removeClass("active");

        var month;
        month = event.srcElement.innerText;

        var block = document.getElementsByClassName("block_04_3_l1");
        var active = block[0].getElementsByClassName("active");
        var year = active[0].textContent; // 2xxx
        getpoint(year, month);
    });


    function getpoint(syear, smonth) {

        var year = syear.split(' ')[0];
        var month = smonth.split('月')[0];
        var yearmonth = year + "-" + month;

        var mytitle = document.getElementsByClassName("mytitle");
        //var user_id = mytitle[0].data("userid");

        var otherMemberId = $("#other_member_id").data("other_member_id");
        var user_id = otherMemberId;
        var url = '/user/expectedlist/ChangeYearMonth/' + yearmonth + '/' + otherMemberId + '/';

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ pyearmonth: yearmonth, user_id : user_id }),
            async: false,
            success: function (result) {

                // 試合リスト表示
                var gif = result.Games;
                var glists = $("#GameLists");
                var glist ="";
                for (i = 0; i < gif.length; i++)
                {
                    var game = gif[i];
                    var game_datetime = game.GameDateTime;
                    var game_name = game.GameName;
                    var expected_teamname = game.TeamName;
                    var game_comment = game.Comment;

                    // １・・試合2週間以上前 ベット不可、２/３・・試合前 ベット可、４/５・・試合開始5分前～試合開始　ベット不可
                    // ６/７・・試合中　ベット不可、８/９・・試合終了後、１０・・試合中止
                    var game_status = game.GameStatus;
                    var game_status2 = game.GameStatusSimple;    //０・・試合前、１・・試合中、２・・試合後

                    var game_point = String(game.Points).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1,'); // 1,234
                    var bed_point = game.Points / 100;
                    var game_url = game.Url;
                    var article_url = game.UrlArticle;
                    var sports_id = game.SportsID;
                    var game_id = game.GameID;
                    var game_odds = game.Odds;
                    var oddsid = game_odds * 100;

                    glist = glist +
                    '<tr class="block_04_5_r2" id="Tr1ExpectCancelGameID-' + sports_id + '-' + game_id + '-' + oddsid + '">';
                    // 試合中の場合
                    if (game_status == 6 || game_status == 7 )
                    {
                        glist = glist +
                        '<td><span class="panel_note02">' + game_datetime + '</span></td>';
                    }
                    else
                    {
                        glist = glist +
                        '<td class="fs10">' + game_datetime + '</td>';
                    }
                    glist = glist +
                    '<td><a href="' + game_url + '" target="_blank">' + game_name + '</a></td>';


                    if (game_status <= 3) // 試合前、１
                    {
                        glist = glist +
                        '<td>' + expected_teamname + '</td>';
                    }else if(game_status == 6 || game_status == 7)  //試合中
                    {
                            glist = glist +
                        '<td class="green">' + expected_teamname + '</td>';
                    } else {                                          //試合後
                            if (game.BetWin)
                            {
                            glist = glist +
                                '<td class="organge">' + expected_teamname + '</td>';
                            }else{
                            glist = glist +
                                    '<td>' + expected_teamname + '</td>';
                            }
                    }

                    //ベット関係
                    glist = glist + '<td>';
                   // if (game_status == 1  )   // １・・ベット不可
                   /** {
                        glist = glist +
                            '<span class="fs14 bold"></span>';
                    } else if (game_status == 2 || game_status == 3)    //試合前 ベット可
                    {
                        glist = glist +
                            '<input type="text" class="expect_point" id="ExpectPoint-' + sports_id + '-' + game_id + '-' + oddsid + '" value=' + bed_point + ' name="pprice" />' +
                            '<span class="fs14 bold">00 pt</span>';
                    }
                    else
                    {*/
                        glist = glist +
                        '<span class="fs14 bold">' + game_point + ' pt</span>';
                    //}
                    //
                    glist = glist +
                    '</td>' +
                    '</tr>' +
                    '<tr class="block_04_5_r3" id="Tr2ExpectCancelGameID-' + sports_id + '-' + game_id + '-' + oddsid + '">' +
                        '<td colspan="4">' +
                        '<a href="/user_article/new/5/' + sports_id + '/' + game_id + '"><span class="icon edit"> </span>記事を作成する</a>';

                    //予想のＷＩＮＬＯＳＥ // 
                    if (game_status >= 8 && game_status < 10) {
                        if (game.BetWin) {
                        glist = glist +
                        '<span class="panel_note03">WIN</span>';
                        } else (game.WinLose == 2)
                        {
                            glist = glist +
                            '<span class="panel_note04">LOSE</span>';
                        }
                    }

                    // 他ユーザなので、入力不可
                    //if (game_status == 1)   // １・・予想、２・・キャンセル、３・・中止、４・・結果確定
                    //    {
                    //        glist = glist +
                    //        '<a  class="expect_cancel" id="ExpectCancelGameID-' + sports_id + '-' + game_id + '-' + oddsid + '" href="#"><span class="icon remove" id="' + game_id + '"> </span>予想をやめる</a>';
                    //    }
                        glist = glist + 
                        '<span class="panel_note01" id="ExpectCommentGameID-' + sports_id + '-' + game_id + '">' + game_comment + '</span>';
                        glist = glist + 
                        '</td>' +
                    '</tr>';
                }
                glists.html(glist);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //                location.reload();
            }
        });
    }
});