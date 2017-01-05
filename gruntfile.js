module.exports = function (grunt) {

    grunt.initConfig({
        copy: {
            main: {
                files: [{
                    cwd: 'setup/css',
                    src: '**/*.*',
                    dest: 'wwwroot/css',
                    expand: true
                },
                {
                    cwd: 'setup/posts/hello-downr',
                    src: '**/*.*',
                    dest: 'wwwroot/posts/hello-downr',
                    expand: true
                }]
            }
        }
    });

    grunt.registerTask('default', ['copy']);

    grunt.loadNpmTasks('grunt-contrib-copy');
};