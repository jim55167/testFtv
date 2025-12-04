// cookie
(function (m) { var h = !1; "function" === typeof define && define.amd && (define(m), h = !0); "object" === typeof exports && (module.exports = m(), h = !0); if (!h) { var e = window.Cookies, a = window.Cookies = m(); a.noConflict = function () { window.Cookies = e; return a } } })(function () {
  function m() { for (var e = 0, a = {}; e < arguments.length; e++) { var b = arguments[e], c; for (c in b) a[c] = b[c] } return a } function h(e) {
    function a(b, c, d) {
      var f; if ("undefined" !== typeof document) {
        if (1 < arguments.length) {
          d = m({ path: "/" }, a.defaults, d); if ("number" === typeof d.expires) {
            var k =
              new Date; k.setMilliseconds(k.getMilliseconds() + 864E5 * d.expires); d.expires = k
          } d.expires = d.expires ? d.expires.toUTCString() : ""; try { f = JSON.stringify(c), /^[\{\[]/.test(f) && (c = f) } catch (p) { } c = e.write ? e.write(c, b) : encodeURIComponent(String(c)).replace(/%(23|24|26|2B|3A|3C|3E|3D|2F|3F|40|5B|5D|5E|60|7B|7D|7C)/g, decodeURIComponent); b = encodeURIComponent(String(b)); b = b.replace(/%(23|24|26|2B|5E|60|7C)/g, decodeURIComponent); b = b.replace(/[\(\)]/g, escape); f = ""; for (var l in d) d[l] && (f += "; " + l, !0 !== d[l] && (f += "=" + d[l]));
          return document.cookie = b + "=" + c + f
        } b || (f = {}); l = document.cookie ? document.cookie.split("; ") : []; for (var h = /(%[0-9A-Z]{2})+/g, n = 0; n < l.length; n++) { var q = l[n].split("="), g = q.slice(1).join("="); '"' === g.charAt(0) && (g = g.slice(1, -1)); try { k = q[0].replace(h, decodeURIComponent); g = e.read ? e.read(g, k) : e(g, k) || g.replace(h, decodeURIComponent); if (this.json) try { g = JSON.parse(g) } catch (p) { } if (b === k) { f = g; break } b || (f[k] = g) } catch (p) { } } return f
      }
    } a.set = a; a.get = function (b) { return a.call(a, b) }; a.getJSON = function () {
      return a.apply({ json: !0 },
        [].slice.call(arguments))
    }; a.defaults = {}; a.remove = function (b, c) { a(b, "", m(c, { expires: -1 })) }; a.withConverter = h; return a
  } return h(function () { })
});

// cookie privacy policy
$(function () {
  $('body').append('<style> .Delete { cursor: pointer; position: absolute; right: 6px; top: 6px; width: 20px; height: 20px; transform: rotate(45deg); } .Delete:after { width: 2px; left: 50%; top: 0; bottom: 0; margin-left: -1px; } .Delete:before { height: 2px; top: 50%; right: 0; left: 0; margin-top: -1px; } .Delete:after, .Delete:before { background: rgb(255, 255, 255); content: ""; position: absolute; } </style><div id="Allow" style="width: 88%;position: fixed;bottom: 0;background: rgba(10, 10, 10, 0.74);margin: auto;left: 0;right: 0;padding:20px 28px 20px;z-index:999999;"><i id="close" class="Delete"></i><p style="text-align: left;line-height: 1.5;font-size: .95rem;color: #fff;text-shadow: 0 0 10px rgba(0,0,0,.8);">民視新聞網使用 cookie 技術為您提供更好的用戶體驗。 通過使用我們的官網，您確認並同意我們的 cookie 政策。 更多訊息，請閱讀民視新聞網的<a href="https://www.ftvnews.com.tw/privacy" style="color: #00c2ff;" target="_blank">隱私權政策</a></p></div>');

  $('#close').click(function () {
    Cookies.set('Allow', "1", { domain: 'ftvnews.com.tw', expires: 99 * 365 });
    $('#Allow').hide();
  });
  var dataAllow = Cookies.get('Allow');
  if (Cookies.get('Allow')) {
    $('#Allow').hide();
  } else {
    $('#Allow').show();
  };

})

//scroll header hide
$(function () {
  var bodyClass = document.body.classList,
    lastScrollY = 0;
  window.addEventListener('scroll', function () {
    var tsy = this.scrollY;
    if (tsy < 80) {
      bodyClass.remove('hideNav');
    } else {
      bodyClass.add('hideNav');
    }
    //lastScrollY = tsy;

    if ($(".article-body").length > 0) {
      var adscrtop = $(".article-body").offset().top;
      var scrollVal = $(this).scrollTop();
      if (scrollVal < adscrtop) {
        bodyClass.remove('sh-up');
      } else {
        bodyClass.add('sh-up');
      }
    }
  });
})



//fitVids
$(document).ready(function () {
  // Target your .container, .wrapper, .post, etc.
  $(".fitVids").fitVids();
});


//側邊欄RWD開合
$(function () {
  //新增div.overlay
  $('<div class="overlay"></div>').appendTo('.side-body');

  if ($(".navbar-toggle").find(".collapsed") !== null) {
    //點擊選單
    $('.navbar-toggle').click(function () {
      $('.side-menu-container').toggleClass('slide-in');
      $('.navbar').toggleClass('body-slide-in');
      $('.navbar-toggle').toggleClass('collapsed');
      $('.overlay').toggleClass('overlay-isvisible');
    });

  } else {
    //點擊選單
    $('.navbar-toggle').click(function () {
      $('.side-menu-container').removeClass('slide-in');
      $('.navbar').removeClass('body-slide-in');
      $('.navbar-toggle').toggleClass('collapsed');
      $('.overlay').removeClass('overlay-isvisible');
    });
  }
  // Remove menu
  $('.overlay').on('mousedown touchstart', function (e) {
    $('.side-menu-container').removeClass('slide-in');
    $('.navbar').removeClass('body-slide-in');
    $('.navbar-toggle').addClass('collapsed');
    $('.overlay').removeClass('overlay-isvisible');
  });
});


//top
$(document).ready(function () {
  // browser window scroll (in pixels) after which the "back to top" link is shown
  var offset = 300,
    //browser window scroll (in pixels) after which the "back to top" link opacity is reduced
    offset_opacity = 1200,
    //duration of the top scrolling animation (in ms)
    scroll_top_duration = 700,
    //grab the "back to top" link
    $back_to_top = $('.go-top, .scrollTop');

  //hide or show the "back to top" link
  $(window).scroll(function () {
    ($(this).scrollTop() > offset) ? $back_to_top.addClass('cd-is-visible') : $back_to_top.removeClass('cd-is-visible cd-fade-out');
    if ($(this).scrollTop() > offset_opacity) {
      $back_to_top.addClass('cd-fade-out');
    }
  });

  //smooth scroll to top
  $back_to_top.on('click', function (event) {
    event.preventDefault();
    $('body,html').animate({
      scrollTop: 0,
    }, scroll_top_duration);
  });

});


//nicescroll
$(function () {
  $(".nicescroll").niceScroll(
    {
      cursorcolor: "#fff",
      cursoropacitymin: 0.4,
      cursoropacitymax: 1,
      cursorwidth: "4px",
      cursordragontouch: true,
      emulatetouch: true,
    }
  );
});

//Nav
$(document).ready(function () {
  let isiOS = /(iPad|iPhone|iPod)/ig.test(navigator.userAgent);

  if (!isiOS) {
    $(".jscrollControl").niceScroll({
      cursoropacitymin: 0,
      cursoropacitymax: 0.6,
      cursorwidth: '3px',
      cursorborder: 'none',
      directionlockdeadzone: 100,
      railpadding: { top: 0, right: 0, left: 0, bottom: 2 },
      bouncescroll: true,
      cursordragontouch: true,
      touchbehavior: true
    });
  };

  if ($('.jscrollControl ul li.selected').length) {
    if (isiOS) {
      $(".jscrollControl").scrollLeft($('.jscrollControl ul li.selected').position().left - 40);
    } else {
      $(".jscrollControl").getNiceScroll(0).doScrollLeft($('.jscrollControl ul li.selected').position().left - 40, 0);
    }
  }

});

$(document).ready(function () {
  $(this).on("click", ".more-tags-btn", function () {
    $(this).parent().parent().addClass("show-tags-box");
    console.log("click more");
  })

  var fbBtn = $("a[data-type=facebook]");
  var twBtn = $("a[data-type=twitter]");
  var lineBtn = $("a[data-type=line]");

  $(fbBtn).click(function () {
    console.log($(this).data('title'))
    window.open('https://www.facebook.com/sharer/sharer.php?u=' + $(this).data('url'), $(this).data('title'), config = 'height=500,width=500');
  });

  $(twBtn).click(function () {
    console.log($(this).data('title'))
    window.open('https://twitter.com/intent/tweet?text=' + $(this).data('title') + '-%20' + $(this).data('url'), $(this).data('title'), config = 'height=500,width=500');
  });

  $(lineBtn).click(function () {
    console.log($(this).data('title'))
    window.open('https://lineit.line.me/share/ui?url=' + $(this).data('url'), $(this).data('title'), config = 'height=500,width=500');
  });

})

$(function () {
  //DailyMotion Player
  var player = $("#dm-player")
  if (player.length !== 0) {
    var playerId = $("#dm-player").data().key;
    dailymotion
      .createPlayer("dm-player", {
        video: playerId,
      })
      .then((player) => {
        player.on(dailymotion.events.PLAYER_START, (state) => {
        })
      })
      .catch((e) => console.error(e));
  }

  //18禁
  $("#View_PornModal").each(function () {
    $("#modal-porn").fadeIn().addClass('show');
    $("body").addClass('modal-open');

    $('#porn-success').click(function (event) {
      event.preventDefault();
      $(".modal").fadeOut().removeClass('show');
      $(".modal-backdrop.show").css('display', 'none');
      $("body").removeClass('modal-open');
    })
    $('#porn-fail').click(function (event) {
      event.preventDefault();
      window.location.replace("/");
    })
  });

  //近期熱播與新聞有影
  $(".playLive").click(function () {
    const videoID = $(this).data("videoidst");
    const videoTitle = $(this).data("videotitlest");
    const videoPlatform = $(this).data("videoplatformst");
    const ftvCdn = $(this).data("ftvcdn");
    const newsID = $(this).data("videonewsid");
    const YTSrc = `https://www.youtube.com/embed/${videoID}`;
    const DPSrc = `https://geo.dailymotion.com/player.html?video=${videoID}`;
    const FTVSrc = `https://embed.ftvnews.com.tw/${newsID}`
    const NewsSrc = (newsID === "" || newsID === null || newsID === undefined) ? `https://www.youtube.com/watch?v=${videoID}` : `/news/detail/${newsID}`;
    let videoHtml = "";

    if (ftvCdn !== "" && ftvCdn !== undefined) {     
      videoHtml = `<iframe src="${FTVSrc}" allowfullscreen='true' webkitallowfullscreen='true' mozallowfullscreen='true' height="100%" width="100%" style="border:none"></iframe>` 
    } else {
      switch (videoPlatform) {
        case 'DP':
        case 'D':
          videoHtml = `<iframe frameborder="0" src="${DPSrc}" allowfullscreen allow="autoplay;fullscreen; picture-in-picture;" height="100%" width="100%"></iframe>`;
          break;
        default:
          videoHtml = `<iframe src="${YTSrc}" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen></iframe>`;
          break;
      }
    }
    
    // 創建一個新的a標籤元素
    let linkElement = document.createElement("a");
    linkElement.href = NewsSrc;
    linkElement.innerText = videoTitle;

    // 根據NewsSrc的值設置target屬性
    if (NewsSrc.startsWith("https://www.youtube.com/watch?v=")) {
      linkElement.target = "_blank";
    } 
    $("#iframeCover").html(videoHtml);
    $("#iframeTitle").html(linkElement);
  });

  // 監聽 modal 隱藏事件，移除 <iframe> 元素
  $("#video_view").on("hidden.bs.modal", function () {
    $("#iframeCover").empty();
  });

  //首頁點擊錨點滾動
  $(".anchorPoint").click(function (event) {
    event.preventDefault();
    let target = $(this).attr("href");
    let targetPos = $(target).offset().top - 90;
    $("html, body").animate({ scrollTop: targetPos }, 500);
  });
});

//WordCloud
$(function () {
  if (document.getElementById('word_cloud')) {
    let list = [];
    for (var i in db) {
      list.push([db[i]["keyword"], db[i]["freq"], db[i]["url"]]);
    }

    wordCloudElement = WordCloud(document.getElementById('word_cloud'), {
      list: list,
      minFontSize: 1,
      color: function () {
        return (['#FF9C38', '#115A95', '#53A1E2', '#B39F8A', '#F47130'])[Math.floor(Math.random() * 5)]
      },
      gridSize: 5,
      fontFamily: 'Noto Sans TC',
      wait: 50,
      rotateRatio: 0,
      click: function (item) {
        if (item) {
          window.location.href = item[2];
        }
      }
    });
  }
})

//AI導讀
$(function () {
  let isOpen = false;
  let continueWriting = true;

  function toggleAI() {
    if (isOpen) {
      closeAI();
      document.getElementById("ai_content").innerHTML = "";
    } else {
      openAI();
    }
    isOpen = !isOpen;
  }

  function openAI() {
    document.getElementById("mwt_border").style.display = "block";
    document.getElementById("modal-overlay").style.display = "block";
    document.body.classList.add('modal-open');
    continueWriting = true;
    const container = document.getElementById("ai_content");
    let index = 0;

    function writing() {
      if (index < data.split("").length && continueWriting) {
        container.innerHTML += data[index++];
        setTimeout(writing, 30);
      }
    }
    writing();
  }

  function closeAI() {
    document.getElementById("mwt_border").style.display = "none";
    document.getElementById("modal-overlay").style.display = "none";
    document.body.classList.remove('modal-open');
    continueWriting = false;
  }

  $("#modal-overlay").on("click", function (event) {
    if (event.target === this) {
      toggleAI();
    }
  });

  $(".AI, .closeIcon").on("click", toggleAI);
});


function handleClick(id, event) {
  setMenuSession('FTV-MenuList', id);
}

window.onload = function () {
  const currentPath = location.pathname;
  const regex = /\/tag\/(.*?)\//;
  const match = currentPath.match(regex);

  if (match && match.length > 1) {
    let menuId = getMenuSession('FTV-MenuList');
    if (menuId) {
      let activeMenuItem = document.getElementById(menuId);
      if (activeMenuItem) {
        activeMenuItem.classList.add('active');
      }
    }
  } else {
    clearMenuSession('FTV-MenuList');
  }
}

function setMenuSession(key, value) {
  sessionStorage.setItem(key, JSON.stringify(value));
}

function getMenuSession(key) {
  var value = sessionStorage.getItem(key);
  return value ? JSON.parse(value) : null;
}

function clearMenuSession(key) {
  sessionStorage.removeItem(key);
}

$(window).on('scroll', function () {
  let shareBox = $('#share-box');
  let scrollHeight = $(window).scrollTop();
  if (scrollHeight > 950) {
    shareBox.addClass('show');
  } else {
    shareBox.removeClass('show');
  }
});