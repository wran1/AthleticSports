//顶部二维码显示
$(document).ready(function () {
    $("a.wechat").click(function () {
        $(this).find("img").slideToggle();
    });
});