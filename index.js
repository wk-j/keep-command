#!/usr/bin/env node

var spawn = require('child_process').spawn;
var process = require("process");

var file = __dirname + "/KeepCommand/bin/Debug/KeepCommand.exe";

if(process.platform === "win32") {
    spawn(file, [], { stdio: "inherit"});
} else {
    spawn("mono", [file] , {stdio: "inherit"});
}