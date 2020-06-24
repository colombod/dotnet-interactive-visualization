const path = require('path');
const version = require('./package.json').version;

// Custom webpack rules
const rules = [
  { test: /\.ts$/, loader: 'ts-loader' },
  { test: /\.js$/, loader: 'source-map-loader' },
  { test: /\.css$/, use: ['style-loader', 'css-loader'] }
];

// Packages that shouldn't be bundled but loaded at runtime
const externals = ['@jupyter-widgets/base'];

const resolve = {
  // Add '.ts' and '.tsx' as resolvable extensions.
  extensions: [".webpack.js", ".web.js", ".ts", ".js"]
};

module.exports = [
  {
    entry: './src/index.ts',
    output: {
      filename: 'index.js',
      path: path.resolve(__dirname, 'dist'),
      libraryTarget: 'umd'
    },
    module: {
      rules: rules
    },
    devtool: 'source-map',
    externals,
    resolve,
    optimization: {
      minimize: true
    }
  },

  {
    entry: './src/index.ts',
    output: {
      filename: 'lib.js',
      path: path.resolve(__dirname, 'dist'),
      libraryTarget: 'commonjs'
    },
    module: {
      rules: rules
    },
    devtool: 'source-map',
    externals,
    resolve,
    optimization: {
      minimize: true
    }
  },


  /**
   * Documentation widget bundle
   *
   * This bundle is used to embed widgets in the package documentation.
   */
  {
    entry: './src/index.ts',
    output: {
      filename: 'embed-bundle.js',
      path: path.resolve(__dirname, 'docs', 'source', '_static'),
      library: "@microsoft/sanddance-interactive",
      libraryTarget: 'amd'
    },
    module: {
      rules: rules
    },
    devtool: 'source-map',
    externals,
    resolve,
    optimization: {
      minimize: false
    }
  }

];
