// Usage:
// var x = new initAdminPanel("YourControllerName", "YourEntityName");
// x.init(x);

function initAdminPanel(controllerName, entityName, addBtnId, detailAreaId, dialogId, panelId) {
    if (controllerName && entityName) {
        core = {
            self: this,
            controllerName: controllerName,
            entityName: entityName,
            addBtnId: addBtnId || "add-entity",
            detailAreaId: detailAreaId || "detail-area",
            dialogId: dialogId || "detail-dialog",
            loadResult: function () {
                htmlAjax("/" + this.controllerName + "/Query" + this.entityName + "", panelId ? getFormJson($("#" + panelId)) : {}, "result");
            },
            renderEntity: function (key) {
                $("#" + this.detailAreaId + ' .datepicker').datepicker('destroy');
                $("#" + this.detailAreaId).html("Loading ...");
                htmlAjax("/" + this.controllerName + "/" + this.entityName + "Detail/" + key, {}, this.detailAreaId,
                    function () {
                        $("#" + this.detailAreaId + ' .datepicker').datepicker({ format: 'yyyy-mm-dd' });
                    },
                    function () {
                    }
                );

                $('#' + this.dialogId).modal("show");
            },
            createOrUpdateEntity: function () {
                var json = getFormJson($("#" + this.dialogId));
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
                        if (confirm("Confirm to delete?")) {
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

