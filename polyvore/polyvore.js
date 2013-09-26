(function() {
  var async, complierFileName, fs, jsp, pro, request, _;

  fs = require('fs');

  request = require('request');

  _ = require('underscore');

  async = require('async');

  jsp = require("uglify-js").parser;

  pro = require("uglify-js").uglify;

  complierFileName = './polyvore.json';

  fs.readFile(complierFileName, function(err, data) {
    var polyvoreMergeFiles;
    polyvoreMergeFiles = JSON.parse(data);
    return request(polyvoreMergeFiles.mergeUrl, function(err, res, body) {
      var mergeData;
      body = JSON.parse(body);
      if (body.code !== 0) {
        console.log('get data faile');
        return;
      }
      data = body.data;
      mergeData = _.values(data.js);
      return _.each(mergeData, function(mergeFiles) {
        var mergeFileName;
        mergeFileName = mergeFiles.shift();
        mergeData = "/*merge with these files:" + (mergeFiles.join()) + "*/";
        _.each(mergeFiles, function(mergeFile) {
          console.log(polyvoreMergeFiles.filePath + mergeFile);
          mergeData += fs.readFileSync(polyvoreMergeFiles.filePath + mergeFile);
          return mergeData += ';';
        });
        fs.openSync(polyvoreMergeFiles.filePath + mergeFileName, 'w', function(err, fd) {
          if (err) return console.log(err);
        });
        return fs.writeFileSync(polyvoreMergeFiles.filePath + mergeFileName, mergeData);
      });
    });
  });

}).call(this);
