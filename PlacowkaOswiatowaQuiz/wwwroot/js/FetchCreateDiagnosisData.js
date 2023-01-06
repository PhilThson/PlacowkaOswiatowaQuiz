$(document).ready(function () {
    var currentEmployeeId = $("#employeesDropdown").attr('currentEmployeeId');
    $.ajax({
        type: "GET",
        url: "/Employee/GetAllEmployees",
        data: "{}",
        success: function (data) {
            $.each(data, function (i, employee) {
                if (employee.id != currentEmployeeId)
                    $("#employeesDropdown").append(
                        "<option value=" + employee.id + ">" +
                        employee.firstName + ' ' + employee.lastName + "</option>");
                else
                    $("#employeesDropdown").append(
                        "<option selected value=" + employee.id + ">" +
                        employee.firstName + ' ' + employee.lastName + "</option>");
            });
        }
    });
});

$(document).ready(function () {
    var currentStudentId = $("#studentsDropdown").attr('currentStudentId');
    $.ajax({
        type: "GET",
        url: "/Student/GetAllStudents",
        data: "{}",
        success: function (data) {
            $.each(data, function (i, student) {
                if (student.id != currentStudentId)
                    $("#studentsDropdown").append(
                        "<option value=" + student.id + ">" +
                        student.firstName + ' ' + student.lastName + "</option>");
                else
                    $("#studentsDropdown").append(
                        "<option selected value=" + student.id + ">" +
                        student.firstName + ' ' + student.lastName + "</option>");
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
                        difficulty.description + "</option>");
                else
                    $("#difficultiesDropdown").append(
                        "<option selected value=" + difficulty.id + ">" +
                        difficulty.description + "</option>");
            });
        }
    });
});
