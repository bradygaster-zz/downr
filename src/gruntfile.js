module.exports = function (grunt) {

    grunt.initConfig({
        copy: {
            content: {
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
            posts: 'wwwroot/posts'
        }
    });

    grunt.registerTask('postpublish', ['clean:posts', 'copy:content']);

    grunt.registerTask('precompile', ['copy:views']);

    grunt.loadNpmTasks('grunt-contrib-copy');

    grunt.loadNpmTasks('grunt-contrib-clean');
};