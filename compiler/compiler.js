var fs = require('fs');
var logger  = require('log4js').getLogger();
var path = require('path');
var _ = require('underscore');
var async = require('async');
var less = require('less');
var uglifyJS = require("uglify-js");
var coffeeScript = require('coffee-script');
var mkdirp = require('mkdirp');
var argv = require('optimist').argv;
var noop = function(){};
/**
 * getWatchFiles 获取监听文件列表
 * @param  {[type]} watchPath  监听目录
 * @param  {[type]} ext        监听文件后缀
 * @param  {[type]} resultFiles 用于保存监听文件列表的数组
 * @param  {[type]} cbf        完成时调用的callback
 * @return {[type]}            [description]
 */
var getWatchFiles = function(watchPath, ext, resultFiles, cbf){
  fs.readdir(watchPath, function(err, files){
    if(err){
      logger.error(err);
      cbf();
      return ;
    }
    async.forEachLimit(files, 10, function(file, cbf){
      if(file.substring(0, 1) === '.'){
        process.nextTick(cbf);
      }else{
        file = path.join(watchPath, file);
        fs.stat(file, function(err, stat){
          if(err){
            cbf();
          }
          else if(stat.isFile()){
            if(ext){
              if(path.extname(file) === ext && file.indexOf('.min' + ext) === -1){
                resultFiles.push(file);
              }
            }else{
              resultFiles.push(file);
            }
            cbf();
          }else{
            getWatchFiles(file, ext, resultFiles, cbf);
          }
        });
      }
    },function(err){
      if(err){
        logger.error(err);
      }
      cbf();
    });
  });
};

/**
 * compileLess 编译less文件
 * @param  {[type]} file    less源文件
 * @param  {[type]} cssFile 编译之后保存的css文件
 * @return {[type]}         [description]
 */
var compileLess = function(file, cssFile, cbf){
  cbf = cbf || noop;
  fs.readFile(file, 'utf8', function(err, data){
    if(err){
      logger.error(err);
      cbf();
      return ;
    }
    var env = {
      paths : [path.dirname(file)]
    };
    
    var parser = new less.Parser(env);
    try{
      parser.parse(data, function(err, tree){
        if(err){
          logger.error(err);
          cbf();
          return ;
        }
        data = tree.toCSS({compress : true}).trim();
        if(data.length !== 0){
          fs.writeFile(cssFile, data.replace(/\n/g, ''), 'utf8', function(err){
            cbf();
            if(err){
              logger.error(err);
            }else{
              logger.info('complie less to file:' + cssFile + ' successful');
            }
          });
        }
      });
    }catch(err){
      cbf();
      logger.error(err);
    }
  });
};

/**
 * compressJavascript 压缩javascript文件
 * @param  {[type]} file      未压缩的js源文件
 * @param  {[type]} minJsFile 压缩之后保存的js文件
 * @return {[type]}           [description]
 */
var compressJavascript = function(file, minJsFile, cbf){
  cbf = cbf || noop;
  var result;
  try{
    result = uglifyJS.minify(file, {
      // warnings : true
    });
  }catch(err){
    logger.error(err);
    return;
  }
  if(result && result.code){
    fs.writeFile(minJsFile, result.code, 'utf8', function(err){
      cbf();
      if(err){
        logger.error(err);
      }else{
        logger.info('compress js to file:' + minJsFile + ' successful');
      }
    });
  }else{
    process.nextTick(cbf);
    logger.error('fail to compress js file:' + file);
  }
};

/**
 * compileCoffeeScript 编译coffee到javascript
 * @param  {[type]} file   coffee源文件
 * @param  {[type]} jsFile javascript目标文件
 * @return {[type]}        [description]
 */
var compileCoffeeScript = function(file, jsFile, cbf){
  cbf = cbf || noop;
  fs.readFile(file, 'utf8', function(err, data){
    if(err){
      cbf();
      logger.error(err);
      return ;
    }
    var jsCode;
    try{
      jsCode = coffeeScript.compile(data);
    }catch(err){
      if(err){
        cbf();
        logger.error(err);
        return ;
      }
    }
    fs.writeFile(jsFile, jsCode, 'utf8', function(err){
      cbf();
      if(err){
        logger.error(err);
      }else{
        logger.info('compile coffeescript to file:' + jsFile + ' successful');
      }
    });
  });
};

