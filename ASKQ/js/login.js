$(document).ready(function () {
    //get all hobbies from DB
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
    alert("succses");
}
function errorLog() {
    alert("Error log");
}