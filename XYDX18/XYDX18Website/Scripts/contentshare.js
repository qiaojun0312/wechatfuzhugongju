wx.ready(function () {

    wx.onMenuShareAppMessage({
        title: descContent, // 分享标题
        desc: '.', // 分享描述
        link: lineLink, // 分享链接
        imgUrl: imgUrl, // 分享图标
        success: function () {
            // 用户确认分享后执行的回调函数
        },
        cancel: function () {
            // 用户取消分享后执行的回调函数
        }
    });


});

wx.error(function (res) {
    alert(res.errMsg);
});


