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
                    src: ['**/*', '!**/media/**'],
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
        },
        imagemin: {
            dynamic: {
                files: [{
                    expand: true,                  // Enable dynamic expansion
                    cwd: '../posts',               // Src matches are relative to this path
                    src: ['**/*.{png,jpg,gif}'],   // Actual patterns to match
                    dest: 'wwwroot/posts'          // Destination path prefix
                }]
            }
          }
    });

    grunt.registerTask('postpublish', ['clean:posts', 'copy:content', 'imagemin']);

    grunt.registerTask('precompile', ['copy:views', 'clean:style', 'copy:style']);

    grunt.loadNpmTasks('grunt-contrib-copy');

    grunt.loadNpmTasks('grunt-contrib-clean');

    grunt.loadNpmTasks('grunt-contrib-imagemin');
};