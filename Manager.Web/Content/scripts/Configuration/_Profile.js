var MODULE = (function ($, SITE, window, document, abp, undefined) {

    if (!$) return;

    var SELF,
        config = {
            settings: SITE.config.settings,
            elems: $.extend(SITE.config.elems, {
                getProfile: function () {
                    var obj = {};

                    obj.area = $("#ProfileArea");
                    obj.form = obj.area.find("form");
                    obj.form.url = abp.appPath + "Configuration/UpdateProfile";
                    obj.form.passwordInput = obj.form.find("#PasswordInput");
                    obj.form.passwordRepeatInput = obj.form.find("#PasswordRepeatInput");
                    obj.form.passwordIcon = obj.form.find(".icon-password");
                    obj.form.passwordRepeatIcon = obj.form.find(".icon-password-repeat");
                    obj.form.name = obj.form.find('#NameInput');
                    obj.form.surname = obj.form.find('#SurnameInput');
                    obj.form.emailAddress = obj.form.find('#EmailAddressInput');
                    obj.form.password = obj.form.find('#PasswordInput');
                    obj.form.passwordRepeat = obj.form.find('#PasswordRepeatInput');
                    obj.form.btn = obj.form.find("[type='submit']");

                    return obj;
                }
            }),
            validation: $.extend(SITE.config.validation, {
                PROFILE: {
                    rule: SITE.AREA.CONFIGURATION.PROFILE.validation.getRules()
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
            SELF.settings.setFloatLabel();
        }

        var bindProfile = function () {
            var profile = SELF.elems.getProfile()

            SELF.settings.setFocusElem(profile.form.find('input:not([type=hidden]):first'));
            SELF.settings.setDisabledElem(profile.form.btn, true);

            profile.form.passwordIcon.mousedown(function () {
                profile.form.passwordInput.attr('type', 'text');
            }).mouseup(function () {
                profile.form.passwordInput.attr('type', 'password');
            }).mouseout(function () {
                profile.form.passwordInput.attr('type', 'password');
            });

            profile.form.passwordRepeatIcon.mousedown(function () {
                profile.form.passwordRepeatInput.attr('type', 'text');
            }).mouseup(function () {
                profile.form.passwordRepeatInput.attr('type', 'password');
            }).mouseout(function () {
                profile.form.passwordRepeatInput.attr('type', 'password');
            });

            profile.form.btn.click(function (event) {
                event.preventDefault();

                if (!profile.form.valid()) return;

                var model = {
                    Name: profile.form.name.parent().hasClass("populated") ? $.trim(profile.form.name.val()) : "",
                    Surname: profile.form.surname.parent().hasClass("populated") ? $.trim(profile.form.surname.val()) : "",
                    EmailAddress: profile.form.emailAddress.parent().hasClass("populated") ? $.trim(profile.form.emailAddress.val()) : "",
                    Password: $.trim(profile.form.password.val()),
                    PasswordRepeat:  $.trim(profile.form.passwordRepeat.val()) 
                }

                abp.ui.setBusy(null, SELF.settings.callAjaxServer(profile.form.url, model)
                    .done(function (data) {
                        abp.notify.success(data.message);

                        var nav = SELF.elems.getNav();
                        nav.navUserArea.find(".info").html("<p>" + $.trim(profile.form.name.val()) + "</p>");

                        if (profile.form.passwordInput.val().length > 0)
                            profile.form.passwordInput.val("");

                        if (profile.form.passwordRepeatInput.val().length > 0)
                            profile.form.passwordRepeatInput.val("");
                    }
                ));
            });

            profile.form.validate({
                rules: {
                    Name: {
                        required: true,
                        maxlength: SELF.validation.PROFILE.rule.Name_MaxLength
                    },
                    Surname: {
                        required: true,
                        maxlength: SELF.validation.PROFILE.rule.Surname_MaxLength
                    },
                    EmailAddress: {
                        required: true,
                        maxlength: SELF.validation.PROFILE.rule.EmailAddress_MaxLength,
                        email: SELF.validation.addMethodEmail()
                    },
                    Password: {
                        required: false,
                        minlength: SELF.validation.PROFILE.rule.Password_MinLength,
                        maxlength: SELF.validation.PROFILE.rule.Password_MaxLength
                    },
                    PasswordRepeat: {
                        required: false,
                        minlength: SELF.validation.PROFILE.rule.Password_MinLength,
                        maxlength: SELF.validation.PROFILE.rule.Password_MaxLength,
                        equalTo: "#PasswordInput"
                    }
                },
                messages: {
                    Name: {
                        required: abp.localization.abpWeb('COMMON.MSG.VALIDATION.Mandatory'),
                        maxlength: abp.localization.abpWeb('COMMON.MSG.VALIDATION.MaxLength', SELF.validation.PROFILE.rule.Name_MaxLength)
                    },
                    Surname: {
                        required: abp.localization.abpWeb('COMMON.MSG.VALIDATION.Mandatory'),
                        maxlength: abp.localization.abpWeb('COMMON.MSG.VALIDATION.MaxLength', SELF.validation.PROFILE.rule.Surname_MaxLength)
                    },
                    EmailAddress: {
                        required: abp.localization.abpWeb('COMMON.MSG.VALIDATION.Mandatory'),
                        maxlength: abp.localization.abpWeb('COMMON.MSG.VALIDATION.MaxLength', SELF.validation.PROFILE.rule.EmailAddress_MaxLength),
                        email: abp.localization.abpWeb('COMMON.MSG.VALIDATION.InvalidFormat')
                    },
                    Password: {
                        minlength: abp.localization.abpWeb('COMMON.MSG.VALIDATION.MinLength', SELF.validation.PROFILE.rule.Password_MinLength),
                        maxlength: abp.localization.abpWeb('COMMON.MSG.VALIDATION.MaxLength', SELF.validation.PROFILE.rule.Password_MaxLength)
                    },
                    PasswordRepeat: {
                        minlength: abp.localization.abpWeb('COMMON.MSG.VALIDATION.MinLength', SELF.validation.PROFILE.rule.Password_MinLength),
                        maxlength: abp.localization.abpWeb('COMMON.MSG.VALIDATION.MaxLength', SELF.validation.PROFILE.rule.Password_MaxLength),
                        equalTo: abp.localization.abpWeb('COMMON.MSG.VALIDATION.NotMatch')
                    }
                },
                onkeyup: function (element) {
                    $(element).valid();
                },
                onfocusout: function () {
                    return profile.form.passwordInput.val() === profile.form.passwordRepeatInput.val();
                },
                onclick: false,
                highlight: function (element) {
                    SELF.validation.getHighlight(element);
                    SELF.settings.setDisabledElem(profile.form.btn, true);
                },
                unhighlight: function (element) {
                    SELF.validation.getUnhighlight(element);

                    if (!profile.form.find('div').hasClass('has-error') && profile.form.passwordInput.val() === profile.form.passwordRepeatInput.val())
                        SELF.settings.setDisabledElem(profile.form.btn, false);
                },
                errorElement: 'span',
                errorClass: 'help-block',
                errorPlacement: function (error, element) {
                    if (element.length)
                        error.insertAfter(element);
                }
            });
        }

        bindGlobal();
        bindProfile();
    }

    return {
        init: init
    };

})(jQuery, SITE || {}, window, document, abp);

MODULE.init();