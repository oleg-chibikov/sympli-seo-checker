<template>
  <div class="q-pa-md" style="min-width: 100px">
    <RequestForm />
    <ResultsWrapper />
  </div>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";

import RequestForm from "./RequestForm.vue";
import ResultsWrapper from "./ResultsWrapper.vue";

@Options({
  components: { ResultsWrapper, RequestForm },
})
export default class SeoChecker extends Vue {}
</script>
