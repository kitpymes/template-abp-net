var MODULE = (function ($, SITE, window, document, abp, undefined) {
    "use strict";

    if (!$) return;

    var SELF,
        config = {
            settings: SITE.config.settings,
            elems: $.extend(SITE.config.elems, {
                getLogin: function () {
                    var obj = {};

                    obj.area = $("#LoginArea");
                    obj.form = obj.area.find("form");
                    obj.form.url = abp.appPath + "Account/Login";
                    obj.form.tenancyName = obj.form.find("#TenancyName"),
                    obj.form.emailAddress = obj.form.find("#EmailAddressInput");
                    obj.form.password = obj.form.find("#PasswordInput");
                    obj.form.rememberMe = obj.form.find("#RememberMeInput");
                    obj.form.forgotPasswordLink = obj.form.find("#ForgotPasswordLink");
                    obj.form.returnUrlHash = obj.form.find("#ReturnUrlHash");
                    obj.form.btn = obj.form.find("[type='submit']");

                    return obj;
                },
                getForgotPassword: function () {
                    var obj = {};

                    obj.area = $("#LoginForgotPasswordModalArea");
                    obj.form = obj.area.find("form");
                    obj.form.url = abp.appPath + "Account/ForgotPasswordLink";
                    obj.form.emailAddress = obj.form.find('#ForgotPasswordEmailAddress');
                    obj.form.btn = obj.form.find("[type='submit']");
                    obj.forgotPasswordLink = this.getLogin().form.forgotPasswordLink;

                    return obj;
                },
                getBGEffect: function () {
                    var obj = {};

                    obj.area = $("#LoginBGEffectArea");
                    obj.selector = obj.area.find('.segmenter');
                    obj.selector.headline = obj.area.find('.trigger-headline');
                    obj.imageRandom = true;
                    obj.imageDefault = config.settings.background.selectorDefault;
                    obj.effectType = config.settings.background.effectType.SEGMENTER;

                    return obj;
                }
            }),
            validation: $.extend(SITE.config.validation, {
                LOGIN : {
                    rule: SITE.AREA.ACCOUNT.LOGIN.validation.getRules()
                },
                FORGOTPASSWORD : {
                    rule: SITE.AREA.ACCOUNT.FORGOTPASSWORD.validation.getRules()
                }
            })
        };

    function init(options) {
        config.settings = $.extend(config.settings, options);
        SELF = config;
        bindUIActions();
    }

    function bindUIActions() {
        var bindGlobal = function () {
            var effect = SELF.elems.getBGEffect();

            //if (navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {
            //    SELF.settings.background.setBGImage($("body"), effect.imageDefault, effect.imageRandom);
            //}
            //else {
                SELF.settings.background.setBGImage(effect.selector, effect.imageDefault, effect.imageRandom);
                SELF.settings.background.setBGEffect(effect.selector, effect.effectType);
                effect.area.show();
            //}
        };

        var bindLogin = function () {
            var login = SELF.elems.getLogin();

            login.form.returnUrlHash.val(window.location.hash);
            
            SELF.settings.setFocusElem(login.form.find("input:not([type=hidden]):first"));
            SELF.settings.setDisabledElem(login.form.btn, true);

            login.form.validate({
                rules: {
                    EmailAddress: {
                        required: true,
                        email: SELF.validation.addMethodEmail(),
                        maxlength: SELF.validation.LOGIN.rule.EmailAddress_MaxLength
                    },
                    Password: {
                        required: true,
                        minlength: SELF.validation.LOGIN.rule.Password_MinLength,
                        maxlength: SELF.validation.LOGIN.rule.Password_MaxLength
                    },
                },
                messages: {
                    EmailAddress: {
                        required: abp.localization.abpWeb('COMMON.MSG.VALIDATION.Mandatory'),
                        maxlength: abp.localization.abpWeb('COMMON.MSG.VALIDATION.MaxLength', SELF.validation.LOGIN.rule.EmailAddress_MaxLength),
                        email: abp.localization.abpWeb('COMMON.MSG.VALIDATION.InvalidFormat')
                    },
                    Password: {
                        required: abp.localization.abpWeb('COMMON.MSG.VALIDATION.Mandatory'),
                        minlength: abp.localization.abpWeb('COMMON.MSG.VALIDATION.MinLength', SELF.validation.LOGIN.rule.Password_MinLength),
                        maxlength: abp.localization.abpWeb('COMMON.MSG.VALIDATION.MaxLength', SELF.validation.LOGIN.rule.Password_MaxLength)
                    }
                },
                onkeyup: function (element) { $(element).valid() },
                onfocusout: false,
                onclick: false,
                highlight: function (element) {
                    SELF.validation.getHighlight(element);
                    SELF.settings.setDisabledElem(login.form.btn, true);
                },
                unhighlight: function (element) {
                    SELF.validation.getUnhighlight(element);

                    if (!login.form.find('div').hasClass('has-error'))
                        SELF.settings.setDisabledElem(login.form.btn, false);
                },
                errorElement: 'span',
                errorClass: 'help-block',
                errorPlacement: function (error, element) {
                    if (element.length) {
                        error.insertAfter(element);
                    } else {
                        error.insertAfter(element);
                    }
                }
            });

            login.form.btn.click(function (e) {
                e.preventDefault();

                if (!login.form.valid()) return;

                var model = {
                    tenancyName: $.trim(login.form.tenancyName.val()),
                    emailAddress: $.trim(login.form.emailAddress.val()),
                    password: $.trim(login.form.password.val()),
                    rememberMe: login.form.rememberMe.is(':checked'),
                    returnUrlHash: $.trim(login.form.returnUrlHash.val())
                }

                abp.ui.setBusy(null, SELF.settings.callAjaxServer(login.form.url, model));
            });
        }

        var bindForgotPassword = function () {
            var forgot = SELF.elems.getForgotPassword();

            SELF.settings.setDisabledElem(forgot.form.btn, true);

            forgot.forgotPasswordLink.click(function () {
                forgot.area.modal('show');
                SELF.settings.setFocusElem(forgot.form.find("input:not([type=hidden]):first"));
            });

            forgot.area.on('hidden.bs.modal', function () {
                SELF.settings.setFocusElem(SELF.elems.getLogin().form.find("input:not([type=hidden]):first"));
            })

            forgot.form.validate({
                rules: {
                    ForgotPasswordEmailAddress: {
                        required: true,
                        maxlength: SELF.validation.FORGOTPASSWORD.rule.EmailAddress_MaxLength,
                        email: SELF.validation.addMethodEmail()
                    }
                },
                messages: {
                    ForgotPasswordEmailAddress: {
                        required: abp.localization.abpWeb('COMMON.MSG.VALIDATION.Mandatory'),
                        maxlength: abp.localization.abpWeb('COMMON.MSG.VALIDATION.MaxLength', SELF.validation.FORGOTPASSWORD.rule.EmailAddress_MaxLength),
                        email: abp.localization.abpWeb('COMMON.MSG.VALIDATION.InvalidFormat')
                    }
                },
                onkeyup: function (element) {
                    $(element).valid();
                },
                onfocusout: false,
                onclick: false,
                highlight: function (element) {
                    SELF.validation.getHighlight(element);
                    SELF.settings.setDisabledElem(forgot.form.btn, true);
                },
                unhighlight: function (element) {
                    SELF.validation.getUnhighlight(element);

                    if (!forgot.form.find('div').hasClass('has-error'))
                        SELF.settings.setDisabledElem(forgot.form.btn, false);
                },
                errorElement: 'span',
                errorClass: 'help-block',
                errorPlacement: function (error, element) {
                    if (element.length) {
                        error.insertAfter(element);
                    } else {
                        error.insertAfter(element);
                    }
                }
            });

            forgot.form.btn.click(function (e) {
                e.preventDefault();

                if (!forgot.form.valid()) return;

                var model = {
                    emailAddress: $.trim(forgot.form.emailAddress.val())
                }

                abp.ui.setBusy(null, SELF.settings.callAjaxServer(forgot.form.url, model)
                    .done(function (data) {
                        forgot.area.modal('hide');
                        abp.message.success(data.message);
                    }
                ));
            });
        }

        bindGlobal();
        bindLogin();
        bindForgotPassword();
    }

    return {
        init: init
    };

})(jQuery, SITE || {}, window, document, abp);

MODULE.init();