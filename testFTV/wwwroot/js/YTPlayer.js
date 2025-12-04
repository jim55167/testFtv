//youtube ID
var youtubeId = 'XxJKnDLYZz4'; //FTV
var player;
var is_playing = true;
var done = false;

// Load the IFrame Player API code asynchronously.
function loadYTapi() {
  var tag = document.createElement('script');
  tag.src = 'https://www.youtube.com/player_api';
  var firstScriptTag = document.getElementsByTagName('script')[0];
  firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
}
// var tag = document.createElement('script');
// tag.src = 'https://www.youtube.com/player_api';
// var firstScriptTag = document.getElementsByTagName('script')[0];
// firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

// Replace the '#top-live' element with an <iframe> and
// YouTube player after the API code downloads.

function onYouTubePlayerAPIReady() {
  player = new YT.Player(document.querySelector('#top-live'), {
    height: '100%',
    width: '100%',
    videoId: youtubeId,
    playerVars: {
      autoplay: 1,
      controls: 1,
      disablekb: 1,
      enablejsapi: 1,
      modestbranding: 1,
      playsinline: 1,
      rel: 0,
      origin: window.location.origin,
    },
    events: {
      onReady: onPlayerReady,
      onStateChange: onPlayerStateChange,
    },
  });
}

function onPlayerReady(event) {
  player.setVolume(0);
  //var muteButton = $("#yt-mute");
  var playButton = $('#yt-play');
  var stopButton = $('#yt-stop');
  var that = this;

  //play
  playButton.on('click', function (click_event) {
    player.playVideo();
    that.is_playing = false;
    playButton.fadeOut();
    stopButton.fadeIn();
  });
  //stop
  stopButton.on('click', function (click_event) {
    player.pauseVideo();
    that.is_playing = true;
    stopButton.fadeOut();
    playButton.fadeIn();
  });
}

function changeBorderColor(playerStatus) {
  var playButton = $('#yt-play');
  var stopButton = $('#yt-stop');
  var color;
  if (playerStatus == -1) {
    //color = "#37474F"; // unstarted = gray
  } else if (playerStatus == 0) {
    //color = "#FFFF00"; // ended = yellow
  } else if (playerStatus == 1) {
    //color = "#33691E"; // playing = green
    playButton.fadeOut();
    stopButton.fadeIn();
    $('#ytblock').removeClass('is-hidden');
  } else if (playerStatus == 2) {
    //color = "#DD2C00"; // paused = red
    stopButton.fadeOut();
    playButton.fadeIn();
    $('#ytblock').addClass('is-hidden');
  } else if (playerStatus == 3) {
    color = '#AA00FF'; // buffering = purple
  } else if (playerStatus == 5) {
    //scolor = "#FF6DOO"; // video cued = orange
  }
  //if (color) {
  //$('#ytwrap').css("border", color + " solid 1px");
  //$('#ytwrap').css("background", color);
  //}
}

function onPlayerStateChange(event) {
  if (event.data == YT.PlayerState.PLAYING && !done) {
    this.is_playing = true;
  }
  changeBorderColor(event.data);
}

// $(document).ready(function () {
//   /*Floating Code for Iframe Start*/

//   if ($('#top-live').css('display') !== 'none') {
//     if (jQuery('#top-live').length > 0) {
//       /*Wrap (all code inside div) all vedio code inside div*/
//       jQuery('#top-live').wrap(
//         "<div id='ytwrap' class='video-container' ><div class='iframe-parent-class'><div id='ytblock' class='is-hidden'></div></div></div>"
//       );

//       //add button
//       $('#ytblock').append(
//         "<button id='yt-play'><i class='fas fa-play'></i></button><button id='yt-stop'><i class='fas fa-times'></i></button>"
//       );

//       /*main code of each (particular) vedio*/
//       jQuery('#top-live').each(function (index) {
//         /*Floating js Start*/
//         var windows = $(window);
//         var iframeWrap = $('#top-live').parent().parent();
//         //var iframeWrap = jQuery(this).parent();

//         var iframeblock = $('#top-live').parent();

//         var iframe = $('#top-live');
//         //var iframe = jQuery(this);
//         var iframeHeight = iframe.parent().outerHeight();
//         //var iframeHeight = iframe.outerHeight();
//         var iframeElement = $('#top-live').get(0);
//         //var iframeElement = iframe.get(0);

//         windows.on('scroll', function () {
//           var windowScrollTop = windows.scrollTop();
//           //var windowScrollTop = windows.scrollTop();
//           var iframeBottom = iframeHeight + iframeWrap.offset().top;
//           //var iframeBottom = iframeHeight + iframeWrap.offset().top;
//           //alert(iframeBottom);

