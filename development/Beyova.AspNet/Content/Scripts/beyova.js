function showMessage(title, message, whenClickOk) {
    alert(message);
}

function showErrorMessage(message, whenClickOk) {
    showMessage("Error", message, whenClickOk);
}

// url:string
// method: string
// jsonObj: {} or string
// whenOK: function(bodyText){}
// whenError: bool function(status, exJson){}, prevent further handling if return true.
function apiAjax(url, method, jsonObj, whenOK, whenError) {
    $.ajax({
        url: url,
        method: method || "GET",
        contentType: "application/json",
        dataType: "json",
        cache: false,
        data: JSON.stringify(jsonObj || {}),
        beforeSend: function (request) {
            var token = localStorage.getItem("token");
            if (typeof (token) === "string" && token.length > 0) {
                request.setRequestHeader("X-BA-TOKEN", token);
            }
        },
        complete: function (xhr) {
            var status = xhr.status;
            var text = xhr.responseText;
            var response = text ? JSON.parse(text) : "";

            if (status && status.toString()[0] === "2") {
                if (typeof (whenOK) === "function") {
                    whenOK(response);
                }
            } else {
                if (!(whenError && whenError(status, response))) {
                    switch (status) {
                        case 401:
                            window.location.href = "/account/cellphoneLogin?returnUrl=" + encodeURI(window.location);
                            break;
                        default:
                            showErrorMessage(response["message"]);
                            break;
                    }
                }
            }
        }
    });
}

// url:string
// method: POST
// jsonObj: {} or string
// renderContainerId: DOM id for hosting result html.
function htmlAjax(url, jsonObj, renderContainerId, completedFunc, cleanFunc, method) {
    if (cleanFunc) {
        cleanFunc();
    }
    $.ajax({
        url: url,
        method: method || "POST",
        contentType: "application/json",
        dataType: "html",
        cache: false,
        beforeSend: function (request) {
            var token = localStorage.getItem("token");
            if (typeof (token) === "string" && token.length > 0) {
                request.setRequestHeader("X-BA-TOKEN", token);
            }
        },
        data: JSON.stringify(jsonObj || undefined),
        complete: function (xhr) {
            var status = xhr.status;
            var text = xhr.responseText;

            if (status && status.toString()[0] === "2") {
                if (renderContainerId) {
                    $("#" + renderContainerId).children().remove();
                    $("#" + renderContainerId).html(text);

                    if (completedFunc && typeof (completedFunc) === "function") {
                        completedFunc();
                    }
                }
            }
        }
    });
}

function collapseNavBar(container) {
    var items = container.find("a[urlprefix]");
    var url = window.location.pathname.toLowerCase();
    if (url.indexOf(".", url.lastIndexOf("/")) < 0 && url[url.length - 1] !== "/") {
        url += "/";
    }
    items.each(function (i, e) {
        var urlprefix = $(e).attr("urlprefix").toLowerCase();
        if (urlprefix && url.indexOf(urlprefix) === 0) {
            // this might be different based on templates.
            $(e).closest("ul").prev("a").click();

            //break each loop.
            return false;
        }
    });
}

function getFormJson(domObject, classConstraint) {
    if (domObject) {
        var items = domObject.find(typeof (classConstraint) === "string" ? ("input[name]." + classConstraint + ",select[name]." + classConstraint + ",textarea[name]." + classConstraint) : "input[name],select[name],textarea[name]");
        var result = {};
        items.each(function (i, c) {
            var value = ($(c).prop("type") !== "checkbox" || $(c).is(":checked")) ? $(c).val() : undefined;
            var dataType = $(c).attr("data-type");
            if (dataType !== undefined) {
                switch (dataType) {
                    case "enum":
                    case "int":
                        value = parseInt(value);
                        break;
                    case "float":
                        value = parseFloat(value);
                        break;
                    case "bool":
                    case "boolean":
                        if (value) {
                            if (value === "true" || value === "True") {
                                value = true;
                            } else if (value === "false" || value === "False") {
                                value = false;
                            }
                        } else {
                            value = undefined;
                        }
                        break;
                    default:
                        break;
                }
            }
            var namePath = $(c).attr("name");
            if (namePath.indexOf(".") > -1) {
                paths = namePath.split(".");
                var tmp = result;
                for (var pathIndex = 0; pathIndex < paths.length - 1; pathIndex++) {
                    if (tmp[paths[pathIndex]] === undefined) {
                        tmp[paths[pathIndex]] = {};
                    }
                    tmp = tmp[paths[pathIndex]];
                }
                tmp[paths[paths.length - 1]] = value;
            } else {
                result[$(c).attr("name")] = value;
            }
        });

        return result;
    }
}

