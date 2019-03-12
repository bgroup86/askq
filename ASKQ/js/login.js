$(document).ready(function () {
    $("#login-form").submit(f2);
});
function f2() {
    enterPerson();
    return false;
}
function enterPerson() {
    var Id = $("#your_id").val();
    var Userpassword = $("#your_pass").val();
    ajaxCall("GET", "../api/login/?PersonId=" + Id + "&Userpassword=" + Userpassword, "", successLog, errorLog);
}
function successLog(data) {
    console.log(data);
    if (data["Type"] == "Student") {
        window.location.href = "indexStudent.html";
    }
    else {
        window.location.href = "indexLecturer.html";
    }
    swal("Welcome!", "Let's start learning!", "success");
}

function errorLog() {
    swal("Oops", "Sorry, one of your details is incorrect", "error");
}