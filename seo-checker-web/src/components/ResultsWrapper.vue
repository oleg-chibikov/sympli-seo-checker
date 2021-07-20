<template>
  <transition mode="out-in" name="fade">
    <Spinner v-if="isLoading" />
    <Results v-else />
  </transition>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { namespace } from "vuex-class";

import Results from "./Results.vue";
import Spinner from "./Spinner.vue";

const seoChecker = namespace("seoChecker");

@Options({
  components: { Results, Spinner },
})
export default class ResultsWrapper extends Vue {
  @seoChecker.State("isLoading") isLoading!: boolean;
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s;
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
}
</style>
