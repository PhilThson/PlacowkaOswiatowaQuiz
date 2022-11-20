var Download = function (id) {
    var url = "/Attachment/Download?attachmentId=" + id;
    //var fileName = $("[name='FileName']");

    var fileName = $('#FileName').text();
    $.ajax({
        url: url,
        cache: false,
        xhr: function () {
            var xhr = new XMLHttpRequest();
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 2) {
                    if (xhr.status == 200) {
                        xhr.responseType = "blob";
                    } else {
                        xhr.responseType = "text";
                    }
                }
            };
            return xhr;
        },
        success: function (data) {
            //Convert the Byte Data to BLOB object.
            var blob = new Blob([data], { type: "application/octetstream" });
            var url = window.URL || window.webkitURL;
            link = url.createObjectURL(blob);
            var a = $("<a />");
            a.attr("download", fileName);
            console.log(fileName)
            a.attr("href", link);
            $("body").append(a);
            a[0].click();
            $("body").remove(a);
            var message = "Załącznik jest pobierany";
            Swal.fire({
                title: message,
                type: "success",
                onAfterClose: () => {
                    location.reload();
                }
            });
        }
    });
};

