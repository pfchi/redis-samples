"use strict";

var teletubbies = ["丁丁", "迪西", "拉拉", "小波", "努努"];
var chickenSoup = [
    "好好活下去，每天都有新打擊。",
    "條條大路通羅馬，但有些人一出生就在羅馬。",
    "有時候不努力一下，不知道什麼叫絕望。",
    "在哪裡跌倒，就在哪裡躺好。",
    "萬丈高樓買不起。",
    "勤能補拙，看來你要非常的勤勞了。",
    "機會是留給錢準備好的人。",
    "不要在意別人怎麼看你，因為根本沒有人看你。",
    "麻雀雖小，哪有我衰小。",
    "今天不開心沒關係，反正明天也不會開心。",
    "好好活下去，每天都有新打擊。",
    "你才不是一無所有，你還有病。",
    "忍一時越想越氣，退一步越想越虧。",
    "努力不一定會成功，但不努力一定會很輕鬆。",
    "你要很愛自己，因為沒人會愛你。",
    "爬得越高，就有人等著把你推下山。",
    "每天起床照照鏡子，今天的自己又離夢想遠了一點。",
    "當你覺得自己醜、窮的時候，別絕望，至少你的判斷是對的。"
];


document.getElementById("Nickname").value = teletubbies[parseInt(Math.random() * 10) % teletubbies.length];

function farts() {
    document.getElementById("Message").value = chickenSoup[parseInt(Math.random() * 100 % chickenSoup.length)]
}


var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").prepend(li);
    var now = new Date();
    li.innerHTML = `[${now.toLocaleDateString()} ${now.toLocaleTimeString()}] <br/><b>${user}</b> : ${message}`;
});

connection.on("Connected", function (message) {
    console.log(message);
    var li = document.createElement("li");
    document.getElementById("messagesList").prepend(li);
    var now = new Date();
    li.innerHTML = `[${now.toLocaleDateString()} ${now.toLocaleTimeString()}] <br/>${message}`;
    farts();
});


connection.start().then(function (e, v, a) {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("Nickname").value;
    var message = document.getElementById("Message").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    farts();
    event.preventDefault();
});