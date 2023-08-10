

// (function ($) {
// "use strict"; 


// var active = false;
// var hover = false;
// $(document).ready(function () {
//     $(".sub-menu .content .hover-menu ul li").hover(function () {
//         $(this).children("ul").show();
//     }, function () {
//         $(this).children("ul").hide();
//     });
// });



/*----------
  Set header(991)
  ----------*/

function responsiveheader() {
  var this_window_width = $(window).width();
  {
    if (this_window_width >= 1200) {
      jQuery('.header-center').insertBefore('.header-right');
    }
    else if (this_window_width <= 1199 && this_window_width >= 992) {
      jQuery('.header-center').insertAfter('.header-inner');
    }
    else {
      jQuery('.header-center').insertBefore('.search-content');
    }
    prevWidth = this_window_width;
    // prevWidth = $(window).width();
  }
}


/*----------
    Update column & content in responsive
    -----------*/
function updateColumnsAndContent() {
  if ($(window).width() < 992) {
    $('#column-left, #column-right').insertAfter('#content');

    // menu
    if ($("#menu .dropdown.menulist .toggle-menu").length == 0) {
      $("#menu .dropdown.menulist").append("<span class='toggle-menu'><i class='fa fa-plus'></i></span>");
      $("#menu .dropdown.menulist .dropdown-submenu.sub-menu-item").append("<span class='toggle-menu'><i class='fa fa-plus'></i></span>");
      $('#topCategoryList ul.sub-menu').removeAttr("style");
      $('#topCategoryList div.dropdown-menu').removeAttr("style");
      $('#topCategoryList').hide();
      // call explan-collapse
      responsiveMenuExpandCollapse();
    }

    // left, right
    $("#column-left .box-category .toggle-open, #column-right .box-category .toggle-open, #column-left .box-content .toggle-open, #column-right .box-content .toggle-open").remove();
    $("#column-left .box-category h3, #column-right .box-category h3, #column-left .box-content h3, #column-right .box-content h3").append("<span class='toggle-open'><i class='fa fa-chevron-down'></i></span>");
    $('#column-left ul.parent, #column-right ul.parent, #column-left .block_box, #column-right .block_box, #column-left .box-content ul, #column-right .box-content ul, #column-left .filter_box, #column-right .filter_box').hide();

    // footer
    if ($(".footer-top .toggle-open").length == 0) {
      $(".footer-top h5").append("<span class='toggle-open'><i class='fa fa-chevron-down'></i></span>");
      $('.footer-top ul.list-unstyled').hide();
    }
  } else {
    $('#column-right').insertAfter('#content');
    $('#column-left').insertBefore('#content');

    // menu
    $("#menu .dropdown.menulist .toggle-menu").remove();
    $('#topCategoryList').show();
    $('#topCategoryList ul.sub-menu').removeAttr("style");
    $('#topCategoryList div.dropdown-menu').css("display", "");

    // left, right
    $("#column-left .box-category .toggle-open, #column-left .box-content .toggle-open").remove();
    $("#column-right .box-category .toggle-open, #column-right .box-content .toggle-open").remove();
    $('#column-left ul.parent, #column-right ul.parent, #column-left .block_box, #column-right .block_box, #column-left .box-content ul, #column-right .box-content ul, #column-left .filter_box, #column-right .filter_box').show();

    // footer
    $(".footer-top .toggle-open").remove();
    $('.footer-top ul.list-unstyled').show();
  }
}


// Product List
$('#list-view').click(function() {
  $('#content .product-grid > .clearfix').remove();

  $('#content .row > .product-grid').attr('class', 'product-layout product-list col-xs-12');
  $('#grid-view').removeClass('active');
  $('#list-view').addClass('active');

  localStorage.setItem('display', 'list');
});













/*----------
    Responsive menu
    ----------*/
function responsiveMenuExpandCollapse() {
  // expand-collapse
  $('#topCategoryList .dropdown.menulist .toggle-menu').on('click', function (e) {
    e.preventDefault();
    if ($(this).parent().find('> ul.sub-menu').length != 0) {
      $(this).parent().find('> ul.sub-menu').slideToggle('slow');
    } else {
      $(this).parent().find('> div.dropdown-menu').slideToggle('slow');
    }
    $(this).parent().toggleClass('active');
    $(this).toggleClass('active');
    return false;

  });
}


/*----------
  Footer Toggle
  ----------*/
