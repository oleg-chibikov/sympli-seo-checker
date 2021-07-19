<template>
  <div class="about">
    <h1>SEO checker developed as a part of Sympli interview process</h1>
    <h2>Author: {{ author }}</h2>
  </div>
</template>

<script lang="ts">
import { Vue } from "vue-class-component";
import { State } from "vuex-class";

export default class About extends Vue {
  @State("author") author!: string;
}
</script>
