
$(".js-download").click(function (e) {
    e.preventDefault();
    var button = $(e.target);
    var id = button.attr("data-attachment-id");
    var url = "/Attachment/Download?attachmentId=" + id;

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
    }).done(function (data) {
        //Zamiana tablicy bajtów na obiekt Blob
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
    }).fail(function (result, status, xhr) {
        console.log(result.responseText);
        toastr.error(result.responseText, null, {
            timeOut: 5000, positionClass: "toast-bottom-right"
        });
    });
});