function footerExplanCollapse() {
  $(".footer-top h5").addClass('toggled');
  $('.footer-top .toggled').on('click', function (e) {
    e.preventDefault();
    if ($(window).width() < 992) {
      $(this).toggleClass('active');
      $(this).parent().find('ul').toggleClass('active').toggle('slow');
    }
  });
}



/*----------
   Category page click events
   ----------*/
function clickEventsInCategoryPage() {
  $('.box-category .toggled').on('click', function (e) {
    e.preventDefault();
    if ($(window).width() < 992) {
      $(this).toggleClass('active');
      $(this).parent().find('ul.parent').toggleClass('active').slideToggle('slow');
    }
  });

  $('#column-left .box-content .toggled').on('click', function (e) {
    e.preventDefault();
    if ($(window).width() < 992) {
      $(this).toggleClass('active');
      if ($(this).parent().find('ul').length != 0) {
        $(this).parent().find('ul').toggleClass('active').slideToggle('slow');
      } else {
        $(this).parent().find('.filter_box').toggleClass('active').slideToggle('slow');
        $(this).parent().find('.block_box').toggleClass('active').slideToggle('slow');
      }
    }
  });

  $('#column-right .box-content .toggled').on('click', function (e) {
    e.preventDefault();
    if ($(window).width() < 992) {
      $(this).toggleClass('active');
      if ($(this).parent().find('ul').length != 0) {
        $(this).parent().find('ul').toggleClass('active').slideToggle('slow');
      } else {
        $(this).parent().find('.filter_box').toggleClass('active').slideToggle('slow');
        $(this).parent().find('.block_box').toggleClass('active').slideToggle('slow');
      }
    }
  });
}

$(document).ready(function () {
  // Highlight any found errors
  $('.text-danger').each(function () {
    var element = $(this).parent().parent();

    if (element.hasClass('form-group')) {
      element.addClass('has-error');
    }
  });

  // Currency
  $('#form-currency .currency-select').on('click', function (e) {
    e.preventDefault();

    $('#form-currency input[name=\'code\']').val($(this).attr('name'));

    $('#form-currency').submit();
  });

  // Language
  $('#form-language .language-select').on('click', function (e) {
    e.preventDefault();

    $('#form-language input[name=\'code\']').val($(this).attr('name'));

    $('#form-language').submit();
  });

  /* Search */
  $('#search input[name=\'search\']').parent().find('button').on('click', function () {
    var url = $('base').attr('href') + 'index.php?route=product/search';

    var value = $('header #search input[name=\'search\']').val();

    if (value) {
      url += '&search=' + encodeURIComponent(value);
    }

    location = url;
  });

  $('#search input[name=\'search\']').on('keydown', function (e) {
    if (e.keyCode == 13) {
      $('header #search input[name=\'search\']').parent().find('button').trigger('click');
    }
  });

  // Menu
  $('#menu .dropdown-menu').each(function () {
    var menu = $('#menu').offset();
    var dropdown = $(this).parent().offset();

    var i = (dropdown.left + $(this).outerWidth()) - (menu.left + $('#menu').outerWidth());

    if (i > 0) {
      $(this).css('margin-left', '-' + (i + 10) + 'px');
    }
  });

  // Product List
  $('#list-view').click(function () {
    $('#content .product-grid > .clearfix').remove();

    $('#content .row > .product-grid').attr('class', 'product-layout product-list col-xs-12');
    $('#grid-view').removeClass('active');
    $('#list-view').addClass('active');

    localStorage.setItem('display', 'list');
  });

  // Product Grid
  $('#grid-view').click(function () {
    // What a shame bootstrap does not take into account dynamically loaded columns
    var cols = $('#column-right, #column-left').length;

    if (cols == 2) {
      $('#content .product-list').attr('class', 'product-layout product-grid col-lg-6 col-md-6 col-sm-4 col-xs-4');
    } else if (cols == 1) {
      $('#content .product-list').attr('class', 'product-layout product-grid col-lg-4 col-md-6 col-sm-4 col-xs-4');
    } else {
      $('#content .product-list').attr('class', 'product-layout product-grid col-lg-3 col-md-4 col-sm-4 col-xs-4');
    }

    $('#list-view').removeClass('active');
    $('#grid-view').addClass('active');

    localStorage.setItem('display', 'grid');
  });

  if (localStorage.getItem('display') == 'list') {
    $('#list-view').trigger('click');
    $('#list-view').addClass('active');
  } else {
    $('#grid-view').trigger('click');
    $('#grid-view').addClass('active');
  }

  // Checkout
  $(document).on('keydown', '#collapse-checkout-option input[name=\'email\'], #collapse-checkout-option input[name=\'password\']', function (e) {
    if (e.keyCode == 13) {
      $('#collapse-checkout-option #button-login').trigger('click');
    }
  });

  // tooltips on hover
  // $('[data-toggle=\'tooltip\']').tooltip({container: 'body'});

  // Makes tooltips work on ajax generated content
  $(document).ajaxStop(function () {
    $('[data-toggle=\'tooltip\']').tooltip({ container: 'body' });
  });
});



