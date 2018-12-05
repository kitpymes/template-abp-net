/* 
******************** SE UTILIZA PARA JAVASCRIPT:

- Funciones autoejecutables.
- Patrón Modulos.

******************** REFERENCIAS
 
- http://www.etnassoft.com/2011/03/14/funciones-autoejecutables-en-javascript/
- https://toddmotto.com/what-function-window-document-undefined-iife-really-means/

******************** BUENAS PRACTICAS

- Saber si una variable es null o no ha sido definida:

var data;
console.log(!data); // true

var data1 = null;
console.log(!data1); // true

- Saber si una variable es solo null:

var data = null;
console.log(data === null); //true

- Saber si una variable es solo de tipo undefined, utilizar el operador typeof, 
el cual retorna en formato String, el nombre del tipo de dicha variable:

usar typeof: Una razón para usar typeof es que no devuelve un error si la variable no fue declarada.
var x;
// x no fue declarada antes
if (typeof x === 'undefined') { // devuelve true
   //se ejecutan estas instrucciones
}

if (x === undefined) { // devuelve un ReferenceError

}

******************** EJEMPLO:

var MODULE = (function ($, SITE, window, document, abp, undefined) {

    if (!$) return;

    var SELF,
        config = {
            settings: SITE.config.settings, 
            // Si precisamos extender settings =>
            settings: $.extend(SITE.config.settings, { 
            
            }),
            elems: SITE.config.elems,
            // Si precisamos extender elems =>
            elems: $.extend(SITE.config.elems, { 
            
            }),
            elems: SITE.config.validation,
            // Si precisamos extender validation =>
            validation: $.extend(SITE.config.validation, { 
            
            })
        };

    function init(options) {
        config.settings = $.extend(config.settings, options);
        SELF = config;
        bindUIActions();
    }

    function bindUIActions() {

        var bindXxxxxxx = function () {
                 
        }

        var bindGlobal = function () {
            
        }

        bindXxxxxxx();
        bindGlobal();
    }

    return {
        init: init
    };

})(jQuery, SITE || {}, window, document, abp);

MODULE.init();

*/


