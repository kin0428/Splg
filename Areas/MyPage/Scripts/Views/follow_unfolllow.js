$(document).ready(function () {

    //フォローする
    $(".follow_link").click(function (event) { follow(event);});

    // フォローを外す
    $(".unfollow_link").click(function (event) {unfollow(event); });

});


//フォローする
function follow(event) {

    var followingMemberId;

    var elementId = String(event.target.id);
    followingMemberId = elementId.split('-')[1];

    var url = '/mypage/following/follow/' + followingMemberId + '/';

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ followingMemberId: followingMemberId }),
        async: false,
        success: function (result) {
            $("#follow-" + followingMemberId).fadeOut(100, function () {
                $("#unfollow-" + followingMemberId).fadeIn(0);
            });

            var totalCounElmt = $("#totalCount")[0];
            if (totalCounElmt != null) {
                var totalCount = totalCounElmt.innerText;
                totalCounElmt.innerText = totalCount * 1 + 1;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("フォローできませんでした。" + errorThrown);
        }
    });
}

// フォローを外す
function unfollow(event) {

    var followingMemberId;

    var elementId = String(event.target.id);
    followingMemberId = elementId.split('-')[1];

    var url = '/mypage/following/unfollow/' + followingMemberId + '/';

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ followingMemberId: followingMemberId }),
        async: false,
        success: function (result) {
            $("#unfollow-" + followingMemberId).fadeOut(0, function () {
                $("#follow-" + followingMemberId).fadeIn(100);
            });

            var totalCounElmt = $("#totalCount")[0];
            if (totalCounElmt != null) {
                var totalCount = totalCounElmt.innerText;
                totalCounElmt.innerText = totalCount - 1;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("フォローを外せませんでした。" + errorThrown);
        }
    });
}