/* Contact Form */
if (window.location.href.indexOf('submitted=true') !== -1) {
  $('.submitted').show();
}
/* .Contact Form */








jQuery(document).ready(function () {
  // responsive header
  responsiveheader();
  // category page click events
  clickEventsInCategoryPage();
  updateColumnsAndContent();
  // footer
  footerExplanCollapse();
  

  /* headerfixed */
  var headerfixed = 1;
  if (headerfixed == 1) {
    $(window).scroll(function () {
      if ($(this).scrollTop() > 160) {
        $('header').addClass('header-fixed');
      } else {
        $('header').removeClass('header-fixed');
      }
    });
  }
  else {
    $('header').removeClass('header-fixed');
  }

  /* .headerfixed */
});

jQuery(window).resize(function () {
  responsiveheader();
  updateColumnsAndContent();
});

function set_cart_scroll(){
  var header_height = $("header").height();
  var screen_height = $( window ).height();
  var cart_list_height = $("#cart .dropdown-menu .table-striped").height();
  var cart_total_height = $("#cart .dropdown-menu li+li").height();
  var cart_div_height = parseInt(cart_list_height) + parseInt(cart_total_height) + 10;
  var cart_div_max_height = parseInt(screen_height) - parseInt(header_height);
  var cart_total_pro = jQuery('.cart-content-product table  tr').length;
  if(screen_height < cart_div_height){
    $("#cart .dropdown-menu").css({"overflow-y":"unset","max-height":"unset"});
    $("ul.dropdown-menu.pull-right").addClass("scroll_cart_dropdown").css({"overflow-y":"auto","max-height":cart_div_max_height+"px"});
  } else {   
    $("ul.dropdown-menu.pull-right").removeClass("scroll_cart_dropdown").css({"overflow-y":"unset","max-height":"unset"});
    $("#cart .dropdown-menu ul").css({"overflow-y":"auto","max-height":"240px"});
  }
}

/*----------
   Dropdown Toggle
   ----------*/
$(function () {
  $("#form-language .dropdown-toggle").on('click', function () {
    $(".language-dropdown").slideToggle("2000");
    $(".header-cart-toggle, .currency-dropdown, .account-link-toggle").slideUp("slow");
    return false;
  });

  $("#form-currency .dropdown-toggle").on('click', function () {
    $(".currency-dropdown").slideToggle("2000");
    $(".header-cart-toggle, .language-dropdown, .account-link-toggle").slideUp("slow");
    return false;
  });

  $("#cart button.dropdown-toggle").on('click', function () {
    $(".header-cart-toggle").slideToggle("2000");
    $(".account-link-toggle, .currency-dropdown, .language-dropdown").slideUp("slow");
    setTimeout(function() { set_cart_scroll(); }, 500);
    return false;
  });

  $("#header_ac a.dropdown-toggle").on('click', function () {
    $(".account-link-toggle").slideToggle("2000");
    $(".header-cart-toggle, .currency-dropdown, .language-dropdown").slideUp("slow");
    return false;
  });

  $(".search-content .search-btn-outer").on('click', function () {
    $(this).toggleClass('active');
    $(".header-search").slideToggle("2000");
    $(".account-link-toggle, .currency-dropdown, .header-cart-toggle, .language-dropdown").hide();
    return false;
  });

  // Hide Search Dropdown On Scroll 
  $(window).scroll(function () {
    $('.ui-autocomplete.ui-widget-content').hide();
  });

});

$(document).on('click', function () {
  $(".header-cart-toggle, .currency-dropdown, .language-dropdown, .account-link-toggle").slideUp('slow')
});



