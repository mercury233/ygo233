var fs = require("fs");
var md5File = require("md5-file");

//相关文件路径
var config = fs.readFileSync("./config.json");
config=JSON.parse(config);

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

var files_json = fs.readFileSync("./files.json");
files_json=JSON.parse(files_json);

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

//console.log(JSON.stringify(files_need_update));

var packages_download = [];

var packages_json = fs.readFileSync("./packages.json");
packages_json=JSON.parse(packages_json);

var pack_size = 0;

for(var i in packages_json.packages) {
  var package = packages_json.packages[i];
  var count=0;
  for(var j in package.files) {
    var file=package.files[j];
    var index=files_need_update.indexOf(file);
    if (index>=0) {
      count++;
    }
  }
  if (count>=package.filecount*0.66 || count>=100) {
    packages_download.push(package.filename);
    pack_size += package.filesize;
    for(var k in package.files) {
      var file=package.files[k];
      var index=files_need_update.indexOf(file);
      if (index>=0) {
        files_need_update.splice(index, 1);
      }
    }
  }
}

var download_json={};

download_json.files_download=files_need_update;
download_json.packages_download=packages_download;
download_json.pack_size=pack_size;
download_json.files_count=files_need_update.length+packages_download.length;

fs.writeFileSync("download.json", JSON.stringify(download_json));
