$(document).ready(function () {

    toastr.options = {
        "positionClass": "toast-top-center",
        "preventDuplicates": true
    }

    //未使用
    $("#settingdeleteaccount").click(function (event) {
        deleteAccount();
    });

    function deleteAccount() {

        var expass = $("#expass").val();
        var npass = $("#npass").val();
        var npass_confirm = $("#npass_confirm").val();

        var url = '/mypage/setting/password/' + expass + "/" + npass + "/" + npass_confirm + '/change/';
        toastr.options = {
            "positionClass": "toast-top-center",
            "preventDuplicates": true
        }

        var post_data = {
            MemberRegisterInfo: {
                expass: expass,
                npass: npass,
                npass_confirm:npass_confirm
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
                    toastr.error("パスワードを変更できませんでした。" + result.Message);
                }
                else {
                    toastr.success('パスワードを変更しました。');
                    //document.location.href = '/mypage/';
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                toastr.error("パスワードを変更できませんでした。");
            }
        });

    }

});