jQuery(".additional-carousel a.elevatezoom-gallery").click(function (e) {
  e.preventDefault();
  // var this_img = jQuery(this).find("img");
  // var this_img_src = jQuery(this_img).attr("src");
  var this_img_src = jQuery(this).attr("data-zoom-image");
  jQuery("#prozoom").attr("src", this_img_src);
  // console.log(this_img_src);
  return;
});






/* Slider Js */

$('.banner-owl-carousel').owlCarousel({
  loop: true,
  dots: false,
  nav: true,
  margin: 0,
  rewind: false,
  navText: ['<i class="fa fa-angle-left" aria-hidden="true">', '<i class="fa fa-angle-right" aria-hidden="true">'],
  autoplay: false,
  autoplayTimeout: 3000,
  animateOut: 'fadeOut',
  items: 1,
  responsiveClass: false,
  responsive: {

    0: {
      items: 1
    },
    600: {
      items: 1
    },
    1024: {
      items: 1
    }
  }
});


$(document).ready(function () {
  var owl = $('.banner-owl-carousel');
 

  const items = 4;
  $('.top-latest').owlCarousel({
    loop: false,
    dots: false,
    nav: true,
    padding: 15,
    rewind: false,
    navText: ['<i class="fa fa-angle-left" aria-hidden="true">', '<i class="fa fa-angle-right" aria-hidden="true">'],
    autoplay: false,
    autoplayTimeout: 3000,
    animateOut: 'fadeOut',
    items: items,
    responsiveClass: false,
    responsive: {
      0: { items: 1 },
      320: { items: ((items - 2) > 1) ? (items - 2) : 1, },
      541: { items: ((items - 1) > 1) ? (items - 1) : 1, },

      1200: { items: items }
    }
  });

  $('.top-special').owlCarousel({
    loop: false,
    dots: false,
    nav: true, 
    rewind: false,
    navText: ['<i class="fa fa-angle-left" aria-hidden="true">', '<i class="fa fa-angle-right" aria-hidden="true">'],
    autoplay: false,
    autoplayTimeout: 3000,
    animateOut: 'fadeOut',
    items: items,
    responsiveClass: false,
    responsive: {
      0: { items: 1 },
      320: { items: ((items - 2) > 1) ? (items - 2) : 1 },
      541: { items: ((items - 1) > 1) ? (items - 1) : 1 },
      1200: { items: items }
    }
  });
  $('.top-bestseller').owlCarousel({
    loop: false,
    dots: false,
    nav: true, 
    rewind: false,
    navText: ['<i class="fa fa-angle-left" aria-hidden="true">', '<i class="fa fa-angle-right" aria-hidden="true">'],
    autoplay: false,
    autoplayTimeout: 3000,
    animateOut: 'fadeOut',
    items: items,
    responsiveClass: false,
    responsive: {
      0: { items: 1 },
      320: { items: ((items - 2) > 1) ? (items - 2) : 1 },
      541: { items: ((items - 1) > 1) ? (items - 1) : 1 },
      1200: { items: items }
    }
  });

  /*----------
 Slider for product
  ----------*/



  /* category_carousel */
  $('.category-carousel').owlCarousel({
    loop: true,
    dots: false,
    nav: true,
    rewind: false,
    navText: ['<i class="fa fa-angle-left" aria-hidden="true">', '<i class="fa fa-angle-right" aria-hidden="true">'],
    autoplay: false,
    autoplayTimeout: 3000,
    animateOut: 'fadeOut',
    items: 1,
    responsiveClass: false,
    responsive: {

      300: {
        items: 2
      },
      375: {
        items: 2
      },
      425: {
        items: 2
      },
      480: {
        items: 2
      },
      481: {
        items: 3
      },
      992: {
        items: 4
      },
      1024: {
        items: 4
      }
    }
  });
  /* .category_carousel */

  /* Customers_Said  */

  $('.customers-said').owlCarousel({
    loop: true,
    dots: true,
    nav: false,
    rewind: false,
    navText: ['<i class="fa fa-angle-left" aria-hidden="true">', '<i class="fa fa-angle-right" aria-hidden="true">'],
    autoplay: false,
    autoplayTimeout: 3000,
    animateOut: 'fadeOut',
    items: 1,
    responsiveClass: false,
    responsive: {
      480: {
        items: 1
      } 
    }
  });

  /* .Customers_Said  */

  /* FROM THE BLOG  */
  $('.blog-carousel').owlCarousel({
    loop: true,
    dots: false,
    nav: true,
    rewind: false,
    navText: ['<i class="fa fa-angle-left" aria-hidden="true">', '<i class="fa fa-angle-right" aria-hidden="true">'],
    autoplay: false,
    autoplayTimeout: 3000,
    animateOut: 'fadeOut',
    items: 1,
    responsiveClass: false,
    responsive: {
      600: {
        items: 1
      },
      601: {
        items: 2
      },
      767: {
        items: 2
      },
      991: {
        items: 2
      },
      992: {
        items: 2
      },
      1024: {
        items: 2
      },
      1200: {
        items: 3
      }
    }
  });

  /* FROM THE BLOG  */

  /* company  brand  */

  $('.brand-carousel').owlCarousel({
    loop: true,
    dots: false,
    nav: true,
    rewind: false,
    navText: ['<i class="fa fa-angle-left" aria-hidden="true">', '<i class="fa fa-angle-right" aria-hidden="true">'],
    autoplay: false,
    autoplayTimeout: 3000,
    animateOut: 'fadeOut',
    items: 1,
    responsiveClass: false,
    responsive: {

      301: {
        items: 2
      },
      425: {
        items: 2
      },
      426: {
        items: 3
      },
      
      601: {
        items: 4
      },
      1024: {
        items: 5
      },
      
      1199: {
        items: 5
      },
      1200: {
        items: 6
      }
    }
  });

  /* .company brand */

  /*  Buttercrust */

  $('.additional-carousel').owlCarousel({
    loop: false,
    dots: false,
    nav: true,
    rewind: false,
    navText: ['<i class="fa fa-angle-left" aria-hidden="true">', '<i class="fa fa-angle-right" aria-hidden="true">'],
    autoplay: false,
    autoplayTimeout: 3000,
    animateOut: 'fadeOut',
    items: 5,
    responsiveClass: false,
    responsive: {
      301: {
        items: 2
      },
      425: {
        items: 3
      },
      480: {
        items: 3
      },
      481: {
        items: 4
      }, 
      1200: {
        items: 4
      }
    }
  });


  $('.related-products-1').owlCarousel({
    loop: true,
    dots: false,
    nav: true,
    padding: 15,
    rewind: false,
    navText: ['<i class="fa fa-angle-left" aria-hidden="true">', '<i class="fa fa-angle-right" aria-hidden="true">'],
    autoplay: false,
    autoplayTimeout: 3000,
    animateOut: 'fadeOut',
    items: items,
    responsiveClass: false,
    responsive: {
      0: { items: 1 },
      320: { items: ((items - 2) > 1) ? (items - 2) : 1, margin: 5 },
      541: { items: ((items - 1) > 1) ? (items - 1) : 1, },
      991: {
        items: 3
      },
      1200: { items: items }
    }
  });
});

