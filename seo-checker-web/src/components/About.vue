<template>
  <div>
    <div class="text-h6">SEO checker</div>
    <div class="text-subtitle2">by {{ author }}</div>
  </div>
</template>

<script lang="ts">
import { Vue } from "vue-class-component";
import { State } from "vuex-class";

export default class About extends Vue {
  @State("author") author!: string;
}
</script>
