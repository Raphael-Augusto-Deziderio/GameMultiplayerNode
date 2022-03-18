var express = require("express");
var app = express();
var http = require("http").Server(app);
var io = require("socket.io")(http);

io.on("connection", function (socket) {
  socket.on("PING", function (pack) {
    console.log("Mensagem recebida do unity: " + pack.message);

    var json_pack = {
      message: "pong!!!",
    };

    socket.emit("PONG", json_pack);
  });

  socket.on("SPAWN", function (pack) {
    socket.emit("SPAWN", pack.message);
    console.log("RECEBI DO CLIENT: " + pack.message);
  });
});

http.listen(3000, function () {
  console.log("server listen on 3000!");
});

console.log("----------------NodeJS Server is running------------------");
