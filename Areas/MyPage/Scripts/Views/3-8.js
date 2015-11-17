var managementnoticecount;
var usernoticecount;
var loadCount = 1;

function setUnreadNoticeCount(event) {


    // NoticeDeliverySubjectIdの取得
    var noticeDeliverySubjectId = $(this).attr('data-noticedeliverysubjectid');

    // 既読処理
    var url = "/mypage/notice/alreadyread/" + noticeDeliverySubjectId + "/";

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ noticeDeliverySubjectId: noticeDeliverySubjectId }),
        async: false,
        success: function (result) {

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("エラーが起こりました。" + errorThrown);
        }
    });

}

$(document).ready(function () {

    //初期表示
    //loadNotice();
    loadUserNotice();

    //運営からのお知らせのもっとみる検索
    $("#submitManagementNotice").click(function (event) {

        //loadNotice();

    });

    //○○さんへのお知らせのもっとみる検索
    $("#submitUserNotice").click(function (event) {
        loadUserNotice();
    });

    //既読処理
    $(".usernoticeload0" ).each(
        function () {
            $(this).live('click', setUnreadNoticeCount);
        }
        );

});

function loadNotice() {

    managementnoticecount = $("#submitManagementNotice").attr("data-managementnoticecount");
    if (managementnoticecount == null) {
        managementnoticecount = 0;
    }
    var url = '/mypage/notice/management/' + managementnoticecount + '/';
    loadCount++;

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ managementnoticecount: managementnoticecount }),
        async: false,
        success: function (result) {

            var notices = result.ManagementNotices;
            var managementNoticeTotalCount = result.ManagementNoticeTotalCount;

            var lines = $("#managementNoticeLines");

            for (var i = 0; i < notices.length; i++) {

                var n = notices[i];

                var line = '<li>' +
                            '<p class="mname"><a href="' + n.TransitionsURL + '" data-noticedeliverysubjectid="' + n.NoticeDeliverySubjectId + '">' +
                            //n.Title + '</a></p>' +
                            n.NoticeBody + '</a></p>' +
                            //'<p class="mday">' + n.FormattedCreatedDate + '</p>' +
                            '</li>';

                lines.append(line);

            }

            var newmanagementnoticecount = notices.length * 1 + managementnoticecount;
            $('#submitManagementNotice').data("managementnoticecount", newmanagementnoticecount);

            if (managementNoticeTotalCount <= newmanagementnoticecount)
                $('#submitManagementNotice').hide();


        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("エラーが起こりました。" + errorThrown);
        }
    });


}


function loadUserNotice(event) {

    usernoticecount = $("#submitUserNotice").data("usernoticecount");
    if (usernoticecount == null) {
        usernoticecount = 0;
    }
    var url = '/mypage/notice/user/' + usernoticecount + '/';
    loadCount++;

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ usernoticecount: usernoticecount }),
        async: false,
        success: function (result) {

            var notices = result.UserNotices;
            var userNoticeTotalCount = result.UserNoticeTotalCount;

            var lines = $("#userNoticeLines");

            for (var i = 0; i < notices.length; i++) {

                var n = notices[i];

                //既読処理
                var notreadyetclass = "";
                if (!n.AlreadyReadFlg) {
                    notreadyetclass = 'usernoticeload' + loadCount;
                } else {
                    notreadyetclass = 'gray';
                }

                var line = '<li>' +
                            '<p class="mname j_rank"><a class="' + notreadyetclass + '" href="' + n.TransitionsURL + '" data-noticedeliverysubjectid="' + n.NoticeDeliverySubjectId + '">' +
                            n.NoticeBody + '</a></p>' +
                            //'<p class="mday">' + n.FormattedCreatedDate + '</p>' +
                            '</li>';

                 lines.append(line);
            }

            $(".usernoticeload" + loadCount).each(
                function () {
                    $(this).live('click', setUnreadNoticeCount);
                }
             );

            var newusernoticecount = notices.length * 1 + usernoticecount;
            $('#submitUserNotice').data("usernoticecount", newusernoticecount);

            if (userNoticeTotalCount <= newusernoticecount)
                $('#submitUserNotice').hide();

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("エラーが起こりました。" + errorThrown);
        }

    });

}