/**
 * startWatchFiles 开始监听目录
 * @param  {[type]} watchConfig 一些关于监听的配置参数
 * @param {[type]} immediatelyCompile 是否立即先编译一次该文件
 * @return {[type]}             [description]
 */
var startWatchFiles = function(watchConfig, immediatelyCompile){
  var resultFiles = [];
  var cbf = function(files){
    logger.info(files);
    async.forEachLimit(files, 10, function(file, cbf){
      var compileFunc = _.debounce(function(file){
        compileHandle(file, watchConfig);
      }, watchConfig.delay);
      if(immediatelyCompile){
        compileHandle(file, watchConfig, cbf);
      }
      else{
        process.nextTick(cbf);
      }
      fs.watchFile(file, {persistent : true, interval : 2000},function(curr, prev){
        compileFunc(file);
      });
    }, function(){
      console.log('complete...............');
    });
  };

  var tmpFunc = function(){
    var files = [];
    getWatchFiles(watchConfig.path, watchConfig.ext, files, function(){
      var results = _.difference(files, resultFiles);
      if(results.length){
        cbf(results);
        resultFiles = resultFiles.concat(results);
      }
      setTimeout(tmpFunc, 5000);
    });

  };
  tmpFunc();

};



/**
 * compileHandle 根据不同的文件执行各自对应的编译方法
 * @param  {[type]} file        [description]
 * @param  {[type]} watchConfig [description]
 * @return {[type]}             [description]
 */
var compileHandle = function(file, watchConfig, cbf){
  cbf = cbf || noop;
  var ext = path.extname(file);
  var saveFile, newExt;
  switch(ext){
    case '.less':
      newExt = '.css';
    break;
    case '.js':
      newExt = '.min.js';
    break;
    case '.coffee':
      newExt = '.js';
    break;
  }
  if(newExt){
    saveFile = file.replace(ext, newExt);
    if(watchConfig.targetPath){
      saveFile = saveFile.replace(watchConfig.path, watchConfig.targetPath);
    }
    var handleFunction = HANDLE_FUNCTIONS[ext.substring(1)];
    if(handleFunction){
      var savePath = path.dirname(saveFile);
      async.waterfall([
        function(cbf){
          fs.exists(savePath, function(exists){
            cbf(null, exists);
          });
        },
        function(exists, cbf){
          if(exists){
            handleFunction(file, saveFile, cbf);
          }else{
            mkdirp(savePath, function(err){
              if(err){
                logger.error(err);
                cbf();
              }else{
                handleFunction(file, saveFile, cbf);
              }
            });
          }
        }
      ], function(err, result){
        cbf();
      });
      
    }
  }
};

/**
 * compileStart 开始监听编译器
 * @return {[type]} [description]
 */
var compileStart = function(){
  var compileJson = argv.j || './compile.json';
  var immediatelyCompile = false;
  if(argv.c === 'all'){
    immediatelyCompile = true;
  }
  fs.readFile(compileJson, 'utf8', function(err, data){
    if(err){
      logger.error(err);
    }else{
      try{
        compileJson = JSON.parse(data);
        if(_.isArray(compileJson)){
          _.each(compileJson, function(compile){
            startWatchFiles(compile, immediatelyCompile);
          });
        }else{
          startWatchFiles(compileJson, immediatelyCompile);
        }
      }catch(err){
        logger.error(err);
      }
    }
  });
};


// var copyFiles = function(sourcePath, destPath, filterExt){
//   var resultFiles = [];
//   getWatchFiles('/Users/vicanso/workspace/vicansocode/projects', '/Users/vicanso/workspace/vicansocode/sources', resultFiles, function(){
//     logger.info(resultFiles);
//   });
// };

var HANDLE_FUNCTIONS = {
  less : compileLess,
  coffee : compileCoffeeScript,
  js : compressJavascript
};

compileStart();

// var jsfileWatchConfig = {
//   path : '/Users/vicanso/tmp',
//   targetPath : '/Users/vicanso/target',
//   ext : '.less',
//   delay : 1000
// };
// startWatchFiles(jsfileWatchConfig);
