String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g,
        function (m, i) {
            return args[i];
        });
};

function getForm(form) {
    var result = {};
    $(form).find("*[name]").each(function (i, c) {
        var f = $(c);
        result[f.attr("name")] = f.val();
    });

    return result;
};

function setForm(form, obj) {
    if (form && obj) {
        $(form).find("*[name]").each(function (i, c) {
            var f = $(c);
            var options = f.children("option");
            var value = obj[f.attr("name")];
            if (options.length) {
                var option = options.filter("*[value='" + value + "']");
                if (!option.length) {
                    option = options.eq(0);
                }
                option.prop("selected", true);
            } else {
                f.val(obj[f.attr("name")]);
            }
        });
    }
};

function callAction(url, data, successFunc, failedFunc, completeFunc, headers) {
    $.ajax({
        async: true,
        dataType: "text",
        contentType: "application/json",
        type: "POST",
        headers: headers,
        url: url || "",
        data: typeof (data) == 'string' ? data : JSON.stringify(data),
        success: function (response) {
            if (successFunc) successFunc(response);
        },
        error: function (xhr, status, text) {
            var response = $.parseJSON(xhr.responseText);
            var title = "", body = "";
            var minorCode = response["Code"]["Minor"];
            var httpStatus = xhr.status;

            switch (httpStatus) {
                case 401:
                    title = "Unauthorized (401)";
                    if (url.match(/^\/account\//gi)) {
                        body = "Please check your account and try again.";
                    } else {
                        body = "Please confirm you have logined and you have proper permission granted.";
                    }
                    break;
                case 402:
                    title = "Credit cannot affort (402)";
                    body = "Please check your commercial/finance info.";
                    break;
                case 403:
                    title = "Action forbidden (403)";
                    body = "Please refresh page and retry. If issue still occurs, please confirm your permission.";
                    break;
                case 404:
                    title = "Not found (404)";
                    body = "The page or resource you are accessing is not existed or has already deleted.";
                    break;
                case 500:
                    title = "Server error (500)";
                    body = response["Message"];
                    break;
            }

            showDialog(title, body);

            if (failedFunc) {
                failedFunc(response);
            }
        },
        complete: function () {
            if (completeFunc) {
                completeFunc();
            }
        }
    });
};
///BaseUrl: http://xxx.com/api/v1/ or /api/v1/
function callApi(baseUrl, resource, action, method, queryString, data, successFunc, failedFunc, headers) {
    $.ajax({
        async: true,
        dataType: "text",
        contentType: "application/json",
        headers: headers,
        type: method || "POST",
        url: ("{0}/{1}/{2}/".format(baseUrl, resource, action || "")).replace(/\/\//gi, '/') + (queryString ? ("?" + queryString) : ""),
        data: typeof (data) == 'string' ? data : JSON.stringify(data),
        success: function (response) {
            if (successFunc) successFunc(response);
        },
        failure: function (response) {
            if (failedFunc) failedFunc(response);
        }
    });
};
/// Parameter sample: 
/// buttons:[{
///     id: "xxx",
///     classset: "xxxx",
///     text: "xxx",
///     func: function(dialog){}
/// }]
function showDialog(title, htmlBody, buttons, unclosable) {
    var dom = $('<div class="modal fade" aria-hidden="true" style="display: none;"><div class="modal-dialog"><div class="modal-content"><div class="modal-header">'
        + (unclosable ? "" : '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>')
        + '<h3 class="modal-title"></h3></div><div class="modal-body"><img src="" /></div><div class="modal-footer"></div></div></div></div>');
    dom.find("h3.modal-title").text(title || "");
    dom.find("div.modal-body").html(htmlBody || "");
    var foot = dom.find("div.modal-footer");

    if (buttons && buttons.length) {
        for (var i = 0; i < buttons.length; i++) {
            var btn = $("<a></a>");

            btn.prop("id", buttons[i].id);
            btn.attr("class", buttons[i].classset);
            btn.text(buttons[i].text);

            var func = buttons[i].func;
            if (!func) {
                btn.attr("data-dismiss", "modal");
            } else {
                btn.bind("click", { dialog: dom, func: func }, function (e) {
                    e.data.func(e.data.dialog);
                });
            }

            foot.append(btn);
        }
    }
    dom.modal();

    return dom;
}
function viewImage(url) {
    if (url) {
        var image = $("<img />");
        image.attr("href", url);
        showDialog("", image);
    }
}

function showConfirmDialog(title, message, okFunc, cancelFunc) {
    showDialog(title || "Confirmation", message || "Are you sure?", [
        {
            classset: "btn btn-primary",
            text: "No",
            func: function (dialog) {
                if (cancelFunc) cancelFunc(dialog);
                dialog.modal("hide");
            }
        }, {
            classset: "btn btn-primary",
            text: "Yes",
            func: function (dialog) {
                if (okFunc) okFunc(dialog);
                dialog.modal("hide");
            }
        }
    ]);
}
// Usage:
// Create: var mask= ajaxMask("xxxx");
// Update: ajaxMask(mask, "xxxx");
// Dispose: ajaxMask(mask);
function ajaxMask(p1, p2) {
    if (typeof (p1) == "object") {
        if (typeof (p2) == "string") {
            $(p1).find("span").text(p2);
            return $(p1);
        } else if (typeof (p2) == "undefined") {
            $(p1).modal("hide").remove();
        }
    } else if (typeof (p1) == "string") {
        var div = $("<div><i class='fa fa-spinner fa-pulse'></i><span style='margin-left:5px;'></span></div>");
        div.find("span").text(p1 || "Processing ...");
        return showDialog("Processing", div, undefined, true);
    }
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1, c.length);
        }
        if (c.indexOf(nameEQ) == 0) {
            return unescape(c.substring(nameEQ.length, c.length));
        }
    }
    return false;
}
function setQr(domContainer, url) {
    if (domContainer && domContainer.length && url) {
        var qrcode = new QRCode(domContainer[0], {
            width: domContainer.width(),
            height: domContainer.height()
        });

        qrcode.makeCode(url);
    }
}

