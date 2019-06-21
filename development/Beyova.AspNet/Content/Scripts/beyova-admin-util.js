// Usage:
// var x = new initAdminPanel("YourControllerName", "YourEntityName");
// or var x = new initAdminPanel("YourControllerName", "YourEntityName", options);
// x.init(x);
// Options:
// string based selector
//  - addBtnId
//  - detailAreaId
//  - dialogId
//  - panelId
//  - queryBtnId
// objects
//  - withParameters:{}
//  - restoreAutoComplete:[{
//          name: ""
//          keyId:
//          valueId:
//     }]
// function
//  - completedFunc(container){}
//  - entityPostAdjust(form, originalJson){}
//  - beforeQuery(){}
function initAdminPanel(controllerName, entityName, options) {
    if (controllerName && entityName) {
        if (typeof (options) !== "object") {
            options = {};
        }
        core = {
            self: this,
            controllerName: controllerName,
            entityName: entityName,
            addBtnId: options.addBtnId || "add-entity",
            detailAreaId: options.detailAreaId || "detail-area",
            dialogId: options.dialogId || "detail-dialog",
            panelId: options.panelId || "criteria",
            completedFunc: options.completedFunc || function (container) { },
            entityPostAdjust: options.entityPostAdjust || function (form, originalJson) { return originalJson; },
            queryBtnId: options.queryBtnId,
            beforeQuery: options.beforeQuery,
            withParameters: typeof (options.withParameters) === "object" ? options.withParameters : {},
            restoreAutoComplete: $.isArray(options.restoreAutoComplete) ? options.restoreAutoComplete : [],
            loadResult: function () {
                var json = $.extend({}, core.withParameters);
                $.extend(json, core.panelId ? getFormJson($("#" + core.panelId)) : {});

                htmlAjax("/" + this.controllerName + "/Query" + this.entityName + "", json, "result");
            },
            renderEntity: function (key) {
                //$("#" + this.detailAreaId + ' .datepicker').datepicker('destroy');
                $("#" + this.detailAreaId).html("Loading ...");
                var json = $.extend({}, core.withParameters);
                json["key"] = key || "";
                htmlAjax("/" + this.controllerName + "/" + this.entityName + "Detail/", json, this.detailAreaId,
                    function () {
                        var container = $("#" + core.detailAreaId);
                        core.completedFunc(container);

                        if (core.restoreAutoComplete.length) {
                            for (var i = 0; i < core.restoreAutoComplete.length; i++) {
                                var restoreOption = core.restoreAutoComplete[i] || {};
                                var restoreName = restoreOption["name"];
                                var restoreKeyId = restoreOption["keyId"];
                                var restoreValueId = restoreOption["valueId"];

                                if (restoreName && restoreKeyId && restoreValueId) {
                                    var textDom = (container).find("input[name='" + restoreName + "']");
                                    textDom.val($("#" + restoreKeyId).val() || "");
                                    textDom.parent().children("input[name='" + restoreName + "_text']").val($("#" + restoreValueId).val() || "");
                                }
                            }
                        }
                    },
                    function () {
                    });

                $('#' + core.dialogId).modal("show");
            },
            createOrUpdateEntity: function () {
                var form = undefined;

                if (typeof (jQuery.validator) !== "undefined") {
                    form = $("#" + this.dialogId + " form");
                    if (!form.validate().form()) {
                        return;
                    }
                } else {
                    form = $("#" + this.dialogId);
                }

                var json = $.extend({}, core.withParameters);
                $.extend(json, getFormJson(form));
                json = this.entityPostAdjust(form, json);

                apiAjax("/" + controllerName + "/CreateOrUpdate" + entityName + "/", "POST", json, function (e) {
                    $('#' + self.dialogId).modal("hide");
                    self.loadResult();
                });
            },
            init: function (instance) {
                if (instance) {
                    self = instance;
                }

                $("#" + self.detailAreaId).delegate("#form-submit", "click", { self: self }, function (e) {
                    e.data.self.createOrUpdateEntity();
                });

                $("#" + self.detailAreaId).delegate("#form-discard", "click", { self: self }, function (e) {
                    $('#' + e.data.self.dialogId).modal("hide");
                });

                $("#" + self.detailAreaId).delegate("#form-delete", "click", { self: self }, function (e) {
                    var key = $(this).attr("key");
                    if (key) {
                        if (confirm("Confirm to continue?")) {
                            apiAjax("/" + controllerName + "/Delete" + entityName + "/", "POST", { key: key }, function (e) {
                                $('#' + self.dialogId).modal("hide");
                                self.loadResult();
                            });
                        }
                    }

                    $('#' + e.data.self.dialogId).modal("hide");
                });

                $("#" + self.addBtnId).bind("click", { self: self }, function (e) {
                    e.data.self.renderEntity();
                });

                if (typeof (self.queryBtnId) === "string") {
                    $("#" + self.queryBtnId).bind("click", { self: self, beforeQuery: core.beforeQuery }, function (e) {
                        if (typeof (e.data.beforeQuery) === "function") {
                            e.data.beforeQuery();
                        }
                        e.data.self.loadResult();
                    });
                }

                $('#' + self.dialogId).modal({ show: false });

                $("#result").delegate(".detail-clickable", "click", { self: self }, function (e) {
                    var key = $(this).attr("key");
                    if (key) {
                        e.data.self.renderEntity(key);
                    }
                });

                self.loadResult();
            }
        };

        return core;
    }
}

// obsoleted
function setAutoCompleteDefaultValue(name, key, text, container) {
    if (name && key && text) {
        var textDom = (container || $("body")).find("input[name='" + name + "']");
        textDom.val(key || "");
        textDom.parent().children("input[name='" + name + "_text']").val(text || "");
    }
}

function ensureUrlEndWithSlash(url) {
    if (typeof (url) === "string") {
        if (url.length > 0 && url[url.length - 1] != '/') {
            return url + "/";
        }
    }
    return url;
}