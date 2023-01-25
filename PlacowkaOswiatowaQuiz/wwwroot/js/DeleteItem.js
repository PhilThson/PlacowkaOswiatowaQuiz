

function DeleteItem(endpoint, itemId) {
    var url = "/" + endpoint + "/Delete?id=" + itemId;
    Swal.fire({
        title: 'Czy na pewno usunąć wybrany rekord?',
        type: 'warning',
        showCancelButton: true,
        cancelButtonText: 'Anuluj',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Usuń'
    }).then((result) => {
        if (result.value == true) {
            $.ajax({
                type: "DELETE",
                url: url,
            })
            .done(function (result) {
                Swal.fire(
                    'Usunięto!',
                    'Poprawnie usunięto rekord.',
                    'success');
                $('#tr_' + questionId).remove();
            })
            .fail(function (result) {
                Swal.fire(
                    'Błąd!',
                    'Wystąpił błąd podczas usuwania. Odpowiedź serwera:' +
                    result.responseText,
                    'warning');
            });
        }
    });
};
