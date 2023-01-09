//Obsolete
//Okazało się że NIE można przechwycić pliku pdf z odpowiedzi Ajax
//przynajmniej mi się nie udało
var Download = function (id) {
    var url = "/Report/GetDiagnosisReport?diagnosisId=" + id;

    var fileName = "Diagnoza_nr_" + id + ".pdf";

    $.ajax({
        url: url,
        method: "GET",
        cache: false,
        timeout: 60000,
        dataType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        })
        .done(function (data, textStatus, xhr) {
            var disposition = xhr.getResponseHeader('Content-Disposition');

            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) fileName = matches[1].replace(/['"]/g, '');
            }
            //zamiana tablicy bajtów na blob
            //var atobData = atob(data);
            //var num = new Array(atobData.length);
            //for (var i = 0; i < atobData.length; i++) {
            //    num[i] = atobData.charCodeAt(i);
            //}
            //var pdfData = new Uint8Array(num);
            var blob = new Blob([data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            //var blob = new Blob([pdfData], { type: "application/pdf;base64" });
            var url = window.URL || window.webkitURL;
            link = url.createObjectURL(blob);
            var a = $("<a />");
            a.attr("download", fileName);
            a.attr("href", link);
            $("body").append(a);
            a[0].click();
            $("body").remove(a);
            var message = "Raport jest pobierany";
            toastr.success(message, null,
                { timeOut: 5000, positionClass: "toast-bottom-right" });
            //Swal.fire("Sukces", message, "success");
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