let prevWidth = null;
(function ($) {
    "use strict";
    /*----------
    Loader
    ----------*/
    $(window).on("load", function () {
        $('.loader').fadeOut('slow',function(){
            $(this).remove();
        });
    });




    $(document).ready(function() {        
        // Back to top
        backToTop(); 
    });

     
    
    /*----------
    quantity seter
    ----------*/
    $( document ).on( 'click', '.plus, .minus', function( e ) {
        e.preventDefault();
        var parent = $( this ).parents( '.product-btn-quantity' );
        var quantity = parent.find( '[name="quantity"]' );
        var val = quantity.val();
        if ( $( this ).hasClass( 'plus' ) ) {
            val = parseInt( val ) + 1;
        } else {
            if(val == 1) {
                val = 1;
            }
            else {
                val = val >= 1 ? parseInt( val ) - 1 : 0;
            }
        }
        quantity.val( val );
        quantity.trigger("change");
        return false;
    });

    /*----------
    Back to top function
    ----------*/

    function backToTop() {
        //Check to see if the window is top if not then display button
        $(".scrollToTop").hide();
        $(window).scroll(function(){
            if ($(this).scrollTop() > 250) {
                $('.scrollToTop').fadeIn();
            } else {
                $('.scrollToTop').fadeOut();
            }
        });
        //Click event to scroll to top
        $('.scrollToTop').on('click',function(){
            $('html, body').animate({scrollTop : 0},800);
            return false;
        });
    }

    /*----------
    Hide Tooltip After Click
    ----------*/
    $(document).ready(function(){
        $('[data-toggle="tooltip"]').click(function () {
           $('[data-toggle="tooltip"]').tooltip("hide");
 
        });
    });

})(jQuery);