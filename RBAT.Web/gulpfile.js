/// <binding BeforeBuild="default, less" Clean="clean" />

var gulp = require('gulp'),
    bower = require('gulp-bower'),
    //scss = require("gulp-scss"),
    del = require("del");

var paths = {
    //scripts: ["scripts/**/*.js", "scripts/**/*.map"],
    libjs: [
        "node_modules/jquery/dist/jquery.js",
        "node_modules/jquery-ui-dist/jquery-ui.js",
        "node_modules/popper.js/dist/umd/popper.js", // bootstrap dependency
        "node_modules/bootstrap/dist/js/bootstrap.js",
        "node_modules/bootstrap-datepicker/dist/js/bootstrap-datepicker.js",
        "node_modules/datatables.net/js/jquery.dataTables.js",
        "node_modules/jszip/dist/jszip.js",
        "node_modules/pdfmake/build/pdfmake.js",
        "node_modules/pdfmake/build/vfs_fonts.js",
        "node_modules/datatables.net-buttons/js/dataTables.buttons.js",
        "node_modules/datatables.net-buttons/js/buttons.html5.js",
        "node_modules/datatables.net-buttons/js/buttons.print.js",
        "node_modules/datatables.net-buttons-bs/js/buttons.bootstrap.js",
        "node_modules/datatables.net-select/js/dataTables.select.js",
        "node_modules/datatables.net-bs/js/dataTables.bootstrap.js",
        "node_modules/datatables.net-scroller-bs4/js/scroller.bootstrap4.min.js",
        "node_modules/jquery-validation/dist/jquery.validate.js",
        "node_modules/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js",
        "node_modules/jquery.easing/jquery.easing.js",
        "node_modules/datatables.net-rowreorder/js/dataTables.rowReorder.js",
        "node_modules/dygraphs/dist/dygraph.min.js"
    ],
    libcss: [
        "node_modules/datatables.net-buttons-bs/css/buttons.bootstrap.css",
        "node_modules/datatables.net-scroller-bs4/css/scroller.bootstrap4.css",
        "node_modules/bootstrap/dist/css/bootstrap.css",
        "node_modules/bootstrap-datepicker/dist/css/bootstrap-datepicker.css",
        "node_modules/@fortawesome/fontawesome-free/css/all.css",
        "node_modules/jquery-ui-dist/jquery-ui.css",
        "node_modules/dygraphs/dist/dygraph.css"

    ],
    libfonts: [
        "node_modules/bootstrap/fonts/glyphicons-halflings-regular.eot",
        "node_modules/bootstrap/fonts/glyphicons-halflings-regular.svg",
        "node_modules/bootstrap/fonts/glyphicons-halflings-regular.ttf",
        "node_modules/bootstrap/fonts/glyphicons-halflings-regular.woff",
        "node_modules/bootstrap/fonts/glyphicons-halflings-regular.woff2"

    ],
    libwebfonts: [
        "node_modules/@fortawesome/fontawesome-free/webfonts/fa-regular-400.eot",
        "node_modules/@fortawesome/fontawesome-free/webfonts/fa-regular-400.svg",
        "node_modules/@fortawesome/fontawesome-free/webfonts/fa-regular-400.ttf",
        "node_modules/@fortawesome/fontawesome-free/webfonts/fa-regular-400.woff",
        "node_modules/@fortawesome/fontawesome-free/webfonts/fa-regular-400.woff2",

        "node_modules/@fortawesome/fontawesome-free/webfonts/fa-regular-400.eot",
        "node_modules/@fortawesome/fontawesome-free/webfonts/fa-regular-400.svg",
        "node_modules/@fortawesome/fontawesome-free/webfonts/fa-regular-400.ttf",
        "node_modules/@fortawesome/fontawesome-free/webfonts/fa-regular-400.woff",
        "node_modules/@fortawesome/fontawesome-free/webfonts/fa-regular-400.woff2",

        "node_modules/@fortawesome/fontawesome-free/webfonts/fa-solid-900.eot",
        "node_modules/@fortawesome/fontawesome-free/webfonts/fa-solid-900.svg",
        "node_modules/@fortawesome/fontawesome-free/webfonts/fa-solid-900.ttf",
        "node_modules/@fortawesome/fontawesome-free/webfonts/fa-solid-900.woff",
        "node_modules/@fortawesome/fontawesome-free/webfonts/fa-solid-900.woff2"
    ]
};

// Remove generated TypeScript files
gulp.task("clean", function () {
    return del([
        "wwwroot/lib/**/*"
    ]);
});

gulp.task("default", function () {
    // Copy generated TypeScript JS files to wwwroot - NOTE: Note needed with rootDir and outDir set in tsconfig.json?
    //gulp.src(paths.scripts).pipe(gulp.dest("wwwroot/scripts"));
    // Copy lib js to wwwroot/lib
    gulp.src(paths.libjs).pipe(gulp.dest("wwwroot/lib/js"));
    // Copy lib css to wwwroot/lib/css
    gulp.src(paths.libcss).pipe(gulp.dest("wwwroot/lib/css"));
    // Copy lib css to wwwroot/lib/css/webfonts
    gulp.src(paths.libwebfonts).pipe(gulp.dest("wwwroot/lib/webfonts"));
    // Copy lib fonts to wwwroot/lib/fonts
    gulp.src(paths.libfonts).pipe(gulp.dest("wwwroot/lib/fonts"));
});