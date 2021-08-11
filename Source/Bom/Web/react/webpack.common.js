﻿const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const BrowserSyncPlugin = require('browser-sync-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const isDevelopment = process.env.NODE_ENV === 'development';
//const CopyPlugin = require('copy-webpack-plugin');
const outPath = path.join(__dirname, './../wwwroot');

module.exports = {
    mode: 'development',
    entry: {
        index: ['./src/main.tsx'],
    },
    devtool: isDevelopment ? 'cheap-module-eval-source-map' : 'source-map',
    output: {
        path: outPath,
        //  path: path.resolve(__dirname, './../../dist'),
        filename: '[name].[contenthash].js',
        publicPath: '', // path in LayoutApp.cshtml, must be without '/' so base tag is interpreted correctly
        clean: true,
    },
    resolve: {
        extensions: ['.ts', '.tsx', '.js', '.css', '.scss'],
        alias: {
            app: path.resolve(__dirname, 'src/app/'),
            //react: path.resolve('./node_modules/react'),
            //'react-dom': path.resolve('./node_modules/react-dom'),
        },
    },
    module: {
        rules: [
            {
                use: {
                    loader: 'babel-loader',
                },
                test: /\.js$/,
                exclude: /node_modules/,
            },
            {
                use: {
                    loader: 'ts-loader',
                },
                test: /\.(tsx|ts)$/,
                exclude: /node_modules/,
            },
            {
                test: /\.module\.s(a|c)ss$/,
                use: [
                    {
                        loader: isDevelopment ? 'css-loader' : MiniCssExtractPlugin.loader,
                    },
                    {
                        loader: 'css-loader',
                        options: {
                            sourceMap: isDevelopment,
                            importLoaders: 1,
                            modules: {
                                localIdentName: '[name]__[local]--[hash:base64:5]',
                            },
                        },
                    },
                    {
                        loader: 'sass-loader',
                        options: {
                            sourceMap: isDevelopment,
                        },
                    },
                ],
            },
            {
                test: /\.s(a|c)ss$/,
                exclude: /\.module.(s(a|c)ss)$/,
                use: [
                    {
                        loader: isDevelopment ? 'css-loader' : MiniCssExtractPlugin.loader,
                    },
                    {
                        loader: 'css-loader',
                    },
                    {
                        loader: 'sass-loader',
                        options: {
                            sourceMap: isDevelopment,
                        },
                    },
                ],
            },
            {
                test: /\.css$/,
                use: ['style-loader', 'css-loader'],
            },
            {
                test: /\.(woff|woff2|eot|ttf|otf)$/,
                type: 'asset/inline',
            },
            {
                test: /\.(png|svg|jpg|gif)$/,
                type: 'asset/inline',
            },
        ],
    },
    optimization: {
        splitChunks: {
            name: 'r',
            cacheGroups: {
                commons: {
                    chunks: 'initial',
                    minChunks: 2,
                },
                vendors: {
                    test: /[\\/]node_modules[\\/]/,
                    chunks: 'all',
                    filename: 'vendor.[contenthash].js',
                    priority: -10,
                },
            },
        },
        runtimeChunk: true,
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: '[name].[contenthash].css',
        }),
        new BrowserSyncPlugin({
            https: true,
        }),
        new HtmlWebpackPlugin({
            inject: 'body',
            filename: '../../Web/Identity/Views/Home/_LayoutApp.cshtml',
            template: './../Identity/Views/Home/_LayoutApp_Template.cshtml',
            minify: false, // affects template (not js etc.)
        }),
        // new CopyPlugin({
        //     patterns: [
        //       { from: './src/resources', to: 'resources' },
        //     ]
        // }),
    ],
};
