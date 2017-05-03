module.exports = function (grunt) {

    grunt.initConfig({
        copy: {
            content: {
                files: [{
                    cwd: '../posts',
                    src: ['**/*', '!**/media/**'],
                    dest: 'wwwroot/posts',
                    expand: true
                },
                {
                    cwd: '../pages',
                    src: ['**/*', '!**/media/**'],
                    dest: 'wwwroot/pages',
                    expand: true
                },
                {
                    cwd: '../css',
                    src: '**/*',
                    dest: 'wwwroot/css',
                    expand: true
                }]
            },
            views: {
                files: [{
                    cwd: '../templates',
                    src: '**/*',
                    dest: 'Views/Shared',
                    expand: true
                }]
            }
        },
        clean: {
            pages: 'wwwroot/pages',
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
                }, {
                    expand: true,                  // Enable dynamic expansion
                    cwd: '../pages',               // Src matches are relative to this path
                    src: ['**/*.{png,jpg,gif}'],   // Actual patterns to match
                    dest: 'wwwroot/pages'          // Destination path prefix
                }]
            }
        }
    });

    grunt.registerTask('postpublish', ['clean:posts','clean:pages', 'copy:content','imagemin']);

    grunt.registerTask('precompile', ['copy:views']);

    grunt.loadNpmTasks('grunt-contrib-copy');

    grunt.loadNpmTasks('grunt-contrib-clean');

    grunt.loadNpmTasks('grunt-contrib-imagemin');
};