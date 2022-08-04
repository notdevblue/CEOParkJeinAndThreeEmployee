const path = require("path");
const { exit, exitCode } = require("process");

const fs = require("fs").promises;
const dir = path.join(process.env.PWD, ".Log");

// .Log 폴더 확인
fs.access(dir)
   .catch(err => {
      fs.mkdir(dir);
   });

// 로그 헤더
function getHeader() {
   var date = new Date();
   return ` ${date.getHours()}-${date.getMinutes()}-${date.getSeconds()}-${date.getMilliseconds()} `;
}
function getDate() {
   var date = new Date();
   return `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`;
}

// 로그 작성
function write(msg, ipAddr) {
   var filePath = path.join(dir, getDate() + ".log");
   var header = getDate() + getHeader() + (ipAddr == undefined ? "\t\t\t\t\t" : ("\t\t[" + ipAddr + "]\t"));

   fs.access(filePath)
      .catch(err => {
         fs.writeFile(filePath, "", err => {
            console.log(`[II] Created new log file: ${filePath}`);
         });
      });
   
   msg = header + msg;
   
   console.log(msg);
   fs.appendFile(filePath, [msg + "\r\n"]);
}

module.exports = write;