SITE = (function ($, window, document, abp, undefined) {
    "use strict";

    if (!$) return;

    var SELF,
        AREA = {},
        config = {
            settings: {
                background: {
                    colorDefault: "white",
                    imageDefault: "/Content/images/background/1.jpg",
                    imageRandom: false,
                    imageRange: { MIN: 1, MAX: 52 },
                    selectorDefault: $("body"),
                    effectType: { SEGMENTER: 1, PARTICLES: 2, SLIDES: 3},
                    setBGColor: function (selector, color) {
                        selector = selector || config.settings.background.selectorDefault;
                        selector.style.backgroundColor = color || config.settings.background.colorDefault;
                    },
                    setBGImage: function (selector, imageDefault, imageRandom) {
                        selector = selector || config.settings.background.selectorDefault;
                        imageDefault = imageDefault || config.settings.background.imageDefault;
                        imageRandom = imageRandom || config.settings.background.imageRandom;

                        if (imageRandom) {
                            imageDefault = abp.utils.formatString("/Content/images/background/{0}.jpg",
                                config.settings.getRandomInt(config.settings.background.imageRange.MIN, config.settings.background.imageRange.MAX));
                        }
                     

                        selector.css("background-image", "url(" + imageDefault + ")");
                    },
                    setBGEffect: function (selector, effectType) {
                        selector = selector || config.settings.background.selectorDefault;

                        switch (effectType) {
                            case config.settings.background.effectType.SEGMENTER:

                                $('head').append('<link rel="stylesheet" href="/Content/effects/segmente/segmenteffect.min.css" type="text/css" />');
                                $.when(
                                    $.getScript("/Content/effects/segmente/anime.min.js"),
                                    $.getScript("/Content/effects/segmente/imagesloaded.pkgd.min.js"),
                                    $.getScript("/Content/effects/segmente/segmenteffect.min.js"),
                                    $.Deferred(function (deferred) {
                                        $(deferred.resolve);
                                    })
                                ).done(function () {

                                    var headline = selector.headline.get(0),
                                      segmenter = new Segmenter(selector.get(0), {
                                          pieces: 19,
                                          positions: [
                                              { top: 30, left: 5, width: 40, height: 80 },
                                              { top: 50, left: 25, width: 30, height: 30 },
                                              { top: 5, left: 75, width: 40, height: 20 },
                                              { top: 30, left: 45, width: 40, height: 20 },
                                              { top: 45, left: 15, width: 50, height: 40 },
                                              { top: 10, left: 40, width: 10, height: 20 },
                                              { top: 20, left: 50, width: 30, height: 70 },
                                              { top: 0, left: 10, width: 50, height: 60 },
                                              { top: 70, left: 40, width: 30, height: 30 }
                                          ],
                                          animation: {
                                              duration: 6000,
                                              easing: 'easeInOutCubic',
                                              delay: 1,
                                              opacity: 0.8,
                                              translateZ: 85,
                                              translateX: { min: -20, max: 20 },
                                              translateY: { min: -20, max: 20 }
                                          },
                                          parallax: true,
                                          parallaxMovement: { min: 5, max: 10 },
                                          onReady: function () {
                                              segmenter.animate();
                                              headline.classList.remove('trigger-headline--hidden');
                                          }
                                      });

                                });

                                break;
                            case config.settings.background.effectType.PARTICLES:
                                $.when(
                                    $.getScript("/Content/effects/particles/particles.min.js"),
                                    $.Deferred(function (deferred) {
                                        $(deferred.resolve);
                                    })
                                ).done(function () {

                                    particlesJS(selector.get(0), {
                                        particles: {
                                            color: '#fff',
                                            shape: 'circle', // "circle", "edge" or "triangle"
                                            opacity: 1,
                                            size: 10,
                                            size_random: true,
                                            nb: 150,
                                            line_linked: {
                                                enable_auto: true,
                                                distance: 100,
                                                color: '#fff',
                                                opacity: 1,
                                                width: 1,
                                                condensed_mode: {
                                                    enable: false,
                                                    rotateX: 600,
                                                    rotateY: 600
                                                }
                                            },
                                            anim: {
                                                enable: true,
                                                speed: 1
                                            }
                                        },
                                        interactivity: {
                                            enable: true,
                                            mouse: {
                                                distance: 250
                                            },
                                            detect_on: 'canvas', // "canvas" or "window"
                                            mode: 'grab',
                                            line_linked: {
                                                opacity: .5
                                            },
                                            events: {
                                                onclick: {
                                                    enable: true,
                                                    mode: 'push', // "push" or "remove" (particles)
                                                    nb: 4
                                                }
                                            }
                                        },
                                        /* Retina Display Support */
                                        retina_detect: true
                                    });

                                });

                                break;

                            case config.settings.background.effectType.SLIDES:
                                break;
                            default:
                        }
                    }
                },
                font: {
                    color: "white"
                },
                floatlabel: {
                    content: ".float-label"
                },
                currentPageUrl: "",
                setDisabledElem: function (elem, disabled) {
                    elem.attr("disabled", disabled);
                },
                setFocusElem: function (elem) {
                    window.setTimeout(function () {
                        elem.focus();
                    }, 1000);
                },
                setCurrentPageUrl: function (url) {
                    config.settings.currentPageUrl = url;
                },
                setFloatLabel: function (content) {
                    var selector = content || this.floatlabel.content;

                    $(selector).FloatLabel();
                },
                getRandomInt: function (min, max) {
                    return Math.floor(Math.random() * (max - min)) + min;
                },
                callAjaxServer: function (url, data, type, dataType) {
                    if (!url)return;

                    return abp.ajax({
                        url: url,
                        data: JSON.stringify(data),
                        type: type || "POST",
                        dataType: dataType || "json"
                    });
                }
            },
            elems: {
                contentScroll: $('.scrollable-content'),
                bodyArea: $('#BodyArea'),
                tabContent: $('#tab-content .tab-pane'),
                getNav: function () {
                    var navArea = $('#NavArea'),
                        navUserArea = navArea.find('#NavUserArea'),
                        navModulesArea = navArea.find('#NavModulesArea'),

                        navUser = {
                            btnFullScreenNavUser: navUserArea.find('.icon-full-screen a'),
                            btnShowMenuNavUser: navUserArea.find('.toggle-menu a'),
                            btnConfigurationNavUser: navUserArea.find('.icon-setting a'),
                            btnLogoutNavUser: navUserArea.find('.icon-logout  a')
                        },

                        navModules = {
                            titleNavModules: navModulesArea.find('.modules-title'),
                            iconsNavModules: navModulesArea.find('.modules-item i'),
                            btnItemsNavModules: navModulesArea.find('.modules-item a')
                        }

                    return {
                        navArea: navArea,
                        navUserArea: navUserArea,
                        navModulesArea: navModulesArea,
                        navUser: navUser,
                        navModules: navModules
                    }
                },
            },
            validation: {
                addMethodEmail: function () {
                    $.validator.methods.email = function (value, element) {
                        return this.optional(element) || /[a-z]+@[a-z]+\.[a-z]+/.test(value);
                    }
                },
                getHighlight: function (element) {
                    var id_attr = "#valid" + $(element).attr("id");
                    $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
                    $(id_attr).removeClass('glyphicon-ok').addClass('glyphicon-remove');
                },
                getUnhighlight: function (element) {
                    var id_attr = "#valid" + $(element).attr("id");
                    $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
                    $(id_attr).removeClass('glyphicon-remove').addClass('glyphicon-ok');
                }
            }
        };

    function init(options) {
        config.settings = $.extend(config.settings, options);
        SELF = config;
        bindUIActions();
    }

    function bindUIActions() {

        var nav = SELF.elems.getNav();

        var bindGlobal = function () {
            $(document)
                .tooltip({ container: 'body', selector: '[data-toggle="tooltip"]' })
                .on('webkitfullscreenchange mozfullscreenchange fullscreenchange', function () {
                    var state = document.fullScreen || document.mozFullScreen || document.webkitIsFullScreen;
                    var event = state ? 'FullscreenOn' : 'FullscreenOff';
                    if (event === 'FullscreenOff')
                        nav.navUserArea.find('.icon-full-screen').removeClass('active');
                });

            SELF.elems.contentScroll.asScrollable({
                namespace: 'asScrollable',
                contentSelector: null,
                containerSelector: null,
                draggingClass: 'is-dragging',
                hoveringClass: 'is-hovering',
                scrollingClass: 'is-scrolling',
                direction: 'vertical', // vertical, horizontal, both, auto
                showOnHover: true,
                showOnBarHover: false,
                duration: 500,
                easing: 'ease-in', // linear, ease, ease-in, ease-out, ease-in-out
                responsive: true,
                throttle: 20,
                scrollbar: {}
            });
        }

        var bindNav = function () {

            /* Nav User */
            nav.navUser.btnFullScreenNavUser.click(function (event) {
                event.preventDefault();

                if (!document.fullscreenElement && !document.mozFullScreenElement && !document.webkitFullscreenElement) {
                    $(this).parent('li').addClass('active');
                    if (document.documentElement.requestFullscreen) {
                        document.documentElement.requestFullscreen();
                    } else if (document.documentElement.mozRequestFullScreen) {
                        document.documentElement.mozRequestFullScreen();
                    } else if (document.documentElement.webkitRequestFullscreen) {
                        document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
                    }else if (document.documentElement.msRequestFullscreen) {
                                document.documentElement.msRequestFullscreen();
                            }
                } else {
                    if (document.cancelFullScreen) {
                        document.cancelFullScreen();
                    } else if (document.mozCancelFullScreen) {
                        document.mozCancelFullScreen();
                    } else if (document.webkitCancelFullScreen) {
                        document.webkitCancelFullScreen();
                    } else if (document.exitFullscreen) {
                        document.exitFullscreen();
                    } else if (document.mozCancelFullScreen) {
                         document.mozCancelFullScreen();
                    } else if (document.webkitExitFullscreen) {
                        document.webkitExitFullscreen();
                    } else if (document.msExitFullscreen) {
                        document.msExitFullscreen();
                    }
                }
            });

            nav.navUser.btnShowMenuNavUser.click(function (event) {
                event.preventDefault();

                nav.navArea.toggleClass('nav-area-open').toggleClass('nav-area-close');
                SELF.elems.bodyArea.toggleClass('body-area-open').toggleClass('body-area-close');

                $(this).find('i').toggleClass('fa-chevron-left').toggleClass('fa-chevron-right')

                if (nav.navModules.titleNavModules.hasClass('title-visible')) {
                    nav.navModules.titleNavModules.removeClass('title-visible');
                    nav.navModules.iconsNavModules.css('visibility', 'hidden');
                }
                else {
                    nav.navModules.titleNavModules.addClass('title-visible');
                    nav.navModules.titleNavModules.collapse('show');
                    nav.navModules.iconsNavModules.css('visibility', 'visible');
                }
            });

            nav.navUser.btnConfigurationNavUser.click(function (event) {
                event.preventDefault();

                nav.navModulesArea.find('li').removeClass('active');
                $(this).parent('li').addClass('active');

                var url = $(this).data('url');

                abp.ui.setBusy(null, SELF.settings.callAjaxServer(url, "", "", "html")
                  .done(function (data) {
                      SELF.elems.bodyArea.html(data);
                  }
                ));
            });

            nav.navUser.btnLogoutNavUser.click(function (event) {
                event.preventDefault();

                var url = $(this).data('url');
                window.location = url;
            });
            /* END */

            /* Nav Modules */
            nav.navModules.titleNavModules.click(function (event) {
                event.preventDefault();

                $(this).find('span').toggleClass('glyphicon-minus').toggleClass('glyphicon-plus');
            });

            nav.navModules.btnItemsNavModules.click(function (event) {
                event.preventDefault();

                nav.navModulesArea.find('li').removeClass('active');
                nav.navUser.btnConfigurationNavUser.parent().removeClass('active');
                $(this).parent('li').addClass('active');

                var url = $(this).data('url');

                abp.ui.setBusy(null, SELF.settings.callAjaxServer(url, "", "", "html")
                  .done(function (data) {
                      SELF.elems.bodyArea.html(data);
                  }
                ));
            });
            /* END */
        }

        var bindTabs = function () {
            SELF.elems.bodyArea.on('click', '#tab-header ul li a', function (event) {
                event.preventDefault();

                var url = $(this).data('url');

                abp.ui.setBusy(null, SELF.settings.callAjaxServer(url, "", "", "html")
                  .done(function (data) {
                      $('#tab-content .tab-pane').html(data);
                  }
                ));
            });
        }

        bindNav();
        bindTabs();
        bindGlobal();
    }

    return {
        init: init,
        config: config,
        AREA: AREA
    };

})(jQuery, window, document, abp);

SITE.init();