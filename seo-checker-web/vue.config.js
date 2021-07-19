module.exports = {
  transpileDependencies: ["vuex-persist", "quasar"],

  pluginOptions: {
    quasar: {
      importStrategy: "kebab",
      rtlSupport: true,
    },
  },
};
