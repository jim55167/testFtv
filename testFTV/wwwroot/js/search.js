$(function () {
  $('#search-submit').on('click', function () {
    let val = $('#search-input2').val().trim();
    const regex = new RegExp("[|!><$%*?.{}@#,'/+~]", "g")
    let strReplace = val.replace(regex, "")
    let searchVal = strReplace.indexOf('&')
    if (strReplace.length !== 0) {
      if (searchVal !== -1) {
        let str = strReplace.replace('&', '＆');
        location.href = '/search/' + str;
      } else {
        location.href = '/search/' + strReplace;
      }
    }
  });

  $('.search-input').on('keydown', function (e) {
    let keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
      let val = $(this).val().trim();
      const regex = new RegExp("[|!><$%*?.{}@#,'/+~]", "g")
      let strReplace = val.replace(regex, "")
      let searchVal = strReplace.indexOf('&')
      if (strReplace.length !== 0) {
        if (searchVal !== -1) {
          let str = strReplace.replace('&', '＆');
          location.href = '/search/' + str;
        } else {
          location.href = '/search/' + strReplace;
        }
      } else {
        $("#searchText").text("請輸入有效之關鍵字")
      }
    }
  })

  $('#search-clear').on('click', function () {
    $('#search-input').val('')
  })
});