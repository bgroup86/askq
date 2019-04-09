$(document).ready(function () {
    p = JSON.parse(localStorage.person);
    document.getElementById('imgUser').src = "../" + p.Img;
    document.getElementById('imgUser2').src = "../" + p.Img;
    document.getElementById('imgUser4').src = "../" + p.Img;
    document.getElementById('imgUser3').src = "../" + p.Img;
    document.getElementById('profileName').innerHTML = p.FirstName + " " + p.LastName;
    document.getElementById('profileName2').innerHTML = p.FirstName + " " + p.LastName;

    GetCourse();
});
$('#topicModal').on('show.bs.modal', function (e) {
    $trigger = $(e.relatedTarget);
    newTopicId = e.relatedTarget.value;
});

function AddNewCourse() {
    var d = new Date();
    var n = d.getFullYear();
    newCourse = {
        Id:p.Id,
        CourseName: $("#CourseName").val(),
        Info: $("#CourseInfo").val(),
        CourseYear: n
    };
    ajaxCall("POST", "../api/Course", JSON.stringify(newCourse), successNewCourse, errorNewCourse);
}

function successNewCourse() {
    GetCourse();
}
function errorNewCourse() {
    alert("error");
}
function GetCourse() {
    ajaxCall("GET", "../api/Course/?id=" + p.Id, "", successGetC, errorGetC);
}

function successGetC(data) {
    document.getElementById('myCourses').innerHTML = "";
    console.log(data);
    courses = data;
    for (var i = 0; i < courses.length; i++) {
        var Name = courses[i].CourseName;
        var id = courses[i].CourseId;
        newcourse = '<div id="' + id + '" style="text-align: right;" ><button  value = "' + Name + '" id = "' + id + '" onclick = "getLesson(' + id + ')" type = "button" class="btn btn-outline-secondary waves-effect c'+id+'" > ' + Name+'</button ></div > ';
        document.getElementById('myCourses').innerHTML += newcourse;
    }
}

function errorGetC() {
    swal("Oops", "Sorry, Some Problem with DB", "error");
}

function getLesson(idCourse) {
    $('#sidenav-collapse-main').removeClass("show");
    courseId2 = idCourse;
    document.getElementById('courseName').innerHTML = $(".c"+idCourse).val();
    var btn = '<div id="btnC" class="btn-group btn-group-justified"><button data-toggle="modal" value="' + idCourse +'" data-target="#topicModal" type="button" class="btn btn-success">Add new topic</button><button type="button" class="btn btn-success">Update/Delete Topic</button></div>';
    document.getElementById('showBtn').innerHTML = btn;
    ajaxCall("GET", "../api/Lesson/?id=" + p.Id + "&courseId=" + idCourse, "", successGetL, errorGetL);
}

function successGetL(data) {
    document.getElementById('myLesson').innerHTML = "";
    console.log(data);
    lessons = data;
    for (var i = 0; i < lessons.length; i++) {
        var Name = lessons[i].LessonName;
        var id = lessons[i].LessonId;
        var btnLesson = '<div class="btn-group"><button type="button" value="' + id + '" data-toggle="modal" data-target="#newFileModal" class="btn btn-success">Add New File</button><button type="button" class="btn btn-success">Update/Delete File</button><div class="btn-group"><button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown">Ask-Q <span class="caret"></span></button><ul class="dropdown-menu" role="menu"><li><a href="#">Yes / No Question</a></li><li><a href="#">Multiple Choice Question</a></li></ul></div></div><button onclick="Lets_Start(' + id +')" type="button" class="btn btn-success" style="margin-left: 15px;">Lets Start!</button>';
        newlesson = '<div class="container lessons" data-toggle="collapse" onclick="saveIdLesson(' + id + ')" href="#collapse' + id + '"><div class="panel-group"><div class="panel panel-default"><div class="panel-heading"><h4 class="panel-title" >' + Name + '</a ></h4 ></div ><div id = "collapse' + id + '" class="panel - collapse collapse" >' + btnLesson +' <ul id="F'+id+'" class="list - group"> </ul ></div ></div ></div ></div > ';
        document.getElementById('myLesson').innerHTML += newlesson;
    }
}

function saveIdLesson(idL) {
    lessonId = idL;
    var idcourse = courseId2;
    GetFiles(idcourse, idL);
    ajaxCall("POST", "../api/AddFile/?lessonId=" + lessonId + "&courseId=" + idcourse, "", "", "");
}

function errorGetL() {
    alert("error with get lessons");
}

function AddNewTopic() {
    newTopic = {
        LessonName: $("#TopicName").val(),
        Info: $("#TopicInfo").val(),
        IsActive: false,
        timeStampLesson: $("#TopicTime").val(),
        courseId: courseId2,
        Id: p.Id
    };
    ajaxCall("POST", "../api/Lesson", JSON.stringify(newTopic), successNewTopic, errorNewTopic);

}
function successNewTopic() {
    alert('succses');
    getLesson(newTopicId);
}
function errorNewTopic() {
    alert('error lesson');
}

function AddNewFile() {
    var fname = courseId2 + "/" + lessonId + "/";
    var file = $("#fileupload").get(0).files[0];
    newFile = {
        LessonId: lessonId,
        CourseId: courseId2,
        FileName: $("#fileName").val() ,
        FileDescription: $("#fileDescription").val(),
        Path: fname + file.name,
        TypeFile: $("#typeFile").val()
    };
    addfile_ajax(newFile, lessonId, courseId2, fname,file);
}

function AddFileSuccsess(newFile) {
    ajaxCall("POST", "../api/AddFile", JSON.stringify(newFile), successNewFile, errorNewFile);
}
function successNewFile() {
    alert("succses");
    GetFiles(courseId2,lessonId);
}
function errorNewFile() {
    alert("error to upload your file");
}
function GetFiles(idc,idl) {
    ajaxCall("GET", "../api/AddFile/?lessonId=" + idl + "&courseId=" + idc, "", successGetF, errorGetF);
}
function successGetF(data) {
    files = data; 
    var idc = files[0].CourseId;
    var idl = files[0].LessonId;
    document.getElementById("F"+idl).innerHTML = "";
    for (var i = 0; i < files.length; i++) {
        var idF = files[i].Idfile;
        var fileName = files[i].FileName;
        var fPath = files[i].Path;
        var li = '<li id="' + idF + '" class="list - group - item"><a href="../uploadedFiles/' + fPath + '" download>' + fileName + '</a></li>';
        document.getElementById("F"+idl).innerHTML += li;
    }

}
function errorGetF() {
    alert("error with getting files");
}

var config = {
    apiKey: "AIzaSyBp55jCWT6yobw7s2T6sKnB5YhF-Qb0fzQ",
    authDomain: "askq-1234.firebaseapp.com",
    databaseURL: "https://askq-1234.firebaseio.com",
    projectId: "askq-1234",
    storageBucket: "askq-1234.appspot.com",
    messagingSenderId: "713703772173"
};
firebase.initializeApp(config);



DataRef = firebase.database().ref("/lessonAndCourse");
questionRef = firebase.database().ref("/isQuestionLocked");
questionRef.update(({ 'state2': "false" }));

function Lets_Start(lId) {
    questionRef.update(({ 'state2': "true" }));
    toChat = {
        Lid: lId,
        Cid: courseId2
    }

    DataRef.update(({ 'lesson': lId }));
    DataRef.update(({ 'course': courseId2 }));

    localStorage.setItem('toChat', JSON.stringify(toChat));

    window.location.href = "ChatNew.html";
}