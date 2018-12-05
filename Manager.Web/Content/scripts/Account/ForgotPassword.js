var MODULE = (function ($, SITE, window, document, abp, undefined) {
    "use strict";

    if (!$) return;

    var SELF,
        config = {
            settings: SITE.config.settings,
            elems: $.extend(SITE.config.elems, {
                getForgotPassword: function () {
                    var obj = {};

                    obj.area = $("#ForgotPasswordArea");
                    obj.form = obj.area.find("form");
                    obj.form.url = abp.appPath + "Account/ForgotPassword";
                    obj.form.userId = obj.form.find("#ForgotPasswordUserIdInput");
                    obj.form.password = obj.form.find("#ForgotPasswordInput");
                    obj.form.passwordRepeat = obj.form.find("#ForgotPasswordRepeatInput");
                    obj.form.passwordResetCode = obj.form.find("#ForgotPasswordResetCodeInput");
                    obj.form.btn = obj.form.find("[type='submit']");

                    return obj;
                },
                getBGEffect: function () {
                    var obj = {};

                    obj.area = $("#ForgotPasswordBGEffectArea");
                    obj.selector = obj.area.find("#particles-js");
                    obj.imageRandom = true;
                    obj.imageDefault = config.settings.background.selectorDefault;
                    obj.effectType = config.settings.background.effectType.PARTICLES;

                    return obj;
                }
            }),
            validation: $.extend(SITE.config.validation, {
                RESETPASSWORD: {
                    rule: SITE.AREA.ACCOUNT.RESETPASSWORD.validation.getRules()
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

            SELF.settings.background.setBGImage(effect.selector, effect.imageDefault, effect.imageRandom);
            SELF.settings.background.setBGEffect(effect.selector, effect.effectType);
        };

        var bindForgotPassword = function () {
            var reset = SELF.elems.getForgotPassword();

            SELF.settings.setFocusElem(reset.form.find('input:not([type=hidden]):first'));
            SELF.settings.setDisabledElem(reset.form.btn, true);

            reset.form.validate({
                rules: {
                    ForgotPassword: {
                        required: true,
                        minlength: SELF.validation.RESETPASSWORD.rule.Password_MinLength,
                        maxlength: SELF.validation.RESETPASSWORD.rule.Password_MaxLength
                    },
                    ForgotPasswordRepeat: {
                        required: true,
                        minlength: SELF.validation.RESETPASSWORD.rule.Password_MinLength,
                        maxlength: SELF.validation.RESETPASSWORD.rule.Password_MaxLength,
                        equalTo: "#ForgotPasswordInput"
                    }
                },
                messages: {
                    ForgotPassword: {
                        required: abp.localization.abpWeb('COMMON.MSG.VALIDATION.Mandatory'),
                        minlength: abp.localization.abpWeb('COMMON.MSG.VALIDATION.MinLength', SELF.validation.RESETPASSWORD.rule.Password_MinLength),
                        maxlength: abp.localization.abpWeb('COMMON.MSG.VALIDATION.MaxLength', SELF.validation.RESETPASSWORD.rule.Password_MaxLength)
                    },
                    ForgotPasswordRepeat: {
                        required: abp.localization.abpWeb('COMMON.MSG.VALIDATION.Mandatory'),
                        minlength: abp.localization.abpWeb('COMMON.MSG.VALIDATION.MinLength', SELF.validation.RESETPASSWORD.rule.Password_MinLength),
                        maxlength: abp.localization.abpWeb('COMMON.MSG.VALIDATION.MaxLength', SELF.validation.RESETPASSWORD.rule.Password_MaxLength),
                        equalTo: abp.localization.abpWeb('COMMON.MSG.VALIDATION.NotMatch')
                    }
                },
                onkeyup: function (element) {
                    $(element).valid();
                },
                onfocusout: function () {
                    return reset.form.password.val() === reset.form.passwordRepeat.val();
                },
                onclick: false,
                highlight: function (element) {
                    SELF.validation.getHighlight(element);
                    SELF.settings.setDisabledElem(reset.form.btn, true);
                },
                unhighlight: function (element) {
                    SELF.validation.getUnhighlight(element);

                    if (!reset.form.find('div').hasClass('has-error') && reset.form.password.val() === reset.form.passwordRepeat.val())
                        SELF.settings.setDisabledElem(reset.form.btn, false);
                },
                errorElement: 'span',
                errorClass: 'help-block',
                errorPlacement: function (error, element) {
                    error.insertAfter(element);
                }
            });

            reset.form.btn.click(function (e) {
                e.preventDefault();

                if (!reset.form.valid()) return;

                var model = {
                    UserId: $.trim(reset.form.userId.val()),
                    Password: $.trim(reset.form.password.val()),
                    PasswordRepeat: $.trim(reset.form.passwordRepeat.val()),
                    PasswordResetCode: $.trim(reset.form.passwordResetCode.val())
                }

                abp.ui.setBusy(null, SELF.settings.callAjaxServer(reset.form.url, model)
                    .done(function (data) {
                        abp.notify.success(data.message);

                        window.setTimeout(function () {
                            abp.ajax({ url: data.targetUrl });
                        }, 2000);
                    }
                ));
            });
        }

        bindGlobal();
        bindForgotPassword();
    }

    return {
        init: init
    };

})(jQuery, SITE || {}, window, document, abp);

MODULE.init();