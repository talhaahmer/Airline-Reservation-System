(function($){
	"use strict";

	// Header Sticky
	$(window).on('scroll',function() {
		if ($(this).scrollTop() > 120){  
			$('.navbar-area').addClass("is-sticky");
		}
		else{
			$('.navbar-area').removeClass("is-sticky");
		}
	});

	// Mean Menu
	$('.mean-menu').meanmenu({
		meanScreenWidth: "991"
	});

	// Navbar Search
	$(".others-option .search-btn").on("click", function(){
		$(".search-overlay").toggleClass("search-overlay-active");
	});
	$(".search-overlay-close").on("click", function(){
		$(".search-overlay").removeClass("search-overlay-active");
	});

	// Performance Specifications Image Slides
	$('.performance-specifications-image-slides').owlCarousel({
		items: 1,
		dot: true,
		nav: false,
		loop: true,
		autoplay: true,
		animateOut: 'fadeOut',
		autoplayHoverPause: false
	});

	// Feedback Slides
	$('.feedback-slides').owlCarousel({
		nav: true,
		loop: true,
		margin: 25,
		dots: false,
		autoplay: false,
		autoplayHoverPause: true,
		navText: [
			"<i class='ph-bold ph-arrow-left'></i>",
			"<i class='ph-bold ph-arrow-right'></i>",
		],
		responsive: {
			0: {
				items: 1
			},
			576: {
				items: 1
			},
			768: {
				items: 2
			},
			992: {
				items: 2
			},
			1200: {
				items: 2
			}
		}
	});

	


	// Go to Top
	$(function(){
		// Scroll Event
		$(window).on('scroll', function(){
			var scrolled = $(window).scrollTop();
			if (scrolled > 600) $('.go-top').addClass('active');
			if (scrolled < 600) $('.go-top').removeClass('active');
		});  
		// Click Event
		$('.go-top').on('click', function() {
			$("html, body").animate({ scrollTop: "0" },  500);
		});
	});

	// Preloader
	$(window).on('load', function() {
		$('.preloader-area').addClass('deactivate');
	});

}(jQuery));

// Counter Js
if ("IntersectionObserver" in window) {  
	let counterObserver = new IntersectionObserver(function (entries, observer) {
		entries.forEach(function (entry) {
			if (entry.isIntersecting) {
			let counter = entry.target;
			let target = parseInt(counter.innerText);
			let step = target / 200;
			let current = 0;
			let timer = setInterval(function () {
				current += step;
				counter.innerText = Math.floor(current);
				if (parseInt(counter.innerText) >= target) {
				clearInterval(timer);
				}
			}, 10);
			counterObserver.unobserve(counter);
			}
		});
	});
	let counters = document.querySelectorAll(".counter");
	counters.forEach(function (counter) {
		counterObserver.observe(counter);
	});
}

// ScrollCue
scrollCue.init();