var yesCount = 0;
var noCount = 0;
var QuestionId;
var QuestionContent = "";
var oneClick = false;
var x;
var lockTextBox;
var tempScrollTop;


$(document).ready(function () {
    //ajaxCall("GET", "../api/RealTimeQuestion/get", "", success, error);
    toChat = JSON.parse(localStorage.toChat);
    lessonId = toChat.Lid;
    courseId = toChat.Cid;
    GetFiles(courseId, lessonId)
});

function GetFiles(idc, idl) {
    ajaxCall("GET", "../api/AddFile/?lessonId=" + idl + "&courseId=" + idc, "", successFile, errorFile);
}
function successFile(data) {
    files = data;
    document.getElementById('UlFiles').innerHTML = "";
    for (var i = 0; i < files.length; i++) {
        var idF = files[i].Idfile;
        var fileName = files[i].FileName;
        var fPath = files[i].Path;
        var li = '<div onclick="openFile(this)" name="' + fPath + '" id = "' + idF + '" class="list-group-item ' + fPath+'"> <a href="../uploadedFiles/' + fPath + '"> ' + fileName + '</a ></div > ';
        document.getElementById('UlFiles').innerHTML += li;
    }

}
function openFile(event) {
    $('.clock').html("");
    $("#chartContainer").html("");
    $('.centered').html("");
    //clearInterval(x);
    $("#omer").removeClass("show");
    var str ="../uploadedFiles/";
    for (var i = 1; i < event.classList.length; i++) {
        str+=event.classList[i]+" ";
    }
    str = str.substring(0, str.length - 1);
    iframe = '<iframe id="showFile" src="' + str + '" frameborder="0" scrolling="auto" style="border: 0px; width: 100%; height: 500px;"></iframe>';

    $('.showfiles').html(iframe);
}
function errorFile() {
    alert("error files");
}

// Initialize Firebase
var config = {
    apiKey: "AIzaSyBp55jCWT6yobw7s2T6sKnB5YhF-Qb0fzQ",
    authDomain: "askq-1234.firebaseapp.com",
    databaseURL: "https://askq-1234.firebaseio.com",
    projectId: "askq-1234",
    storageBucket: "askq-1234.appspot.com",
    messagingSenderId: "713703772173"
};
firebase.initializeApp(config);

ref = firebase.database().ref("/chat");
ref.on("child_added", function render2() {
    $('#divChatWindow').html("");

    ajaxCall("GET", "../api/RealTimeQuestion/get/?lessonId=" + lessonId + "&courseId=" + courseId, "", success, error);
});

ref1 = firebase.database().ref("/Answers");
ref1.on("child_added", function render3() {
    $('#divLecturerWindow').html("");
    if (QuestionId == undefined) return;
    ajaxCall("GET", "../api/studentsAnswers/get/?id=" + QuestionId, "", successAnswers, error);
});

ref2 = firebase.database().ref("/isTextLocked");
ref2.update(({ 'state': "false" }));

ref3 = firebase.database().ref("/lessonAndCourse");
ref4 = firebase.database().ref("/YesNo");

function render() {
    
    $('#divChatWindow').html("");

    ajaxCall("GET", "../api/RealTimeQuestion/get/?lessonId=" + lessonId + "&courseId=" + courseId, "", success, error);
}

function success(messages) {

    for (i = 0; i < messages.length; i++) {

        AddMessage(messages[i]);

    }

}

function error(err) {
    console.log(JSON.stringify(err))
    alert("EROR: " + JSON.stringify(err));
}

// Add new question to the chat list
function AddMessage(message) {

    var divChat = '<div class="direct-chat-msg"> <div class="direct-chat-info clearfix"></div> <div id="text_wrapper" class="text_wrapper scale-up-top" dir="rtl">' + message.Content + '</br> <center><input type="image" onClick="deleteQuestion(' + message.ID + ',' + " 'isAnswered' " + ')" src="../images/check-button.png" width="18px" height="18px" style="margin-top: 5px"> <input type="image" onClick="deleteQuestion(' + message.ID + ',' + " 'isDeleted' " + ')" src="../images/delete-button.png" width="18px" height="18px" style="margin-top: 5px"> <input type="image" onClick="yesNoQuestion(' + message.ID + ',' + " '" + message.Content + "' " + ')" src="../images/information.png" width="18px" height="18px" style="margin-top: 5px"> <input type="image" onClick="AddAnswer(' + message.ID + ',' + " '" + message.Content + "' " + ')" src="../images/back-button.png" width="18px" height="18px" style="margin-top: 5px"> </center> </div> </div>';

    $('#divChatWindow').append(divChat);

    var height = $('#divChatWindow')[0].scrollHeight;
    $('#divChatWindow').scrollTop(height);

}

