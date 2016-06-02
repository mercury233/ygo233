var fs = require("fs");
var md5File = require("md5-file");
var spawnSync = require("child_process").spawnSync;

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

spawnSync(config.7z_exe, ["a", "-i!script\\*", "exe.7z", "ygopro.exe"], { cwd: config.base_path, env: process.env });

spawnSync(config.7z_exe, ["a", "db.7z", "cards.cdb"], { cwd: config.base_path, env: process.env });

var packages = fs.readdirSync(config.packages_path);
for (var i in packages) {
  var pack = packages[i];
  
  var pack_text = fs.readFileSync(config.packages_path+pack,{encoding:"ASCII"})
  var pack_text_array = pack_text.split("\n");
  var pack_array = [];
  for (var i in pack_text_array) {
    var card=parseInt(pack_text_array[i]);
    if (!isNaN(card)) {
      pack_array.push(card);
    }
  }
  
  var args = ["a"];
  
  if (pack.indexOf(".ydk")>0) {
    fs.writeFileSync(config.base_path+"deck\\new_"+pack, pack_text);
    args.push("-i!deck\\new_"+pack);
  }
  
  args.push(pack.replace(".ydk", "") + ".7z");

  for (var j in pack_array) {
    args.push("pics\\"+pack_array[j]+".jpg");
    args.push("pics\\thumbnail\\"+pack_array[j]+".jpg");
  }
  
  spawnSync(config.7z_exe, args, { cwd: config.base_path, env: process.env });
}