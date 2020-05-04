const webpack = require('webpack');
const path = require('path');
const package = require('./package.json');

// variables
const isProduction = process.argv.indexOf('-p') >= 0 || process.env.NODE_ENV === 'production';
const sourcePath = path.join(__dirname, './src');
const outPath = path.join(__dirname, './build');

// plugins
const HtmlWebpackPlugin = require('html-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const OptimizeCSSAssetsPlugin = require('optimize-css-assets-webpack-plugin'); // see https://web.dev/minify-css/

module.exports = {
    context: sourcePath,
    entry: {
        app: './main.tsx',
    },
    output: {
        path: outPath,
        filename: isProduction ? '[contenthash].js' : '[hash].js',
        chunkFilename: isProduction ? '[name].[contenthash].js' : '[name].[hash].js',
    },
    target: 'web',
    resolve: {
        extensions: ['.js', '.ts', '.tsx'],
        // Fix webpack's default behavior to not load packages with jsnext:main module
        // (jsnext:main directs not usually distributable es6 format, but es6 sources)
        mainFields: ['module', 'browser', 'main'],
        alias: {
            app: path.resolve(__dirname, 'src/app/'),
        },
    },
    module: {
        rules: [
            // .ts, .tsx
            {
                test: /\.tsx?$/,
                use: [
                    !isProduction && {
                        loader: 'babel-loader',
                        options: { plugins: ['react-hot-loader/babel'] },
                    },
                    'ts-loader',
                ].filter(Boolean),
            },
            // css
            {
                oneOf: [
                    {
                        test: /\.module\.css$/,
                        use: [
                            isProduction ? MiniCssExtractPlugin.loader : 'style-loader',
                            {
                                loader: 'css-loader',
                                query: {
                                    modules: true,
                                    sourceMap: !isProduction,
                                    importLoaders: 1,
                                    modules: {
                                        localIdentName: isProduction ? '[hash:base64:5]' : '[local]__[hash:base64:5]',
                                    },
                                    // localIdentName: isProduction ? '[hash:base64:5]' : '[local]__[hash:base64:5]' see https://github.com/rails/webpacker/issues/2197
                                },
                            },
                            {
                                loader: 'postcss-loader',
                                options: {
                                    ident: 'postcss',
                                    plugins: [
                                        require('postcss-import')({ addDependencyTo: webpack }),
                                        require('postcss-url')(),
                                        require('postcss-preset-env')({
                                            /* use stage 2 features (defaults) */
                                            stage: 2,
                                        }),
                                        require('postcss-reporter')(),
                                        require('postcss-browser-reporter')({
                                            disabled: isProduction,
                                        }),
                                    ],
                                },
                            },
                        ],
                    },
                    {
                        test: /\.css$/,
                        use: [
                            isProduction ? MiniCssExtractPlugin.loader : 'style-loader', //'style-loader',
                            {
                                loader: 'css-loader',
                                options: {
                                    modules: false,
                                },
                            },
                        ],
                    },
                ],
            },
            {
                oneOf: [
                    // static assets
                    { test: /\.html$/, use: 'html-loader', exclude: /index\.html$/ },
                    {
                        test: /\.(a?png|svg|jp?g|gif)$/,
                        use: [
                            {
                                // url load is just like file-loader but will not externalize resources below a certain limit
                                loader: 'url-loader', // example passing limit via query; "'url-loader?limit=10000' "
                                options: {
                                    limit: 8192,
                                    name: 'static/img/[name].[hash:8].[ext]',
                                },
                            },
                        ],
                    },
                    {
                        test: /\.(eot|ttf|woff|woff2)$/,
                        use: [
                            {
                                loader: 'file-loader',
                                options: {
                                    name: 'static/font/[name].[hash:8].[ext]',
                                },
                            },
                        ],
                    },
                    {
                        loader: require.resolve('file-loader'),
                        // exclude files that are processed by other loaders (other solution would be to use oneof)
                        exclude: [/\.(js|mjs|jsx|ts|tsx|css)$/, /\.html$/, /\.json$/],
                        options: {
                            name: 'static/etc/[name].[hash:8].[ext]',
                        },
                    },
                    // dont add a loader below file-loader
                ],
            },
        ],
    },
    optimization: {
        splitChunks: {
            name: true,
            cacheGroups: {
                commons: {
                    chunks: 'initial',
                    minChunks: 2,
                },
                vendors: {
                    test: /[\\/]node_modules[\\/]/,
                    chunks: 'all',
                    filename: isProduction ? 'vendor.[contenthash].js' : 'vendor.[hash].js',
                    priority: -10,
                },
            },
        },
        runtimeChunk: true,
    },
    plugins: [
        new webpack.EnvironmentPlugin({
            NODE_ENV: 'development', // use 'development' unless process.env.NODE_ENV is defined
            DEBUG: false,
        }),
        new CleanWebpackPlugin(),
        //  maybe use this, see https://github.com/johnagan/clean-webpack-plugin/issues/156
        //   new CleanWebpackPlugin({
        //     dry: false,
        //     verbose: true,
        //     cleanStaleWebpackAssets: true,
        //     protectWebpackAssets: false,
        //    cleanOnceBeforeBuildPatterns: ['**/*'],
        //})

        new MiniCssExtractPlugin({
            filename: '[hash].css',
            disable: !isProduction,
        }),
        new OptimizeCSSAssetsPlugin({
            // // this would work too but no sourcemap would be generated:  new OptimizeCSSAssetsPlugin({}),
            // see  https://github.com/NMFR/optimize-css-assets-webpack-plugin/issues/53
            // https://github.com/NMFR/optimize-css-assets-webpack-plugin/issues/81
            cssProcessorOptions: {
                map: {
                    inline: false, // will generate css sourcempas only in dev mode
                    annotation: isProduction,
                },
            },
        }),
        new HtmlWebpackPlugin({
            template: 'assets/index.html',
            favicon: 'assets/favicon.ico',
            scriptLoading: 'defer',
            title: 'React template',
            /* // nice custom logic:  // custom logic: https://github.com/jaketrent/html-webpack-template/tree/86f285d5c790a6c15263f5cc50fd666d51f974fd
            googleAnalytics: {
              trackingId: 'UA-XXXX-XX'
            },
            */
            minify: {
                minifyJS: true,
                minifyCSS: true,
                removeComments: true,
                useShortDoctype: true,
                collapseWhitespace: true,
                collapseInlineTagWhitespace: true,
            },
            append: {
                head: `<script src="//cdn.polyfill.io/v3/polyfill.min.js"></script>`,
            },
            meta: {
                title: package.name,
                description: package.description,
                keywords: Array.isArray(package.keywords) ? package.keywords.join(',') : undefined,
            },
        }),
    ],
    devServer: {
        contentBase: sourcePath,
        hot: true,
        inline: true,
        historyApiFallback: {
            disableDotRule: true,
        },
        stats: 'minimal',
        clientLogLevel: 'warning',
    },
    // https://webpack.js.org/configuration/devtool/
    devtool: isProduction ? 'hidden-source-map' : 'cheap-module-eval-source-map',
    node: {
        // workaround for webpack-dev-server issue
        // https://github.com/webpack/webpack-dev-server/issues/60#issuecomment-103411179
        fs: 'empty',
        net: 'empty',
    },
};
