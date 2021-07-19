<template>
  <div class="home">
    <SeoChecker />
  </div>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";
import SeoChecker from "@/components/SeoChecker.vue"; // @ is an alias to /src

@Options({
  components: {
    SeoChecker,
  },
})
export default class Home extends Vue {}
</script>
