const path = require('path');

module.exports = {
    entry: {
        tiptapVue2: '/Scripts/npm_builder/tiptap-build/tiptap-vue2.js',
        tiptapStarterkit: '/Scripts/npm_builder/tiptap-build/tiptap-starterkit.js',
        tiptapCodeblocklowlight: '/Scripts/npm_builder/tiptap-build/tiptap-codeblocklowlight.js',
        tiptapImage: '/Scripts/npm_builder/tiptap-build/tiptap-image.js',
        tiptapLink: '/Scripts/npm_builder/tiptap-build/tiptap-Link.js',
        tiptapPlaceholder: '/Scripts/npm_builder/tiptap-build/tiptap-placeholder.js',
        tiptapUnderline: '/Scripts/npm_builder/tiptap-build/tiptap-underline.js',
        lowlight: '/Scripts/npm_builder/tiptap-build/lowlight.js',
        hastUtilToHtml: '/Scripts/npm_builder/tiptap-build/hast-util-to-html.js',
    },
    output: {
        path: path.resolve(__dirname, 'Scripts/npm/tiptap-build/'),
        filename: '[name].js',
      },
    optimization: {
        minimize: true,
        splitChunks: {
            chunks: 'all',
        },
        runtimeChunk: 'single',
    }
}