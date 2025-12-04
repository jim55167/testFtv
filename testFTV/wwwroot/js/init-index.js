$(function () {
  if ($('.marquee').length) {
    const mySwiper = new Swiper('.marquee', {
      autoplay: {
        delay: 5000
      },
      grabCursor: true,
      //slidesPerView: 1,
      //loopFillGroupWithBlank: true,
      spaceBetween: 0,
      centeredSlides: true,
      loop: true,
      height: 50,
      autoHeight: true,
      direction: 'vertical',
      navigation: {
        nextEl: '.btn-right',
        prevEl: '.btn-left'
      }
    });
  }
});



$(function () {
  if ($('.focus-news').length) {
    var mySwiper = new Swiper('.focus-news', {
      autoplay: {
        delay: 5000
      },
      grabCursor: true,
      slidesPerView: 1,
      loopFillGroupWithBlank: true,
      spaceBetween: 0,
      centeredSlides: true,
      loop: true,
      pagination: {
        el: ".swiper-pagination",
      },
      navigation: {
        nextEl: '.btn-right',
        prevEl: '.btn-left'
      }
    });
  }
});

$(function () {
  if ($('.anchor-list').length) {
    const mySwiper = new Swiper('.anchor-list', {
      autoplay: {
        delay: 5000,
        waitForTransition: false
      },
      grabCursor: true,
      slidesPerView: 3,
      loopFillGroupWithBlank: true,
      spaceBetween: 20,
      centeredSlides: true,
      loop: true,
      effect: "coverflow",
      coverflowEffect: {
        rotate: 20,
        slideShadows: false,
        depth: 100,
        stretch: -30,
      },
      breakpoints: {
        576: {
          slidesPerView: 1
        },
        768: {
          slidesPerView: 1
        },
        992: {
          slidesPerView: 3
        },
        1200: {
          slidesPerView: 3
        }
      },
      navigation: {
        nextEl: '.btn-right',
        prevEl: '.btn-left'
      }
    });
  }
});

$(function () {
  if ($('.program-news').length) {
    var mySwiper = new Swiper('.program-news', {
      autoplay: {
        delay: 5000
      },
      grabCursor: true,
      slidesPerView: 3,
      spaceBetween: 20,
      centeredSlides: true,
      loop: true,
      breakpoints: {
        576: {
          slidesPerView: 1
        },
        768: {
          slidesPerView: 1
        },
        992: {
          slidesPerView: 3
        },
        1200: {
          slidesPerView: 3
        }
      },
      navigation: {
        nextEl: '.btn-right',
        prevEl: '.btn-left'
      }
    });
  }
});

//首頁圖片導航
$(function () {
  if ($('.swiper-inner').length) {
    const mySwiper = new Swiper('.swiper-inner', {
      lazy: true,
      slidesPerView: 9,
      spaceBetween: 10,
      breakpoints: {
        768: {
          slidesPerView: 4,
        },
        1024: {
          slidesPerView: 5,
        },
        1280: {
          slidesPerView: 7,
        },
      }
    });
  }
});

$(function () {
  if ($('.carouselImage').length) {
    const mySwiper = new Swiper('.carouselImage', {
      loop: true,
      lazy: true,
      slidesPerView: "auto",
      spaceBetween: 20,
      centeredSlides: true,
      pagination: {
        el: ".swiper-pagination",
      },
      navigation: {
        nextEl: '.btn-right',
        prevEl: '.btn-left'
      },
      autoplay: {
        delay: 6000,
        disableOnInteraction: false
      },
    });
  }
});

$(function () {
  if ($('.owl-item').length) {
    const mySwiper = new Swiper('.owl-item', {
      autoplay: {
        delay: 6000,
        disableOnInteraction: false
      },
      grabCursor: true,
      spaceBetween: 30,
      slidesPerView: 1,
      centeredSlides: true,
      loop: true,
      navigation: {
        nextEl: '.btn-right',
        prevEl: '.btn-left'
      },
      pagination: {
        el: ".swiper-pagination",
        clickable: true,
      }
    });
  }
});

$(function () {
  if ($('#short-video').length) {
    const mySwiper = new Swiper('#short-video', {
      autoplay: {
        delay: 7000,
        disableOnInteraction: false
      },
      grabCursor: true,
      spaceBetween: 35,
      slidesPerView: 5,
      loop: true,
      breakpoints: {
        640: {
          slidesPerView: 2,  
          spaceBetween: 20,
        },
        1280: {
          slidesPerView: 3,
          spaceBetween: 30,
        },
        1800: {
          slidesPerView: 4, 
        }
      },
      navigation: {
        nextEl: '.short-btn-right',
        prevEl: '.short-btn-left'
      },
    });
  }
});

$(function () {
  //youtube Remove 'Deleted video'
  var livechannel = $(".live-channel ul li")
  var livechanneltitle = $(".live-channel ul li a .content .title")
  $(livechanneltitle).each(function (index) {
    if ($(this).text() === 'Deleted video') {
      $(this).parent().parent().parent().addClass("d-none");
    } else {
      $(this).parent().parent().parent().removeClass("d-none");
    }
  });
});