<template>
  <div>
    <form @submit.prevent="onSubmit">
      <input v-model="keywords" type="text" />
      <input v-model="referenceToFind" type="text" />
      <input class="button" type="submit" value="Get SEO results" />
    </form>
    <div v-if="isLoading" class="loader">Loading...</div>
  </div>
</template>

<script lang="ts">
import {
  SearchEngineReferencesRequestInfo,
  SeoCheckerActionTypes,
} from "@/store/seoChecker";
import { Options, Vue } from "vue-class-component";
import { namespace } from "vuex-class";

const seoChecker = namespace("seoChecker");
@Options({
  components: {},
})
export default class SeoChecker extends Vue {
  @seoChecker.State("searchEngineReferencesRequestInfo")
  searchEngineReferencesRequestInfo!: SearchEngineReferencesRequestInfo;

  @seoChecker.Action(SeoCheckerActionTypes.REQUEST_DATA)
  requestData!: (payload: {
    keywords: string;
    referenceToFind: string;
  }) => void;

  @seoChecker.State("isLoading") isLoading!: boolean;

  keywords = "";
  referenceToFind = "";

  onSubmit(): void {
    this.requestData({
      keywords: this.keywords,
      referenceToFind: this.referenceToFind,
    });
  }

  created(): void {
    this.keywords = this.searchEngineReferencesRequestInfo.keywords;
    this.referenceToFind =
      this.searchEngineReferencesRequestInfo.referenceToFind;
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped src="@/assets/spinner.css"></style>
