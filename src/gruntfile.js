module.exports = function (grunt) {

    grunt.initConfig({
        copy: {
            style: {
                files: [{
                    cwd: '../css',
                    src: '**/*.*',
                    dest: 'wwwroot/css',
                    expand: true
                }]
            },
            content: {
                files: [{
                    cwd: '../posts',
                    src: '**/*.*',
                    dest: 'wwwroot/posts',
                    expand: true
                }]
            },
            views: {
                files: [{
                    cwd: '../templates',
                    src: '**/*.*',
                    dest: 'Views/Shared',
                    expand: true
                }]
            }
        },
        clean: {
            posts: 'wwwroot/posts',
            style: 'wwwroot/css'
        }
    });

    grunt.registerTask('postpublish', ['clean:posts', 'copy:content']);

    grunt.registerTask('precompile', ['copy:views', 'clean:style', 'copy:style']);

    grunt.loadNpmTasks('grunt-contrib-copy');

    grunt.loadNpmTasks('grunt-contrib-clean');
};