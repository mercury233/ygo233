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

spawnSync(config.rm_exe, config.rm_cmd_base, { cwd: config.base_path, env: process.env });
spawnSync(config.rm_exe, config.rm_cmd_server, { cwd: config.server_path, env: process.env });

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

fs.writeFileSync(config.server_path+"files.json", JSON.stringify(files_json));

var packages_json={};
var packages_array=[];

spawnSync(config.sevenzip_exe, ["a", "-i!script\\*", "exe.7z", "ygopro.exe"], { cwd: config.base_path, env: process.env });
var exe_item={};
exe_item.name="主程序和脚本";
exe_item.filename="exe.7z";
exe_item.filecount=1;
exe_item.filesize=fs.statSync(config.base_path+"exe.7z").size;
exe_item.files=["ygopro.exe"];
packages_array.push(exe_item);

spawnSync(config.sevenzip_exe, ["a", "db.7z", "cards.cdb"], { cwd: config.base_path, env: process.env });
var db_item={};
db_item.name="数据库";
db_item.filename="db.7z";
db_item.filecount=1;
db_item.filesize=fs.statSync(config.base_path+"db.7z").size;
db_item.files=["cards.cdb"];
packages_array.push(db_item);

var packages = fs.readdirSync(config.packages_path);
for (var i = packages.length-1; i >= 0; i--) {
  var pack = packages[i];
  var packname = pack.replace(".ydk", "").replace(".txt", "");
  
  var pack_text = fs.readFileSync(config.packages_path+pack,{encoding:"ASCII"})
  var pack_text_array = pack_text.split("\n");
  var pack_array = [];
  for (var j in pack_text_array) {
    var card=parseInt(pack_text_array[j]);
    if (!isNaN(card)) {
      pack_array.push(card);
    }
  }
  
  var args = ["a"];
  var files = [];
  
  if (pack.indexOf(".ydk")>0) {
    fs.writeFileSync(config.base_path+"deck\\"+pack, pack_text);
    args.push("-i!deck\\"+pack);
  }
  
  args.push(packname + ".7z");

  for (var k in pack_array) {
    files.push("pics\\"+pack_array[k]+".jpg");
    files.push("pics\\thumbnail\\"+pack_array[k]+".jpg");
  }
  
  args=args.concat(files);

  spawnSync(config.sevenzip_exe, args, { cwd: config.base_path, env: process.env });
  
  var item={};
  item.name="卡图包 "+packname;
  item.filename=packname + ".7z";
  item.filecount=files.length;
  item.filesize=fs.statSync(config.base_path+packname+".7z").size;
  item.files=files;
  packages_array.push(item);
}

packages_json.packages=packages_array;

fs.writeFileSync(config.server_path+"packages.json", JSON.stringify(packages_json));

spawnSync(config.mv_exe, config.mv_cmd, { cwd: config.base_path, env: process.env });

spawnSync(config.sevenzip_exe, ["a", "data.7z", "*.json"], { cwd: config.server_path, env: process.env });

spawnSync(config.rm_exe, config.rm_cmd_json, { cwd: config.server_path, env: process.env });
