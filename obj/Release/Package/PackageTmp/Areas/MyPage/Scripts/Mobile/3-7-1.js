$(document).ready(function () {

    toastr.options = {
        "positionClass": "toast-top-center",
        "preventDuplicates": true
    }

    ////Click Emailアドレスの入力
    $("#settingssendnewaddress").click(function (event) {
        checkEmailAddress();
    });

    function checkEmailAddress() {

        var email = $("#entry_mail").val();
        var password = $("#entry_password").val();

        var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        if (!filter.test(email) || email === undefined) {
            toastr.error("Emailアドレスが違います。");
            return;
        }

        var url = '/mypage/setting/address/'+ email + "/" + password + '/change/';

        var post_data = {
            MemberRegisterInfo: {
                email: email,
                password: password
            }
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
                    toastr.error("Emailアドレスを変更できませんでした。" + result.Message);
                }
                else {
                    toastr.success('Emailアドレスの変更に必要なURLを、入力したメールアドレス宛に送信します。<br />' + 
                    'メールに記載されたURLをクリックしないまま24時間が経過した場合、<br />ご登録依頼が解除となりますのでご注意ください。');
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                toastr.error("Emailアドレスを変更できませんでした。");
            }
        });

    }

});

