<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated class="glossy bg-secondary q-pa-sm">
      <q-toolbar>
        <q-toolbar-title><About /></q-toolbar-title>
      </q-toolbar>
    </q-header>

    <q-page-container>
      <SeoChecker />
    </q-page-container>
  </q-layout>
</template>

<script>
import About from "./components/About.vue";
import SeoChecker from "./components/SeoChecker.vue";

export default {
  name: "LayoutDefault",

  components: {
    SeoChecker,
    About,
  },
};
</script>
