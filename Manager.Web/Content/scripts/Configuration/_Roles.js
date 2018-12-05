var MODULE = (function ($, SITE, window, document, abp, undefined) {

    if (!$) return;

    var SELF,
        config = {
            settings: $.extend(SITE.config.settings, {
                currentLanguageName: SITE.AREA.CONFIGURATION.ROLES.settings.currentLanguageName,
                isRolMaster: SITE.AREA.CONFIGURATION.ROLES.settings.isRolMaster,
                isRolAdmin: SITE.AREA.CONFIGURATION.ROLES.settings.isRolAdmin,
                setLanguageGrid: function () {
                  if (this.currentLanguageName !== "en")
                    jsGrid.locale(this.currentLanguageName);
                }
            }),
            elems: $.extend(SITE.config.elems, {
                getRoles: function () {
                    var obj = {};

                    obj.area = $("#RolesArea");
                    obj.grid = obj.area.find("#jsGrid");
                    obj.grid.url = {
                        GetAllRoles: abp.appPath + "Configuration/GetAllRoles",
                        CreateRole: abp.appPath + "Configuration/CreateRole",
                        UpdateRole: abp.appPath + "Configuration/UpdateRole",
                        DeleteRole: abp.appPath + "Configuration/DeleteRole"
                    },
                    obj.grid.columnsDisplayName = {
                        Name: abp.localization.abpWeb("CONFIG.ROLES.GRID.COLUMN.Name"),
                        DisplayName: abp.localization.abpWeb("CONFIG.ROLES.GRID.COLUMN.DisplayName"),
                        IsStatic: abp.localization.abpWeb("CONFIG.ROLES.GRID.COLUMN.IsStatic"),
                        IsDefault: abp.localization.abpWeb("CONFIG.ROLES.GRID.COLUMN.IsDefault"),
                        IsDeleted: abp.localization.abpWeb("CONFIG.ROLES.GRID.COLUMN.IsDeleted")
                    },
                    obj.grid.addRolePlaceholder = {
                        Name: abp.localization.abpWeb("CONFIG.ROLES.GRID.PLACEHOLDER.Name"),
                        DisplayName: abp.localization.abpWeb("CONFIG.ROLES.GRID.PLACEHOLDER.DisplayName")
                    },
                    obj.grid.labels = {
                        STATIC: {
                            name: abp.localization.abpWeb("CONFIG.ROLES.GRID.LABEL.IsStatic.Name"),
                            title: abp.localization.abpWeb("CONFIG.ROLES.GRID.LABEL.IsStatic.Title")
                        },
                        DELETE: {
                            name: abp.localization.abpWeb("CONFIG.ROLES.GRID.LABEL.IsDeleted.Name"),
                            title: abp.localization.abpWeb("CONFIG.ROLES.GRID.LABEL.IsDeleted.Title")
                        },
                        DEFAULT: {
                            name: abp.localization.abpWeb("CONFIG.ROLES.GRID.LABEL.IsDefault.Name"),
                            title: abp.localization.abpWeb("CONFIG.ROLES.GRID.LABEL.IsDefault.Title")
                        }
                    },
                    obj.grid.messages = {
                        isSureDeleteRow: {
                            title: abp.localization.abpWeb("CONFIG.ROLES.GRID.MESSAGES.IsSureDeleteRow.Title"),
                            content: function (item) {
                                return abp.localization.abpWeb("CONFIG.ROLES.GRID.MESSAGES.IsSureDeleteRow.Content", item.name);
                            }
                        },
                        roleIsStaticNotDelete: {
                            title: abp.localization.abpWeb("CONFIG.ROLES.GRID.MESSAGES.RoleIsStaticNotDelete.Title")
                        },
                        roleIsDeleting: {
                            title: abp.localization.abpWeb("CONFIG.ROLES.GRID.MESSAGES.RoleIsDeleting.Title")
                        },
                        roleIsDefaultNotDelete: {
                            title: abp.localization.abpWeb("CONFIG.ROLES.GRID.MESSAGES.RoleIsDefaultNotDelete.Title")
                        }
                    }

                    return obj;
                }
            }),
            validation: SITE.config.validation
        };

    function init(options) {
        config.settings = $.extend(config.settings, options);
        SELF = config;
        bindUIActions();
    }

    function bindUIActions() {

        var bindGrid = function () {
            var roles = SELF.elems.getRoles();

            SELF.settings.setLanguageGrid();

            roles.grid.jsGrid({
                filtering: true,
                editing: true,
                sorting: true,
                paging: true,
                pagerFormat: "{first} {prev} {pages} {next} {last} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; {pageIndex} / {pageCount}",
                pagePrevText: "<",
                pageNextText: ">",
                pageFirstText: "<<",
                pageLastText: ">>",
                inserting: true,
                autoload: true,
                pageSize: 15,
                pageButtonCount: 20,
                confirmDeleting: false,
                invalidNotify: function (args) {
                    var messages = $.map(args.errors, function (error) {
                        return error.field.title + ": " + error.message || null;
                    });

                    abp.message.error((messages).join("\n"), this.invalidMessage);
                },
                onItemDeleting: function (args) {
                    if (!args.item.deleteConfirmed) {
                        args.cancel = true;
                        abp.message.confirm(roles.grid.messages.isSureDeleteRow.content(args.item), roles.grid.messages.isSureDeleteRow.title,
                            function (isConfirmed) {
                                if (isConfirmed) {
                                    args.item.deleteConfirmed = true;
                                    roles.grid.jsGrid('deleteItem', args.item);
                                }
                            }
                        );
                    }
                },
                controller: {
                    loadData: function (filter) {
                        var d = $.Deferred();

                        $.ajax({
                            type: "GET",
                            url: roles.grid.url.GetAllRoles,
                            dataType: "json",
                            data: filter
                        }).done(function (data) {
                            d.resolve(data.result);
                        });

                        return d.promise();
                    },
                    insertItem: function (item) {
                        abp.ui.setBusy(null, SELF.settings.callAjaxServer(roles.grid.url.CreateRole, item)
                           .done(function (data) {
                               abp.notify.success(data.message);

                               if (item.isDefault) {
                                   abp.ui.setBusy(null,
                                       window.setTimeout(function () {
                                           roles.grid.jsGrid("loadData");
                                           abp.ui.clearBusy();
                                       }, 2000)
                                   );
                               }
                           })
                        );
                    },
                    updateItem: function (item) {
                        abp.ui.setBusy(null, SELF.settings.callAjaxServer(roles.grid.url.UpdateRole, item)
                            .done(function (data) {
                                abp.notify.success(data.message);

                                if (item.isDefault) {
                                    abp.ui.setBusy(null,
                                        window.setTimeout(function () {
                                            roles.grid.jsGrid("loadData");
                                            abp.ui.clearBusy();
                                        }, 2000)
                                    );
                                }
                            })
                        );
                    },
                    deleteItem: function (item) {
                        abp.ui.setBusy(null, SELF.settings.callAjaxServer(roles.grid.url.DeleteRole, item)
                             .done(function (data) {
                                 abp.notify.success(data.message);

                                 abp.ui.setBusy(null,
                                     window.setTimeout(function () {
                                         roles.grid.jsGrid("loadData");
                                         abp.ui.clearBusy();
                                     }, 2000)
                                );
                             })
                        );
                    }
                },
                fields: [
                    {
                        name: "id", type: "number", width: 50, align: "center", visible: false
                    },
                    {
                        name: "name",
                        title: roles.grid.columnsDisplayName.Name,
                        type: "text",
                        validate: "required",
                        width: "30%",
                        filterTemplate: function () {
                            if (!this.filtering)
                                return "";

                            var grid = this._grid,
                                $result = this.filterControl = this._createTextBox();

                            if (this.autosearch) {
                                $result.on("keyup", function (e) {
                                    grid.search();
                                    e.preventDefault();
                                });
                            }

                            return $result;
                        },
                        itemTemplate: function (value, item) {
                            var result = "";

                            if (item.isStatic || item.isDeleted || item.isDefault) {
                                result = item.name;

                                if (item.isStatic)
                                    result = result + "<div title='" + roles.grid.labels.STATIC.title
                                        + "' class='label label-info label-static' data-toggle='tooltip'>" + roles.grid.labels.STATIC.name + "</div>";

                                if (item.isDeleted)
                                    result = result + "<div title='" + roles.grid.labels.DELETE.title
                                        + "' class='label label-info label-delete' data-toggle='tooltip'>" + roles.grid.labels.DELETE.name + "</div>";

                                if (item.isDefault)
                                    result = result + "<div title='" + roles.grid.labels.DEFAULT.title
                                        + "' class='label label-info label-default' data-toggle='tooltip'>" + roles.grid.labels.DEFAULT.name + "</div>";

                                return result;
                            }

                            return value;
                        },
                        insertTemplate: function () {
                            var $result = jsGrid.fields.text.prototype.insertTemplate.apply(this, arguments);
                            $result.attr("placeholder", roles.grid.addRolePlaceholder.Name);

                            return $result;
                        },
                        editTemplate: function (value, item) {
                            var $result = this.editControl = this._createTextBox();
                            $result.val(value);

                            if (!SELF.settings.isRolMaster && item !== undefined && item.isStatic || item.isDeleted) {
                                $result.addClass("disabled");
                            }

                            return $result;
                        },
                    },
                    {
                        name: "displayName",
                        title: roles.grid.columnsDisplayName.DisplayName,
                        type: "text",
                        validate: "required",
                        width: "30%",
                        filterTemplate: function () {
                            if (!this.filtering)
                                return "";

                            var grid = this._grid,
                                $result = this.filterControl = this._createTextBox();

                            if (this.autosearch) {
                                $result.on("keyup", function (e) {
                                    grid.search();
                                    e.preventDefault();
                                });
                            }
                            return $result;
                        },
                        insertTemplate: function () {
                            var $result = jsGrid.fields.text.prototype.insertTemplate.apply(this, arguments);
                            $result.attr("placeholder", roles.grid.addRolePlaceholder.DisplayName);

                            return $result;
                        },
                        editTemplate: function (value, item) {
                            var $result = this.editControl = this._createTextBox();
                            $result.val(value);

                            
                            
                            return $result;
                        },
                    },
                    {
                        name: "isStatic",
                        title: roles.grid.columnsDisplayName.IsStatic,
                        type: "checkbox",
                        width: "10%",
                        align: "center",
                        autosearch: true,
                        visible: SELF.settings.isRolMaster,
                        editTemplate: function (value, item) {
                            if (!this.editing)
                                return this.itemTemplate(value);

                            var $result = this.editControl = this._createCheckbox();

                            $result.prop({
                                checked: value,
                                disabled: item.isDeleted
                            });

                            return $result;
                        }
                    },
                    {
                        name: "isDefault",
                        title: roles.grid.columnsDisplayName.IsDefault,
                        autosearch: true,
                        type: "checkbox",
                        width: "10%",
                        align: "center",
                        visible: SELF.settings.isRolMaster || SELF.settings.isRolAdmin,
                        editTemplate: function (value, item) {
                            if (!this.editing)
                                return this.itemTemplate(value);

                            var $result = this.editControl = this._createCheckbox();

                            $result.prop({
                                checked: value,
                                disabled: item.isDeleted
                            });

                            return $result;
                        }
                    },
                    {
                        name: "isDeleted",
                        title: roles.grid.columnsDisplayName.IsDeleted,
                        autosearch: true,
                        type: "checkbox",
                        width: "10%",
                        align: "center",
                        visible: SELF.settings.isRolMaster || SELF.settings.isRolAdmin,
                        editTemplate: function (value, item) {
                            if (!this.editing)
                                return this.itemTemplate(value);

                            var $result = this.editControl = this._createCheckbox();
                            $result.prop({
                                checked: value,
                                disabled: item.isDefault || item.isStatic
                            });

                            return $result;
                        }
                    },
                    {
                        type: "control",
                        width: "10%",
                        align: "center",
                        clearFilterButton: false,
                        searchButton: false,
                        visible: SELF.settings.isRolMaster || SELF.settings.isRolAdmin,
                        _createDeleteButton: function (item) {
                            var tooltipDeleteBtn = this.deleteButtonTooltip,
                                classDeleteBtn = this.deleteButtonClass;

                            if (item.isStatic) {
                                tooltipDeleteBtn = roles.grid.messages.roleIsStaticNotDelete.title;
                                classDeleteBtn = classDeleteBtn + " disabled"
                            }

                            if (item.isDeleted) {
                                tooltipDeleteBtn = roles.grid.messages.roleIsDeleting.title;
                                classDeleteBtn = classDeleteBtn + " disabled"
                            }

                            if (item.isDefault) {
                                tooltipDeleteBtn = roles.grid.messages.roleIsDefaultNotDelete.title;
                                classDeleteBtn = classDeleteBtn + " disabled"
                            }

                            return this._createGridButton(classDeleteBtn, tooltipDeleteBtn, function (grid, e) {
                                grid.deleteItem(item);
                                e.stopPropagation();
                            });
                        },
                        _createGridButton: function (cls, tooltip, clickHandler) {
                            var grid = this._grid;

                            return $("<input>")
                                .addClass(this.buttonClass)
                                .addClass(cls)
                                .attr({
                                    type: "button",
                                    title: tooltip,
                                    'data-toggle': "tooltip"
                                })
                                .on("click", function (e) {
                                    $(this).tooltip('hide');
                                    clickHandler(grid, e);
                                });
                        }
                    }
                ]
            });
        }

        var bindGlobal = function () {

        }

        bindGrid();
        bindGlobal();
    }

    return {
        init: init
    };

})(jQuery, SITE || {}, window, document, abp);

MODULE.init();