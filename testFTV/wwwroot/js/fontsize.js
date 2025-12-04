function setFontSize(size) {
  switch (size) {
    case 'small':
      //$('#contentarea').css('font-size', 'small');
      $('.article-body').addClass('font-s').removeClass('font-m font-l');
      $('.fontsize .mid').removeClass('selected');
      $('.fontsize .big').removeClass('selected');

      $('.fontsize .sml').addClass('selected');

      break;
    case 'medium':
      //$('#contentarea').css('font-size', 'medium');
      $('.article-body').addClass('font-m').removeClass('font-s font-l');
      $('.fontsize .sml').removeClass('selected');
      $('.fontsize .big').removeClass('selected');

      $('.fontsize .mid').addClass('selected');
      break;
    case 'large':
      //$('#contentarea').css('font-size', 'large');
      $('.article-body').addClass('font-l').removeClass('font-m font-s');
      $('.fontsize .mid').removeClass('selected');
      $('.fontsize .sml').removeClass('selected');

      $('.fontsize .big').addClass('selected');
      break;

    default:
      break;
  }
}

$('.fontsize .sml').on('click', function (e) {
  e.preventDefault();
  setFontSize('small');
});

$('.fontsize .mid').on('click', function (e) {
  e.preventDefault();
  setFontSize('medium');
});

$('.fontsize .big').on('click', function (e) {
  e.preventDefault();
  setFontSize('large');
});
