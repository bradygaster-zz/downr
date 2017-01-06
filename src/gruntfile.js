module.exports = function (grunt) {

    grunt.initConfig({
        copy: {
            main: {
                files: [{
                    cwd: '../css',
                    src: '**/*.*',
                    dest: 'wwwroot/css',
                    expand: true
                },
                {
                    cwd: '../posts',
                    src: '**/*.*',
                    dest: 'wwwroot/posts',
                    expand: true
                }]
            }
        }
    });

    grunt.registerTask('default', ['copy']);

    grunt.loadNpmTasks('grunt-contrib-copy');
};