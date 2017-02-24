var exec = require('child_process').exec;

module.exports.takeImage = function(file, onSuccess) {
    var cmd = 'fswebcam ' + file;
    exec(onSuccess);   
};