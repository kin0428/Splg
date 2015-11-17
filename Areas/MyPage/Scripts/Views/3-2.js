$(document).ready(function () {

    toastr.options = {
        "positionClass": "toast-top-center",
        "preventDuplicates": true
    }

    var urlCommon = "/mypage/expectedlist/ShowExpectedList";

    //右カラムの現在の所持ポイントロード用。
    //試合情報パネルは表示しない
    var linkUrl = "/RightContent/ShowExpectedPoints?showCurrentPointInfoOnly=true";

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

    //Click 予想をやめる
    $('.expect_cancel').live('click', function (event) {
        $(this).live('click', function (event) {
            alert('只今処理中です。\nそのままお待ちください。');
            return false;
        });
        event.preventDefault();
        expectcancel();
        var block = document.getElementsByClassName("block_04_3_l1");
        var active = block[0].getElementsByClassName("active");
        var year = active[0].textContent; // 2xxx
        block = document.getElementsByClassName("j_rank");
        active = block[0].getElementsByClassName("active");
        var month = active[0].textContent; // 3月
        getpoint(year, month);
    });

    function expectcancel() {

        var cancelid = String(event.target.id);
        var sports_id = cancelid.split('-')[1];
        var game_id = cancelid.split('-')[2];
        var game_odds = cancelid.split('-')[3];
        var url = '/mypage/expectedlist/ExpectCancel/' + game_id + '/';
        
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
                var cmt = 'オッズは' + odds + '倍で、的中すると0 ptになります。';
                $("#ExpectCommentGameID-" + game_id).html(cmt);

                //var text = $(this).text();
                //$("input").val(text);
//                document.getElementById("ExpectPoint-" + game_id)[0].value = 0;
                //var inputtag = $("#ExpectPoint-" + game_id);
                //inputtag.value = 0;

                //Load div pointPrediction
                $('#show_expected_points').load(linkUrl);

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
        var block = document.getElementsByClassName("block_04_3_l1");
        var active = block[0].getElementsByClassName("active");
        var year = active[0].textContent; // 2xxx
        block = document.getElementsByClassName("j_rank");
        active = block[0].getElementsByClassName("active");
        var month = active[0].textContent; // 3月
        getpoint(year, month);
    });

    function expectpoint(event) {

        var bedPoint = $(event.target).attr('data-bedpoint') * 100;
        var pointid = String(event.target.id);
        var sports_id = pointid.split('-')[1];
        var game_id = pointid.split('-')[2];
        var game_odds = pointid.split('-')[3];
        var game_point = String(event.target.value * 100);
        var url = '/mypage/expectedlist/ExpectPoint/' + game_id + '/';

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
                    $(event.target).attr('value', String(bedPoint / 100));
                    $(event.target).attr('data-bedpoint', String(bedPoint / 100));
                    toastr.error("予想ポイントを設定できませんでした。" + result.Message);
                } else {

                    var odds = (game_odds / 100).toFixed(1);
                    $(event.target).attr('value', game_point / 100);
                    $(event.target).attr('data-bedpoint', game_point / 100);

                }

                //Load div pointPrediction
                $('#show_expected_points').load(linkUrl);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                location.reload();
            }
        });
    }

    function getpoint(syear, smonth) {

        var year = syear.split(' ')[0];
        var month = smonth.split('月')[0];

        var url = urlCommon + '?target_year=' + year + '&target_month=' + month;

        $('#GameLists').load(url);

    }
});