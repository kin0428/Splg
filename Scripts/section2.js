
$(document).ready(function () {

    var memberID = $("#txtMemberId").val();
    //check user type news on url
    if (window.location.href.indexOf("/user_article/new/") > -1) {

        if (memberID == "" | typeof memberID === 'undefined') {

            document.location.href = '/login';
        }
    }
    //Mobile Detected
    var isMobile = {
        Android: function () {
            return navigator.userAgent.match(/Android/i);
        },
        BlackBerry: function () {
            return navigator.userAgent.match(/BlackBerry/i);
        },
        iOS: function () {
            return navigator.userAgent.match(/iPhone|iPad|iPod/i);
        },
        Opera: function () {
            return navigator.userAgent.match(/Opera Mini/i);
        },
        Windows: function () {
            return navigator.userAgent.match(/IEMobile/i);
        },
        any: function () {
            return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
        }
    };

    //post new on pc
    $(".edit_btn02 a,.panel-link2 a").click(function (event) {

        if (memberID == "" | typeof memberID === 'undefined')
        {
            //check mobile
            if (isMobile.any())
            {
                $(".overlay").show(300);
                $('html, body').animate({ scrollTop: $('.head-logo').offset().top }, 500);
            }
            else
            {
                $('#overlay').css('height', winH);
                $('#overlay').css('width', winW);
                $("#overlay").show(300);
                event.preventDefault();
            }
            event.preventDefault();
        }
    });
});



function checkEmailinForgotPassword() {
    var result = true;
    var emailVal = $('#emailAddr').val();
    var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
    var isAvailable = checkEmailAvailability(emailVal);
    if (!filter.test(emailVal) || emailVal === undefined || isAvailable) {
        $('#error').html("メールアドレスが違います。");
        result = false;
    }
    if (result === true) {
        document.getElementById('forgotForm').submit();
    }
}


function changePassword() {
    var password = $('#password').val();
    var passwordConfirm = $('#passwordConfirm').val();
    var result = true;
    if (password != passwordConfirm) {
        $('#error').html("入力されたパスワードが違います。");
        result = false;
    }
    if (password.length < 6 || password.length > 32) {
        $('#error').html("パスワードは6文字以上32文字以下の半角英数字を入力下さい。");
        result = false;
    }
    if (result === true) {
        document.getElementById('changePassForm').submit();
    }
}
function checkEmailAvailability(email) {
    var flag = true;
    $.ajax({
        type: "POST",
        url: '/Member/CheckEmailAvailability',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ email: email }),
        async: false,
        success: function (result) {
            if (result === 'existed') {
                flag = false;
            }
        }
    });
    return flag;
}
function checkUserSession() {
    var memberID = "";
    $.ajax({
        type: "POST",
        url: '/Member/CheckIfSessionExisted',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            if (!(result === null)) {
                memberID = result;
            }
        }
    });
    return memberID;
}




//function redirectToPostArticle(type)
//{
//    //var url = $("#RedirectURL").val();
//    //var memberID = $("#CurrentMemberSession").val();//checkUserSession();
//    //if (memberID != null && memberID != "")
//    //{
//    //    window.location.href = url;
//    //}
//    //else
//    //{
//    //    if (type > 0)
//    //    {
//    //        $('#overlay').css('height', winH);
//    //        $('#overlay').css('width', winW);
//    //        $("#overlay").show(300);
//    //        $("#urlVal").val(url);
//    //    }
//    //    else
//    //    {
//    //        $(".overlay").show(300);
//    //        $('html, body').animate({ scrollTop: $('.head-logo').offset().top }, 500);
//    //    }
//    //}
//}

//function checkArticle() {
    //var title = $('#postTitle').val();
    //var content = $('#postBody').val();
    //var filter = /<(?:.|\n)*?>/;
    //var result = true;
    //title = title.trim();
    //content = content.trim();
    //if (title == '' || title === undefined || filter.test(title) || title === "タイトルを記入してください。。。") {
    //    $('#errorTitlePost').html("タイトル記入することは必要です。");
    //    result = false;
    //} else {
    //    $('#errorTitlePost').html("");
    //}
    //if (content == '' || content === undefined || filter.test(content) || content === "本文を記入してください。。。") {
    //    $('#errorBodyPost').html("本文記入することは必要です。");
    //    result = false;
    //} else {
    //    $('#errorBodyPost').html("");
    //}
    //if (result === true) {
    //    var postForm = document.getElementById('postForm');
    //    postForm.onsubmit = function () {
    //        content = content.replace(/(?:\r\n|\r|\n)/g, '<br/>');
    //        $('#postBody').html(content);

    //    };
    //    postForm.submit();
    //}
    //else {
    //    return false;
    //}
//}

function PostConfirmDlg() {
    $('#cancelPost').css('height', winH);
    $('#cancelPost').css('width', winW);
    $("#cancelPost").show(300);
}
function CloseConfirmDlg() {
    $("#cancelPost").hide(300);
}
