var urlPoint = "/Home/ShowPointAlert";

$(document).ready(function () {
    // -- topic path 
    $(".block-path > li ul").hide();
    $(".block-path > li > a").click(function (event) {
        event.preventDefault();
        $(this).siblings('ul').toggle();
    });

    // foot-nav -----------
    $(".subfmenu").hide();
    $(".fmenu > li > a").click(function (event) {
        event.preventDefault();
        $(this).parent().toggleClass("active");
        $(this).siblings('.subfmenu').toggle();
    });

    // tabs-list -----------
    $(function () {
        $(".tabList li").click(function () {
            var num = $(".tabList li").index(this);
            $(".tabFlag").addClass('hide');
            $(".tabFlag").eq(num).removeClass('hide');
            $(".tabList li").removeClass('active');
            $(this).addClass('active')
        });
    });
    $("#FormationTab").click(function (event) {
        $("#Formaton").reload;
    });

    // tabList2 -----------
    $(function () {
        $(".tabList2 li").click(function () {
            var num = $(".tabList2 li").index(this);
            $(".tabFlag2").addClass('hide');
            $(".tabFlag2").eq(num).removeClass('hide');
            $(".tabList2 li").removeClass('active');
            $(this).addClass('active')
        });
    });

    // --- scroll to content
    $('.anchor').on('click', function (event) {
        event.preventDefault();
        $(this).addClass("active");
        $(this).siblings("a").removeClass("active");
        var target = "#" + $(this).data('target');
        $('html, body').animate({
            scrollTop: $(target).offset().top
        }, 500);
    });
    // jleague topic-path 
    $(".topic_sub1").hide();
    $(".topic_sub2 ul").hide();
    $(".topic-path > p > a").click(function (event) {
        event.preventDefault();
        $('.topic_sub1').toggle();
    });
    $(".topic_sub2 p").click(function (event) {
        event.preventDefault();
        $(this).parent().addClass("active");
        $(this).parent().siblings().removeClass("active");
        $(this).parent().siblings().find('ul').hide();
        $(this).siblings('ul').show();
    });
    $("#tab01").show();
    $(".tab-defaut a").click(function (event) {
        event.preventDefault();
        $(this).addClass("active");
        $(this).siblings().removeClass("active");
        var tab = $(this).attr("href");
        $(".tab-flag").not(tab).hide();
        $(tab).show();
    });
    $(".team-list > li").addClass("roll");
    $(".list01 > li:lt(5)").css('display', 'table');
    $(".list02 > li:lt(5)").css('display', 'table');
    $(".list03 > li:lt(5)").css('display', 'table');
    $(".list04 > li:lt(5)").css('display', 'table');
    $(".list05 > li:lt(5)").css('display', 'table');
    $(".list06 > li:lt(5)").css('display', 'table');
    $(".list07 > li:lt(5)").css('display', 'table');
    $(".list08 > li:lt(5)").css('display', 'table');
    $(".list09 > li:lt(5)").css('display', 'table');
    $(".list10 > li:lt(5)").css('display', 'table');
    $(".list11 > li:lt(5)").css('display', 'table');
    $(".list12 > li:lt(5)").css('display', 'table');
    $(".more_view").each(function () {
        var num = 5;
        $(this).click(function (event) {
            event.preventDefault();
            num = num + 5;
            $(this).siblings().find(".roll:lt(" + num + ")").css('display', 'table');
        });
    });

    // Login Popup
    $(".login-link").click(function () {
        $(".overlay").show(300);
        //	    $('html, body').animate({
        //	        scrollTop: $('.head-logo').offset().top
        //	    }, 500);
        //	    
    });
    $(".overlay").click(function () {
        // todo ログインできない為コメントアウト
        // 履歴を確認すること
        //$(".overlay").hide(300);
    });

    function formatNumber(num) {
        return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
    }

    // --- faq -list
    $(".faq_list dd").hide();
    $(".faq_list dt").click(function (event) {
        event.preventDefault();
        $(this).toggleClass("active");
        $(this).siblings('dd').toggle();
    });

    // -- register box
    //	$(".reg-link").click(function () {
    //	    $('.head-link').toggle();
    //	});

    // User Article cancel post Popup
    $("#postConfirmDlg").click(function () {
        $(".cancelPost").show(300);
        $('html, body').animate({
            scrollTop: $('.head-logo').offset().top
        }, 500);
        //	    $('.head-link').css('display', 'none');
        $('#PosessionPoint').load(urlPoint);
    });
    $("#close_post_popup").click(function () {
        $(".cancelPost").hide(300);
    });
    $(".close_post_popup").click(function () {
        $(".cancelPost").hide(300);
    });

    // add popup 26/6
    $(".popclose").click(function () {
        closePopupTutorial();
    });
});

function closePopupTutorial() {
    $(".outside_overclay").hide();
    $(".tutorial_popup").hide();
}

function hiddenFormationTab() {
    $("#FormationTab").addClass("hide");
}