//           if (windowScrollTop > iframeBottom) {
//             iframeWrap.height(iframeHeight);
//             //iframe.addClass('stuck');
//             iframeblock.addClass('stuck');
//           } else {
//             iframeWrap.height('auto');
//             //iframe.addClass('stuck');
//             iframeblock.removeClass('stuck');
//           }
//         });
//         /*Floating js End*/
//       });
//     }
//   }
//   /*Floating Code for Iframe End*/
// });

function floatingIframe() {
  /*Floating Code for Iframe Start*/

  if ($('#top-live').css('display') !== 'none') {
    if (jQuery('#top-live').length > 0) {
      /*Wrap (all code inside div) all vedio code inside div*/
      jQuery('#top-live').wrap(
        "<div id='ytwrap' class='video-container' ><div class='iframe-parent-class'><div id='ytblock' class='is-hidden'></div></div></div>"
      );

      //add button
      $('#ytblock').append(
        "<a id='yt-play' href='javascript:;'><i class='fas fa-play'></i></a><a id='yt-stop' href='javascript:;'><i class='fas fa-times'></i></a>"
      );

      /*main code of each (particular) vedio*/
      jQuery('#top-live').each(function (index) {
        /*Floating js Start*/
        var windows = $(window);
        var iframeWrap = $('#top-live').parent().parent();
        var iframeblock = $('#top-live').parent();
        var iframe = $('#top-live');
        var iframeHeight = iframe.parent().outerHeight();
        var iframeElement = $('#top-live').get(0);

        windows.on('scroll', function () {
          var windowScrollTop = windows.scrollTop();
          var iframeBottom = iframeHeight + iframeWrap.offset().top;

          if (windowScrollTop > iframeBottom) {
            iframeWrap.height(iframeHeight);
            iframeblock.addClass('stuck');
          } else {
            iframeWrap.height('auto');
            //iframe.addClass('stuck');
            iframeblock.removeClass('stuck');
          }
        });
        /*Floating js End*/
      });
    }
  }
  /*Floating Code for Iframe End*/
}

/**
 * @description 產生直播項目
 * @param {*} data {id,title,img,videoId,link}
 * @returns 直播項目Jquery
 */
function createLivingItem(data) {
  let str = '';
  str += '<li class="col-12 living-item">';
  str += '  <a href="#" class="video" data-vid="' + data.videoId + '">';
  str += '    <div class="img-block">';
  str += '      <img src="' + data.img + '" alt="' + data.title + '" />';
  str += '    </div>';
  str += '    <div class="content">';
  str += '      <div class="title">' + data.title + '</div>';
  str += '     </div>';
  str += '  </a>';
  str += '</li>';

  let block = $(str);
  let a = block.find('a');

  a.on('click', function (e) {
    e.preventDefault();
    let self = $(this);

    if (!self.hasClass('active')) {
      let li = self.parent();
      let siblings = li.siblings();
      let vid = self.data('vid');

      siblings.each(function (index, elem) {
        $(this).find('.video').removeClass('active');
      });
      self.addClass('active');
      youtubeId = vid;
      player.loadVideoById(youtubeId);
    }
  });

  return block;
}

$(function () {
  //$('.living').css({"display":"block"});
  // 讀取現正直播資料
  let api_domain = 'https://ftvapi.ftvnews.com.tw/';
  $.ajax({
    url: api_domain + 'API/YTHomeLive.aspx',
    contentType: 'application/json',
    //二階段驗證
    method: "GET",
    timeout: 0,
    headers: {
      "Ocp-Apim-Subscription-Key": "f0513aa877834e0a873e160cf16d4471",
      "Ocp-Apim-Trace": "true"
    },
    // xhrFields: {
    //   withCredentials: true,
    // },
    // crossDomain: true,
  })
    .done(function (data, textStatus, jqXHR) {
      if (data.Status == 'Success') {
        let arr = data.data;

        // 建立直播列表
        let list = $('#living-list');
        $('.living').css({"display":"block"});
        if (arr.length > 0) {
          $('.living').css({"display":"block"});
          // 產生直播項目並 append 到 list
          arr.forEach(function (item, index, array) {
            let living_item = createLivingItem(item);
            if (index === 0) {
              // living_item.hide();
              youtubeId = item.videoId;
              living_item.find('.video').addClass('active');
            }
            living_item.appendTo(list);
          });

          floatingIframe();

          // 載入YT api
          loadYTapi();

        } else {
          $('#top-live').hide();
          $('.living').hide();
        }

        // if (arr.length <= 1) {
        //   $('.living').hide();
        // }
      } else {
        $('#top-live').hide();
        $('.living').hide();
      }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
      $('#top-live').hide();
      $('.living').hide();
    });
});
