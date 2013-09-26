(function() {
  var async, combineCssFile, combineFiles, combineJsFiles, complierFileName, fs, getCombineFileList, initWebUrlList, jsp, pro, request;

  fs = require('fs');

  request = require('request');

  async = require('async');

  jsp = require("uglify-js").parser;

  pro = require("uglify-js").uglify;

  complierFileName = './compiler.json';

  initWebUrlList = function(guideTemplateList) {
    var key, module_widthValue, tValue, templateValue, tmpParams, value, values, webListParams, webUrlList, _i, _j, _k, _len, _len2, _len3, _ref, _ref2;
    webUrlList = [];
    webListParams = [];
    values = guideTemplateList.values;
    for (key in values) {
      templateValue = values[key];
      tmpParams = [];
      for (_i = 0, _len = templateValue.length; _i < _len; _i++) {
        value = templateValue[_i];
        tmpParams.push("" + key + "=" + value);
      }
      webListParams.push(tmpParams);
    }
    _ref = webListParams[0];
    for (_j = 0, _len2 = _ref.length; _j < _len2; _j++) {
      tValue = _ref[_j];
      _ref2 = webListParams[1];
      for (_k = 0, _len3 = _ref2.length; _k < _len3; _k++) {
        module_widthValue = _ref2[_k];
        webUrlList.push({
          name: (tValue.replace('=', '')) + '_' + module_widthValue.replace('module_width=', ''),
          url: "" + guideTemplateList.baseUrl + "?seller_nick=" + guideTemplateList.seller_nick + "&" + tValue + "&" + module_widthValue + "&th=99999"
        });
      }
    }
    return webUrlList;
  };

  getCombineFileList = function(guideTemplateList) {
    var combineJSON, linkReg, scriptRge;
    combineJSON = {};
    linkReg = /<link.*?href\s*=\s*"([\w\/\?\.]+)"/g;
    scriptRge = /<script.*?src\s*=\s*"([\w\/\?\.]+)"/g;
    return async.forEachLimit(initWebUrlList(guideTemplateList), 10, function(item, finish) {
      console.log(item.url);
      return request(item.url, function(err, res, body) {
        var cssFiles, cssIndex, hrefIndex, jsFiles, jsIndex, result, results, srcIndex, _i, _j, _len, _len2;
        if (err) throw err;
        results = body.match(linkReg);
        cssFiles = [];
        for (_i = 0, _len = results.length; _i < _len; _i++) {
          result = results[_i];
          hrefIndex = result.indexOf('href=');
          cssIndex = result.indexOf('.css');
          cssFiles.push(result.substring(hrefIndex + 6, cssIndex) + '.css');
        }
        results = body.match(scriptRge);
        jsFiles = [];
        for (_j = 0, _len2 = results.length; _j < _len2; _j++) {
          result = results[_j];
          srcIndex = result.indexOf('src=');
          jsIndex = result.indexOf('.js');
          jsFiles.push(result.substring(srcIndex + 5, jsIndex) + '.js');
        }
        combineJSON[item.name] = {
          css: cssFiles,
          js: jsFiles
        };
        return finish();
      });
    }, function(err) {
      return combineFiles(combineJSON, guideTemplateList.filePath);
    });
  };

  combineFiles = function(combineJSON, path) {
    var file, files;
    for (file in combineJSON) {
      console.log("css files:" + file);
      files = combineJSON[file].css;
      if (files != null) {
        combineCssFile(files, path, file, function() {
          return combineFiles(combineJSON, path);
        });
        delete combineJSON[file].css;
      } else {
        files = combineJSON[file].js;
        if (files != null) {
          combineJsFiles(files, path, file, function() {
            return combineFiles(combineJSON, path);
          });
          delete combineJSON[file];
          return;
        }
      }
    }
    return console.log('finished');
  };

  combineCssFile = function(files, path, saveFile, finished) {
    var saveData;
    console.log("files:" + (files.join()));
    saveData = '';
    return async.forEachSeries(files, function(item, cbf) {
      return fs.readFile(path + item, function(err, data) {
        if (err) throw err;
        saveData += data;
        return cbf();
      });
    }, function(err) {
      if (err) throw err;
      console.log("save " + path + "css/min/" + saveFile + ".css");
      fs.writeFile("" + path + "css/min/" + saveFile + ".css", saveData.replace(/\.\.\/\.\.\/images\//g, './images/'));
      return finished();
    });
  };

  combineJsFiles = function(files, path, saveFile, finished) {
    var saveData;
    console.log("files:" + (files.join()));
    if (saveFile.indexOf('_950') !== -1) {
      return process.nextTick(function() {
        return finished();
      });
    } else {
      saveFile = saveFile.replace('_750', '');
      saveData = '';
      return async.forEachSeries(files, function(item, cbf) {
        return fs.readFile(path + item, function(err, data) {
          if (err) throw err;
          saveData += data;
          return cbf();
        });
      }, function(err) {
        var ast, final_code;
        if (err) throw err;
        console.log("save " + path + "js/min/" + saveFile + ".js");
        ast = jsp.parse(saveData);
        ast = pro.ast_mangle(ast);
        ast = pro.ast_squeeze(ast);
        final_code = pro.gen_code(ast);
        fs.writeFile("" + path + "js/min/" + saveFile + ".js", final_code);
        return finished();
      });
    }
  };

  fs.readFile(complierFileName, function(err, data) {
    var guideTemplateList;
    guideTemplateList = JSON.parse(data);
    return getCombineFileList(guideTemplateList);
  });

  /*
  request 'http://192.168.100.23/guide/front.php?seller_nick=%E4%B8%80%E4%BA%9B%E4%BA%8B%E4%B8%80%E4%BA%9B%E6%83%852007&t=1&module_width=950', (err, res, body) ->
    if not err and res.statusCode == 200
      console.log body
  */

  /*
  fs = require "fs"
  spawn = require("child_process").spawn;
  args = process.argv.slice 1
  LISTEN_FILE_LIST = []
  FILE_COMPILE_TIMER_LIST = []
  COMPLIER_JSON = null
  DIRECTORY_TOTAL = 0
  COMPILER_LESS_CSS_INTERVAL = 5000
  COMPILER_JAVASCRIPT_INTERVAL = 60 * 1000
  
  
  if args[1] is "-c"
    complierFileName = args[2]
  else
    complierFileName = "./compiler.json"
  fs.readFile complierFileName, (err, data) ->
    if not err
      COMPLIER_JSON = JSON.parse data
      if COMPLIER_JSON.lessDelayCompile?
        COMPILER_LESS_CSS_INTERVAL = COMPLIER_JSON.lessDelayCompile
      if COMPLIER_JSON.jsDelayCompile?
        COMPILER_JAVASCRIPT_INTERVAL = COMPLIER_JSON.jsDelayCompile
      startListenFiles COMPLIER_JSON
  
  startListenFiles = (data) ->
    paths = data.listenPathArray
    for path in paths
      DIRECTORY_TOTAL++
      getFiles path
  
  
  getFiles = (path) ->
    if path[path.length - 1] isnt "/"
      path += "/"
    checkFinishTotal = 0
    fs.readdir path, (err, files) ->
      if err or (files.length is 0)
        DIRECTORY_TOTAL--
        return
      for file in files
        do(file) ->
          fs.stat path + file, (err, stats) ->
            if (file.substring 0, 1) != "."
              if stats.isFile() is true
                checkFile path, file
              else
                DIRECTORY_TOTAL++
                getFiles path + file
            checkFinishTotal++
            if checkFinishTotal is files.length
              DIRECTORY_TOTAL--
            if DIRECTORY_TOTAL is 0
              listenFiles LISTEN_FILE_LIST
  
  
  checkFile = (path, file) ->
    listenFileType = COMPLIER_JSON.listenFileType
    result = false
    for type in listenFileType
      if ((file.indexOf type) isnt -1) and ((file.indexOf(".min" + type)) is -1)
        result = true
        break;
    if result is true
      LISTEN_FILE_LIST.push path + file
      FILE_COMPILE_TIMER_LIST.push null
  
  listenFiles = (fileList) ->
    console.log "listenFiles: #{fileList.join '\n'}"
    for file in fileList
      do(file) ->
        fs.watch file, (event, fileName) ->
          if event is "change"
            compileFunc file
  
  compileFunc = (file) ->
    index = LISTEN_FILE_LIST.indexOf file
    
    if FILE_COMPILE_TIMER_LIST[index]?
      clearTimeout FILE_COMPILE_TIMER_LIST[index]
    if (file.indexOf ".less") != -1
      FILE_COMPILE_TIMER_LIST[index] = setTimeout (()->
        compileLessCss file
        FILE_COMPILE_TIMER_LIST[index] = null
        ), COMPILER_LESS_CSS_INTERVAL
    else
      FILE_COMPILE_TIMER_LIST[index] = setTimeout (()-> 
        compileJavaScript file
        FILE_COMPILE_TIMER_LIST[index] = null
        ), COMPILER_JAVASCRIPT_INTERVAL
      
  compileLessCss = (file) ->
    console.log "compileLessCss: #{file} (date: #{new Date()})"
    
    cssFile = file.replace ".less", ".css"
    compile = spawn "node", ["../lesscss/bin/lessc", "-x", file, cssFile] ;
    compile.stdout.on "data", (data)->
      console.log "stout: #{data}"
      
    compile.stderr.on "data", (data)->
      console.log "stderr: #{data}"
    
    compile.on "exit", (code)->
      console.log "exited: #{code}"
  
  compileJavaScript = (file) ->
    console.log "compileJavaScript: #{file} (date: #{new Date()})"
    jsFile = file.replace ".js", ".min.js"
    compile = spawn "node", ["../uglifyjs/bin/uglifyjs", "-o", jsFile, file]
    compile.stdout.on "data", (data)->
      console.log "stout: #{data}"
      
    compile.stderr.on "data", (data)->
      console.log "stderr: #{data}"
    
    compile.on "exit", (code)->
      console.log "exited: #{code}"
  */

}).call(this);
