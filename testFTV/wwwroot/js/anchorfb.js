$(function () {
  let token = 'TaorYl3gXIYCYzsa1e6w57YmFvqDWMVCTn4P2ZGd';
  let h1_tag = $('#tagtxt');
  let listId = '';
  /*let loadbtn = $('#loadbtn'); */
  if (h1_tag.data('lists')) {
    listId = h1_tag.data('lists');
  }
  
  let tagtxt = '';
  if (h1_tag.data('tag')) {
  }
  
  let nowDate = new Date();
  let utcYear = nowDate.getUTCFullYear();

  $.ajax({
    url: 'https://api.crowdtangle.com/posts',
    data: {
      token: token,
      listIds: listId,
      searchTerm: tagtxt,
      count: 18,
      offset: 0,
      startDate: utcYear + '-01-01',
      sortBy:'date', //時間排序
    },
    method: 'GET',
  })
    .done(function (data, textStatus, jqXHR) {
      let output = '';
      if (data.result.posts.length > 0) {
        output = createHTML(data.result.posts);
      }

      let $container = $('#post_list');
      $container.append(output);
      $container.imagesLoaded(function() {
        $container.masonry({
          itemSelector: '.grid-item',
          columnWidth: 0
        });
      });
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
      loadbtn.prop('disabled', true);
      console.log(textStatus);
    });

  /**
   * 產生文章html
   * @param {array} posts 文章陣列
   * @returns jQuery包裝的html
   */
  function createHTML(posts) {
    let output = '';
    posts.forEach(function (element, i) {
      let mediaPhoto = '';
      if (Object.prototype.hasOwnProperty.call(element, 'media')) {
        mediaPhoto = element.media.find(function (item, index, array) {
          return item.type == 'photo';
        });
      }

      let showDate = moment(element.date + '+00:00');
      showDate = showDate.local();
      showDate = showDate.format('YYYY-MM-DD HH:mm:ss');

      output += '<li class="col-sm-12 col-md-6 col-lg-4 grid-item">';
      output += '<a href="' + element.postUrl + '" target="_blank">';
      output += '<div class="header">';
      output += '<div class="img-block" style="background-image:url(' + element.account.profileImage + ')">';
      output += '<img src="' + element.account.profileImage + '" alt="" />';
      output += '</div>';
      output += '<div class="cite">';
      output += '<div class="name">' + element.account.name + '</div>';
      output += '<div class="date fbtime">' + showDate + '</div>';
      output += '</div>';
      output += '</div>';
      output += '<div class="content">';
      if (mediaPhoto) {
        output += '<img src="' + mediaPhoto.url + '" alt="" />';
      }

      let content_text = '';
      switch (element.type) {
        case 'link':
          content_text = cutContent(element.message, 0, 200);

          break;

        default:
          if (element.message) {
            content_text = cutContent(element.message, 0, 200);
          } else {
            content_text = '';
          }
          break;
      }

      output += content_text;

      output += '</div>';
      output += '</a>';
      output += '</li>';
    });

    return convertToRelativeDate($(output));
  }

  /**
   * 將顯示日期轉換為相對時間
   * @param {jQuery} elems jQuery包裝的html
   * @returns jQuery包裝的html
   */
  function convertToRelativeDate(elems) {
    elems.find('.fbtime').each(function (index, element) {
      let current_date = moment();
      let before_date = current_date.subtract(1, 'd');
      let str = $(element).text();
      let format_date = moment(str);
      if (!format_date.isBefore(before_date)) {
        $(element).text(format_date.fromNow());
      }
    });

    return elems;
  }

  /**
   * 截斷文章
   * @param {string} content 要處理的字串
   * @param {number} indexStart 起始索引
   * @param {number} indexEnd 結束索引
   * @returns 處理後的字串
   */
  function cutContent(content, indexStart, indexEnd) {
    if (isNaN(parseInt(indexStart))) {
      indexStart = 0;
    }
    if (isNaN(parseInt(indexEnd))) {
      indexEnd = content.length;
    }
    if (content.length > indexEnd) {
      content = content.substring(indexStart, indexEnd);
      content = content + '...';
    }

    return content;
  }

});
