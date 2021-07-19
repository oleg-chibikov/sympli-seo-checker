<template>
  <div>
    <SearchEngineResultsCard
      v-for="(result, index) in results"
      :key="result.searchEngine"
      :class="index % 2 ? 'bg-teal-6' : 'bg-light-green-5'"
      :search-engine-results="result"
    />
  </div>
</template>

<script lang="ts">
import { SearchEngineResults } from "@/store/seoChecker";
import { Options, Vue } from "vue-class-component";
import { namespace } from "vuex-class";
import SearchEngineResultsCard from "./SearchEngineResultsCard.vue";

const seoChecker = namespace("seoChecker");
@Options({
  components: { SearchEngineResultsCard },
})
export default class Results extends Vue {
  @seoChecker.State("results")
  results!: SearchEngineResults[];
}
</script>
