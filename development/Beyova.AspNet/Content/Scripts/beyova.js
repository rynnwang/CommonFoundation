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
        method: (method || "GET"),
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(jsonObj || {}),
        complete: function (xhr) {
            var status = xhr.status;
            var text = xhr.responseText;
            var response = text ? JSON.parse(text) : "";

            if (status && status.toString()[0] === "2") {
                whenOK(response);
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
        contentType: "text/html",
        dataType: "json",
        data: JSON.stringify(jsonObj || undefined),
        complete: function (xhr) {
            var status = xhr.status;
            var text = xhr.responseText;

            if (status && status.toString()[0] == "2") {
                if (renderContainerId) {
                    $("#" + renderContainerId).children().remove();
                    $("#" + renderContainerId).html(text);

                    if (completedFunc && typeof (completedFunc) === "function") {
                        completedFunc();
                    }
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

function getFormJson(domObject, limitToClass) {
    if (domObject) {
        var items = domObject.find(limitToClass ? ("input[name]." + limitToClass + ",select[name]." + limitToClass + ",textarea[name]." + limitToClass) : "input[name],select[name],textarea[name]");
        var result = {};
        items.each(function (i, c) {
            var value = (($(c).prop("type") !== "checkbox") || $(c).is(":checked")) ? $(c).val() : undefined;
            var namePath = $(c).attr("name");
            if (namePath.indexOf(".") > -1) {
                paths = namePath.split(".'");
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

function timeDown(btnObject, seconds, availableText, waitingText) {
    if (btnObject) {
        var time = seconds;
        var timeStop = setInterval(function () {
            time--;
            if (time > 0) {
                btnObject.text(waitingText + "(" + time + ")");
                btnObject.prop("disabled", true);
            } else {
                timeo = seconds;
                btnObject.text(availableText || "");
                clearInterval(timeStop);
                btnObject.prop("disabled", false);
            }
        }, 1000);
    }
}