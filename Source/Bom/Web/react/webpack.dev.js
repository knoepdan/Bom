const { merge } = require('webpack-merge');
process.env.NODE_ENV = 'development'; // before require!!
const common = require('./webpack.common.js');

module.exports = merge(common, {
    mode: 'development',
    devtool: 'inline-source-map',
});
