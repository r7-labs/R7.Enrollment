var ExtractTextPlugin = require ("extract-text-webpack-plugin");
var path = require ("path");

var jsxConfig = {
    mode: "production",
    entry: {
        HelloWorld: "./R7.Enrollment.Dnn/Views/HelloWorld.jsx"
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

module.exports = [jsxConfig];