function setFormJson(json, domObject, classConstraint) {
    if (typeof (json) === "object" && domObject) {
        var items = domObject.find(typeof (classConstraint) === "string" ? ("input[name]." + classConstraint + ",select[name]." + classConstraint + ",textarea[name]." + classConstraint) : "input[name],select[name],textarea[name]");
        for (var key in json) {
            var propertyValue = json[key];
            if (typeof (propertyValue) !== "undefined") {
                var control = items.filter("[name='" + key + "']");
                if (control.prop("type") === "checkbox") {
                    control.attr("checked", propertyValue === control.attr("value") || propertyValue === true || propertyValue === "true" || propertyValue === "True");
                } else {
                    control.val(propertyValue);
                }
            }
        }
    }
}

function getArray(domObject, rowSelector, rowValueValidator) {
    if (domObject) {
        var items = rowSelector ? domObject.find(rowSelector) : domObject.children();
        var result = [];

        if (items.length) {
            items.each(function (i, c) {
                var obj = getFormJson($(c));
                if (!isEmptyObject(obj) && (!rowValueValidator || rowValueValidator(obj))) {
                    result.push(obj);
                }
            });
        }

        return result;
    }
}

function isGuid(value) {
    return value && (/^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i).test(value);
}

function isEmptyObject(obj) {
    return obj && Object.keys(obj).length === 0 && obj.constructor === Object;
}

// Options:
//  -seconds
//  -availableText
//  -secondUnitText
//  -toFocus
function timeDown(btnObject, options) {
    if (btnObject && typeof (options) === "object") {
        var time = options.seconds || 60;
        var availableText = options.availableText || "Get Code";
        var secondUnitText = options.secondUnitText || "s";
        var toFocus = options.toFocus;

        var timeStop = setInterval(function () {
            time--;
            if (time > 0) {
                btnObject.text(time + secondUnitText);
                btnObject.prop("disabled", true);
            } else {
                time = options.seconds || 60;
                btnObject.text(availableText || "");
                clearInterval(timeStop);
                btnObject.prop("disabled", false);
            }
        }, 1000);

        if (toFocus && toFocus instanceof jQuery) {
            toFocus.focus();
        }
    }
}
// options
// - url
// - updateLink:function(identifier){}
// response: {
//      identifier:"",
//      name: ""
// }
function blobUpload(fileBtnObject, options) {
    if (fileBtnObject) {
        var core = {
            origin: fileBtnObject,
            url: (options || {})['url'],
            form: undefined,
            fileInput: undefined,
            name: fileBtnObject.prop("name"),
            init: function () {
                var form = $("<form method='post' action='" + (this.url || "") + "' enctype='multipart/form-data'><div class='function'><input type='file' name='ajaxFile' style='width:100%;display:block;' /><input type='hidden' /></div><div class='progress' style='display:none'><div class='progress-bar progress-bar-striped progress-bar-animated' role='progressbar' aria-valuemax='100'></div></div></form>");
                fileBtnObject.replaceWith(form);
                core.form = form;
                core.functionArea = form.find("div.function");
                core.progressArea = form.find("div.progress");
                core.fileInput = form.find("input:file");
                core.vaueInput = form.find("input:hidden");
                core.progressBar = form.find("div.progress div.progress-bar");

                core.vaueInput.prop("name", core.origin.prop("name"));
                core.vaueInput.prop("class", core.origin.attr("class"));

                form.find("input:file").bind("change", { core: core }, function (e) {
                    var localCore = e.data.core;
                    var self = $(this);

                    if (self.val()) {
                        localCore.form.ajaxSubmit({
                            dataType: "json",
                            beforeSend: function () {
                                localCore.functionArea.hide();
                                localCore.progressArea.show();
                            },
                            success: function (data) {
                                localCore.progressArea.hide();
                                if (data) {
                                    core.vaueInput.val(data.identifier);
                                }
                                localCore.functionArea.show();
                            },
                            uploadProgress: function (event, position, total, percentageComplete) {
                                localCore.progressBar.width(percentageComplete + '%');
                            }
                        });
                    }
                });
            }
        };

        core.init();
    }
}

function considerEmptyArrayAsNull(arrayObject) {
    return ($.isArray(arrayObject) && (arrayObject.length === 0 || (arrayObject.length === 1 && arrayObject[0] === ""))) ? undefined : arrayObject;
}

function newid(length) {
    if (typeof (length) !== "number" || length < 1) {
        length = 16;
    }

    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;
    for (var i = 0; i < length; i++) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
}