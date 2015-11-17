//Flag used for showing popup6, 7 when open in the first time.
var flgPopup6 = false;
var flgPopup7 = false;
var numClick = 0;
var urlCommon = "/mypage/today/ShowExpectedListForPC";
//右カラムの現在の所持ポイントロード用。
//試合情報パネルは表示しない
var linkUrl = "/RightContent/ShowExpectedPoints?showCurrentPointInfoOnly=true";

var date = new Date();

$(document).ready(function () {

    //Show tutorial 6 when open the first time.
    showPopup6();

    $("#nextEvent").click(function () {
        numClick++;
        if (flgPopup6 == true && flgPopup7 == false) {
            showPopup7();
        }
        if (flgPopup6 == true && flgPopup7 == true) {
            if (numClick == 2) {
                closePopupTutorial();
                numClick = 0;
            }
        }
    });
    //Click 予想をやめる
    $('.expect_cancel').live('click', function (event) {
        $(this).live('click', function (event) {
            alert('只今処理中です。\nそのままお待ちください。');
            return false;
        });
        event.preventDefault();
        expectcancel();
        getpoint();
    });

    function expectcancel() {

        var cancelid = String(event.target.id);
        var sports_id = cancelid.split('-')[1];
        var game_id = cancelid.split('-')[2];
        var game_odds = cancelid.split('-')[3];
        var url = '/mypage/today/ExpectCancel/' + game_id + '/';

        var post_data = {
            SportsID: sports_id,
            GameID: game_id,
            ExpectPoint: 0
        };

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(post_data),
            async: false,
            success: function (result) {

                var plists = $("#Tr1" + cancelid);
                if (plists != null) {
                    plists.remove();
                }
                var plists = $("#Tr2" + cancelid);
                if (plists != null) {
                    plists.remove();
                }

                $("#ExpectPoint-" + sports_id + "-" + game_id + "-" + game_odds).val("0");

                var odds = (game_odds / 100).toFixed(1);

                //var text = $(this).text();
                //$("input").val(text);
                //                document.getElementById("ExpectPoint-" + game_id)[0].value = 0;
                //var inputtag = $("#ExpectPoint-" + game_id);
                //inputtag.value = 0;

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                location.reload();
            }
        });
    }

    //Click 予想ポイントの入力
    $('.expect_point').live('change', function (event) {
        $(this).live('change', function (event) {
            alert('只今処理中です。\nそのままお待ちください。');
            return false;
        });
        expectpoint(event);
        getpoint();
    });

    function expectpoint(event) {

        var bedPoint = $(event.target).attr('data-bedpoint');
        var pointid = String(event.target.id);
        var sports_id = pointid.split('-')[1];
        var game_id = pointid.split('-')[2];
        var game_odds = pointid.split('-')[3];
        var game_point = String(event.target.value);
        var url = '/mypage/today/ExpectPoint/' + game_id + '/';

        var post_data = {
            SportsID: sports_id,
            GameID: game_id,
            ExpectPoint: game_point,
            OldExpectPoint: bedPoint
        };
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(post_data),
            async: false,
            success: function (result) {

                if (result.HasError) {
                    $(event.target).attr('value', String(bedPoint));
                    $(event.target).attr('data-bedpoint', String(bedPoint));
                    toastr.error("予想ポイントを設定できませんでした。" + result.Message);
                } else {

                    var odds = (game_odds / 100).toFixed(1);
                    $(event.target).attr('value', game_point);
                    $(event.target).attr('data-bedpoint', game_point);

                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                location.reload();
            }
        });
    }

    function getpoint() {

        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();
        // getMonthは0～11がとれるくさい。のでmonthは+1してるが、月としては元の数値が正しいので。。。
        var yesterday = new Date(year, month - 1, day - 1);
        var lastyear = yesterday.getFullYear();
        var lastmonth = yesterday.getMonth() + 1;
        var lastday = yesterday.getDate();
         
        var urlTdy = urlCommon + '?target_year=' + year + '&target_month=' + month + '&target_date=' + day;
        $('#group02').load(urlTdy);
        var urlYdy = urlCommon + '?target_year=' + lastyear + '&target_month=' + lastmonth + '&target_date=' + lastday;
        $('#group01').load(urlYdy);

        //Load div pointPrediction
        $('#show_expected_points').load(linkUrl);

    }
});

function showPopup6() {
    var strKeyPopup6 = "splg_tutorial06";
    if (docCookies.getItem(strKeyPopup6) == null) {
        //Change content of popup1 -> popup3
        $('#popTitle').html('試合結果確認');
        $('#content01').html('試合結果ページでは、これまでに予想した試合の結果を確認することができます。<br>見事的中した場合は、倍率に応じたポイントが獲得できます。');
        $('#content02').html('「スポログ」では試合予想のヒントになるスタメン情報や新着ニュース、試合速報の他に、コアなスポーツファンの投稿記事なども見る事ができます！是非参考にしてください！');
        $('#content03').html('');
        $('.add_image').hide();
        $('#nextEvent a').text('ポイントの使い方');

        flgPopup6 = true;

        //Show popup6
        $(".outside_overclay").css('display', 'block');
        //$(".outside_overclay").show();
        $(".add_popup").show();

        //Insert cookie popup6
        docCookies.setItem(strKeyPopup6, "true", Infinity);
    }
}

function showPopup7() {
    var strKeyPopup7 = "splg_tutorial07";
    if (docCookies.getItem(strKeyPopup7) == null) {
        //Change content of popup6 -> popup7
        $('#popTitle').html('ポイントの使い方');
        $('#content01').html('見事的中させて増えたポイントはそのまま引き継いで次の試合に使うことができます。');
        $('#content02').html('また貯めたポイントを使って「スポログ」で開催される懸賞に応募することもできます！');
        $('#content03').html('それでは結果をご確認いただき、「スポログ」でスポーツ観戦を「より熱く！より楽しく！」お楽しみください！');
        $('#add_image01').hide();
        $('#add_image02').show();
        $('#add_image02').attr('src', '/Content/img/tmp/tutorial_07_pc.gif');
        $('#nextEvent a').text('閉じる');

        flgPopup7 = true;

        //Show popup7            
        $(".outside_overclay").show();
        $(".add_popup").show();

        //Insert cookie popup6
        docCookies.setItem(strKeyPopup7, "true", Infinity);
    }
}
