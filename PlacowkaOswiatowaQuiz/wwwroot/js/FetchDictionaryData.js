$(document).ready(function () {
    var currentAreaId = $("#areasDropdown").attr('currentAreaId');
    $.ajax({
        type: "GET",
        url: "/Dictionary/GetAreas",
        data: "{}",
        success: function (data) {
            $.each(data, function (i, area) {
                if (area.id != currentAreaId)
                    $("#areasDropdown").append(
                        "<option value=" + area.id + ">" +
                        area.name + "</option>");
                else
                    $("#areasDropdown").append(
                        "<option selected value=" + area.id + ">" +
                        area.name + "</option>");
            });
        }
    });
});
$(document).ready(function () {
    var currentDifficultyId = $("#difficultiesDropdown").attr('currentDifficultyId');
    $.ajax({
        type: "GET",
        url: "/Dictionary/GetDifficulties",
        data: "{}",
        success: function (data) {
            $.each(data, function (i, difficulty) {
                if (difficulty.id != currentDifficultyId)
                    $("#difficultiesDropdown").append(
                        "<option value=" + difficulty.id + ">" +
                        difficulty.name + "</option>");
                else
                    $("#difficultiesDropdown").append(
                        "<option selected value=" + difficulty.id + ">" +
                        difficulty.name + "</option>");
            });
        }
    });
});