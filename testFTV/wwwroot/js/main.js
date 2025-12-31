<script type="text/javascript" src="https://cdn.ftvnews.com.tw/client/js/jquery-ui.min.js"></script>
<script type="text/javascript" src="https://cdn.ftvnews.com.tw/client/js/bootstrap.min.js"></script>

<script type="text/javascript" src="https://cdn.ftvnews.com.tw/client/js/jquery.lazy.min.js"></script>
<script type="text/javascript" src="https://cdn.ftvnews.com.tw/client/js/jquery.lazy.plugins.min.js"></script>

<script type="text/javascript" src="https://cdn.ftvnews.com.tw/client/js/jquery.fitvids.js?ver=20220929"></script>

<script type="text/javascript" src="https://cdn.ftvnews.com.tw/client/js/jquery.nicescroll.min.js"></script>
<script type="text/javascript" src="https://cdn.ftvnews.com.tw/client/js/script.min.js?ver=20241118"></script>
<script type="text/javascript" src="https://cdn.ftvnews.com.tw/client/js/init-index.min.js?ver=20250805"></script>

<script src="https://cdn.ftvnews.com.tw/client/js/wordcloud2.min.js"></script>

<script src="https://cdn.ftvnews.com.tw/client/js/swiper.min.js"></script>

<script src="https://cdn.ftvnews.com.tw/client/js/dayjs/dayjs.min.js"></script>
<script src="https://cdn.ftvnews.com.tw/client/js/dayjs/relativeTime.js"></script>
<script src="https://cdn.ftvnews.com.tw/client/js/dayjs/isSameOrBefore.js"></script>
<script src="https://cdn.ftvnews.com.tw/client/js/dayjs/customParseFormat.js"></script>
<script src="https://cdn.ftvnews.com.tw/client/js/dayjs/zh-tw.js"></script>

<script>
    function timeRelativeTime(func) {
        dayjs.locale('zh-tw');
        dayjs.extend(dayjs_plugin_relativeTime);
        dayjs.extend(dayjs_plugin_isSameOrBefore);
        dayjs.extend(dayjs_plugin_customParseFormat);

        $('.time[data-time]').each(function (index, element) {
            let current_date = dayjs();
            let before_date = current_date.subtract(1, 'd');
            //let str = $(element).text();
            let str = $(element).data('time');
            let format_date = dayjs(str, 'YYYY/MM/DD HH:mm:ss');

            if (!format_date.isBefore(before_date)) {
                $(element).text(format_date.fromNow());
            }
        });
    };

    $(function () {
        var datatime = $('.time[data-time]')
        if (datatime.length > 0) {
            timeRelativeTime();
        };
    });


    $(function () {
        let idleVal = $("#idleVal").val();
        let interval;
        let idletime = idleVal * 60 * 1000;

        setIdleTimeout(idletime, function () {
            $(".interstitialAd").show();
            $("body").css("overflow", "hidden");
            $("body").css("max-height", "100vh");            
        }, function () {
            $(".interstitialAd").hide();
            $("body").css("overflow", "auto");
            $("body").css("max-height", "none");
        });

        function setIdleTimeout(millis, onIdle, onUnidle) {
            let timeout = 0;
            startTimer();
            function startTimer() {
                timeout = setTimeout(onExpires, millis);
                document.addEventListener("mousedown", onActivity);
                document.addEventListener("keypress", onActivity);
            }
            function onExpires() {
                timeout = 0;
                onIdle();
            }
            function onActivity() {
                if (timeout) clearTimeout(timeout);
                else onUnidle();
                
                //since the mouse is moving, we turn off our event hooks for 1 second
                document.removeEventListener("mousedown", onActivity);
                document.removeEventListener("keypress", onActivity);
                setTimeout(startTimer, 1000);
            }

            $(".interAd_close").click(function () {
                $(".interstitialAd").hide();
                $("body").css("overflow", "auto");
                $("body").css("max-height", "none");
            });
        }
    });

  $(function () {
    const getLiveIframe = $('#embed-responsive-id')[0];
    const getDetailIframe = document.querySelector('figure > iframe')
    if (getLiveIframe) {
      const liveIframe = getLiveIframe.contentWindow
      window.addEventListener('load', () => {
        liveIframe.postMessage(location.href, 'https://embed.4gtv.tv');
      });
    } else if (getDetailIframe) {
      const found = getDetailIframe.src.indexOf('https://embed.4gtv.tv')
      if (found != -1) {
        const detailIframe = getDetailIframe.contentWindow
        window.addEventListener('load', () => {
          detailIframe.postMessage(location.href, 'https://embed.4gtv.tv');
        });
      }
    }
  })
</script>