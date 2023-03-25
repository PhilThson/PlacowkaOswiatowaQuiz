$(document).ready(function () {
    var currentAreaId = $("#areasDropdown").attr('currentAreaId');
    $.ajax({
        type: "GET",
        url: "/Dictionary/GetAreas"
    })
    .done(function (data, status) {
        $.each(data, function (i, area) {
            if (area.id != currentAreaId)
                $("#areasDropdown").append(
                    "<option value=" + area.id + ">" +
                    area.description + "</option>");
            else
                $("#areasDropdown").append(
                    "<option selected value=" + area.id + ">" +
                    area.description + "</option>");
        });
    })
    .fail(function (result, status) {
        toastr.error(result.responseText, null,
            { timeOut: 7000, positionClass: "toast-bottom-right" });
    });

    var currentDifficultyId = $("#difficultiesDropdown").attr('currentDifficultyId');
    $.ajax({
        type: "GET",
        url: "/Dictionary/GetDifficulties"
    })
    .done(function (data, status) {
        $.each(data, function (i, difficulty) {
            if (difficulty.id != currentDifficultyId)
                $("#difficultiesDropdown").append(
                    "<option value=" + difficulty.id + ">" +
                    difficulty.description + "</option>");
            else
                $("#difficultiesDropdown").append(
                    "<option selected value=" + difficulty.id + ">" +
                    difficulty.description + "</option>");
        });
    })
    .fail(function (result, status) {
        toastr.error(result.responseText, null,
            { timeOut: 7000, positionClass: "toast-bottom-right" });
    });
});