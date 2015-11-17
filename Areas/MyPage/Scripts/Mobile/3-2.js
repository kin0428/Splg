
$(document).ready(function () {
    var mypageTopDate = new Date();
    toastr.options = {
        "positionClass": "toast-top-center",
        "preventDuplicates": true
    }
    
    var urlCommon = "/mypage/expectedlist/ShowExpectedList";
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

    //Click 予想をやめる
    $(".my-link4").live('click', function (event) {
        $(this).live('click', function (event) {
            alert('只今処理中です。\nそのままお待ちください。');
            return false;
        });
        event.preventDefault();
        expectcancel(event);
        getpoint(0);
    });

    function expectcancel(event) {

        var gameDivBlockId = $(event.target).attr('data-gamedivblockid');
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

                if (result.HasError) {
                    toastr.error("予想をキャンセルできませんでした。" + result.Message);
                }
                else {
                    var plists = $("#" + gameDivBlockId);
                    plists.remove();

                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                location.reload();
            }
        });
    };

    //Click 予想ポイントの入力
    $(".my-ip01").live('change',function (event) {
        $(this).live('change', function (event) {
            alert('只今処理中です。\nそのままお待ちください。');
            return false;
        });
        expectpoint(event);
        getpoint(0);
    });

    function expectpoint(event) {

        var bedPoint = $(event.target).attr('data-bedpoint');
        var pointid = String(event.target.id);
        var sports_id = pointid.split('-')[1];
        var game_id = pointid.split('-')[2];
        var game_odds = pointid.split('-')[3];
        var game_point = String(event.target.value);
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
                    toastr.error("予想ポイントを設定できませんでした。" + result.Message);
                    $(event.target).attr('data-bedpoint', bedPoint);
                    $(event.target).attr('value', bedPoint);
                }
                else {
                    $(event.target).attr('data-bedpoint', game_point);
                    $(event.target).attr('value', game_point);

                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                location.reload();
            }
        });
    };

    function getpoint(monthDifference) {

        mypageTopDate.setMonth(mypageTopDate.getMonth() + monthDifference);
        var year = mypageTopDate.getFullYear();
        var month =  mypageTopDate.getMonth() + 1;
        var url = urlCommon + '?target_year=' + year + '&target_month=' + month;

        $("#mypage_selected_month").text(year + " 年 " + month + " 月");

        $('#GameLists').load(url);
    };
});