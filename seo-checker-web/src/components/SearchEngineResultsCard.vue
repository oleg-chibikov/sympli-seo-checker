<template>
  <q-card class="text-white q-my-md">
    <q-card-section
      :class="searchEngineResults.hasError ? 'bg-negative' : undefined"
    >
      <div class="text-h6">{{ searchEngineResults.searchEngine }}</div>

      <q-separator dark></q-separator>
      <q-badge
        v-for="resultNumberInSearchEngine in searchEngineResults.indexesInSearchResults"
        :key="resultNumberInSearchEngine"
        outline
        class="q-px-md q-my-sm q-mr-sm text-h6 bg-accent"
        color="secondary"
        >#{{ resultNumberInSearchEngine }}
      </q-badge>
    </q-card-section>
  </q-card>
</template>

<script lang="ts">
import { Vue } from "vue-class-component";
import { Prop } from "vue-property-decorator";

import { SearchEngineResults } from "@/store/seoChecker";

export default class SearchEngineResultsCard extends Vue {
  @Prop()
  searchEngineResults!: SearchEngineResults;
}
</script>