// Remove question from the chat list
function deleteQuestion(id, field) {

    let str = "../api/RealTimeQuestion/put/?Qid=" + id + "&field=" + field;
    ajaxCall("PUT", str, "", render, error);

}

//Add the open question to the YesNoQuestion list and play timer
function yesNoQuestion(id, content) {
    ref4.update(({ 'state': "true" }));
    clearInterval(x);
    $('.showfiles').html("");
    $('#divLecturerWindow').html("");
    $('#chartContainer').html("");
    localStorage.setItem('id', id);
    var d = new Date();
    d.toString();
    YesNoQ = {
        RealTimeQuestionId: id,
        Content: content,
        UploadDate: d
    }

    ajaxCall("Post", "../api/YesNoQuestion", JSON.stringify(YesNoQ), successUpdate, error);

    var seconds = 11;

    $(".centered").html('<input type="image"  id="returnBTN" src = "../images/return.png" onclick="back()"> ');
    $(".centered").append("<p>" + content + "</p>");


    // Update the timer down every 1 second
    x = setInterval(function () {

        seconds = seconds - 1;
        $(".clock").html("<br/><p>" + seconds + "s </p>");

        // If the count down is over, write EXPIRED
        if (seconds <= 0) {

            clearInterval(x);
            $(".clock").html("<p> EXPIRED! </p>");

            ajaxCall("GET", "../api/YesNoQuestion/?id=" + YesNoQ.RealTimeQuestionId, "", successChart, error);

        }
    }, 1000);

}

function successUpdate() {
    $("#divLecturerWindow").scrollTop(tempScrollTop);
}

function successChart(data) {

    yesCount = data.YesCounter;
    noCount = data.NoCounter;

    successChart2();

}

// Show chart, with yesCounter and noCounter
window.successChart2 = function () {

    chart = new CanvasJS.Chart("chartContainer", {
        theme: "light1",
        animationEnabled: false,
        title: {
            text: "What do you think?"
        },
        data: [
            {
                type: "column",
                dataPoints: [
                    { label: "Yes", y: yesCount },
                    { label: "No", y: noCount }
                ]
            }
        ]
    });
    chart.render();

}

// Show the question on the lecturer side
function AddAnswer(qid, qcontent) {

    ref2.update(({ 'state': "true" }));
    clearInterval(x);
    $('.showfiles').html("");
    $('.clock').html("");
    $("#chartContainer").html("");
    $(".centered").html('<input type="image"  id="returnBTN" src = "../images/return.png" onclick="back()"> ');
    $(".centered").append('<p> ' + qcontent + "</p> ");

    localStorage.setItem('id', qid);
    localStorage.setItem('content', qcontent);

    QuestionId = qid;
    QuestionContent = qcontent;

    ajaxCall("GET", "../api/studentsAnswers/get/?id=" + qid, "", successAnswers, error);
}


function back() {
    location.href = "ChatNew.html";
}

function goToIndex() {
    location.href = "indexLecturer.html";
}


//Show all students answers on the lecturer side
function successAnswers(Answer) {

    oneClick = false;
    $('#divLecturerWindow').html("");

    for (i = 0; i < Answer.length; i++) {

        AddAnswer2(Answer[i]);

    }

}

function AddAnswer2(Answer) {

    Aid = Answer.ID;
    AContent = Answer.AnswerContent;
    var divChat = '<div class="direct-chat-msg"> <div class="direct-chat-info clearfix"> </div> <div id="answer' + Answer.ID + '" class="text_wrapper scale-up-top answer' + Answer.ID + ' ' + Answer.ID + '" dir="rtl" onClick="saveAll(this)">' + Answer.AnswerContent + '</br> </div> </div> ';
    document.body.innerHTML += "<style> #answer" + Answer.ID + ":after { border-left-color: #75C9C8; } </style>";

    $('#divLecturerWindow').append(divChat);


}


function saveAll(event) {

    if (oneClick == false)
        oneClick = true;
    else return;


    tempScrollTop = $("#divLecturerWindow").scrollTop();

    answerId = event.classList[2];  //get the word answer + id like answer4
    id = event.classList[3];    //get the id like 4

    $(event).css('background-color', 'green');

    document.body.innerHTML += "<style> #" + answerId + ":after { border-left-color: green; } </style>";
    ref2.update(({ 'state': "false" }));
    let str = "../api/QuestionAndAnswer/put/?Qid=" + id;
    ajaxCall("Put", str, "", successUpdate, error);

}