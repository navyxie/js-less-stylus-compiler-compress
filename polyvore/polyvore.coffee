fs = require 'fs'
request = require 'request'
_ = require 'underscore'
async = require 'async'
jsp = require("uglify-js").parser
pro = require("uglify-js").uglify

complierFileName = './polyvore.json'

fs.readFile complierFileName, (err, data) ->
  polyvoreMergeFiles = JSON.parse data
  request polyvoreMergeFiles.mergeUrl, (err, res, body) ->
    body = JSON.parse body
    if body.code isnt 0
      console.log 'get data faile'
      return
    data = body.data
    mergeData = _.values data.js
    _.each mergeData, (mergeFiles) ->
        mergeFileName = mergeFiles.shift()
        mergeData = "/*merge with these files:#{mergeFiles.join()}*/"
        _.each mergeFiles, (mergeFile) ->
            console.log polyvoreMergeFiles.filePath + mergeFile
            mergeData += (fs.readFileSync polyvoreMergeFiles.filePath + mergeFile)
            mergeData += ';'
        fs.openSync polyvoreMergeFiles.filePath + mergeFileName, 'w', (err, fd) ->
            if err
                console.log err
        fs.writeFileSync polyvoreMergeFiles.filePath + mergeFileName, mergeData
        
