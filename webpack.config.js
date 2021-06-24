var ExtractTextPlugin = require ("extract-text-webpack-plugin");
var path = require ("path");

var jsxConfig = {
    mode: "production",
    entry: {
        RatingSearch: "./R7.Enrollment.Dnn/Views/Ratings/RatingSearch.jsx"
    },
    output: {
        path: path.resolve (__dirname, "R7.Enrollment.Dnn/assets/js"),
        filename: "[name].min.js"
    },
    module: {
        rules: [
            {
                test: /\.js$|\.jsx$/,
                exclude: /(node_modules|bower_components)/,
                use: {
                    loader: "babel-loader",
                    options: {
                        presets: ["@babel/preset-react", "@babel/preset-env"]
                    }
                }
            }
        ]
    },
};

var jsConfig = {
    mode: "production",
    entry: {
        EnrollmentService: "./R7.Enrollment.Dnn/Services/EnrollmentService.js"
    },
    output: {
        path: path.resolve (__dirname, "R7.Enrollment.Dnn/assets/js"),
        filename: "[name].min.js"
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /(node_modules|bower_components)/,
                use: {
                    loader: "babel-loader",
                    options: {
                        presets: ["@babel/preset-env"]
                    }
                }
            }
        ]
    },
};

module.exports = [jsxConfig, jsConfig];
