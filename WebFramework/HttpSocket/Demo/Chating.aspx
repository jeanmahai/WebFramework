<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chating.aspx.cs" Inherits="Demo.Chating" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>chating</title>
    <script src="jquery-2.0.3.min.js" type="text/javascript"></script>
</head>
<body>
    <div>
        <input type="text" id="txtUserName" /></div>
    <div>
        <input type="text" id="txtMessage" />
        <button onclick="sendMessage()">
            send</button>
        <input type="text" id="txtTo" />
    </div>
    <div id="divMessage">
    </div>
    <script type="text/javascript">
        function sendMessage() {
            var msg = $.trim($("#txtMessage").val());
            if (msg === "") return;
            var userName = $.trim($("#txtUserName").val());
            if (userName === "") return;
            var to = $.trim($("#txtTo").val());
            if (to === "") return;
            $.get("MessageHandler.aspx", {
                From: userName,
                Msg: msg,
                To: to,
                Action: "send"
            }, function (result) {
                alert(result);
            });
        }

        var msgContainer = $("#divMessage");
        function pullMessage() {
            console.info("pull...");
            var timer;
            var userName = $.trim($("#txtUserName").val());
            if (userName === "") {
                timer = setTimeout(function () {
                    pullMessage();
                    if (timer) clearTimeout(timer);
                    timer = null;
                }, 1000);
            }
            var to = $.trim($("#txtTo").val());
            if (to === "") {
                timer = setTimeout(function () {
                    pullMessage();
                    if (timer) clearTimeout(timer);
                    timer = null;
                }, 1000);
            }
            $.getJSON("MessageHandler.aspx", {
                To: to,
                Action: "get"
            }, function (result) {
                var i, len = result.length;
                for (i = 0; i < len; i += 1) {
                    msgContainer.append("<div>from:" +
                        result[i].From + " " +
                        result[i].ReceiveDate + " " + result[i].Content + "</div>");
                }
                console.info("pulled");
                pullMessage();
            });
        }

        pullMessage();
    </script>
</body>
</html>
