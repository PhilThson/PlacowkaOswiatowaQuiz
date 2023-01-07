
var Download = function (id) {
    var url = "/Attachment/GetDiagnosisReport?diagnosisId=" + id;

    var fileName = "Diagnoza_nr_" + id + ".pdf";

    $.ajax({
        url: url,
        method: "GET",
        cache: false,
        timeout: 60000,
        })
        .done(function (data, textStatus, xhr) {
            //Pobranie nazwy pliku
            console.log("text status:");
            console.log(textStatus);
            console.log(xhr);
            
            var disposition = xhr.getResponseHeader('Content-Disposition');
            console.log(disposition);

            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) fileName = matches[1].replace(/['"]/g, '');
            }
            //zamiana tablicy bajtów na blob
            var atobData = atob(data);
            var num = new Array(atobData.length);
            for (var i = 0; i < atobData.length; i++) {
                num[i] = atobData.charCodeAt(i);
            }
            var pdfData = new Uint8Array(num);
            //var blob = new Blob([data], { type: "application/octet-stream" });
            var blob = new Blob([pdfData], { type: "application/pdf;base64" });
            var url = window.URL || window.webkitURL;
            link = url.createObjectURL(blob);
            var a = $("<a />");
            a.attr("download", fileName);
            console.log(fileName)
            a.attr("href", link);
            $("body").append(a);
            a[0].click();
            $("body").remove(a);
            var message = "Raport jest pobierany";
            Swal.fire("Sukces", message, "success");
        })
        .fail(function (data, textStatus, errorThrown) {
            var title = "Nie udało się pobrać pliku";
            if (textStatus === 'timeout') {
                var message = "Przekroczono limit czasu oczekiwania";
                Swal.fire(title, message, "error");
            }
            else {
                Swal.fire(title, "Odpowiedź serwera: " + errorThrown, "error");
            }
        });
};

