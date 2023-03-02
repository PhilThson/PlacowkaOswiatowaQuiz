
$(document).ready(function () {
    var currentRatingId = $("#ratingsDropdown").attr('currentRatingId');

    //uzupełnienie combo boxa danymi z ukrytej listy ocen wcześniej
    //załadowanego widoku zestawu pytań
    $('input[name="questionsSetRating"]')
        .each(function (i, rating) {
            if (rating.id != currentRatingId)
                $("#ratingsDropdown").append(
                    "<option value=" + rating.id + ">" +
                    rating.value + "</option>");
            else
                $("#ratingsDropdown").append(
                    "<option selected value=" + rating.id + ">" +
                    rating.value + "</option>");
    });
    setValidation();
});

$("#saveResult").click(function (e) {
    e.preventDefault();
    var validationResult = $("#partialResultForm").valid();
    if (validationResult == false) {
        return;
    }
    var selectedQuestionsSetRatingId = $("#ratingsDropdown").find(":selected").val();

    $.ajax({
        type: "POST",
        url: "/Diagnosis/SaveResult",
        data: $("#partialResultForm").serialize() +
            "&QuestionsSetRating.Id=" + selectedQuestionsSetRatingId
    })
    .done(function (result, status) {
        toastr.success(result, null,
            { timeOut: 5000, positionClass: "toast-bottom-right" });
    })
    .fail(function (result, status) {
        toastr.error(result.responseText, null,
            { timeOut: 7000, positionClass: "toast-bottom-right" });
    });
});

var setValidation = function() {
    $("#partialResultForm").removeData("validator");
    $("#partialResultForm").removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse("#partialResultForm");
}