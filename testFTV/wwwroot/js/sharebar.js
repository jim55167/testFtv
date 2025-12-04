function shareOnTwitter(title) {
  var url = "https://twitter.com/intent/tweet?text=" + encodeURIComponent(title) + "&url=" + encodeURIComponent(location.href);
  window.open(url, '_blank');
};

$(function () {
  $.ajaxSetup({ cache: true });
  $.getScript('https://connect.facebook.net/zh_TW/sdk.js', function () {
    FB.init({
      appId: 3065264970445931,
      autoLogAppEvents: true,
      xfbml: true,
      version: 'v3.2',
    });

    $('.share_fb').on('click', function (event) {
      event.preventDefault();
      FB.ui(
        {
          display: 'popup',
          method: 'share',
          href: location.href
        },
        function (response) {}
      );
    });
  });

  window.twttr = (function (d, s, id) {
    var js,
      fjs = d.getElementsByTagName(s)[0],
      t = window.twttr || {};
    if (d.getElementById(id)) return t;
    js = d.createElement(s);
    js.id = id;
    js.src = 'https://platform.twitter.com/widgets.js';
    fjs.parentNode.insertBefore(js, fjs);

    t._e = [];
    t.ready = function (f) {
      t._e.push(f);
    };

    return t;
  })(document, 'script', 'twitter-wjs');

  $('.share_line').on('click', function (event) {
    event.preventDefault();
    window.open('https://lineit.line.me/share/ui?url=' + encodeURIComponent(location.href));
  });

  $('.share_link').on('click', function (event) {
    event.preventDefault();
    navigator.clipboard.writeText(location.href);
    alert("網址複製成功！")
  })
});