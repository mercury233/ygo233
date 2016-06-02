var fs = require("fs");
var md5File = require("md5-file");

//相关文件路径
var config = require("./config.json");

//为减小文件大小，取MD5的前8位
var hash = function(relpath, fullpath) {
  if (!fullpath) {
    fullpath = config.base_path + relpath;
  }
  var h = md5File.sync(fullpath);
  h = h.slice(0, 8);
  return h;
}

//检查文件的hash是否与new_hash相等
var checkfile = function(relpath, new_hash) {
  try {
    var local_hash = hash(relpath);
    if (local_hash != new_hash) {
      console.log("diff", relpath, local_hash, new_hash);
      files_need_update.push(relpath);
    }
  }
  catch(e) {
    console.log("not found", relpath, new_hash)
    files_need_update.push(relpath);
  }
}

var files_need_update = [];

var files_json = require("../server/files.json");

checkfile(config.ygopro_exe, files_json.ygopro_exe);

for(var i in files_json.files) {
  var file = files_json.files[i];
  checkfile(file.name, file.hash);
}

for(var folder in files_json.folders) {
  var folder_files = files_json.folders[folder];
  for (var j in folder_files) {
    var file = folder_files[j];
    checkfile(folder + "\\" + file.name, file.hash);
  }
}

console.log(JSON.stringify(files_need_update));
