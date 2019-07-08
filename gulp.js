var gulp = require('gulp');

gulp.task('default', defaultTask);

function defaultTask(done){
   
    console.log("Hello gulp has been integrated in your project.");
    done();
}