/*!
 * Beyova.DropZoneAjax
 * http://github.com/rynnwang/DropZoneAjax
 *
 * Description:
 * Provide JavaScript function for customizing ajax form submit within file upload (including click to upload and drop to upload). 
 * Compared with other library, like DropZone.js, this file help to implement more customization and less limitation.
 * It is still a good sample to show how to submit with both form data (key-value fields) and binaries in one ajax function, using $.ajax().
 * 
 * Dependency:
 * jQuery
 *
 * Copyright 2005, Rynn Wang
 *
 */
// extensionLimit: .exe, .txt, etc.
function dropZoneAjax(formInstance, dropAreaContainer, dropAreaMessage, dropAreaNote, extensionLimit, url, httpMethod, dataType, progressFn, beforeSendFn, successFn, errorFn) {

    if (formInstance && formInstance.length && dropAreaContainer && dropAreaContainer.length) {

        // Draw UI.
        var dropContainer = $("<div class='drop'><div></div></div>");
        var note = $("<span class='note'></span>");
        var fileControl = $("<input type='file' class='file-upload'>");

        note.text(dropAreaNote);

        dropContainer.children("div").text(dropAreaMessage).append(note);

        var progressContainer = $("<div class='progress'><div class='progress-bar progress-bar-striped' role='progressbar' aria-valuenow='0' aria-valuemin='0' aria-valuemax='100' style='width: 0'><span class='sr-only'>0%</span></div></div>");

        dropAreaContainer.append(fileControl).append(dropContainer).append(progressContainer);
        var dropArea = $(dropAreaContainer).find(".drop");

        // handle extension. Convert to array.
        if (extensionLimit && (typeof (extensionLimit) == 'string')) {
            extensionLimit = [extensionLimit];
        }

        var dropbox = dropArea[0];
        dropbox.ondragover = dropbox.ondragenter = function (e) {
            e.stopPropagation();
            e.preventDefault();
        };
        dropbox.addEventListener("drop", function (e) {
            e.stopPropagation();
            e.preventDefault();

            var dt = e.dataTransfer;
            var files = dt.files;

            if (extensionLimit && !checkFileExtension(files[0], extensionLimit)) {
                return;
            }

            dropArea.hide();
            formInstance.find(".progress").show();

            commitForm(formInstance, files[0], url, httpMethod, dataType, progressFn, beforeSendFn, successFn, errorFn);
        }, false);

        $(dropAreaContainer).find("input:file").change(function (e) {
            if (extensionLimit && !checkFileExtension($(this), extensionLimit)) {
                return;
            }

            commitForm(formInstance, null, url, httpMethod, dataType, progressFn, beforeSendFn, successFn, errorFn);
        });

        dropArea.click(function (e) {
            formInstance.find("input:file").click();
        });
    }
}
function checkFileExtension(fileOrControl, allowedExtensions) {
    if (typeof (allowedExtensions) == "undefined" && fileOrControl) { return true; }

    var found = false;
    if (fileOrControl && allowedExtensions.length) {
        var isControl = !(fileOrControl.constructor.name == "File");

        var fileName = isControl ? fileOrControl.val() : fileOrControl.name;
        var lastDot = fileName.lastIndexOf('.');
        var fileExt = (lastDot > -1 ? fileName.substr(lastDot, fileName.length - lastDot) : "").toLowerCase();

        for (var i = 0; i < allowedExtensions.length; i++) {
            if (allowedExtensions[i].toLowerCase() == fileExt) {
                found = true;
                break;
            }
        }

        if (!found) {
            if (isControl) {
                fileOrControl.val('');
            }
            return false;
        } else {
            found = true;
        }
    }

    return found;
}

function commitForm(formInstance, file, url, httpMethod, dataType, progressFn, beforeSendFn, successFn, errorFn) {
    if (file) {
        $(formInstance).find('input:file').remove();
    } else {
        file = $(formInstance).find('input:file').first().prop("files")[0];
    }

    var formData = new FormData($(formInstance)[0]);
    if (file) {
        formData.append('file', file);
    }

    $.ajax({
        url: url,
        type: httpMethod,
        dataType: dataType || "text",
        xhr: function () {
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) {
                myXhr.upload.addEventListener('progress', function (e) {
                    var done = e.position || e.loaded, total = e.totalSize || e.total;
                    var percentVal = (Math.floor(done / total * 100000) / 1000) + '%';
                    var progressContainer = formInstance.find(".drop-area .progress");
                    progressContainer.children("div").css("width", percentVal);
                    progressContainer.find("span").html(percentVal);

                    if (progressFn) {
                        progressFn(e);
                    }
                }, false);
            }
            return myXhr;
        },
        beforeSend: beforeSendFn,
        success: successFn,
        error: errorFn,
        data: formData,
        cache: false,
        contentType: false,
        processData: false
    });
}