function clearCookie(name) {
    setCookie(name, "", -1);
}

function setCookie(name, value, seconds) {
    seconds = seconds || 0;
    var expires = "";
    if (seconds != 0) {
        var date = new Date();
        date.setTime(date.getTime() + (seconds * 1000));
        expires = "; expires=" + date.toGMTString();
    }
    document.cookie = name + "=" + escape(value) + expires + "; path=/";
}

function getUrlParameter(url) {
    var dic = {};
    if (typeof (url) == "string") {
        var index = url.indexOf("?");
        if (index > -1) {
            var pairs = url.split("&");
            for (var i = 0; i < pairs.length; i++) {
                var t = pairs[i].split("=");
                dic[t[0]] = t[1] ? decodeURI(t[1]) : t[1];
            }
        }
    }
    return dic;
};

function setUrlParameter(url, key, value) {
    if (typeof (url) == "string" && typeof (key) == "string" && value != undefined) {
        var parameters = this.getUrlParameter(url);
        parameters[key] = value;

        var p = "?";
        for (var one in parameters) {
            if (parameters.hasOwnProperty(one)) {
                p += one + "=" + parameters[one] + "&";
            }
        }

        var index = url.indexOf("?");
        return (index < 0) ? (url + p) : url.substr(0, index) + p;
    }

    return url;
};

function getStringByFormat(obj, format) {
    var result = format;
    if (format && format.length) {
        format.replace(/\{([0-9a-z_]+)\}/g,
            function (m, i) {
                result.replace(new RegExp("{" + m + "}", "g"), obj[m] == null ? "" : obj[m].toString());
            });
    }
    return result;
};

function setDefaultTimeZone(select, offsetAttribute) {
    var offset = (-1) * (new Date().getTimezoneOffset());
    $(select).children("option[" + (offsetAttribute || "offset") + "='" + offset + "']").eq(0).prop("selected", true);
}

/// Data sample: 
/// columns: [{
///     name:"",
///     text:"",
///     style: "",
///     format: ""
/// }]
/// rows: [{
///     style: "" //style on tr
///     data:{}] 
/// }]
function createTable(container, columns, rows) {
    if (container && columns && columns.length) {
        container = $(container);

        container.children().remove();

        //draw head
        var thead = $("<thead></thead>");
        var headTr = $("<tr></tr>");
        for (var i = 0; i < columns.length; i++) {
            var th = $("<th></th>");
            th.text(columns[i]["text"]);
            th.attr("class", columns[i]["style"]);
            headTr.append(th);
        }
        thead.append(headTr);
        container.append(thead);

        //draw body
        var tbody = $("<tbody></tbody>");
        if (rows && rows.lengh) {
            for (var x = 0; x < rows.length; x++) {
                var tr = $("<tr></tr>");
                tr.attr("class", rows[x]["style"]);

                var cells = rows[x]["cells"];
                var data = rows[x]["data"] || {};

                for (var y = 0; y < columns.length; y++) {
                    var td = $("<td></td>");
                    td.attr("class", columns[y]["style"]);

                    var name = columns[y]["name"];
                    var format = columns[y]["format"];

                    td.text(data[name]);

                    if (format) {
                        td.attr("value", getStringByFormat(data, format));
                    }

                    tr.append(td);
                }

                tbody.append(tr);
                container.append(tbody);
            }
        }
    }
}


function axisIntervalParse(stringValue) {
    if (stringValue) {
        var parts = stringValue.split('_');
        if (parts.length == 3 && parts[0] == 'every') {
            var result = {
                Type: 7,//EveryNTimes
                N: parseInt(parts[1])
            };

            switch (parts[2]) {
                case "Minute":
                    result["Unit"] = 0;
                    break;
                case "Hour":
                    result["Unit"] = 1;
                    break;
                case "Day":
                    result["Unit"] = 2;
                    break;
                case "Week":
                    result["Unit"] = 3;
                    break;
                case "Month":
                    result["Unit"] = 4;
                    break;
                default:
            }

            return result;
        }
    }
};

function showEntityDialog(entityKey, keyUrl, dialogId) {
    dialogId = dialogId || "entityForm";

    if (entityKey) {
        callAction((keyUrl || "") + entityKey, {}, function (response) {

            var obj = JSON.parse(response);
            var form = $("#" + dialogId);
            setForm(form, obj);
            form.find(".visible-to-update-only").show();

            form.modal("show");

        }, undefined, function () {
        });
    } else {
        var form = $("#" + dialogId);
        setForm(form, {});

        form.find(".visible-to-update-only").hide();
        form.modal("show");
    }
}

function createorUpdateEntity(postUrl, formId, specialDataHandle, sececssFunc) {
    formId = formId || "entityForm";
    if (sececssFunc == undefined) {
        sececssFunc = function (json) {
            window.location.reload();
        }
    }

    var form = $("#" + formId);
    form.validator('validate');

    if (form.find(".has-error").length) {
        return;
    }

    var data = getForm(form);
    data = specialDataHandle ? specialDataHandle(form, data) : data;
    form.find("*").prop("disabled", true);

    var thisRef = $(this);
    thisRef.text("Saving ...");

    callAction(postUrl, data, function (response) {
        var json = JSON.parse(response);
        sececssFunc(json);
    }, undefined, function () {
        form.find("*").prop("disabled", false);
    });
}