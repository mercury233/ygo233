var fs = require("fs");
var md5File = require("md5-file");

//相关文件路径
var config = require("./config.json");
//ygopro文件路径
var ygopro_files = require("./ygopro-files.json");

//为减小文件大小，取MD5的前8位
var hash = function(relpath, fullpath) {
  if (!fullpath) {
    fullpath = config.base_path + relpath;
  }
  var h = md5File.sync(fullpath);
  h = h.slice(0, 8);
  return h;
}

//要生成的YGOPRO所有文件的hash
var files_json = {};

//主程序需要单独处理
files_json.ygopro_exe = hash(ygopro_files.ygopro_exe);

//几个在主程序目录的文件
files_json.files=[];

for (var i in ygopro_files.files) {
  var file = ygopro_files.files[i];
  var item = {};
  item.name = file;
  item.hash = hash(file);
  files_json.files.push(item);
}

//需要检查这些文件夹里所有的文件
files_json.folders={};

for (var i in ygopro_files.folders) {
  var folder = ygopro_files.folders[i];
  var folder_hashes = [];
  var folder_files=fs.readdirSync(config.base_path+folder);
  for (var j in folder_files) {
    var file = folder_files[j];
    if (file.indexOf(".")>0) { //文件必然有扩展名
      var filepath=folder + "\\" + file;
      console.log(filepath);
      var item = {};
      item.name = file;
      item.hash = hash(filepath);
      folder_hashes.push(item);
    }
  }
  files_json.folders[folder] = folder_hashes;
}

fs.writeFileSync(config.files_json_output, JSON.stringify(files_json));